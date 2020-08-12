using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AphrieTask.DTOs;
using AphrieTask.Interfaces;
using AphrieTask.Localize;
using AphrieTask.Models;
using AphrieTask.NLog_Exceptions;
using AphrieTask.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OtpNet;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AphrieTask.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _singInManager;
        private readonly IOptions<ApplicationSettings> appSettings;
        private readonly IUnitOfWork repository;
        private readonly IOptions<TwilioSettings> twilioSettings1;
        private readonly ILog logger;
        private readonly IStringLocalizer<Resource> localizer;
        private readonly ApplicationSettings _appSettings;
        private readonly TwilioSettings twilioSettings;
        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<ApplicationSettings> appSettings, IUnitOfWork repository, IOptions<TwilioSettings> twilioSettings , ILog logger , IStringLocalizer<Resource> localizer)
        {

            _userManager = userManager;
            _singInManager = signInManager;
            this.appSettings = appSettings;
            this.repository = repository;
            twilioSettings1 = twilioSettings;
            this.logger = logger;
            this.localizer = localizer;
            _appSettings = appSettings.Value;
            this.twilioSettings = twilioSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/Authentication/Register
        public async Task<Object> Register([FromBody] RegisterViewModel model)
        {
            IdentityUser identityUser = new IdentityUser();
            identityUser.UserName = model.username;
            identityUser.PhoneNumber = model.phone;
            try
            {
                var Result = await _userManager.CreateAsync(identityUser, model.password);
                if (!Result.Succeeded)
                {
                    foreach (var item in Result.Errors)
                    {
                        return BadRequest(item.Description);
                    }
                }
                sendOTP(identityUser.Id,model.phone);
                var json = JsonConvert.SerializeObject(Result.Succeeded);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false ,
                    message = "Registration process has failed"
                };
                return BadRequest(dto);
            }
        }


        [HttpGet, Route("sendOTP")]
        public IActionResult sendOTP(string userId, string phone)
        {
            try
            {
                TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
                string allowedNums = "0123456789";
                Random random = new Random();
                char[] chars = new char[6];
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = allowedNums[(int)(chars.Length * random.NextDouble())];
                }
                string OTP = new string(chars);
                var message = MessageResource.Create(
                    body: $"Aphrie Verification Code is : {OTP}",
                    from: new Twilio.Types.PhoneNumber("+18133287387"),
                    to: new Twilio.Types.PhoneNumber($"+2{phone}")
                    );
                phoneOTP phoneOTP = new phoneOTP()
                {
                    user_Id = userId,
                    OTP = int.Parse(OTP),
                    phone = phone,
                    isValid=true
                };
                repository.PhoneOTP.Add(phoneOTP);
                repository.save();
                return Ok(message);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false,
                    message = "Sending OTP Code has failed"
                };
                return BadRequest(dto);
            }
        }

        [HttpPost,Route("verifyOTP")]
        public IActionResult verifyOTP(VerficationViewModel model)
        {
            DTO dto = new DTO();
            try
            {
                IdentityUser user = repository.User.FindOneByCondition(u=>u.PhoneNumber.Equals(model.phone));
                phoneOTP phoneOTP = repository.PhoneOTP.FindOneByCondition(o => o.user_Id == user.Id);              
                if (phoneOTP.OTP == model.OTP && phoneOTP.phone == model.phone && phoneOTP.isValid == true)
                {
                    user.PhoneNumberConfirmed = true;
                    phoneOTP.isValid = false;
                    repository.save();
                    string token = getToken(user).Value;
                    return Ok(token);
                }
                else
                {
                    dto.success = false;
                    dto.message = "OTP is invalid";
                    return BadRequest(dto);
                }                
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                dto.success = false;
                dto.message = "Operation has failed";
                return BadRequest(dto);
            }         
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/Authentication/Login
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.password))
            {
                if (user.PhoneNumberConfirmed == true)
                {
                    string token = getToken(user).Value;
                    return Ok(token);
                }
                else
                {
                    DTO dto = new DTO()
                    {
                        success = false ,
                        message = "Your phone Number is not confirmed"
                    };
                    return BadRequest(dto);
                }
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }

        
        [HttpGet , Route("getToken")]
        public ActionResult<string> getToken(IdentityUser user)
        {
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return token;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false , 
                    message = "Error has occured"
                };
                return BadRequest(dto);
            }

        }


        [HttpPost, Route("changePassword")]
        public async Task<IActionResult> changePassword(changePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(model.id);
                    if (user == null)
                    {
                        return BadRequest("user id is not assigned");
                    }
                    var result = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Ok();
                    }
                    await _singInManager.RefreshSignInAsync(user);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("UserID",user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    var response = new { token };
                    var json = JsonConvert.SerializeObject(response);
                    return new OkObjectResult(json);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false,
                    message = "change Password process has failed"
                };
                return BadRequest(dto);
            }            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AphrieTask.DTOs;
using AphrieTask.Interfaces;
using AphrieTask.Localize;
using AphrieTask.Models;
using AphrieTask.NLog_Exceptions;
using AphrieTask.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AphrieTask.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _singInManager;
        private readonly IOptions<ApplicationSettings> appSettings;
        private readonly IUnitOfWork repository;
        private readonly ILog logger;
        private readonly IStringLocalizer<Resource> localizer;
        private readonly ApplicationSettings _appSettings;
        public FriendsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<ApplicationSettings> appSettings, IUnitOfWork repository , ILog logger , IStringLocalizer<Resource> localizer)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            this.appSettings = appSettings;
            this.repository = repository;
            this.logger = logger;
            this.localizer = localizer;
            _appSettings = appSettings.Value;
        }


        [HttpPost , Route("addFriend")]
        public IActionResult addFriend(addFriendViewModel model)
        {
            try
            {
                var user = repository.User.FindOneByCondition(f => f.UserName == model.username);
                Friend oldRecord1 = repository.Friend.FindOneByCondition(f => f.client_Id == model.id && f.friend_Id == user.Id);
                Friend oldRecord2 = repository.Friend.FindOneByCondition(f => f.friend_Id == model.id && f.client_Id == user.Id);
                List<Friend> friendList = new List<Friend>();
                if (oldRecord1==null && oldRecord2 == null)
                {
                    Friend friend = new Friend()
                    {
                        client_Id = model.id,
                        friend_Id = repository.User.FindOneByCondition(u=>u.UserName.Equals(model.username)).Id,
                        isUnfriend = false
                    };
                    repository.Friend.Add(friend);
                    repository.save();
                    DTO dto = new DTO()
                    {
                        success = true,
                        message = $"you and {user.UserName} are fiends now , and you can see his/her posts"
                    };
                    return Ok(dto);
                }
                else if(oldRecord1 != null && oldRecord1.isUnfriend == true)
                {
                    oldRecord1.isUnfriend = false;                   
                    repository.save();
                    DTO dto = new DTO()
                    {
                        success = true,
                        message = $"you and {user.UserName} are fiends now , and you can see his/her posts"
                    };
                    return Ok(dto);
                }
                else if (oldRecord2 != null && oldRecord2.isUnfriend == true)
                {
                    oldRecord2.isUnfriend = false;
                    repository.save();
                    DTO dto = new DTO()
                    {
                        success = true,
                        message = $"you and {user.UserName} are fiends now , and you can see his/her posts"
                    };
                    return Ok(dto);
                }
                else
                {
                    DTO dto = new DTO()
                    {
                        success = false,
                        message = $"{user.UserName} is already a friend"
                    };
                    return BadRequest(dto);
                }
            }
            catch (Exception ex) 
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false,
                    message = "User has NOT been added to your Friend List , Operation has failed !!!!"
                };
                return BadRequest(dto);
            }
        }

        [HttpPost , Route("unFriend")]
        public ActionResult unFriend(addFriendViewModel model)
        {
            try
            {
                IdentityUser user = repository.User.FindOneByCondition(f => f.UserName == model.username);
                Friend oldRecord1 = repository.Friend.FindOneByCondition(f => f.client_Id == model.id && f.friend_Id == user.Id);
                Friend oldRecord2 = repository.Friend.FindOneByCondition(f => f.friend_Id == model.id && f.client_Id == user.Id);                
                if (oldRecord1 != null && oldRecord1.isUnfriend == false)
                {
                    oldRecord1.isUnfriend = true;                                 
                }
                else if (oldRecord2 != null && oldRecord2.isUnfriend == false)
                {
                    oldRecord2.isUnfriend = true;
                }
                repository.save();
                DTO dto = new DTO();
                dto.success = true;
                dto.message = $"{user.UserName} has been removed from your friend-list";
                return Ok(dto);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false,
                    message = "operation doesn't success"
                };
                return BadRequest(dto);
            }
        }
    }
}

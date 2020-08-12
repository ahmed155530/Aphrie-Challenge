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
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AphrieTask.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    public class ReactsController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _singInManager;
        private readonly IOptions<ApplicationSettings> appSettings;
        private readonly IUnitOfWork repository;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper mapper;
        private readonly ILog logger;
        private readonly IStringLocalizer<Resource> localizer;

        public ReactsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<ApplicationSettings> appSettings, IUnitOfWork repository, IMapper mapper , ILog logger , IStringLocalizer<Resource> localizer)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            this.appSettings = appSettings;
            this.repository = repository;
            _appSettings = appSettings.Value;
            this.mapper = mapper;
            this.logger = logger;
            this.localizer = localizer;
        }

        [HttpPost ,Route("likePost")]
        public ActionResult<DTO> likePost([FromBody]ReactViewModel model)
        {
            React OldReact = repository.React.FindOneByCondition(r => r.user_Id == model.userId && r.post_Id == model.postId);
            DTO dto = new DTO();
            try
            {
                if(OldReact == null)
                {
                    React react = new React()
                    {
                        user_Id = model.userId , 
                        post_Id = model.postId,
                        isLiked = true,
                        isLoved = false
                    };                    
                    repository.React.Add(react);
                    dto.success = true;
                    dto.message = "You liked this post";
                }
                else if(OldReact != null && OldReact.isLiked == false)
                {
                    OldReact.isLiked = true;
                    OldReact.isLoved = false;
                    dto.success = true;
                    dto.message = "You liked this post";
                }
                else if(OldReact != null && OldReact.isLiked == true &&  OldReact.isLoved == false)
                {
                    OldReact.isLiked = false;
                    dto.success = true;
                    dto.message = "You unliked this post";
                }
                repository.save();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                dto.success = false;
                dto.message = "operation failed";
                return Ok(dto);
            }           
        }




        [HttpPost, Route("lovePost")]
        public ActionResult<DTO> lovePost([FromBody] ReactViewModel model)
        {
            React OldReact = repository.React.FindOneByCondition(r => r.user_Id == model.userId && r.post_Id == model.postId);
            DTO dto = new DTO();
            try
            {
                if (OldReact == null)
                {
                    React react = new React();
                    react.isLoved = true;
                    react.isLiked = false;
                    repository.React.Add(react);
                    dto.success = true;
                    dto.message = "You loved this post";
                }
                else if (OldReact != null && OldReact.isLoved == false)
                {
                    OldReact.isLoved = true;
                    OldReact.isLiked = false;
                    dto.success = true;
                    dto.message = "You loved this post";
                }
                else if (OldReact != null && OldReact.isLoved == true && OldReact.isLiked == false)
                {
                    OldReact.isLoved = false;
                    OldReact.isLiked = false;
                    dto.success = true;
                    dto.message = "You unloved this post";
                }
                repository.save();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                dto.success = false;
                dto.message = "operation failed";
                return Ok(dto);
            }

        }
    }
}

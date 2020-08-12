using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AphrieTask.Models;
using Microsoft.AspNetCore.Identity;
using AphrieTask.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.IO;
using AphrieTask.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using AutoMapper;
using Microsoft.Extensions.Localization;
using AphrieTask.Localize;
using AphrieTask.NLog_Exceptions;

namespace AphrieTask.Controllers
{
    [Route("{culture:culture}/api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _singInManager;
        private readonly IOptions<ApplicationSettings> appSettings;
        private readonly IUnitOfWork repository;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<Resource> localizer;
        private readonly ILog logger;

        public PostsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<ApplicationSettings> appSettings, IUnitOfWork repository , IMapper mapper , IStringLocalizer<Resource> localizer , ILog logger)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            this.appSettings = appSettings;
            this.repository = repository;
            _appSettings = appSettings.Value;
            this.mapper = mapper;
            this.localizer = localizer;
            this.logger = logger;
        }

        // GET: api/Posts
        [HttpGet,Route("getPosts/{id}")]
        public ActionResult<IEnumerable<Post>> GetPosts(string id)
        {
            try
            {
                List<Friend> clientsList = repository.Friend.FindByCondition(u=>u.client_Id == id && u.isUnfriend == false).ToList();
                List<Friend> friendsList = repository.Friend.FindByCondition(u => u.friend_Id == id && u.isUnfriend == false).ToList();
                IEnumerable<Friend> finalList =  clientsList.Union(friendsList);
                List<Post> posts = new List<Post>();
                foreach (var item in clientsList)
                {
                    List<Post> friendPosts1 = repository.Post.FindByCondition(u => u.user_Id == item.friend_Id.ToString() && u.isDeleted == false).ToList();
                    foreach (var post in friendPosts1)
                    {
                        posts.Add(post);
                    }
                }
                foreach (var item in friendsList)
                {
                    List<Post> friendPosts2 = repository.Post.FindByCondition(u => u.user_Id == item.friend_Id.ToString() && u.isDeleted == false).ToList();
                    foreach (var post in friendPosts2)
                    {
                        posts.Add(post);
                    }
                }
                PostsDTO dto = new PostsDTO()
                {
                    success = true,
                    data = mapper.Map<List<PostDTO>>(posts)
                };
            return Ok(dto);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false,
                    message = "An Error has occured"
                };
                return Ok(dto);
            }
        }

       
        // POST: api/Posts
        [HttpPost,Route("addPost")]
        public ActionResult<Post> PostPost([FromForm]Post post)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Images", "Post's Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    post.image = fileName;
                    repository.Post.Add(post);
                    repository.save();
                    DTO dto = new DTO()
                    {
                        success = true,
                        message = "our post has been added"
                    };
                    return Ok(dto);
                }
                else
                {
                    DTO dto = new DTO()
                    {
                        success = false,
                        message = "our post has NOT been added"
                    };
                    return BadRequest(dto);
                }
            }
            catch (Exception)
            {
                DTO dto = new DTO()
                {
                    success = false,
                    message = "our post has NOT been added"
                };
                return BadRequest(dto);
            }
            
        }

        // DELETE: api/Posts/5
        [HttpDelete , Route("deletePost/{id}")]
        public ActionResult<Post> DeletePost(Guid id)
        {
            try
            {
                Post post = repository.Post.FindOneByCondition(p => p.id == id);
                if (post == null)
                {
                    return NotFound();
                }
                post.isDeleted = true;
                repository.save();
                DTO dto = new DTO()
                {
                    success = true,
                    message = "your post has been deleted"
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Warning(ex.StackTrace);
                DTO dto = new DTO()
                {
                    success = false,
                    message = "your post has NOT been deleted"
                };
                return BadRequest(dto);
            }

        }

        [HttpGet , Route("getName")]
        public ActionResult<string> get()
        {
            string word = localizer["name"];
            return Ok(word);
        }
    }
}

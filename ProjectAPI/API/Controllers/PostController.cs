using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using PostEntity = DataAccess.Data.Post;

namespace API.Controllers.Post
{
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly BaseService<PostEntity> PostService;
        private readonly ILogger Logger;
        public PostController(BaseService<PostEntity> postService, ILogger<PostController> logger)
        {
            PostService = postService;
            Logger = logger;
        }

        [HttpGet()]
        public ActionResult<IQueryable<PostEntity>> GetAll()
        {
            try
            {
                var posts = PostService.GetAll();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        public ActionResult<PostEntity> Create([FromBodyAttribute] PostEntity entity)
        {
            try
            {
                var post = PostService.Create(entity);
                return Ok(post);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Many")]
        public ActionResult<IQueryable<PostEntity>> CreateManyPost([FromBodyAttribute] IQueryable<PostEntity> entity)
        {
            try
            {
                var posts = PostService.CreateMany(entity);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut()]
        public ActionResult<PostEntity> Update([FromBodyAttribute] PostEntity entity)
        {
            try
            {
                var post = PostService.Update(entity.PostId, entity, out bool _changed);
                return Ok(post);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete()]
        public ActionResult<PostEntity> Delete([FromBodyAttribute] PostEntity entity)
        {
            try
            {
                var post = PostService.Delete(entity);
                return Ok(post);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}

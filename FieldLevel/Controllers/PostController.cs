using FieldLevel.Models.Contract;
using FieldLevel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FieldLevel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService PostService;

        public PostController(IPostService postService)
        {
            this.PostService = postService;
        }

        /// <summary>
        /// Returns a list of PostDtos that are the latest post (based on post.id) for each user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLatestPostForEachUser")]
        public async Task<IActionResult> GetLatestPostForEachUser()
        {
            try
            {
                List<PostDto> postDtos = await PostService.GetLatestPostByUser();
                return Ok(postDtos);
            }
            catch(HttpRequestException httpEx)
            {
                Log.Error(httpEx, "Data source was unable to be reached.");
                throw;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Error geting latest post for each user.");
                throw;
            }
        }
    }
}

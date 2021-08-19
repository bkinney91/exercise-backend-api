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

        [HttpGet]
        [Route("GetLatestPostForEachUser")]
        public async Task<IActionResult> GetLatestPostForEachUser(int id)
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
            catch (Exception ex)
            {
                Log.Error(ex, $"Error geting latest post for each user.");
                throw;
            }
        }
    }
}

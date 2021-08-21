using FieldLevel.DataProviders.Interfaces;
using FieldLevel.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FieldLevel.DataProviders
{
    public class PostFileProvider : IPostProvider
    {
        private readonly string RootPath;
        public PostFileProvider(IWebHostEnvironment webHostEnvironment)
        {
            RootPath = webHostEnvironment.ContentRootPath;
        }

        /// <summary>
        /// Reads JSON sample file from posts.json in the root directory
        /// </summary>
        /// <returns></returns>
        public async Task<List<Post>> GetPosts()
        {
            string jsonPosts = System.IO.File.ReadAllText(RootPath + "/posts.json");
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(jsonPosts);
            return posts;
        }
    }
}

using FieldLevel.Models.Contract;
using FieldLevel.Models.Entity;
using FieldLevel.Properties;
using FieldLevel.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FieldLevel.Services
{
    public class PostService : IPostService
    {
        /// <summary>
        /// Client factory for getting posts
        /// </summary>
        private readonly IHttpClientFactory HttpClientFactory;
        public PostService(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Retrieves the latest posts from the data source and
        /// sorts them to only retain the latest post for each user.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostDto>> GetLatestPostByUser()
        {
            List<Post> posts = await GetPostsFromDataSource();
            List<PostDto> postDtos = posts.OrderByDescending(x => x.Id) //Start by ordering the Ids by latest/highest
                .GroupBy(x => x.UserId) //Group by users
                .Select(x => x.First()) //Get only the ID that is latest/highest
                .Select(x => new PostDto { Id = x.Id, UserId = x.UserId, Body = x.Body, Title = x.Title }) //Translate to DTO so we can give to controller
                .ToList();
            return postDtos;

        }

        /// <summary>
        /// Uses an Http client to get the JSON payload
        /// </summary>
        /// <returns></returns>
        private async Task<List<Post>> GetPostsFromDataSource()
        {
            HttpClient postClient = HttpClientFactory.CreateClient("postClient");
            var url = new Uri(postClient.BaseAddress.ToString());
            HttpResponseMessage postResponse = await postClient.GetAsync(url);
            postResponse.EnsureSuccessStatusCode(); //Throws if not 200
            string jsonPosts = await postResponse.Content.ReadAsStringAsync();
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(jsonPosts);
            return posts;
        }
    }
}

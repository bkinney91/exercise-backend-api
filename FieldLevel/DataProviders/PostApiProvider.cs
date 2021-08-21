using FieldLevel.DataProviders.Interfaces;
using FieldLevel.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FieldLevel.DataProviders
{
    public class PostApiProvider : IPostProvider
    {
        /// <summary>
        /// Client factory for getting posts
        /// </summary>
        private readonly IHttpClientFactory HttpClientFactory;
        public PostApiProvider(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Uses an Http client to get the JSON payload
        /// </summary>
        /// <returns></returns>
        public async Task<List<Post>> GetPosts()
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

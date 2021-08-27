using FieldLevel.DataProviders.Interfaces;
using FieldLevel.Models.Contract;
using FieldLevel.Models.Entity;
using FieldLevel.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FieldLevel.Services
{
    public class PostService : IPostService
    {
        /// <summary>
        /// Client factory for getting posts
        /// </summary>
        private readonly IPostProvider PostProvider;
        private IMemoryCache PostCache;
        private readonly string PostsCacheKey = "Posts";
        public PostService(IPostProvider postPrvovider, IMemoryCache postCache)
        {
            this.PostProvider = postPrvovider;
            this.PostCache = postCache;
        }

        /// <summary>
        /// Retrieves the all posts from the data source and
        /// sorts them to only retain the latest post for each user.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostDto>> GetLatestPostByUser()
        {
            List<Post> posts = new List<Post>();
            //Check cache first, if it's available save it to posts local var
            if (PostCache.TryGetValue(PostsCacheKey, out posts) == false)
            {
                //Key was not in cache, get fresh data from api
                posts = await PostProvider.GetPosts();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    //Set to expire in one minute
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                //Save frest posts into cache
                PostCache.Set(PostsCacheKey, posts, cacheEntryOptions);
            }            
            List<PostDto> postDtos = posts.OrderByDescending(x => x.Id) //Start by ordering the Ids by latest/highest
                .GroupBy(x => x.UserId) //Group by users
                .Select(x => x.First()) //Get only the ID that is latest/highest
                .Select(x => new PostDto { Id = x.Id, UserId = x.UserId, Body = x.Body, Title = x.Title }) //Translate to DTO so we can give to controller
                .ToList();
            return postDtos;
        }

    }
}

using FieldLevel.DataProviders.Interfaces;
using FieldLevel.Models.Contract;
using FieldLevel.Models.Entity;
using FieldLevel.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldLevel.Services
{
    public class PostService : IPostService
    {
        /// <summary>
        /// Client factory for getting posts
        /// </summary>
        private readonly IPostProvider PostProvider;
        public PostService(IPostProvider postPrvovider)
        {
            this.PostProvider = postPrvovider;
        }

        /// <summary>
        /// Retrieves the latest posts from the data source and
        /// sorts them to only retain the latest post for each user.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostDto>> GetLatestPostByUser()
        {
            List<Post> posts = await PostProvider.GetPosts();
            List<PostDto> postDtos = posts.OrderByDescending(x => x.Id) //Start by ordering the Ids by latest/highest
                .GroupBy(x => x.UserId) //Group by users
                .Select(x => x.First()) //Get only the ID that is latest/highest
                .Select(x => new PostDto { Id = x.Id, UserId = x.UserId, Body = x.Body, Title = x.Title }) //Translate to DTO so we can give to controller
                .ToList();
            return postDtos;
        }

    }
}

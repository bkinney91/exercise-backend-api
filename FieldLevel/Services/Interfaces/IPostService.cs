using FieldLevel.Models.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FieldLevel.Services.Interfaces
{
    public interface IPostService
    {
        public Task<List<PostDto>> GetLatestPostByUser();
    }
}

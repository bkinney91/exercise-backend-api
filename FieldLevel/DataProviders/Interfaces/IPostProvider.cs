using FieldLevel.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FieldLevel.DataProviders.Interfaces
{
    public interface IPostProvider
    {
        public Task<List<Post>> GetPosts();
    }
}

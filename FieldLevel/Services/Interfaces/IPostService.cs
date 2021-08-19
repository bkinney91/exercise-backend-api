using FieldLevel.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldLevel.Services.Interfaces
{
    public interface IPostService
    {
        public Task<List<PostDto>> GetLatestPostByUser();
    }
}

using AphrieTask.Interfaces;
using AphrieTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Repositories
{
    public class PostRepository:RepositoryBase<Post> , IPostRepository
    {
        public PostRepository(RepositoryContext repositoryContext):base(repositoryContext)
        {
        }
    }
}

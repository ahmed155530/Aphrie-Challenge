using AphrieTask.Interfaces;
using AphrieTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Repositories
{
    public class ReactRepository : RepositoryBase<React> , IReactRepository
    {
        public ReactRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}

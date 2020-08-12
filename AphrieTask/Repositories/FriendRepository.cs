using AphrieTask.Interfaces;
using AphrieTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Repositories
{
    public class FriendRepository:RepositoryBase<Friend> , IFriendRepository
    {
        public FriendRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}

using AphrieTask.Interfaces;
using AphrieTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Repositories
{
    public class phoneOTPRepository : RepositoryBase<phoneOTP>, IPhoneOTPRepository
    {
        public phoneOTPRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository Post {get;}
        IFriendRepository Friend { get; }
        IReactRepository React { get; }
        IUserRepository User { get; }
        IPhoneOTPRepository PhoneOTP { get; }

        void save();
    }
}

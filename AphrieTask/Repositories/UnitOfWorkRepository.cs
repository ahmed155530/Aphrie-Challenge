using AphrieTask.Interfaces;
using AphrieTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private RepositoryContext repositoryContext;
        private PostRepository postRepository;
        private FriendRepository friendRepository;
        private UserRepository userRepository;
        private ReactRepository reactRepository;
        private phoneOTPRepository OTPRepository;



        public UnitOfWorkRepository(RepositoryContext _repositoryContext)
        {
            repositoryContext = _repositoryContext;
        }

        public IPostRepository Post
        {
            get
            {
                if (postRepository == null)
                {
                    postRepository = new PostRepository(repositoryContext);
                }
                return postRepository;
            }
        }
        public IReactRepository React
        {
            get
            {
                if (reactRepository == null)
                {
                    reactRepository = new ReactRepository(repositoryContext);
                }
                return reactRepository;
            }
        }
        public IFriendRepository Friend
        {
            get
            {
                if (friendRepository == null)
                {
                    friendRepository = new FriendRepository(repositoryContext);
                }
                return friendRepository;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(repositoryContext);
                }
                return userRepository;
            }
        }
        public IPhoneOTPRepository PhoneOTP
        {
            get
            {
                if (OTPRepository == null)
                {
                    OTPRepository = new phoneOTPRepository(repositoryContext);
                }
                return OTPRepository;
            }
        }
        public void save()
        {
            repositoryContext.SaveChanges();
        }

        public void Dispose()
        {
            repositoryContext.Dispose();
        }
    }
}

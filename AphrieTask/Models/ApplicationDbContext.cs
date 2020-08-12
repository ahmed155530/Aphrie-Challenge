using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Models
{
    public class RepositoryContext : IdentityDbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }
        public virtual DbSet<phoneOTP> OTPs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<React> Reacts { get; set; }

    }
}

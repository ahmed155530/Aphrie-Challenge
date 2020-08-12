using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Models
{
    public class Post
    {
        public Guid id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public bool? isDeleted { get; set; } = false;
        public DateTime? addingDate { get; set; } = DateTime.Now;
        public string user_Id { get; set; }
        [ForeignKey("user_Id")]
        public IdentityUser user { get; set; }
        public virtual ICollection<React> reacts { get; set; }

    }
}

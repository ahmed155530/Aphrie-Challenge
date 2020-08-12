using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Models
{
    public class User
    {
        [ForeignKey("user")]
        [Key]
        public string userID { get; set; }
        public virtual IdentityUser user { get; set; }
        public bool? isDeleted { get; set; } = false;
       // public virtual ICollection<Friend> users { get; set; }
        //public virtual ICollection<Friend> friends { get; set; }
        //public virtual ICollection<React> reacts { get; set; }
    }
}

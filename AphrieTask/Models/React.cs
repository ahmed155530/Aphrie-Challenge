using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Models
{
    public class React
    {
        [Key]
        public Guid id { get; set; }
        public string user_Id { get; set; }
        [ForeignKey("user_Id")]
        public IdentityUser user { get; set; }
        public Guid post_Id { get; set; }
        [ForeignKey("post_Id")]
        public Post post { get; set; }
        public bool? isLiked { get; set; } = false;
        public bool? isLoved { get; set; } = false;
    }
}

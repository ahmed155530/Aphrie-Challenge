using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Models
{
    public class Friend
    {
        [Key]
        public Guid id { get; set; }


        public string client_Id { get; set; }
        [ForeignKey("client_Id")]
        public IdentityUser client { get; set; }



        public string friend_Id { get; set; }
        [ForeignKey("friend_Id")]
        public IdentityUser friend { get; set; }



        public bool isUnfriend { get; set; }
    }
}

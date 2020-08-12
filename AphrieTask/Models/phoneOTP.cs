using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.Models
{
    public class phoneOTP
    {
        [Key]
        public Guid id { get; set; }
        public string user_Id { get; set; }
        [ForeignKey("user_Id")]
        public IdentityUser user { get; set; }
        [DataType(DataType.PhoneNumber)]
        public virtual string phone { get; set; }
        public int OTP { get; set; }
        public bool isValid { get; set; }
    }
}

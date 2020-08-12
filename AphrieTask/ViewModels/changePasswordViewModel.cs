using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.ViewModels
{
    public class changePasswordViewModel
    {
        public string id { get; set; }
        [DataType(DataType.Password), Required]
        public string currentPassword { get; set; }

        [DataType(DataType.Password), Required]
        public string newPassword { get; set; }

        [DataType(DataType.Password), Required, Compare("newPassword")]
        public string confirmPassword { get; set; }
    }
}

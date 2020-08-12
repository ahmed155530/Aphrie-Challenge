using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.ViewModels
{
    public class RegisterViewModel
    {
        public string username { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.DTOs
{
    public class PostDTO
    {
        public Guid id { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public DateTime? addingDate { get; set; }
        public string user_Id { get; set; }
    }
}

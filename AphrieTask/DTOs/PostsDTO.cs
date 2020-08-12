using AphrieTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AphrieTask.DTOs
{
    public class PostsDTO
    {
        public bool success { get; set; } = false;
        public List<PostDTO> data { get; set; }

    }
}

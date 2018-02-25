using PornSite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Data
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public int User_Id { get; set; }
        public int Video_Id { get; set; }
    }
}
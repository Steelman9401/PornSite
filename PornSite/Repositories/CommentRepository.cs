using DotVVM.Framework.Controls;
using PornSite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.Repositories
{
    public class CommentRepository
    {
        
        public GridViewDataSetLoadedData<CommentDTO> GetCommentsByVideoId(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions, int Id)
        {
            using (var db = new myDb())
            {
                var query = db.Comments
                    .Where(x => x.Video.Id == Id)
                    .Select(p => new CommentDTO()
                    {
                        Id = p.Id,
                        Text = p.Text,
                        Username = p.User.Username,
                        User_Id = p.User.Id
                    }).OrderByDescending(a => a.Id).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }

        public async Task AddComment(CommentDTO comment)
        {
            Comment com = new Comment();
            com.Text = comment.Text;
            using (var db = new myDb())
            {
                Video vid = await db.Videos.FindAsync(comment.Video_Id);
                User user = await db.Users.FindAsync(comment.User_Id);
                com.User = user;
                com.Video = vid;
                db.Comments.Add(com);
                await db.SaveChangesAsync();
            }
        }
    }
}
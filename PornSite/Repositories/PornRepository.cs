using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Repositories
{
    public class PornRepository
    {
        public List<VideoDTO> GetAllVideos()
        {
            using (var db = new myDb())
            {
                return db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Url = x.Url
                }).ToList();
            }
        }
        public List<VideoDTO> GetVideosByCategory(int catId)
        {
            using (var db = new myDb())
            {
                return db.Videos
                     .Where(p => p.Categories.Select(x => x.Id).FirstOrDefault() == catId)
                     .Select(x => new VideoDTO()
                     {
                         Id = x.Id,
                         Img = x.Img,
                         Title = x.Title,
                         Url = x.Url
                     }).OrderByDescending(a => a.Id).ToList();
            }
        }
    }
}
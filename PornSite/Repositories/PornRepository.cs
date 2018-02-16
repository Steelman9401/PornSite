using DotVVM.Framework.Controls;
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
        public GridViewDataSetLoadedData<VideoDTO> GetAllVideos(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {           
            using (var db = new myDb())
            {
                var query = db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Url = x.Url,
                    Views = x.Views
                }).OrderByDescending(p=>p.Id).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
        public GridViewDataSetLoadedData<VideoDTO> GetAllVideosByViews(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {
            using (var db = new myDb())
            {
                var query = db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Url = x.Url,
                    Views = x.Views
                }).OrderByDescending(p => p.Views).AsQueryable();
                return query.GetDataFromQueryable(gridViewDataSetLoadOptions);
            }
        }
            public IEnumerable<VideoDTO> GetVideosByCategory(int catId)
        {
            using (var db = new myDb())
            {
                return db.Videos
                     .Where(p => p.Categories.Select(x => x.Id).ToList().Contains(catId)) //to list mozna neni potreba
                     .Select(x => new VideoDTO()
                     {
                         Id = x.Id,
                         Img = x.Img,
                         Title = x.Title
                     }).OrderByDescending(a => a.Id).ToList();
            }
        }

        public VideoDTO GetVideoById(int Id)
        {
            
            using (var db = new myDb())
            {
                Video video = db.Videos
                    .Where(x => x.Id == Id).FirstOrDefault();
                video.Views++;
                db.SaveChanges();

                VideoDTO videoDTO = new VideoDTO();
                videoDTO.Categories = video.Categories.Select(o => new CategoryDTO
                {
                    Id = o.Id,
                    Name = o.Name
                });
                videoDTO.Description = video.Description;
                videoDTO.Id = video.Id;
                videoDTO.Img = video.Img;
                videoDTO.Title = video.Title;
                videoDTO.Url = video.Url;
                videoDTO.Views = video.Views;
                return videoDTO;
            }
                //return db.Videos
                //    .Where(x => x.Id == Id)
                //    .Select(p => new VideoDTO
                //    {
                //        Id = p.Id,
                //        Img = p.Img,
                //        Title = p.Title,
                //        Url = p.Url,
                //        Description = p.Description,
                //        Categories = p.Categories
                //        .Select(o => new CategoryDTO
                //        {
                //            Id = o.Id,
                //            Name = o.Name
                //        })
                //    }).FirstOrDefault();
            }
        public IEnumerable<VideoDTO> GetSuggestedVideos(List<int> Categories)
        {
            int a = Categories[0];
            int b = Categories[1];
            using (var db = new myDb())
            {
                return db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b))
                      .Select(p => new VideoDTO
                      {
                          Id = p.Id,
                          Title = p.Title,
                          Img = p.Img
                      }).ToList();
                
            }
        }
        }
    }
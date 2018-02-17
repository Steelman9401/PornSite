﻿using DotVVM.Framework.Controls;
using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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
                    Views = x.Views,
                    Preview = x.Preview
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
                    Views = x.Views,
                    Preview = x.Preview
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
                         Title = x.Title,
                         Preview = x.Preview
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
                //videoDTO.Img = video.Img;
                videoDTO.Title = video.Title;   //hnus, musi se upravit, ale zatim aspon funguje
                videoDTO.Url = video.Url;
                videoDTO.Views = video.Views;
                return videoDTO;

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
        }
        public IEnumerable<VideoDTO> GetSuggestedVideos(List<int> Categories)
        {
            if (Categories.Count >= 3)
            {
                int a = Categories[0];
                int b = Categories[1];
                int c = Categories[2];
                using (var db = new myDb())
                {
                    return db.Videos.Where(x => x.Categories.Select(p => p.Id).Contains(a) || x.Categories.Select(p => p.Id).Contains(b) || x.Categories.Select(p=>p.Id).Contains(c)) //hnus
                          .Select(p => new VideoDTO
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Img = p.Img
                          }).Take(8).ToList();
                }
            }
            else
            {
                List<VideoDTO> videos = new List<VideoDTO>();
                return videos;
            }
        }
        public IEnumerable<string> GetUrls()
        {
            using (var db = new myDb())
            {
                return db.Videos.OrderByDescending(p => p.Id)
                    .Select(x => x.Url).Take(100).ToList();
            }
        }

        public void AddPorn(Video video, List<Category> cat)
        {
            using (var db = new myDb())
            {
                foreach (Category item in cat)
                {
                    Category tag = db.Categories
                  .Where(x => x.Name == item.Name)
                  .FirstOrDefault();
                    if (tag == null)
                    {
                        video.Categories.Add(item);
                    }
                    else
                    {
                        video.Categories.Add(tag);
                    }
                }
                db.Videos.Add(video);
                db.SaveChanges();

            }
        }
    }
    }
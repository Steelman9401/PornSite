﻿using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Repositories
{
    public class PornRepository
    {
        public IEnumerable<VideoDTO> GetAllVideos()
        {
            using (var db = new myDb())
            {
                return db.Videos.Select(x => new VideoDTO
                {
                    Id = x.Id,
                    Img = x.Img,
                    Title = x.Title,
                    Url = x.Url
                }).OrderByDescending(a => a.Id).ToList();
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
                         Url = x.Url
                     }).OrderByDescending(a => a.Id).ToList();
            }
        }

        public VideoDTO GetVideoById(int Id)
        {
            using (var db = new myDb())
            {
                return db.Videos
                    .Where(x => x.Id == Id)
                    .Select(p => new VideoDTO
                    {
                        Id = p.Id,
                        Img = p.Img,
                        Title = p.Title,
                        Url = p.Url,
                        Categories = p.Categories
                        .Select(o => new CategoryDTO
                        {
                            Id = o.Id,
                            Name = o.Name
                        })
                    }).FirstOrDefault();
            }
        }
    }
}
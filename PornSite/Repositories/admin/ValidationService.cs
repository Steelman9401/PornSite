using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PornSite.Repositories.admin
{
    public class ValidationService
    {
        public bool VideoExists(string url)
        {
            using (var db = new myDb())
            {
                if (db.Videos.Where(x => x.Url == url).Count() != 0)
                    return true;
                else
                    return false;
            }
        }
        public bool CategoryExists(CategoryDTO cat)
        {
            using (var db = new myDb())
            {
                int check = db.Categories
                    .Where(x => x.Name == cat.Name && x.Name_en == cat.Name_en).Count();
                if (check > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ImageExists(string image, IEnumerable<string> Images)
        {
            var array = image.Split('/');
            string trimmedUrl = "";
            for (int i = 1; i <= 4; i++)
            {
                trimmedUrl = trimmedUrl.Insert(0, array[array.Length - i]);
            }
            if (trimmedUrl.Contains('?'))
            {
                trimmedUrl = trimmedUrl.Substring(0, trimmedUrl.IndexOf("?"));
            }
            int count = Images.Where(x => x == trimmedUrl).Count();
            if (count != 0)
                return true;
            else
                return false;
        }
        public async Task<bool> UserExist(string username, string password)
        {
            using (var db = new myDb())
            {
                string shaPassword = sha256(password);
                var user = await db.Users
                    .Where(x => x.Username == username && x.Password == shaPassword)
                    .FirstOrDefaultAsync();
                if (user != null)
                    return true;
                else
                    return false;
            }
        }

        private string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
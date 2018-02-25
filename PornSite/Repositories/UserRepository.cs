using PornSite.Data;
using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace PornSite.Repositories
{
    public class UserRepository
    {
        public async Task AddUser(string username, string password)
        {
            User dbUser = new User();
            dbUser.Username = username;
            dbUser.Password = sha256(password);
            using (var db = new myDb())
            {
                db.Users.Add(dbUser);
                await db.SaveChangesAsync();
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
        public bool UserExists(string Username)
        {
            using (var db = new myDb())
            {
                User user = db.Users.Where(x => x.Username == Username)
                            .FirstOrDefault();
                if(user==null)
                {
                    return false;
                }
                return true;
            }
        }
        public async Task<UserDTO> UserLogin(string username, string password)
        {
            password = sha256(password);
            using (var db = new myDb())
            {
                return await db.Users
                    .Where(x => x.Username == username && x.Password == password)
                    .Select(p => new UserDTO()
                    {
                        Id = p.Id,
                        Username = p.Username
                    }).FirstOrDefaultAsync();
            }
        }
    }
}
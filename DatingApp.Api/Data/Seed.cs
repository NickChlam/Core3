using System.Collections.Generic;
using System.Linq;
using DatingApp.Api.Models;
using Newtonsoft.Json;

namespace DatingApp.Api.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context) 
        {
            // if no users. 
            if(!context.Users.Any())
            {
                // get the JSON file in a var
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                // deserialize and convert to a list of users 
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach(var user in users)
                {
                    byte[] passwordhash, passwordSalt;
                    // create pass hash and salt 
                    CreatePasswordHash("password", out passwordhash, out passwordSalt);
                    user.Passwordhash = passwordhash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    // add user to context
                    context.Users.Add(user);

                }

                context.SaveChanges();
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // generate hmac key 
            using (var hmac = new System.Security.Cryptography.HMACSHA512() )
            {
                // save key as passwordSalt
                passwordSalt = hmac.Key;
                // save passwordHash as a computed has off password
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            // find user FOD returns null if none found
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);

            if(user == null)
                return null;
            
            // compare password if also return null
            if(!VerifyPasswordHash(password, user.Passwordhash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordSalt)
        {   
            // store hmac with encypted passSalt
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt) )
            {
                // compute a hash from user entered password and pass salt from database 
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                // compare computed hash vs passwordhash from database 
                for(int i = 0; i< computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordhash[i]) return false;
                }

                return true;
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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

        public async Task<User> Register(User user, string Password)
        {
            byte[] passwordhash, passwordSalt;
            CreatePasswordHash(Password,out passwordhash,out passwordSalt);

            user.Passwordhash = passwordhash;
            user.PasswordSalt = passwordSalt;

            // add user to db

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // return user 
            return user;

        }

        

        public async Task<bool> UserExisits(string username)
        {

            if(await _context.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;

        }

      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.helpers;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class DatingRespository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRespository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            var photo = await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain == true);
            return photo;

        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;

        }
    

        public async Task<User> GetUser(int id)
        {
            // include photos and find the first or default that mathches id 
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if (string.IsNullOrEmpty(userParams.Country) == false ){
                 users = users.Where(u => u.Country.ToLower() == userParams.Country.ToLower());
            }

            if (userParams.MinAge !=18 || userParams.MaxAge !=99) 
            {
                var minDOB = DateTime.Now.AddYears(-userParams.MaxAge -1);
                var maxDOB = DateTime.Now.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDOB && u.DateOfBirth <= maxDOB);
            }

            if ( !string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default: 
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            // if we return more than 0 it returns true.
            return await _context.SaveChangesAsync() > 0;
        }
        private static int CalcMinAge(DateTime BirthDay)
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDay.Year;
            if (BirthDay.Date > today.AddYears(age)) age--;
            return age;


        }
    }
}
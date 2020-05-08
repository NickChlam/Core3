using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PagedList(List<T> items, int count, int pageNum, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNum;
            TotalPages =(int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }
        
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNum, int pageSize)
        {
            var count = await source.CountAsync();
            var items= await source.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNum, pageSize);


        }
    }
}
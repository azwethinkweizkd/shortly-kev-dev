using Microsoft.EntityFrameworkCore;
using Shortly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortly.Data.Services
{
    public class UrlsService(AppDbContext appDbContext): IUrlsService
    {
        private readonly AppDbContext _appDbContext = appDbContext;


        public async Task<Url> AddAsync(Url url)
        {
            await _appDbContext.Urls.AddAsync(url);
            await _appDbContext.SaveChangesAsync();

            return url;
        }
        public async Task<Url> GetByIdAsync(int id)
        {
            var url = await _appDbContext.Urls.FirstOrDefaultAsync(x => x.Id == id);
            return url;
        }

        public async Task<List<Url>> GetUrlsAsync(string userId, bool isAdmin)
        {
            var allUrlsQuery =  _appDbContext.Urls.Include(n => n.User);

            if (isAdmin)
            {
                return await allUrlsQuery.ToListAsync();
            } else
            {
                return await allUrlsQuery.Where(n => n.UserId == userId).ToListAsync();
            }
        }

        public async Task<Url> UpdateAsync(int id, Url url)
        {
            var urlDb = await GetByIdAsync(id);
            if (urlDb != null)
            {
                urlDb.OriginalLink = url.OriginalLink;
                urlDb.ShortLink = url.ShortLink;
                urlDb.DateUpdated = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
            }

            return urlDb;
        }


        public async Task DeleteAsync(int id)
        {
            var urlDb = await GetByIdAsync(id);

            if(urlDb != null)
            {
                _appDbContext.Remove(urlDb);
                await _appDbContext.SaveChangesAsync();
            }
        }

    }
}

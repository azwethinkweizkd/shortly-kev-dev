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


        public Url Add(Url url)
        {
            _appDbContext.Urls.Add(url);
            _appDbContext.SaveChanges();

            return url;
        }
        public Url GetById(int id)
        {
            var url = _appDbContext.Urls.FirstOrDefault(x => x.Id == id);
            return url;
        }

        public List<Url> GetUrls() => [.. _appDbContext.Urls.Include(n => n.User)];

        public Url Update(int id, Url url)
        {
            var urlDb = GetById(id);
            if (urlDb != null)
            {
                urlDb.OriginalLink = url.OriginalLink;
                urlDb.ShortLink = url.ShortLink;
                urlDb.DateUpdated = DateTime.UtcNow;

                _appDbContext.SaveChanges();
            }

            return urlDb;
        }


        public void Delete(int id)
        {
            var urlDb = GetById(id);

            if(urlDb != null)
            {
                _appDbContext.Remove(urlDb);
                _appDbContext.SaveChanges();
            }
        }

    }
}

using Shortly.Data;
using Shortly.Data.Models;

namespace Shortly.Client.Data
{
    public class DbSeed
    {
        public static void SeedDefaultData(IApplicationBuilder appBuild)
        {
            using(var serviceScope = appBuild.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!dbContext.Users.Any())
                {
                    dbContext.Users.AddRange(new User()
                    {
                        FullName = "Kevin Devlin",
                        Email = "kevindevlin11@gmail.com"
                    });
                }

                dbContext.SaveChanges();

                if (!dbContext.Urls.Any()) {
                    dbContext.Urls.Add(new Url()
                    {
                        OriginalLink = "https://www.someoriginallink.com",
                        ShortLink = "dnh",
                        NumOfClicks = 23,
                        DateCreated = DateTime.Now,

                        UserId = dbContext.Users.First().Id
                    });
                }

                dbContext.SaveChanges();
            }
        }
    }
}

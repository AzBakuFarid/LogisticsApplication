using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using LogisticsApp.Models.General;
using LogisticsApp.Models.Page;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LogisticsApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public int CustomerNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Balance { get; set; }
        public string Birthday { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string IDCardNumber { get; set; }
        public string FINNumber { get; set; }

        public virtual ICollection<Message> messages { get; set; }
        public virtual ICollection<Inquery> Inqueries { get; set; }
        public virtual ICollection<Transaction> transactions { get; set; }
        public virtual ICollection<Bundle> bundles { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Bundle> bundles { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<CountryInformation> countryInformations { get; set; }
        public DbSet<Inquery> inqueries { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<MessagType> messageTypes { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<FAQ> forum { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<About> abouts { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<Taariff> taariffs { get; set; }
        public DbSet<Carusel> carusel { get; set; }
        public DbSet<Step> steps { get; set; }
        public DbSet<Market> markets { get; set; }
        public DbSet<Status> statuses { get; set; }
        public DbSet<Valuta> valutas { get; set; }

    }
}
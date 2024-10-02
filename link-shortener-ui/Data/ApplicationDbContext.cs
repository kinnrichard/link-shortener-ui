using link_shortener_ui.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;

namespace link_shortener_ui.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Link> Links { get; set; }
    }
}

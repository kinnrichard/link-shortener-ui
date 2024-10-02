using link_shortener_ui.Data;
using link_shortener_ui.Models;
using link_shortener_ui.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace link_shortener_ui.Repositories.Implementation
{
    public class LinkRepository : Repository<Link>, ILinkRepository
    {
        public LinkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Link> GetExistingUrlByUrlName(string url)
        {
            return await context.Links.FirstOrDefaultAsync(u => u.Url == url);
        }
    }
}

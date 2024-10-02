using link_shortener_ui.Models;

namespace link_shortener_ui.Repositories.Interface
{
    public interface ILinkRepository : IRepository<Link>
    {
        Task<Link> GetExistingUrlByUrlName(string url);
    }
}

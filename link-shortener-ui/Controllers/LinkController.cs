using link_shortener_ui.Models;
using link_shortener_ui.Models.Dto;
using link_shortener_ui.Models.Dto.Request;
using link_shortener_ui.Repositories.Interface;
using link_shortener_ui.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace link_shortener_ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILinkRepository linkRepository;
        private readonly UrlShorteningService urlShorteningService;

        public LinkController(ILinkRepository linkRepository, UrlShorteningService urlShorteningService)
        {
            this.linkRepository = linkRepository;
            this.urlShorteningService = urlShorteningService;
        }

        [HttpPost]
        [Route("CreateShortUrl")]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateLinkRequest request)
        {
            if (!Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format");
            }

            var existingUrl = await linkRepository.GetExistingUrlByUrlName(request.Url);

            if (existingUrl != null)
            {
                return Ok(new { message = "URL already exists", shortenUrl = existingUrl.ShortenUrl });
            }

            var shortUrl = urlShorteningService.CreateShortUrl(request.Url);

            var link = new Link
            {
                Url = request.Url,
                ShortenUrl = shortUrl
            };

            await linkRepository.AddAsync(link);

            return CreatedAtAction(nameof(GetLinkById), new { id = link.Id }, new { shortenUrl = shortUrl });
        }

        [HttpGet]
        [Route("GetLinkById/{id}")]
        public async Task<IActionResult> GetLinkById([FromRoute] int id)
        {
            var link = await linkRepository.GetByIdAsync(id);

            if (link is null)
            {
                return NotFound();
            }

            var response = new LinkDto
            {
                Id = link.Id,
                Url = link.Url,
                ShortenUrl = link.ShortenUrl
            };

            return Ok(response);
        }

        [HttpGet("GetAllLinks")]
        public async Task<IActionResult> GetAllLinks()
        {
            var links = await linkRepository.GetAllAsync();

            if (links is null)
            {
                return NotFound();
            }

            var response = new List<LinkDto>();

            foreach (var link in links)
            {
                response.Add(new LinkDto
                {
                    Id = link.Id,
                    Url= link.Url,
                    ShortenUrl= link.ShortenUrl
                });
            }

            return Ok(response);
        }
    }
}

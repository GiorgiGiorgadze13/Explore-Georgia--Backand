using DiscoverGeorgia.API.Data;
using DiscoverGeorgia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscoverGeorgia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlacesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Places  ან  api/Places?categoryId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces([FromQuery] int? categoryId)
        {
            var query = _context.Places.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }

        // GET: api/Places  ან  api/Places?lang=en  ან  api/Places?categoryId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces([FromQuery] int? categoryId, [FromQuery] string lang = "ka")
        {
            var query = _context.Places.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var places = await query.ToListAsync();

            // თუ ფრონტენდი ითხოვს ინგლისურს (lang=en)
            if (lang.ToLower() == "en")
            {
                foreach (var p in places)
                {
                    if (!string.IsNullOrEmpty(p.NameEn)) p.Name = p.NameEn;
                    if (!string.IsNullOrEmpty(p.DescriptionEn)) p.Description = p.DescriptionEn;
                }
            }

            return Ok(places);
        }
    }
}
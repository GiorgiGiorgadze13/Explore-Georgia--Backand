using DiscoverGeorgia.API.Data;
using DiscoverGeorgia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscoverGeorgia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Favorites/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserFavorites(int userId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .ToListAsync();

            return Ok(favorites);
        }

        // 2. POST: api/Favorites
        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromBody] Favorite favorite)
        {
            var exists = await _context.Favorites
                .AnyAsync(f => f.UserId == favorite.UserId && f.PlaceId == favorite.PlaceId);

            if (exists)
            {
                return BadRequest("ეს ადგილი უკვე ფავორიტებშია!");
            }

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok(new { message = "დაემატა ფავორიტებში!" });
        }

        // 3. DELETE: api/Favorites/1/5
        [HttpDelete("{userId}/{placeId}")]
        public async Task<IActionResult> RemoveFromFavorites(int userId, int placeId)
        {
            var fav = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PlaceId == placeId);

            if (fav == null) return NotFound("ფავორიტი ვერ მოიძებნა!");

            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync();

            return Ok(new { message = "წაიშალა ფავორიტებიდან!" });
        }

        // 4. POST: api/Favorites/sync/1
        [HttpPost("sync/{userId}")]
        public async Task<IActionResult> SyncFavorites(int userId, [FromBody] List<int> placeIds)
        {
            if (placeIds == null || !placeIds.Any())
            {
                return Ok(new { message = "სინქრონიზაციისთვის სია ცარიელია." });
            }

            var existingPlaceIds = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.PlaceId)
                .ToListAsync();

            var newFavorites = placeIds
                .Where(id => !existingPlaceIds.Contains(id))
                .Select(placeId => new Favorite
                {
                    UserId = userId,
                    PlaceId = placeId,
                    CreatedAt = DateTime.UtcNow
                })
                .ToList();

            if (newFavorites.Any())
            {
                _context.Favorites.AddRange(newFavorites);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "ფავორიტები წარმატებით სინქრონიზირდა!", addedCount = newFavorites.Count });
        }
    }
}
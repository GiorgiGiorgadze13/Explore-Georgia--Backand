using System.Globalization;
using DiscoverGeorgia.API.Data;
using DiscoverGeorgia.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiscoverGeorgia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlacesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Places
        [HttpGet]
        public IActionResult GetPlaces()
        {
            var csvPath = Path.Combine(_env.ContentRootPath, "Data", "places.csv");
            if (!System.IO.File.Exists(csvPath))
            {
                return NotFound("places.csv file not found.");
            }

            var lines = System.IO.File.ReadAllLines(csvPath);
            if (lines.Length <= 1) return Ok(new List<object>());

            var header = lines[0].Split(';');
            var result = new List<Dictionary<string, object?>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length < header.Length) continue;

                var dict = new Dictionary<string, object?>();
                for (int j = 0; j < header.Length; j++)
                {
                    var colName = header[j].Trim();
                    var val = parts[j].Trim('"');

                    if (colName == "lat" || colName == "lng" || colName == "coord_x" || colName == "coord_y" || colName == "rating")
                    {
                        if (double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out var num))
                        {
                            dict[colName] = num;
                        }
                        else dict[colName] = 0;
                    }
                    else if (colName == "hidden" || colName == "is_local")
                    {
                        if (bool.TryParse(val, out var b)) dict[colName] = b;
                        else dict[colName] = false;
                    }
                    else
                    {
                        dict[colName] = val;
                    }
                }
                result.Add(dict);
            }

            return Ok(result);
        }
    }
}
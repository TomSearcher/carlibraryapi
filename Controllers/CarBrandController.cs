using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carlibraryapi.Model;


namespace carlibraryapi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class CarBrandController : ControllerBase
    {

        private readonly CarLibraryContext _context;

        public CarBrandController(CarLibraryContext context)
        {
            _context = context;
        }

        // GET: CarBrand
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarBrandOut>>> GetCarBrands()
        {
            return await _context.CarBrands.Select(carBrand => CarBrandOut.fromCarBrand(carBrand)).ToListAsync();
        }

        // GET: CarBrand/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarBrandOut>> GetCarBrand(int id)
        {
            var carBrand = await _context.CarBrands.FindAsync(id);

            if (carBrand == null)
            {
                return NotFound();
            }

            return CarBrandOut.fromCarBrand(carBrand);
        }

        // PUT: CarBrand/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CarBrandOut>> PutCarBrand(int id, CarBrandIn carBrand)
        {
            
            CarBrand brand = await _context.CarBrands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            if (_context.CarBrands.Any(b => b.Id != id && b.Name==carBrand.Name)) {

                return BadRequest("Brand name already exists!");
            }

            brand.Name=carBrand.Name;
         

            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarBrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return CreatedAtAction("GetCarBrand", new { id = brand.Id }, CarBrandOut.fromCarBrand(brand));
        }

        // POST: CarBrand
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarBrandOut>> PostCarBrand(CarBrandIn carBrand)
        {
            if (_context.CarBrands.Any(b => b.Name == carBrand.Name)){
                return BadRequest("Brand name already exists!");
            }

            _context.CarBrands.Add(new CarBrand{Name = carBrand.Name});
            await _context.SaveChangesAsync();

            CarBrand brand = await _context.CarBrands.Where(b => b.Name == carBrand.Name).FirstAsync();

            return CreatedAtAction("GetCarBrand", new { id = brand.Id }, CarBrandOut.fromCarBrand(brand));
        }

        // DELETE: CarBrand/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarBrand(int id)
        {
            var carBrand = await _context.CarBrands.FindAsync(id);
            if (carBrand == null)
            {
                return NotFound();
            }

            _context.CarBrands.Remove(carBrand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarBrandExists(int id)
        {
            return _context.CarBrands.Any(e => e.Id == id);
        }
    }
}

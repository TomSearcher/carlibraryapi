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
    public class CarModelController : ControllerBase
    {
        private readonly CarLibraryContext _context;

        public CarModelController(CarLibraryContext context)
        {
            _context = context;
        }

        // GET: CarModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarModelOut>>> GetCarModels()
        {
            return await _context.CarModels.Include(carModel => carModel.CarBrand).Select(carModel => CarModelOut.fromCarModel(carModel)).ToListAsync();
        }

        // GET: CarModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarModelOut>> GetCarModel(int id)
        {
            var carModel = await _context.CarModels.Where(c => c.Id == id).Include(c => c.CarBrand).Select( c => CarModelOut.fromCarModel(c) ).FirstOrDefaultAsync(); //FindAsync(id);

            if (carModel == null)
            {
                return NotFound();
            }

            return carModel;
        }

        // PUT: CarModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CarModelOut>> PutCarModel(int id, CarModelIn carModel)
        {
            CarModel model = await _context.CarModels.FindAsync(id);
            
            if (model == null) return BadRequest("Car model with id "+id+" does not exist!");
            
            
            CarBrand brand = await _context.CarBrands.FindAsync(carModel.CarBrandId);
            if (brand == null) return NotFound("Brand with id "+carModel.CarBrandId+" does not exist.");

            if (_context.CarModels.Any(c => c.CarBrand.Id==brand.Id && carModel.Name==c.Name)){
                return BadRequest("Car model already exists.");
            }

            model.Name=carModel.Name;
            model.CarBrand=brand;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarModel", new { id = model.Id }, CarModelOut.fromCarModel(model));
        }

        // POST: Model
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarModelOut>> PostCarModel(CarModelIn carModel)
        {
            CarBrand brand = await _context.CarBrands.FindAsync(carModel.CarBrandId);
            if (brand == null) return NotFound("Brand with id "+carModel.CarBrandId+" does not exist.");
            if (_context.CarModels.Any(c => c.CarBrand.Id==brand.Id && carModel.Name==c.Name)){
                return BadRequest("Car model already exists.");
            }
            CarModel model = new CarModel {Name=carModel.Name, CarBrand=brand};

            _context.CarModels.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarModel", new { id = model.Id }, CarModelOut.fromCarModel(model));
        }

        // DELETE: CarModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            var carModel = await _context.CarModels.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }

            _context.CarModels.Remove(carModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarModelExists(int id)
        {
            return _context.CarModels.Any(e => e.Id == id);
        }
    }
}

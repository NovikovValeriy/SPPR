using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253504_Novikov.API.Data;
using WEB_253504_Novikov.API.Services.VehicleServices;
using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "admin")]
    public class VehiclesController : ControllerBase
    {
        //private readonly AppDbContext _context;
        private readonly IVehicleService _service;

        public VehiclesController(IVehicleService service)
        {
            _service = service;
        }

        // GET: api/Vehicles
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles(string? vehicleType, int pageNo = 1, int pageSize = 3)
        {
            //return await _context.Vehicles.ToListAsync();
            var response = await _service.GetProductListAsync(vehicleType, pageNo, pageSize);
            if(response.Data == null) return NotFound(response);
            return Ok(response);
        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            /*var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;*/
            var response = await _service.GetProductByIdAsync(id);
            if(response.Data == null) return NotFound(response);
            return Ok(response);
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            /*if (id != vehicle.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();*/
            var response = await _service.UpdateProductAsync(id, vehicle, null);
            if(response.Data == null) return NotFound();
            return Ok(response);
        }

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle([FromForm]Vehicle vehicle, [FromForm]IFormFile? formFile)
        {
            /*_context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);*/
            var response = await _service.CreateProductAsync(vehicle, formFile);
            if(response.Data == null) return NotFound();
            return CreatedAtAction("PostVehicle", response);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            /*var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();*/
            var response = await _service.DeleteProductAsync(id);
            if(!response.Successfull) return NotFound();
            return Ok(response);
        }

        private async Task<ActionResult<bool>> VehicleExists(int id)
        {
            var result = await _service.GetProductByIdAsync(id);
            if(!result.Successfull) return Ok(false);
            return Ok(true);
        }
    }
}

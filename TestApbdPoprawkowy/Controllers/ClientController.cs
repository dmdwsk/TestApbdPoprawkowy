using Microsoft.AspNetCore.Mvc;
using TestApbdPoprawkowy.Dto;
using TestApbdPoprawkowy.Models;
using TestApbdPoprawkowy.Service;

namespace TestApbdPoprawkowy.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
        private readonly IClientService _service;
        public ClientController(IClientService service) => _service = service;
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var result = await _service.GetClientWithRentalsAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        // [HttpPost]
        // public async Task<IActionResult> AddClientWithRental(CreateClientWithRentalRequestDto request) {
        //     if (request.DateTo <= request.DateFrom)
        //         return BadRequest("Invalid rental dates");
        //     var created = await _service.CreateClientWithRentalAsync(request);
        //     if (!created) return NotFound("Car not found");
        //     return Created("", created);
        // }
}
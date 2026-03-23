using Microsoft.AspNetCore.Mvc;
using PurchaseOrderAPI.Application.Services;

namespace PurchaseOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserDto dto)
        {
            var (result, message) = _service.Create(dto);

            var response = new UserResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                Email = result.Email,
                Role = result.Role.ToString()
            };

            return Ok(new { result = response, message });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var (result, message) = _service.GetAll();

            var response = result.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.ToString()
            }).ToList();

            return Ok(new { result = response, message });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var (result, message) = _service.GetById(id);

            var response = new UserResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                Email = result.Email,
                Role = result.Role.ToString()
            };

            return Ok(new { result = response, message });
        }
    }
}
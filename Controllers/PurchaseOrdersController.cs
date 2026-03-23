using Microsoft.AspNetCore.Mvc;
using PurchaseOrderAPI.Application.DTOs;
using PurchaseOrderAPI.Application.Services;
using PurchaseOrderAPI.Domain.Entities;

namespace PurchaseOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly PurchaseOrderService _service;

        public PurchaseOrdersController(PurchaseOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderDto dto)
        {
            var order = new PurchaseOrder
            {
                Items = dto.Items.Select(i => new PurchaseOrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            var (result, message) = _service.Create(order, dto.UserId);

            var response = PurchaseOrderMapper.ToDto(result);

            return Ok(new { result = response, message });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var (result, message) = _service.GetAll();
            var response = result.Select(PurchaseOrderMapper.ToDto).ToList();

            return Ok(new { result = response, message });
        }

        [HttpPatch("{id}/approve")]
        public IActionResult Approve(int id, [FromQuery] int userId)
        {
            var (result, message) = _service.Approve(id, userId);
            var response = PurchaseOrderMapper.ToDto(result);

            return Ok(new { result = response, message });
        }

        [HttpPatch("{id}/request-review")]
        public IActionResult RequestReview(int id, [FromQuery] int userId)
        {
            var (result, message) = _service.RequestReview(id, userId);
            var response = PurchaseOrderMapper.ToDto(result);

            return Ok(new { result = response, message });
        }

        [HttpPatch("{id}/complete")]
        public IActionResult Complete(int id, [FromQuery] int userId)
        {
            var (result, message) = _service.Complete(id, userId);
            var response = PurchaseOrderMapper.ToDto(result);

            return Ok(new { result = response, message });
        }

        // 🔥 NOVO ENDPOINT
        [HttpPatch("{id}/cancel")]
        public IActionResult Cancel(int id, [FromQuery] int userId)
        {
            var (result, message) = _service.Cancel(id, userId);
            var response = PurchaseOrderMapper.ToDto(result);

            return Ok(new { result = response, message });
        }

        [HttpGet("{id}/history")]
        public IActionResult GetHistory(int id)
        {
            var (result, message) = _service.GetHistory(id);

            var response = result.Select(h => new
            {
                h.Action,
                User = h.User?.Name,
                h.CreatedAt
            });

            return Ok(new { result = response, message });
        }
    }
}
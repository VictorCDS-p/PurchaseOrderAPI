using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create(PurchaseOrder order)
        {
            var result = _service.Create(order);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _service.GetAll();
            return Ok(result);
        }

        [HttpPatch("{id}/approve")]
        public IActionResult Approve(int id)
        {
            var result = _service.Approve(id);
            return Ok(result);
        }

        [HttpPatch("{id}/request-review")]
        public IActionResult RequestReview(int id)
        {
            var result = _service.RequestReview(id);
            return Ok(result);
        }

        [HttpPatch("{id}/complete")]
        public IActionResult Complete(int id)
        {
            var result = _service.Complete(id);
            return Ok(result);
        }

        [HttpGet("{id}/history")]
        public IActionResult GetHistory(int id)
        {
            var result = _service.GetHistory(id);
            return Ok(result);
        }
    }
}
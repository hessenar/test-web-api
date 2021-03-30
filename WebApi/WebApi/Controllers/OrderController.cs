using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderControllerService _orderControllerService;
        public OrderController(OrderControllerService orderControllerService)
        {
            _orderControllerService = orderControllerService;
        }

        [HttpPost("{systemType}")]
        public async Task<ActionResult<Order>> PostOrder(string systemType, [FromBody] OrderData orderData, CancellationToken cancellationToken)
        {
            var res=await _orderControllerService.AddAsync(orderData, systemType, cancellationToken);
            return Ok(res);
        }
    }

}

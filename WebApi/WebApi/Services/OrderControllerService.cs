using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public class OrderControllerService : DBServiceBase
    {
        public OrderControllerService(OrderContext context):base(context)
        {

        }

        public async Task<Order> AddAsync(OrderData orderData,string systemType, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                OrderNumber = long.Parse(orderData.OrderNumber),
                CreatedAt=orderData.CreatedAt,
                SourceOrder=JsonConvert.SerializeObject(orderData),
                SystemType=systemType
            };

            var adder = await _context.Orders.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            entity = adder.Entity;
            return entity;
        }

        
    }
}

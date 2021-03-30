using WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApi.Handlers
{
    public class ZomatoHandler : IHandler
    {
        public async Task<OrderData> StartHandler(OrderData order)
        {
            return await Task.Run(() => ChangeProducts(order));
        }

        private static OrderData ChangeProducts(OrderData order)
        {
            var list = new List<Product>();
            foreach (var product in order.Products)
            {
                var p = product;
                p.PaidPrice = (int.Parse(product.PaidPrice)/int.Parse(product.Quantity)).ToString();
                list.Add(p);
            }

            return new OrderData() { OrderNumber = order.OrderNumber, CreatedAt = order.CreatedAt, Products = list };
        }
    }
}

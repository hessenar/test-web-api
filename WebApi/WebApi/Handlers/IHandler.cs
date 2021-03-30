using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Handlers
{
    public interface IHandler
    {
        Task<OrderData> StartHandler(OrderData order);
    }
}

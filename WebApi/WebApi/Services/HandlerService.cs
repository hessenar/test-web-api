using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Middleware;
using WebApi.Models;

namespace WebApi.Services
{
    public class HandlerService : BackgroundService
    {
        public readonly IServiceProvider _services;

        public HandlerService(IServiceProvider services)
        {
            _services = services;
        }

        private async Task TimerCallBack()
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();

            var orders = await context.Orders
                .Where(x => !x.Processed)
                .ToListAsync();

            foreach (var order in orders)
            {
                try
                {
                    var orderData = await StartHandler(order);
                    order.ConvertedOrder = JsonConvert.SerializeObject(orderData);
                }
                catch (Exception ex)
                {
                    await ErrorHandler(ex);
                }
                finally
                {
                    order.Processed = true;
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task<OrderData> StartHandler(Order order)
        {
            var systemType = order.SystemType;
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.Namespace == typeof(Handlers.IHandler).Namespace)
                .Where(x => x.Name.ToUpper().Contains(systemType.ToUpper())).FirstOrDefault();
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(nameof(Handlers.IHandler.StartHandler));
            return await (Task<OrderData>)(method.Invoke(instance, new object[] { JsonConvert.DeserializeObject<OrderData>(order.SourceOrder) }));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(()=> 
                new Timer(async (e) =>await TimerCallBack(),null, 0, 5000));
        }
        
        public static async Task ErrorHandler(Exception exception)
        {
            Log.Error(exception,exception.Message);
            await Task.Run(()=>Thread.Sleep(10000));
        }
    }
}

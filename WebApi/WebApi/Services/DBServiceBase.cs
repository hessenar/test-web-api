using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class DBServiceBase
    {
        protected readonly OrderContext _context;
        public DBServiceBase(OrderContext context)
        {
            _context = context;
        }
    }
}

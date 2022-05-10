using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IOrderService
    {
        public void Add(Order order);
        public List<Application> Get();
    }
}

using System.Collections.Generic;
using SanTech.Models;

namespace SanTech.Interfaces
{
    public interface IOrderService
    {
        public void Add(Order order);
        public List<Application> Get();
        public void Update(int applicationId, string value);
    }
}

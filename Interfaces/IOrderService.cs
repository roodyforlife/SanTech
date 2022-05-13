using SanTech.Models;
using System.Collections.Generic;

namespace SanTech.Interfaces
{
    public interface IOrderService
    {
        public void Add(Order order);
        public List<Application> Get();
        public void Update(int applicationId, string value);
    }
}

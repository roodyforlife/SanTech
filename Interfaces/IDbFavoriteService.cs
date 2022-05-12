using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbFavoriteService
    {
        public bool Add(string email, int productId);
    }
}

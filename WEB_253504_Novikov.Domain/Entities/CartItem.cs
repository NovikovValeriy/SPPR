using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253504_Novikov.Domain.Entities
{
    public class CartItem
    {
        public Vehicle Vehicle { get; set; }
        public int Quantity { get; set; }
    }
}

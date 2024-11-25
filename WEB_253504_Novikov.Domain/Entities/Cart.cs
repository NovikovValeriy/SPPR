using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253504_Novikov.Domain.Entities
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        public virtual void AddToCart(Vehicle vehicle)
        {
            if (CartItems.ContainsKey(vehicle.Id))
            {
                CartItems[vehicle.Id].Quantity++;
            }
            else
            {
                CartItems[vehicle.Id] = new CartItem
                {
                    Vehicle = vehicle,
                    Quantity = 1
                };
            }
        }

        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems.Remove(id);
            }
        }
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Quantity); }

        public double TotalCost
        {
            get => CartItems.Sum(item => item.Value.Vehicle.Cost * item.Value.Quantity);
        }
    }
}

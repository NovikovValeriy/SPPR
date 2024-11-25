using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.UI.Extensions;

namespace WEB_253504_Novikov.UI.Services.CartService
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        private IHttpContextAccessor? _httpContextAccessor;

        [JsonIgnore]
        private ISession Session => _httpContextAccessor?.HttpContext?.Session ?? throw new InvalidOperationException("Session is not available.");

        public SessionCart()
        {
        }

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void AddToCart(Vehicle vehicle)
        {
            base.AddToCart(vehicle);
            SaveChanges();
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            SaveChanges();
        }

        public override void ClearAll()
        {
            base.ClearAll();
            SaveChanges();
        }

        private void SaveChanges()
        {
            Session?.Set("cart", this);
        }

        public static Cart GetCart(IServiceProvider services)
        {
            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            var session = httpContext.Session;

            var cart = session.Get<SessionCart>("cart") ?? new SessionCart();
            cart._httpContextAccessor = httpContextAccessor;
            return cart;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using SimpleStore.Models;

namespace SimpleStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly MyStoreContext _context;
        public OrderController(MyStoreContext myStoreContext)
        {
            _context = myStoreContext;
        }
        public IActionResult OrderDetail(int id)
        {
            List<OrderDetail> ods = (from OrderDetail o in _context.OrderDetails
                                   where o.OrderId == id
                                   select o).ToList();
            foreach (OrderDetail o in ods)
            {
                o.Product = _context.Products.FirstOrDefault(p => p.Id == o.ProductId);
            }
            return View(ods);
        }
        public IActionResult Index()
        {
            List<Order> orders = (from Order o in _context.Orders
                                 where o.User.Username == HttpContext.User.Identity.Name
                                 orderby o.DreatedDate descending
                                 select o).ToList();
            return View(orders);
        }
        public IActionResult Checkout()
        {
            var user = _context.Users.FirstOrDefault(u => u.Username.Equals(HttpContext.User.Identity.Name));
            Cart cart;
            if (HttpContext.Session.GetString("CART") != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("CART"));
            }
            else
            {
                cart = new Cart();
            }
            if (cart.shoppingCart.Count == 0)
            {
                TempData["EmptyCart"] = "Cart is empty";
                return RedirectToAction("Index", "Cart");
            }
            decimal totalOrder = 0;
            List<CartItemVM> cartItems = new List<CartItemVM>();
            foreach (int key in cart.shoppingCart.Keys)
            {
                Product p = _context.Products.FirstOrDefault(i => i.Id == key);
                if (p != null)
                {
                 
                    cartItems.Add(new CartItemVM
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Quantity = cart.shoppingCart[key],
                        Total = p.Price * cart.shoppingCart[key]
                    });
                    totalOrder += p.Price * cart.shoppingCart[key];
                }
            }
            Order order = new Order
            {
                User = user,
                UserId = user.Id,
                Total = totalOrder
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            foreach (var item in cartItems)
            {
                Product p = _context.Products.FirstOrDefault(i => i.Id == item.Id);
                if (p.Quantity < item.Quantity || item.Quantity < 1)
                {
                    TempData["Cart"] = "Out of stock, try again";
                    cart.shoppingCart[item.Id] = p.Quantity;
                    return RedirectToAction("Index","Cart");
                }
                p.Quantity -= item.Quantity;
                _context.Products.Update(p);
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.Id,
                    Product = p,
                    Order = order
                };
                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();
            HttpContext.Session.Remove("CART");
            return RedirectToAction("Index","Order");
        }
    }
}

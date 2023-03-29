using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleStore.Models;

namespace SimpleStore.Controllers
{
    public class CartController : Controller
    {
        private readonly MyStoreContext _context;
        public CartController(MyStoreContext myStoreContext)
        {
            _context = myStoreContext;
        }
        public IActionResult Add(int id)
        {
            Cart cart;
            if (HttpContext.Session.GetString("CART") != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("CART"));
            }
            else
            {
                cart = new Cart();
            }
            Product p = _context.Products.FirstOrDefault(x => x.Id == id);
            if (p != null)
            {
                cart.AddToCart(id, 1, p.Quantity);
                HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Update(int id, int quan)
        {
            Cart cart;
            if (HttpContext.Session.GetString("CART") != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("CART"));
            }
            else
            {
                cart = new Cart();
            }
            Product p = _context.Products.FirstOrDefault(x => x.Id == id);
            if (p != null)
            {
                cart.AddToCart(id, quan, p.Quantity);
                HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            Cart cart;
            if (HttpContext.Session.GetString("CART") != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("CART"));
            }
            else
            {
                cart = new Cart();
            }
            cart.RemoveFromCart(id);
            HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Index()
        {
            Cart cart;
            if (HttpContext.Session.GetString("CART") != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("CART"));
            }
            else
            {
                cart = new Cart();
            }
            List<CartItemVM> cartItems = new List<CartItemVM>();
            foreach(int key in cart.shoppingCart.Keys)
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
                }
            }
            return View(cartItems);
        }
    }
}

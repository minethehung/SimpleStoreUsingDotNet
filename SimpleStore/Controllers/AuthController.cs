using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleStore.Models;
using System.Security.Claims;

namespace SimpleStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly MyStoreContext _context;
        public AuthController(MyStoreContext context)
        {   
            _context = context;
        }
        [Authorize]
        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Update(UpdatePasswordVM vm)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (user.Password != vm.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Wrong password");
                return View(vm);
            }
            if (vm.NewPassword != vm.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Confirm does not match");
            }
            if (ModelState.IsValid)
            {
                user.Password = vm.NewPassword;
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return View(vm);
        }
        public IActionResult Login()
        {
            var user = HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username.Equals(loginVM.Username) && u.Password.Equals(loginVM.Password));
                if (user == null)
                {
                    ModelState.AddModelError("username", "Wrong username or password");
                    return View(loginVM);
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }
            return View(loginVM);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);          
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var tmpUser = _context.Users.FirstOrDefault(u => u.Username.Equals(loginVM.Username));
                if (tmpUser != null)
                {
                    ModelState.AddModelError("username", "Username already exist");
                    return View(loginVM);
                }
                User user = new User();
                user.Username = loginVM.Username;
                user.Password = loginVM.Password;
                user.Role = "USER";
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "Auth");
            }
            return View(loginVM);
        }
    }
}

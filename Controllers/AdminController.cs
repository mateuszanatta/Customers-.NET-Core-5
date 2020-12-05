using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Customers.Data;
using Customers.Models;
using Customers.Models.Views;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Customers.Controllers
{
    public class AdminController : Controller
    {
        private readonly CustomersContext _context;
        public AdminController(CustomersContext context)
        {
            _context = context;
        }

        // GET: Customer
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("isLogged") > 0){
                return RedirectToAction("Index", "Customer");
            }

            if(TempData["error"] != null && TempData["error"].Equals(true)){
                ViewBag.error    = TempData["error"];
                ViewBag.errorMsg = TempData["errorMsg"];
            }
            return View();
        }
        public IActionResult Logout(){
            HttpContext.Session.SetInt32("isLogged", 0);
            HttpContext.Session.SetString("authUser", "");
            
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult Login(string Email, string Password){
            try{
                UserSys signinUser = _context.UserSys.Where<UserSys>(user => user.Email == Email && user.Password == Password).SingleOrDefault();
            
                if(!Object.Equals(signinUser, null)){
                    HttpContext.Session.SetInt32("isLogged", 1);
                    
                    UserSysView userView = new UserSysView(signinUser.Id, signinUser.Login, signinUser.Email, signinUser.UserRole);

                    HttpContext.Session.SetString("authUser", JsonSerializer.Serialize<UserSysView>(userView));
                    
                    return RedirectToAction("Index", "Customer");
                }else{
                    HttpContext.Session.SetInt32("isLogged", 0);
                    HttpContext.Session.SetString("authUser", "");

                    TempData["error"] = true;
                    TempData["errorMsg"] = "User not found";
                    return RedirectToAction("Index", "Admin");
                }
            }catch(Exception ex){
                TempData["error"] = true;
                HttpContext.Session.SetString("authUser", "");

                TempData["errorMsg"] = "Connection Timed Out! Try again!";
                return RedirectToAction("Index", "Admin");
            }
            
            
        }
    }
}
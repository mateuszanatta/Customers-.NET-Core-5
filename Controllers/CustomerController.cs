using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Customers.Data;
using Customers.Models;
using Customers.Models.Views;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq.Expressions;
using Customers.Helpers;


namespace Customers.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomersContext _context;

        public CustomerController(CustomersContext context)
        {
            _context = context;
        }

        private void DropDownGender(){
            IList<Gender> gender = new List<Gender>();

            gender = (from g in _context.Gender select g).ToList();

            gender.Insert(0, new Gender{Id = 0, Name = "-- Select ---"});

            ViewBag.gender = gender;
        }

        private void DropDownCity(){
            IList<City> city = new List<City>();

            city = (from g in _context.City select g).ToList();

            city.Insert(0, new City{Id = 0, Name = "-- Select ---", Region = new Region()});

            ViewBag.city = city;
        }

        private void DropDownRegion(){
            IList<Region> region = new List<Region>();

            region = (from g in _context.Region select g).ToList();

            region.Insert(0, new Region{Id = 0, Name = "-- Select ---"});

            ViewBag.region = region;
        }
        private void DropDownClassification(){
            IList<Classification> classification = new List<Classification>();

            classification = (from g in _context.Classification select g).ToList();

            classification.Insert(0, new Classification{Id = 0, Name = "-- Select ---"});

            ViewBag.classification = classification;
        }
        private void DropDownUserSys(){
            IList<UserSysView> seller = new List<UserSysView>();
            
            //Only get sellers
            seller = new UserSysView().UserSysViewList((from g in _context.UserSys where !g.UserRole.isAdmin select g).ToList());
            seller.Insert(0, new UserSysView(0,"-- Select ---","",new UserRole()));

            ViewBag.seller = seller;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string startdate = null, string enddate = null, 
                                                string SearchName = "", int SearchGender = 0, 
                                                int SearchCity = 0, int SearchRegion = 0,
                                                int SearchClassification = 0, int SearchSeller = 0)
        {
            

            DateTime start = new DateTime(1900, 01, 01);
            DateTime end = new DateTime(9999, 12, 31);
        
            if (startdate != null && enddate != null)
            {
                start = DateManager.GetDate(startdate) ?? DateTime.Now;
                end = DateManager.GetDate(enddate) ?? DateTime.Now;
            }

            #region FILL DROPDOWN LISTS
            DropDownGender();
            DropDownCity();
            DropDownRegion();
            DropDownClassification();
            DropDownUserSys();
            #endregion

            UserSysView LoggedUser = verifyLoggedUser();
            SearchName = SearchName == null ? "" : SearchName;
            ViewData["CurrentFilter"] = SearchName;
            ViewData["CurrentStartDate"] = startdate;
            ViewData["CurrentEndDate"] = enddate;

            #region VERIFY LOGGED USER
            if(object.Equals(LoggedUser, null)){
                return RedirectToAction("Index", "Admin");
            }
            
            ViewBag.authUser = LoggedUser;
            #endregion
            
            IList<Customer> customer = new List<Customer>();
            
            try{

                if(LoggedUser.UserRole.isAdmin){
                    customer = await _context.Customer
                        .Where(x => (x.Name.ToLower().Contains(SearchName.ToLower()) || SearchName == "")
                                        && (x.Gender.Id == SearchGender || SearchGender == 0)
                                        && (x.City.Id == SearchCity || SearchCity == 0)
                                        && (x.Region.Id == SearchRegion || SearchRegion == 0)
                                        && (x.Classification.Id == SearchClassification || SearchClassification == 0)
                                        && (x.User.Id == SearchSeller || SearchSeller == 0)
                                        && (x.LastPurchase >= start && x.LastPurchase <= end)
                                        )
                        .ToListAsync();
                }else{
                    customer = await _context.Customer
                        .Where(x => x.User.Id == LoggedUser.Id
                                && (x.Name.ToLower().Contains(SearchName.ToLower()) || SearchName == "")
                                && (x.Gender.Id == SearchGender || SearchGender == 0)
                                && (x.Region.Id == SearchRegion || SearchRegion == 0)
                                && (x.Classification.Id == SearchClassification || SearchClassification == 0)
                                && (x.User.Id == SearchSeller || SearchSeller == 0)
                                && (x.LastPurchase >= start && x.LastPurchase <= end)
                                )
                        .ToListAsync();
                }
                ViewBag.LoggedUser = LoggedUser;
                return View(customer);
            }catch(Exception){
                TempData["error"] = true;
                TempData["errorMsg"] = "Occured an error while loading customers! Please, try again!";
                return RedirectToAction("Index", "Admin");
            }
            
        }
        
        private UserSysView verifyLoggedUser(){
            UserSysView LoggedUser;
            string autUser = HttpContext.Session.GetString("authUser");

            if(HttpContext.Session.GetInt32("isLogged")  == 0){
                return null;
            }

            if(TempData["error"] != null && TempData["error"].Equals(true)){
                ViewBag.error    = TempData["error"];
                ViewBag.errorMsg = TempData["errorMsg"];
                return null;
            }
            
            if(Object.Equals(autUser, null)){
                return null;   
            }
            LoggedUser = JsonSerializer.Deserialize<UserSysView>(autUser);

            return LoggedUser;
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,LastPurchase")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,LastPurchase")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}

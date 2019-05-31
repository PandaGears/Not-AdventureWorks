using System.Linq;
using System.Web.Mvc;
using WTCPortal.Models;
using WTCPortal.ViewModel;
using WTCPortal.Repository;
using System.Data;
using System.Data.Entity.Validation;
using System;
using System.Collections.Generic;

namespace WTCPortal.Controllers
{
    public class VendorController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        // GET: Vendor
        public ActionResult Index(string searchString)
        {
            VendorPageViewModel pageViewModel = new VendorPageViewModel(this);
            var vendors = pageViewModel.Build();

            var vendorList = vendors;

            if (!String.IsNullOrEmpty(searchString))
            {
                vendors = vendorList.Where(x => x.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return View(vendors);
        }

        public ViewResult Details(int BusinessEntityID)
        {
            Vendor vendor = new Vendor();
            VendorPageViewModel pageViewModel = new VendorPageViewModel(this);

            vendor = pageViewModel.SingleVendor(BusinessEntityID);
            return View(vendor);
        }

        public ActionResult Create()
        {
            var vendor = new Vendor
            {
                CredRate = CredRate(),
                AddressIding = AddressIding(),
            };
            return View(vendor);
        }
        
        [HttpPost]
        public ActionResult Create(Vendor vendor)
        {
            VendorPageViewModel pageViewModel = new VendorPageViewModel(this);
            vendor.CredRate = CredRate();
            vendor.AddressIding = AddressIding();
            try
            {
                if (ModelState.IsValid)
                {
                    pageViewModel.InsertVendor(vendor);
                    unit.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save to db");
                    return View(vendor);
                }

            }
            catch (DataException)
             { 
                     ModelState.AddModelError("", "Unable to save changes.Contact System administrator");
             }
             return View(vendor);
        }

        public ActionResult Edit(int BusinessEntityID)
        {
            Vendor vendor = new Vendor();
            VendorPageViewModel pageViewModel = new VendorPageViewModel(this);

            vendor = pageViewModel.SingleVendor(BusinessEntityID);
            vendor.CredRate = CredRate();
            vendor.AddressIding = AddressIding();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendor vendor)
        {
            VendorPageViewModel pageViewModel = new VendorPageViewModel(this);
            vendor.CredRate = CredRate();
            vendor.AddressIding = AddressIding();
            try
            {
                if (ModelState.IsValid)
                {
                    pageViewModel.UpdateVendor(vendor);
                    unit.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save to db");
                    return View(vendor);
                }

            }
            catch (DbEntityValidationException e)
            {
                var r = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorText = string.Join(";", r);
                var exeptionText = string.Concat(e.Message, "Your Errors are: ", fullErrorText);

                ModelState.AddModelError("", exeptionText);
            }
            return View(vendor);
        }

        protected override void Dispose(bool disposing)
        {
            unit.Save();
            unit.Dispose();
            base.Dispose(disposing);
        }

        private Dictionary<int, string> CredRate()
        {
            return new Dictionary<int, string>()
            {
                { 1, "Superior" },
                { 2, "Excellent"},
                { 3, "Above Average"},
                { 4, "Average"},
                { 5, "Below Average"},
                { 6, "Poor"}
            };
        }
        private Dictionary<int, string> AddressIding()
        {
            return new Dictionary<int, string>()
            {
                { 1, "Billing" },
                { 2, "Home"},
                { 3, "Main Office"},
                { 4, "Primary"},
                { 5, "Shipping"},
                { 6, "Archive"}
            };
        }
    }
}
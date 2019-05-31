using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTCPortal.Models;
using WTCPortal.ViewModel;
using WTCPortal.Repository;
using System.Data;

namespace WTCPortal.Controllers
{
    public class PersonController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        // GET: Person
        public ActionResult Index()
        {
            PersonPageViewModel PageViewModel = new PersonPageViewModel(this);
            var people = PageViewModel.Build();
            return View(people);
        }

        public ViewResult Details(int BusinessEntityID)
        {
            Person person = new Person();
            PersonPageViewModel PageViewModel = new PersonPageViewModel(this);

            person = PageViewModel.SinglePerson(BusinessEntityID);
            return View(person);

        }

        public ActionResult Create()
        {
            return View(new Person());
        }

        [HttpPost]
        public ActionResult Create(Person p)
        {
            PersonPageViewModel PageViewModel = new PersonPageViewModel(this);
            try
            {
                if (ModelState.IsValid)
                {
                    PageViewModel.InsertPerson(p);
                    unit.Save();
                }
                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.Contact System administrator");
            }
            return View(p);
        }

        public ActionResult Edit(int BusinessEntityID)
        {
            Person person = new Person();
            PersonPageViewModel pageViewModel = new PersonPageViewModel(this);

            person = pageViewModel.SinglePerson(BusinessEntityID);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            PersonPageViewModel pageViewModel = new PersonPageViewModel(this);
            try
            {
                if (ModelState.IsValid)
                {
                    pageViewModel.UpdatePerson(person);
                    unit.Save();
                }
                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.Contact System administrator");
            }
            return View(person);
        }

        protected override void Dispose(bool disposing)
        {
            unit.Save();
            unit.Dispose();
            base.Dispose(disposing);
        }

    }
    
}
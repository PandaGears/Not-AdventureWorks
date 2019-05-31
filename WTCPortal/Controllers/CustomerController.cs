using System.Web.Mvc;
using WTCPortal.Models;
using WTCPortal.ViewModel;
using WTCPortal.Repository;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WTCPortal.Controllers
{
    public class CustomerController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();
        // GET: Customer
        public ActionResult Index(string searchString)
        {
           CustomerPageViewModel pageViewModel = new CustomerPageViewModel(this);
            var people = pageViewModel.Build();
            var personList = people;

            if (!String.IsNullOrEmpty(searchString))
            {
                people = personList.Where(x => x.FirstName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || x.LastName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return View(people);
        }

        public ViewResult Details(int BusinessEntityID)
        {
            IndividualCustomer person = new IndividualCustomer();
            CustomerPageViewModel pageViewModel = new CustomerPageViewModel(this);
            person = pageViewModel.SingleCustomer(BusinessEntityID);
            return View(person);
        }

        public ActionResult Create()
        {
            var titles = GetTitles();
            var customer = new IndividualCustomer
            {
                GetCards = GetCards(),
                GetState = GetState(),
                AddressIding = AddressIding(),
                Emailnotifications = Emailnotifictions(),
                Phonetype = Phonetype(),
                TitleList = GetSelectListItems(titles)
            };
            return View(customer);
        }

        [HttpPost]
        public ActionResult Create(IndividualCustomer person)
        {
            CustomerPageViewModel pageViewModel = new CustomerPageViewModel(this);
            IEnumerable<string> titles = GetTitles();
            IEnumerable<string> genders = GetGenders();
            person.TitleList = GetSelectListItems(titles);
            person.GenderList = GetSelectListItems(genders);
            person.GetCards = GetCards();
            person.GetState = GetState();
            person.AddressIding = AddressIding();
            person.Emailnotifications = Emailnotifictions();
            person.Phonetype = Phonetype();
            try
            {
                if (ModelState.IsValid)
                {
                    pageViewModel.InsertCustomer(person);
                    unit.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save to db");
                    return View(person);
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return View(person);
        }

        public ActionResult Edit(int BusinessEntityID)
        {
            IndividualCustomer person = new IndividualCustomer();
            CustomerPageViewModel pageViewModel = new CustomerPageViewModel(this);
            IEnumerable<string> titles = GetTitles();
            IEnumerable<string> genders = GetGenders();
            person.TitleList = GetSelectListItems(titles);
            person.GenderList = GetSelectListItems(genders);
            person.GetCards = GetCards();
            person.GetState = GetState();
            person.AddressIding = AddressIding();
            person.Emailnotifications = Emailnotifictions();
            person.Phonetype = Phonetype();
            person = pageViewModel.SingleCustomer(BusinessEntityID);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IndividualCustomer person)
        {
            CustomerPageViewModel pageViewModel = new CustomerPageViewModel(this);
            IEnumerable<string> titles = GetTitles();
            IEnumerable<string> genders = GetGenders();
            person.TitleList = GetSelectListItems(titles);
            person.GenderList = GetSelectListItems(genders);
            person.GetCards = GetCards();
            person.GetState = GetState();
            person.AddressIding = AddressIding();
            person.Emailnotifications = Emailnotifictions();
            person.Phonetype = Phonetype();
            try
            {
                if (ModelState.IsValid)
                {
                    pageViewModel.UpdateCustomer(person);
                    unit.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save to db");
                    return View(person);
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
            return View(person);
        }

        protected override void Dispose(bool disposing)
        {
            unit.Save();
            unit.Dispose();
            base.Dispose(disposing);
        }

        private Dictionary<int, string> Emailnotifictions()
        {
            return new Dictionary<int, string>()
            {
                {0, "No Email Promotions" },
                {1, "Recieve from AdventureWorks"},
                {2, "Recieve from AdventureWorsk and selected partners" }
            };
        }



        private IEnumerable<string> GetPromotion()
        {
            return new List<string>
            {
                "0", "1", "2"
            };
        }
        private IEnumerable<string> GetPhone()
        {
            return new List<string>
            {
                "1", "2", "3"
            };
        }

        private IEnumerable<string> GetTitles()
        {
            return new List<string>
            {
                "Mr", "Mrs", "Ms", "Dr",
            };
        }
        private IEnumerable<string> GetMaritals()
        {
            return new List<string>
            {
                "Single", "Married"
            };
        }
        private IEnumerable<string> GetGenders()
        {
            return new List<string>
            {
                "M", "F"
            };
        }

        private Dictionary<int, string> GetCards()
        {
            return new Dictionary<int, string>()
            {
                { 1, "SuperiorCard" },
                { 2, "Vista"},
                { 3, "ColonialVoice"},
                { 4, "Distinguish"}
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
        private Dictionary<int, string> GetState()
        {
            return new Dictionary<int, string>()
            {
                {0, "Choose State" },
                {1, "Alberta" },
                {2, "Alaska" },
                {3, "Alabama" },
                {4, "Arkansas" },
                {5, "American Samoa" },
                {6, "Arizona" },
                {7, "British Columbia" },
                {8, "Bayern" },
                {9, "California" },
                {10, "Colorado" },
                {11, "Connecticut" },
                {12, "District of Columbia" },
                {13, "Delaware" },
                {14, "England" },
                {15, "Florida" },
                {16, "Micronesia" },
                {17, "Georgia" },
                {18, "Guam" },
                {19, "Hessen" },
                {20, "Hamburg" },
                {21, "Hawaii" },
                {22, "Iowa" },
                {23, "Idaho" },
                {24, "Illinois" },
                {25, "Indiana" },
                {26, "Kansas" },
                {27, "Kentucky" },
                {28, "Louisiana" },
                {29, "Labrador" },
                {30, "Massachusetts" },
                {31, "Manitoba" },
                {32, "Maryland" },
                {33, "Maine" },
                {34, "Marshall Islands" },
                {35, "Michigan" },
                {36, "Minnesota" },
                {37, "Missouri" },
                {38, "Northern Mariana Islands" },
                {39, "Mississippi" },
                {40, "Montana" },
                {41, "Brunswick" },
                {42, "North Carolina" },
                {43, "North Dakota" },
                {44, "Nebraska" },
                {45, "Newfoundland" },
                {46, "New Hampshire" },
                {47, "New Jersey" },
                {48, "New Mexico" },
                {49, "Nova Scotia" },
                {50, "New South Wales" },
                {51, "Northwest Territories" },
                {52, "Nevada" },
                {53, "Nordrhein-Westfalen" },
                {54, "New York" },
                {55, "Ohio" },
                {56, "Oklahoma" },
                {57, "Ontario" },
                {58, "Oregon" },
                {59, "Pennsylvania" },
                {60, "Prince Edward Island" },
                {61, "Puerto Rico" },
                {62, "Palau" },
                {63, "Quebec" },
                {64, "Queensland" },
                {65, "Rhode Island" },
                {66, "South Australia" },
                {67, "South Carolina" },
                {68, "South Dakota" },
                {69, "Saskatchewan" },
                {70, "Saarland" },
                {71, "Tasmania" },
                {72, "Tennessee" },
                {73, "Texas" },
                {74, "Utah" },
                {75, "Virginia" },
                {76, "Virgin Islands" },
                {77, "Victoria" },
                {78, "Vermont" },
                {79, "Washington" },
                {80, "Wisconsin" },
                {81, "West Virginia" },
                {82, "Wyoming" },
                {83, "Yukon Territory" },
                {84, "France" },
                {85, "Brandenburg" },
                {86, "Saxony" },
                {87, "Ain" },
                {88, "Aisne" },
                {89, "Allier" },
                {90, "Alpes-de-Haute Provence" },
                {91, "Alpes (Haute)" },
                {92, "Alpes-Maritimes" },
                {93, "Ardèche" },
                {94, "Ardennes" },
                {95, "Ariège" },
                {96, "Aube" },
                {97, "Aude" },
                {98, "Aveyron" },
                {99, "Bouches du Rhône" },
                {100, "Calvados" },
                {101, "Cantal" },
                {102, "Charente" },
                {103, "Charente-Maritime" },
                {104, "Cher" },
                {105, "Corrèze" },
                {106, "Corse" },
                {107, "Côte d'Or" },
                {108, "Côtes-d'Armor" },
                {109, "Creuse" },
                {110, "Dordogne" },
                {111, "Toubs" },
                {112, "Drôme" },
                {113, "Eure" },
                {114, "Eure et Loir" },
                {115, "Finistère" },
                {116, "Gard" },
                {117, "Garonne (Haute)" },
                {118, "Gers" },
                {119, "Gironde" },
                {120, "Hérault" },
                {121, "Ille et Vilaine" },
                {122, "Indre" },
                {123, "Indre et Loire" },
                {124, "Isère" },
                {125, "Jura" },
                {126, "Landes" },
                {127, "Loir et Cher" },
                {128, "Loire" },
                {129, "Loire (Haute)" },
                {130, "Loire Atlantique" },
                {131, "Loiret" },
                {132, "Lot" },
                {133, "Lot et Garonne" },
                {134, "Lozère" },
                {135, "Maine et Loire" },
                {136, "Manche" },
                {137, "Marne" },
                {138, "Marne (Haute)" },
                {139, "Mayenne" },
                {140, "Meurthe et Moselle" },
                {141, "Meuse" },
                {142, "Morbihan" },
                {143, "Moselle" },
                {144, "Nièvre" },
                {145, "Nord" },
                {146, "Oise" },
                {147, "Orne" },
                {148, "Pas de Calais" },
                {149, "Puy de Dôme" },
                {150, "Pyrénées Atlantiques" },
                {151, "Pyrénées (Hautes)" },
                {152, "Pyrénées Orientales" },
                {153, "Rhin (Bas)" },
                {154, "Rhin (Haut)" },
                {155, "Rhône" },
                {156, "Saône (Haute)" },
                {157, "Saône et Loire" },
                {158, "Sarthe" },
                {159, "Savoie" },
                {160, "Savoie Haute" },
                {161, "Seine (Paris)" },
                {162, "Seine Maritime" },
                {163, "Seine et Marne" },
                {164, "Yveline" },
                {165, "Sèvres (Deux)" },
                {166, "Somme" },
                {167, "Tarne" },
                {168, "Tarne et Garonne" },
                {169, "Var" },
                {170, "Vaucluse" },
                {171, "La Vendée" },
                {172, "Vienne" },
                {173, "Vienne (Haute)" },
                {174, "Vosges" },
                {175, "Yonne" },
                {176, "Belford (Territoire de)" },
                {177, "Essonne" },
                {178, "Hauts de Seine" },
                {179, "Seine Saint Denis" },
                {180, "Val de Marne" },
                {181, "Val d'Oise" }
            };
        }
        private Dictionary<int, string> Phonetype()
        {
            return new Dictionary<int, string>()
            {
                {1, "Cell"},
                {2, "Home"},
                {3, "Work" }
            };
        }
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }
            return selectList;
        }
    }
}
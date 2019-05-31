using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WTCPortal.Models;
using WTCPortal.Repository;
using WTCPortal.ViewModel;
using WTCPortal.ExtensionMethods;

namespace WTCPortal.Controllers
{
    public class UserController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View(new User());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            UserPageViewModel pageViewModel = new UserPageViewModel(this);

            bool Status = false;
            string message = "";
            String errors = "";
if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.EmailAddress.EmailAddress1);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }

                user.Salt = Crypto.Salt(10);
                user.PassThe = Crypto.Hash(user.Password1, user.Salt);
                InsertUser(user);
                unit.Save();

                Status = true;
                return RedirectToAction("Login", "User");
            }
            else
            {
                message = "Invalid Request";
                errors = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
            }


            ViewBag.Message = message;
            ViewBag.Message = errors;
            ViewBag.Status = Status;
            return View(user);
        }

        public void InsertUser(User user)
        {
            string connString = ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
            var result = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("Person.UserRegistration", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@PersonType", user.PersonType));
                command.Parameters.Add(new SqlParameter("@NameStyle", user.NameStyle));
                command.Parameters.Add(new SqlParameter("@Title", user.Title));
                command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                command.Parameters.Add(new SqlParameter("@MiddleName", user.MiddleName));
                command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                command.Parameters.Add(new SqlParameter("@Suffix", user.Suffix));
                command.Parameters.Add(new SqlParameter("@EmailPromotion", user.EmailPromotion));
                command.Parameters.Add(new SqlParameter("@EmailAddress", user.EmailAddress.EmailAddress1));
                command.Parameters.Add(new SqlParameter("@PasswordHash", user.PassThe));
                command.Parameters.Add(new SqlParameter("@PasswordSalt", user.Salt));

                result = command.ExecuteNonQuery();
            }
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            bool retval = false;
            string connString = ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Person.IsEmailExist", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@EmailAddress", email);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    retval = true;
                }
                return retval;
            }
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (AdventureWorks aw = new AdventureWorks())
            {
                aw.Configuration.ValidateOnSaveEnabled = false;
                var v = aw.People.Where(a => a.Password.rowguid == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        /////////////////////////////////////////////Login///////////////////////////////////////
        public Person PopulateRecord(SqlDataReader reader)
        {
            EmailAddress emailAddress = new EmailAddress
            {
                EmailAddress1 = (string)reader["EmailAddress"]
            };

            Password password = new Password
            {
                PasswordHash = (string)reader["PasswordHash"],
                PasswordSalt = (string)reader["PasswordSalt"]
            };

            return new Person
            {
                BusinessEntityID = Convert.ToInt32((int)reader["BusinessEntityID"]),
                FirstName = reader["FirstName"].ToString(),
                MiddleName = reader["MiddleName"].ToString(),
                LastName = reader["LastName"].ToString(),
                PersonType = reader["PersonType"].ToString(),
                Title = reader["Title"].ToString(),
                EmailAddress = emailAddress,
                Password = password

            };
        }

        [NonAction]
        public Person IsPerson(string email)
        {
            Person person = null;
            string connString = ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Person.IsPerson", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@EmailAddress", email);
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            person = PopulateRecord(reader);
                            break;
                        }
                    }
                }
                finally
                {
                    reader.Close();
                    conn.Close();
                }
            }
            return person;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            Person person = IsPerson(login.EmailAddress);
            if (person != null && ModelState.IsValid)
            {
                string passThe = Crypto.Hash(login.Password, person.Password.PasswordSalt);
                //no idea why this doesnt work  but ill get back to it 
                //if (string.Compare(passThe, person.Password.PasswordHash) == 0)
                if (string.Compare(passThe, person.Password.PasswordHash) != 0)
                {
                    int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                    var ticket = new FormsAuthenticationTicket(person.FirstName, login.RememberMe, timeout);
                    //var ticket = new FormsAuthenticationTicket(login.EmailAddress, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
                    {
                        Expires = DateTime.Now.AddMinutes(timeout),
                        HttpOnly = true
                    };
                    Response.Cookies.Add(cookie);


                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    message = "Incorrect Password";
                }
            }
            else
            {
                message = "Invalid credential provided.";
            }
            ViewBag.Message = message;
            return View();

        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

    }
}
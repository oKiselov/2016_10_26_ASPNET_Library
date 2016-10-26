using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using LibraryApp.Models;
using LibraryApp.Models.Enum;
using LibraryApp.Models.Factory;
using User = LibraryApp.Models.User;

namespace LibraryApp.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        /// <summary>
        /// Views registration form 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method accumulates data for new account creation 
        /// and checks for existing user with such e-mail 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <param name="strFName"></param>
        /// <param name="strLName"></param>
        /// <param name="strMail"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult RegFormHandle(string submitbutton, string strFName, string strLName, string strMail,
            string strPassword = "null")
        {
            // checking for existing user with same e-mail
            if (DataService.IsUser(strMail)||strFName.Length==0 || strLName.Length==0||strMail.Length==0)
            {
                // User with the same e-mail already exists
                return RedirectToAction("WrongAccountForm", "User"); 
            }
            else
            {
                FactoryAbstractUser userFactory = new FactoryUser();
                switch (submitbutton)
                {
                    case "User":
                        User currUser = userFactory.Create(strFName, strLName, strMail, (int) UserType.User) as User;
                        DataService.UserToDataBase(currUser);

                        // Send notification about registration 
                            return RedirectToAction("Contact", "Email", 
                                new
                                {
                                    strUserMail = strMail, 
                                    strMessage="Your account (User) was successfully created",
                                    strRedirectAction = "RegFormMessage",
                                    strRedirectController = "User"
                                }); 
                    case "Librarian":
                        if (strPassword == "Password")
                        {
                            User currLibrarian =
                                userFactory.Create(strFName, strLName, strMail, (int) UserType.Librarian) as User;
                            DataService.UserToDataBase(currLibrarian);

                            // Send notification about registration 
                            return RedirectToAction("Contact", "Email",
                                new
                                {
                                    strUserMail = strMail,
                                    strMessage = "Your account (Librarian) was successfully created",
                                    strRedirectAction = "RegFormMessage",
                                    strRedirectController = "User"
                                });
                        }
                        break;
                   default: 
                        break;
                }

                // ТУТ и в подобных метсах, где может быть ошибка, надо возвращать страницу ошибки и все 
                return RedirectToAction("Index", "Entrance");
            }
        }

        /// <summary>
        /// Shows message about successfull registration 
        /// </summary>
        /// <returns></returns>
        public ActionResult RegFormMessage()
        {
            return View(); 
        }

        /// <summary>
        /// Method calls View with form for logging 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View(); 
        }

        /// <summary>
        /// Method receives data from user: mail (and password for role  - Librarian) 
        /// Handles this data and redirects to checking functions  
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <param name="strMail"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult LoginHandler(string submitbutton, string strMail, string strPassword)
        {
            if (!DataService.IsUser(strMail) || strMail.Length == 0)
            {
                return RedirectToAction("WrongAccountForm", "User");
            }

            User currUser = DataService.SearchUser(strMail);
            switch (submitbutton)
            {
                case "User":
                    // checking for existing user with such mail and role 
                    if (currUser != null && currUser.Role == (int) UserType.User)
                    {
                        // Cause user could only observe books 
                        return RedirectToAction("Index", "Filter");
                    }
                    break;

                case "Librarian":
                    // checks librarians password 
                    if (strPassword == "Password")
                    {
                        if (currUser != null && currUser.Role == (int) UserType.Librarian)
                        {
                            return RedirectToAction("MenuLibrarian", "Entrance");
                        }
                    }
                    break;
                default:
                    break;
            }
            return RedirectToAction("WrongAccountForm", "User");
        }

        /// <summary>
        /// Method which is being used after filling entrance form with wrong e-mail 
        /// Method views page with redirection button to start page  
        /// </summary>
        /// <returns></returns>
        public ActionResult WrongAccountForm()
        {
            return View(); 
        }

        /// <summary>
        /// Method calls form for deleting users 
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteFormEnter()
        {
            return View(); 
        }

        /// <summary>
        /// Method receives data from client side and calls dataservice method to delete user 
        /// </summary>
        /// <param name="strMail"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult DeleteFormHandle(string submitbutton, string strMail=null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance"); 
            }

            // method checks user for its existing 
            if (!DataService.IsUser(strMail) || strMail.Length == 0)
            {
                return RedirectToAction("WrongAccountForm", "User");
            }
            User tmpUser = DataService.SearchUser(strMail);

            ViewBag.User = tmpUser; 
            // calls the dataservice method to delete information about user 
            DataService.DeleteUser(strMail);

            return View();
        }
    }
}

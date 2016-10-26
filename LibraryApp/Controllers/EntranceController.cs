using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryApp.Controllers
{
    public class EntranceController : Controller
    {
        //
        // GET: /
        /// <summary>
        /// MainMenu/
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method receives button from main entrance menu 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult MainMenuHandler(string submitbutton)
        {
            if (submitbutton == "Enter")
            {
                return RedirectToAction("Login", "User");
            }
            else if (submitbutton == "Register")
            {
                return RedirectToAction("Index", "User");
            }

            // exception 
            return View(); 
        }

        /// <summary>
        /// Method calls the main librarian menu 
        /// with activity options of such role 
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuLibrarian()
        {
            return View(); 
        }

        /// <summary>
        /// Method receives button from main librarian menu 
        /// Method redirects to other functions, which are librarian activity options  
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult RedirectionMenuLibrarian(string submitbutton)
        {
            // switch the button from menu 
            switch (submitbutton)
            {
                    // to main search menu 
                case "Search":
                    return RedirectToAction("Index", "Filter");
                    // to add book menu 
                case "Add":
                    return RedirectToAction("AddBookForm", "Book");
                    // to delete book menu 
                case "Remove":
                    return RedirectToAction("DeleteBookForm", "Book");
                    // to issue/return book menu 
                case "Record":
                    return RedirectToAction("Index", "GiveAwayBook");
                    // to cancel membership menu 
                case "Expell":
                    return RedirectToAction("DeleteFormEnter", "User");
                    // to main menu
                case "Menu":
                    return RedirectToAction("Index", "Entrance");
                default:
                    break;
            }
            return RedirectToAction("Index", "Entrance"); 
        }
        /// <summary>
        /// Method calls main user menu, which includes only search options 
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuUser()
        {
            return RedirectToAction("Index", "Filter");
        }

    }
}

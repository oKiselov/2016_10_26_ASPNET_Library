using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Models;
using LibraryApp.Models.Enum;

namespace LibraryApp.Controllers
{
    public class GiveAwayBookController : Controller
    {
        //
        // GET: /GiveAwayBook/

        /// <summary>
        /// Method calls main menu for issue/return book process
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult MenuHandler(string submitbutton)
        {
            switch (submitbutton)
            {
                case "Give":
                    return RedirectToAction("GiveAwayForm", "GiveAwayBook");
                case "Return":
                    return RedirectToAction("ReturnForm", "GiveAwayBook");
                default:
                    break;
            }
            return RedirectToAction("Index", "GiveAwayBook"); 
        }

        /// <summary>
        /// Method calls form for book issue
        /// </summary>
        /// <returns></returns>
        public ActionResult GiveAwayForm()
        {
            return View(); 
        }
        
        /// <summary>
        /// Method calls form for returning option 
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnForm()
        {
            return View(); 
        }

        /// <summary>
        /// Method calls dataservice method and checks book for its existing 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult GiveAwayHandler(string submitbutton, string strTitle = null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance");
            }
            List<Book> lstTmpBooks = DataService.SearchBookByName(strTitle);
            if (lstTmpBooks.Count == 0||strTitle.Length==0)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }
            ViewBag.Books = lstTmpBooks; 
            return View();
        }

        /// <summary>
        /// Method calls dataservice method and checks book for its existing 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult ReturnHandler(string submitbutton, string strTitle=null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance");
            }
            List<Book> lstTmpBooks = DataService.SearchBookByName(strTitle);
            if (lstTmpBooks.Count == 0 || strTitle.Length == 0)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }
            ViewBag.Books = lstTmpBooks; 
            return View();
        }

        /// <summary>
        /// Method receives data about book 
        /// and records to databases history of book usage 
        /// </summary>
        /// <param name="strBookId"></param>
        /// <param name="strMail"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult GiveAwayFinish(string submitbutton, Guid strBookId = new Guid(), string strMail = null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance");
            }

            // if such user doesn't exist
            if (!DataService.IsUser(strMail) || !DataService.IsBook(strBookId)|| strMail.Length == 0)
            {
                return RedirectToAction("WrongAccountForm", "User");
            }
            User currUser = DataService.SearchUser(strMail);
            
            Book currBook = DataService.SearchBookById(strBookId);
            // if book is not available now for giving away 
            if (currBook.Keeper != Guid.Empty)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }

            // puts info to datatable about usage history 
            DataService.UpdateBookAction(currUser.UserId, strBookId, (int)IsAvailable.No);

            // Send notification about book issue  
            return RedirectToAction("Contact", "Email",
                new
                {
                    strUserMail = strMail,
                    strMessage = "You took a book: " + currBook.Title,
                    strRedirectAction = "GiveAwayMessage",
                    strRedirectController = "GiveAwayBook"
                }); 

        }

        public ActionResult GiveAwayMessage()
        {
            return View();
        }


        /// <summary>
        /// Method receives data about book 
        /// and records to databases history of book usage 
        /// </summary>
        /// <param name="strBookId"></param>
        /// <param name="strMail"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult ReturnFinish(string submitbutton, Guid strBookId = new Guid(), string strMail = null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance");
            }

            // if such user doesn't exist
            if (!DataService.IsUser(strMail) || !DataService.IsBook(strBookId) || strMail.Length == 0)
            {
                return RedirectToAction("WrongAccountForm", "User");
            }
            User currUser = DataService.SearchUser(strMail);

            Book currBook = DataService.SearchBookById(strBookId);
            // if book is not available now for return action  
            if (currBook.Keeper == Guid.Empty)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }

            // puts info to datatable about usage history 
            DataService.UpdateBookAction(currUser.UserId, strBookId, (int)IsAvailable.Yes);

            // Send notification about returned book  
            return RedirectToAction("Contact", "Email",
                new
                {
                    strUserMail = strMail,
                    strMessage = "You returned a book: " + currBook.Title,
                    strRedirectAction = "ReturnMessage",
                    strRedirectController = "GiveAwayBook"
                }); 

        }

        public ActionResult ReturnMessage()
        {
            return View();
        }
    }
}

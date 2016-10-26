using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Models;

namespace LibraryApp.Controllers
{
    public class FilterController : Controller
    {
        //
        // GET: /Filter/
        /// <summary>
        /// Method calls main search menu 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // variable saves current amount of rows for paging 
            if (Session["RowNum"] == null)
            {
                Session["RowNum"] = 1;
            }
            return View();
        }

        /// <summary>
        /// Method receives button from main search menu
        /// Method redirects to other serch options 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult FilterMenu(string submitbutton)
        {
            switch (submitbutton)
            {
                    // user chose option - show available books 
                case "Available":
                    return RedirectToAction("AvailableBooks", "Filter",
                        new {submitbutton = "Previous"});
                    // user chose option - show all books in library 
                case "Show all":
                    return RedirectToAction("AllBooks", "Filter",
                        new {submitbutton = "Previous"});
                    // user chose option - show all books taken by user 
                case "Taken":
                    return RedirectToAction("BooksTakenByUser", "Filter");
                    // user chose current book and wanted to see info about it 
                case "Details":
                    return RedirectToAction("GetGuidForBookDetails", "Filter");
                    //user chose option - to search book by its name 
                case "Search":
                    return RedirectToAction("SearchBookByNameForm", "Filter");
                default:
                    break;
            }
            // redirects to main search menu 
            return RedirectToAction("Index", "Filter");
        }

        /// <summary>
        /// Method calls main search by name menu 
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchBookByNameForm()
        {
            return View(); 
        }

        /// <summary>
        /// Method returns to client side the list of books which have the same title 
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult SearchBookByNameHandler(string strTitle)
        {
            List<Book> lstTmpBooks = DataService.SearchBookByName(strTitle);
            if (lstTmpBooks.Count == 0 || strTitle.Length==0)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter"); 
            }
            ViewBag.Books = lstTmpBooks; 
            return View(); 
        }

        /// <summary>
        /// Method which is being called from client-side 
        /// Returns all books in paging view - 10 books on one page 
        /// For moving up - press next or previous button 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult AllBooksPost(string submitbutton)
        {
            List<KeyValuePair<Book, List<Author>>> lstPair = new List<KeyValuePair<Book, List<Author>>>();
            switch (submitbutton)
            {
                case "Next":
                    List<Book> lstBooksIn = DataService.AllBooksFromDataBase(IncreaseForPage());
                    for (int i = 0; i < lstBooksIn.Count; i++)
                    {
                        lstPair.Add(DataService.GetBookDetails(lstBooksIn[i].BookId));
                    }
                    break;
                case "Previous":
                    List<Book> lstBooksDec = DataService.AllBooksFromDataBase(DecreaseForPage());
                    for (int i = 0; i < lstBooksDec.Count; i++)
                    {
                        lstPair.Add(DataService.GetBookDetails(lstBooksDec[i].BookId));
                    }
                    break;
                case "Home":
                    return RedirectToAction("Index", "Filter");
                default:
                    break;
            }
            ViewBag.Books = lstPair; 
            return View();
        }

        /// <summary>
        /// Method which is being redirected from filter menu just once - to show first page  
        /// Returns all books in paging view - 10 books on one page 
        /// For moving up - press next or previous button 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult AllBooks(string submitbutton)
        {
            List<KeyValuePair<Book, List<Author>>> lstPair = new List<KeyValuePair<Book, List<Author>>>();
            List<Book> lstBooksDec = DataService.AllBooksFromDataBase(DecreaseForPage());
            for (int i = 0; i < lstBooksDec.Count; i++)
            {
                lstPair.Add(DataService.GetBookDetails(lstBooksDec[i].BookId));
            }
            ViewBag.Books = lstPair;
            return View();
        }
        
        /// <summary>
        /// Method which is being called from client-side 
        /// Returns all available books in paging view - 10 books on one page 
        /// For moving up - press next or previous button 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult AvailableBooksPost(string submitbutton)
        {
            List<KeyValuePair<Book, List<Author>>> lstPair = new List<KeyValuePair<Book, List<Author>>>();
            switch (submitbutton)
            {
                case "Next":
                    List<Book> lstBooksIn = DataService.AvailableBooksFromDataBase(IncreaseForPage());
                    for (int i = 0; i < lstBooksIn.Count; i++)
                    {
                        lstPair.Add(DataService.GetBookDetails(lstBooksIn[i].BookId));
                    }
                    break;
                case "Previous":
                    List<Book> lstBooksDec = DataService.AvailableBooksFromDataBase(DecreaseForPage());
                    for (int i = 0; i < lstBooksDec.Count; i++)
                    {
                        lstPair.Add(DataService.GetBookDetails(lstBooksDec[i].BookId));
                    }
                    break;
                case "Home":
                    return RedirectToAction("Index", "Filter");
                default:
                    break;
            }
            ViewBag.Books = lstPair;
            return View();
        }

        /// <summary>
        /// Method which is being redirected from filter menu just once - to show first page
        /// Returns all available books in paging view - 10 books on one page 
        /// For moving up - press next or previous button 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult AvailableBooks(string submitbutton)
        {
            List<KeyValuePair<Book, List<Author>>> lstPair = new List<KeyValuePair<Book, List<Author>>>();
            List<Book> lstBooksDec = DataService.AvailableBooksFromDataBase(DecreaseForPage());
            for (int i = 0; i < lstBooksDec.Count; i++)
            {
                lstPair.Add(DataService.GetBookDetails(lstBooksDec[i].BookId));
            }
            ViewBag.Books = lstPair;
            return View();
        }

        public ActionResult BooksTakenByUser()
        {
            return View(); 
        }

        /// <summary>
        /// Methods receives collection of books from database
        /// Those books are taken by current user  
        /// </summary>
        /// <param name="currGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult BooksTakenByUserFinish(Guid currGuid)
        {
            if (!DataService.IsUser(currGuid))
            {
                return RedirectToAction("WrongAccountForm", "User");
            }

            User currUser = DataService.SearchUser(currGuid); 
            List<Book> lstBooks = DataService.GetBooksTakenByUser(currGuid);
            ViewBag.User = currUser; 
            ViewBag.CountOfBooks = lstBooks.Count;
            ViewBag.Books = lstBooks;
            return View(); 
        }

        /// <summary>
        /// Method calls main menu for searching book's details (authors, is available or not) 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGuidForBookDetails()
        {
            return View(); 
        }

        /// <summary>
        /// Method receives Book Id from client side 
        /// Calls methods from data service and returns pair : book and list of its authors 
        /// </summary>
        /// <param name="currGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult BooksDetails(Guid currGuid)
        {
            if (!DataService.IsBook(currGuid))
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }
            KeyValuePair<Book, List<Author>> pairBookDetails = DataService.GetBookDetails(currGuid);
            List<KeyValuePair<Guid, string>> lstPairs = DataService.GetBookHistory(currGuid);

            ViewBag.Books = pairBookDetails;
            ViewBag.Actions = lstPairs; 

            return View(); 
        }

        /// <summary>
        /// Method calls view of search book error 
        /// </summary>
        /// <returns></returns>
        public ActionResult IncorrectBookInfo()
        {
            return View(); 
        }

        /// <summary>
        /// Private method for paging 
        /// </summary>
        /// <returns></returns>
        private int IncreaseForPage()
        {
            int iBegin = 0;
            int.TryParse(Session["RowNum"].ToString(), out iBegin);
            return iBegin + 10;
        }

        /// <summary>
        /// Private method for paging 
        /// </summary>
        /// <returns></returns>
        private int DecreaseForPage()
        {
            int iBegin = 0;
            int.TryParse(Session["RowNum"].ToString(), out iBegin);
            iBegin -= 10; 
            iBegin =(iBegin<1)? 1:iBegin;
            return iBegin; 
        }
    }
}

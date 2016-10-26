using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Models;
using LibraryApp.Models.Abstract;
using LibraryApp.Models.Enum;
using LibraryApp.Models.Factory;

namespace LibraryApp.Controllers
{
    public class BookController : Controller
    {
        /// <summary>
        /// Method views main add book menu 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBookForm()
        {
            return View();
        }

        /// <summary>
        /// Method receives button from main add book menu 
        /// and title of book
        /// Method calls view with form for addition of first author 
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception),View = "Error")]
        public ActionResult AddFirstAuthor(string submitbutton, string strTitle=null)
        {
            switch (submitbutton)
            {
                    // create data container with book's Title 
                case "Next":
                    if (TempData["dataBook"] == null)
                    {
                        TempData["dataBook"] = strTitle;
                    }
                    break;
                case "Home":
                    return RedirectToAction("MenuLibrarian", "Entrance");
                default:
                    break;
            }
            // checks if user entered plain book Title and choose option Next 
            if (strTitle.Length == 0)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter"); 
            }
            return View();
        }


        /// <summary>
        /// Method puts info about author into data container 
        /// As list of authors for current book 
        /// </summary>
        /// <param name="strFName"></param>
        /// <param name="strLName"></param>
        /// <returns></returns>
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult AddOtherAuthorsNotPost(string strFName, string strLName)
        {
            if (TempData["dataAuthor"] == null)
            {
                List<KeyValuePair<string, string>> lstAuthors =
                    new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>(strFName, strLName)
                    };
                TempData["dataAuthor"] = lstAuthors;
            }
            else
            {
                List<KeyValuePair<string, string>> lstAuthors =
                    TempData["dataAuthor"] as List<KeyValuePair<string, string>>;
                lstAuthors.Add(new KeyValuePair<string, string>(strFName, strLName));
            }
            return View();
        }

        /// <summary>
        /// Method receives button from author's additional menu 
        /// Puts data from current book and its authors into database 
        /// </summary>
        /// <param name="strFName"></param>
        /// <param name="strLName"></param>
        /// <param name="submitbutton"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult AddSuccess(string strFName, string strLName, string submitbutton)
        {
            // receives and handles button 
            switch (submitbutton)
            {
                    // To add one more author 
                case "More":
                    return RedirectToAction("AddOtherAuthorsNotPost", "Book",
                        new {strFName = strFName, strLName = strLName});
                    // To back to main librarian menu 
                case "Home":
                    return RedirectToAction("MenuLibrarian", "Entrance");
                    // To complete book additional process 
                case "OK":
                    // handling of data container with authors 
                    if (TempData["dataAuthor"] == null)
                    {
                        List<KeyValuePair<string, string>> lstAuthors =
                            new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string>(strFName, strLName)
                            };
                        TempData["dataAuthor"] = lstAuthors;
                    }
                    else
                    {
                        List<KeyValuePair<string, string>> lstAuthors =
                            TempData["dataAuthor"] as List<KeyValuePair<string, string>>;
                        lstAuthors.Add(new KeyValuePair<string, string>(strFName, strLName));
                    }
                    break;
                default:
                    break;
            }
            
            // creation of collection with authors names 
            string strTitle = TempData["dataBook"] as string;
            List<KeyValuePair<string, string>> lstAuthorsNames =
                TempData["dataAuthor"] as List<KeyValuePair<string, string>>;

            // creation of book with such Title 
            FactoryAbstractPrint bookFactory = new FactoryBook();
            Book currBook = bookFactory.Create(strTitle) as Book;

            // creation of collection with authors 
            FactoryAuthor authorFactory = new FactoryAuthor();
            List<Author> lstAuthorsBook = new List<Author>();
            for (int i = 0; i < lstAuthorsNames.Count; i++)
            {
                Author tmpAuthor = authorFactory.Create(lstAuthorsNames[i].Key, lstAuthorsNames[i].Value);
                lstAuthorsBook.Add(tmpAuthor);
            }

            // Insert information of book and authors into three datatables:
            // 1. Author
            // 2. Book 
            // 3. AuthorBook - with connections between them 
            DataService.BookToDataBase(currBook);
            for (int i = 0; i < lstAuthorsBook.Count; i++)
            {
                DataService.AuthorToDataBase(lstAuthorsBook[i]);
                DataService.BookAuthorToDataBase(currBook.BookId, lstAuthorsBook[i].AuthorId);
            }
            // clearing of data containers 
            TempData["dataBook"] = null;
            TempData["dataAuthor"] = null;

            return View();
        }

        /// <summary>
        /// Method views main menu for book deleting 
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteBookForm()
        {
            return View();
        }

        /// <summary>
        /// Method receives Book Title from user 
        /// Checks for existing book with such Title 
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult DeleteBookFormHandler(string submitbutton, string strTitle = null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance");
            }
            List<Book> lstTmpBook = DataService.SearchBookByName(strTitle);
            // library doesn't contain such book 
            if (lstTmpBook == null)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }
            ViewBag.Books = lstTmpBook; 

            return View();
        }

        /// <summary>
        /// Method deletes book from all databases 
        /// </summary>
        /// <param name="submitbutton"></param>
        /// <param name="strBookId"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult DeleteBookById(string submitbutton, string strBookId = null)
        {
            if (submitbutton == "Home")
            {
                return RedirectToAction("MenuLibrarian", "Entrance");
            }
            // checking for bookID 
            if (strBookId == null)
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }
            Guid guidBookId = Guid.Parse(strBookId);
            if (!DataService.IsBook(guidBookId))
            {
                return RedirectToAction("IncorrectBookInfo", "Filter");
            }
            Book currBook = DataService.SearchBookById(guidBookId);

            DataService.DeleteBookById(guidBookId);

            ViewBag.Books = DataService.SearchBookByName(currBook.Title);
            ViewBag.Guid = strBookId;

            return View();
        }
    }
}
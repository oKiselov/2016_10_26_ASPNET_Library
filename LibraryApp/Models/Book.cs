using System;
using LibraryApp.Models.Abstract;
using LibraryApp.Models.Enum;

namespace LibraryApp.Models
{
    /// <summary>
    /// Class which accumulates information about book 
    /// </summary>
    [Serializable]
    public class Book:AbstractPrint
    {
        public Book(string strTitle)
        {
            Title = strTitle; 
            BookId=Guid.NewGuid();
            Keeper = Guid.Empty;
        }

        public Book(Guid bookGuid, string strTitle, Guid currUserGuid)
        {
            Title = strTitle;
            BookId = bookGuid;
            Keeper = currUserGuid;
        }
        public Book(){}
    }
}
using System;

namespace LibraryApp.Models
{
    /// <summary>
    /// Class desribes model of Author
    /// </summary>
    public class Author
    {
        public Guid AuthorId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Author(string strFName, string strLName)
        {
            AuthorId = Guid.NewGuid();
            FirstName = strFName;
            LastName = strLName; 
        }

        public Author(Guid currGuid, string strFName, string strLName)
        {
            AuthorId = currGuid;
            FirstName = strFName;
            LastName = strLName;
        }

        public Author(){}
    }
}

using System;
using System.Collections.Generic;
using LibraryApp.Models.Abstract;
using LibraryApp.Models.Enum;

namespace LibraryApp.Models
{
    /// <summary>
    /// Class describes model - User 
    /// </summary>
    public class User:AbstractUser
    {
        public string eMail { get; set; }

        public string RegDate { get; set; }

        public User(string strFName, string strLName, string strMail, int iRole)
        {
            UserId=Guid.NewGuid();
            FirstName = strFName;
            LastName = strLName;
            eMail = strMail;
            RegDate = DateTime.UtcNow.ToString();
            Role = iRole; 
        }

        public User(){}
    }
}
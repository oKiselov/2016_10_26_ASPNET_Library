using System;
using System.Collections.Generic;

namespace LibraryApp.Models.History
{
    /// <summary>
    /// Class (POCO type)
    /// includes information for connection with database table: 
    /// Using book history  
    /// </summary>
    public class StatisticUserBook
    {
        public Guid UserId { get; set; }

        public Guid BookId { get; set; }

        public string Date { get; set; }

        public int Action { get; set; }
    }
}
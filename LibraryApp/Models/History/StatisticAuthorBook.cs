using System;
using System.Collections.Generic;

namespace LibraryApp.Models.History
{
    /// <summary>
    /// Class (POCO type) 
    /// includes information for connection with database table: 
    /// Book review  
    /// </summary>
    public class StatisticAuthorBook
    {
        public Guid BookId { get; set; }

        public Guid AuthorId { get; set; }

    }
}
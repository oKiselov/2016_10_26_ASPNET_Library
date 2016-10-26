using System;
using LibraryApp.Models.Enum;

namespace LibraryApp.Models.Abstract
{
    public abstract class AbstractPrint
    {
        public Guid BookId { get; set; }

        public string Title { get; set; }

        public Guid Keeper { get; set; }

    }
}
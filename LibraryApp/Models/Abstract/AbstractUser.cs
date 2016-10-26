using System;

namespace LibraryApp.Models.Abstract
{
    public abstract class AbstractUser
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Role { get; set; }
    }
}
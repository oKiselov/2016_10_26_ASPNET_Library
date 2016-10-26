namespace LibraryApp.Models.Enum
{
    public enum IsAvailable:int 
    {
        Yes = 1,
        No
    }

    public enum UserType : int
    {
        User=1, 
        Librarian
    }

    public enum BookType : int
    {
        AllBooks = 1, 
        AvailableBooks
    }
}
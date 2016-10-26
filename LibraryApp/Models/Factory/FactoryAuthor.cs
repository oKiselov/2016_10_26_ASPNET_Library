namespace LibraryApp.Models.Factory
{
    public class FactoryAuthor
    {
        public Author Create(string strFName, string strLName)
        {
            return new Author(strFName, strLName);
        }
    }
}
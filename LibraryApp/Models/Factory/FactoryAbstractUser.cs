using LibraryApp.Models.Abstract;

namespace LibraryApp.Models.Factory
{
    public abstract class FactoryAbstractUser
    {
        public abstract AbstractUser Create(string strFNAme, string strLName, string strMail, int iRole); 
    }
}
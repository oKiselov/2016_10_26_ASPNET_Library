using LibraryApp.Models.Abstract;

namespace LibraryApp.Models.Factory
{
    public class FactoryUser : FactoryAbstractUser
    {
        public override AbstractUser Create(string strFName, string strLName, string strMail, int iRole)
        {
            return new User(strFName, strLName, strMail, iRole);
        }
    }
}
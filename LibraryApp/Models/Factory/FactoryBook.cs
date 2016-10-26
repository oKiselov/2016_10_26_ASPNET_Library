using LibraryApp.Models.Abstract;

namespace LibraryApp.Models.Factory
{
    public class FactoryBook : FactoryAbstractPrint
    {
        public override AbstractPrint Create(string strTitle)
        {
            return new Book(strTitle);
        }
    }
}
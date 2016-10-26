using LibraryApp.Models.Abstract;

namespace LibraryApp.Models.Factory
{
    public abstract class FactoryAbstractPrint
    {
        public abstract AbstractPrint Create(string strTitle); 
    }
}
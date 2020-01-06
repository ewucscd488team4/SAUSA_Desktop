
namespace SAUSALibrary.Models
{
    public class MiniStackModel
    {
        public long Index { get; set; }

        public string CrateName { get; set; }

        public MiniStackModel (long index, string Crate )
        {
            Index = index;
            CrateName = Crate;
        }
    }
}

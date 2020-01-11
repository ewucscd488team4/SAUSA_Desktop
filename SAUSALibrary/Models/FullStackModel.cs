
namespace SAUSALibrary.Models
{
    public class FullStackModel
    {
        public long Index { get; set; }
        public double XPOS { get; set; }
        public double YPOS { get; set; }
        public double ZPOS { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string CrateName { get; set; }

        public FullStackModel(long index, double xPOS, double yPOS, double zPOS, double length, double width, double height, double weight, string crateName)
        {
            Index = index;
            XPOS = xPOS;
            YPOS = yPOS;
            ZPOS = zPOS;
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            CrateName = crateName;
        }
    }
}

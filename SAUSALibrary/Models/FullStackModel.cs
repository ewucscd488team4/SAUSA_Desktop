
namespace SAUSALibrary.Models
{
    public class FullStackModel
    {
        public long Index { get; set; } = 0;
        public double XPOS { get; set; } = 0.0;
        public double YPOS { get; set; } = 0.0;
        public double ZPOS { get; set; } = 0.0;
        public double Length { get; set; } = 0.0;
        public double Width { get; set; } = 0.0;
        public double Height { get; set; } = 0.0;
        public double Weight { get; set; } = 0.0;
        public string CrateName { get; set; } = "";

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

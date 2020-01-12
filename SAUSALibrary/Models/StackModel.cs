
namespace SAUSALibrary.Models
{
    public class StackModel
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
    }
}

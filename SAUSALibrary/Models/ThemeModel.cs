
namespace SAUSALibrary.Models
{
    /// <summary>
    /// Container for encapsulating the theme setting individually from the settings.xml file
    /// </summary>
    public class ThemeModel
    {
        /// <summary>
        /// String for the theme setting read from the settings xml file
        /// </summary>
        public string ThemeValue { get; set; } = "blank";
    }
}

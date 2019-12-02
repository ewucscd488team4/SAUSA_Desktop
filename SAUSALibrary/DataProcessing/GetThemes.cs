using SAUSALibrary.Models;
using SAUSALibrary.Defaults;
using System;
using System.Collections.Generic;

namespace SAUSALibrary.DataProcessing
{
    public class GetThemes
    {
        /// <summary> 
        /// Returns a List of ThemeModels comprised of all themes listed in the default theme enum.
        /// </summary>
        /// <returns></returns>
        public static List<ThemeModel> GetThemesList()
        {
            List<ThemeModel> list = new List<ThemeModel>();
            foreach (string themeValue in Enum.GetNames(typeof(ThemeEnums)))
            {
                list.Add(new ThemeModel() { ThemeValue = themeValue });
            }
            return list;
        }
    }
}

using SAUSALibrary.Models;
using SAUSALibrary.Models.Database;
using SAUSALibrary.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAUSALibrary.DataProcessing
{
    public class GetEnums
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

        /// <summary>
        /// Gets list of database "types" from the specific enum class for this data type and returns it as a List of type IndividualDatabaseFieldModel.
        /// </summary>
        /// <returns></returns>
        public static List<IndividualDatabaseFieldModel> GetFieldsList()
        {
            List<IndividualDatabaseFieldModel> list = new List<IndividualDatabaseFieldModel>();
            foreach (string fieldValue in Enum.GetNames(typeof(DBFieldsEnum)))
            {
                list.Add(new IndividualDatabaseFieldModel() { FieldType = fieldValue, FieldName = "" });
            }
            return list;
        }
    }
}

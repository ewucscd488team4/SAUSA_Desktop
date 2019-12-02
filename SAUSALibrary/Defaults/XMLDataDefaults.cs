using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAUSALibrary.Defaults
{
    /// <summary>
    /// Definitions for the XML module heirarchy inside various XML files.
    /// </summary>
    public class XMLDataDefaults
    {
        public static string ProjectNameStructure
        {
            get
            {
                return "Sausa/ProjName/Name";
            }
        }

        public static string ProjectDimensionsStructure
        {
            get
            {
                return "Sausa/Storage/Dimensions";
            }
        }

        public static string ProjectDataStructure
        {
            get
            {
                return "Sausa/Stacks/Data";
            }
        }

        public static string SettingsSettingStructure
        {
            get
            {
                return "Sausa/Settings/Setting";
            }
        }
    }
}

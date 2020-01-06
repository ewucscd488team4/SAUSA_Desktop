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

        public static string ProjectExternalDatabaseStructure
        {
            get
            {
                return "Sausa/ExternalDatabase/Data";
            }
        }

        public static string ProjectRoomDimensionsStructure
        {
            get
            {
                return "Sausa/Storage/Dimensions";
            }
        }

        public static string ProjectStackDataStructure
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

        public static string SettingsProjectsStructure
        {
            get
            {
                return "Sausa/Projects/Project";
            }
        }
    }
}

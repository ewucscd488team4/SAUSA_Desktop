using System;

namespace SAUSALibrary.Defaults
{
    /// <summary>
    /// Default file location folders, and explicit settings.xml file location definition
    /// </summary>
    public class FilePathDefaults
    {
        public static string ScratchFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sausa\";
                //on my dev machine, this maps to C:\Users\Diesel\AppData\Roaming\Sausa\
            }
        }

        public static string SettingsFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Sausa\";
                //on my dev machine, this maps to C:\ProgramData\Sausa\
            }
        }

        public static string DefaultSavePath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Sausa\";
                //on my dev machine, this maps to C:\Users\Diesel\Documents\Sausa\
            }
        }

        public static string SettingsFile
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Sausa\";
                //on my dev machine, this maps to C:\ProgramData\Sausa\Settings.xml
            }
        }
    }
}

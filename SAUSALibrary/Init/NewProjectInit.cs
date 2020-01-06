using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.Database.Writing;

namespace SAUSALibrary.Init
{
    public class NewProjectInit
    {
        /// <summary>
        /// Sets up a default project XML file with the given project name.
        /// </summary>
        /// <param name="projectName"></param>
        public static void NewProjectDetailOperations(string filepath, string safefilepath)
        {
            WriteXML.WriteBlankXML(filepath, safefilepath); //write a new project file out with the given path and file name
            WriteSQLite.CreateDatabase(filepath, safefilepath); //write a new project database out with the given path and file name
            //write project to settings
        }
    }
}

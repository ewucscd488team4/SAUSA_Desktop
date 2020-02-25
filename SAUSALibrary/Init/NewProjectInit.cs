using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.FileHandling.Database.Writing;

namespace SAUSALibrary.Init
{
    public class NewProjectInit
    {
        /// <summary>
        /// Sets up a default project XML file with the given project name.
        /// </summary>
        /// <param name="projectName"></param>
        public static void NewProjectDetailOperations(string tempFileDirectory, string newProjectXMLFile, string newProjectDBFile)
        {
            WriteXML.WriteBlankXML(tempFileDirectory, newProjectXMLFile); //write a new project file out with the given path and file name
            WriteSQLite.CreateProjectDatabase(tempFileDirectory, newProjectDBFile); //write a new project database out with the given path and file name
            //TODO -ignore- write project name, savepath, and time accessed to settings XML file, or move to top and update time if already there
        }
    }
}

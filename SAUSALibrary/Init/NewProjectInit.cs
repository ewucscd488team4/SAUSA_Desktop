using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.Defaults;
using System.IO;

namespace SAUSALibrary.Init
{
    class NewProjectInit
    {
        /// <summary>
        /// Sets up a default project XML file with the given project name.
        /// </summary>
        /// <param name="projectName"></param>
        public static void NewProjectDetailOperations(string projectName)
        {
            WriteXML.WriteBlankXML(Path.Combine(FilePathDefaults.DefaultSavePath, projectName)); //write a new project file out with the given name
        }
    }
}

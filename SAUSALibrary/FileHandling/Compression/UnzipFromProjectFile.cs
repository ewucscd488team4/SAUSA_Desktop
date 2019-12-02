using System.IO;
using System.IO.Compression;

namespace SAUSALibrary.FileHandling.Compression
{
    public class UnzipFromProjectFile
    {
        /// <summary>
        /// Unzips files from the given project file to the given working file folder.
        /// </summary>
        /// <param name="fullProjectFilePath"></param>
        /// <param name="workingFolder"></param>
        public static void OpenProject(string fullProjectFilePath, string workingFolder)
        {
            if (File.Exists(fullProjectFilePath))
            {
                ZipFile.ExtractToDirectory(fullProjectFilePath, workingFolder);
            }
            else
            {
                //TODO throw error dialog if project file to unzip does not exist
            }

        }
    }
}

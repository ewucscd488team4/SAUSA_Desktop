using System.IO;
using System.IO.Compression;

namespace SAUSALibrary.FileHandling.Compression
{
    public class ZipFromScratchFolder
    {
        /// <summary>
        /// Zips everything in the given working directory and puts the new zip in the given project file path,
        /// using the given project file name
        /// </summary>
        /// <param name="workingDirectory"></param>
        /// <param name="projectFileName"></param>
        /// <param name="projectFilePath"></param>
        public static void SaveProject(string workingDirectory, string projectFileName, string projectFilePath)
        {
            var fullZipFilePath = Path.Combine(projectFilePath, projectFileName);
            if (File.Exists(fullZipFilePath))
            {
                ZipFile.CreateFromDirectory(workingDirectory, fullZipFilePath);
            }
            else
            {
                //TODO throw error dialog if full project file name to save to does not exist
            }
        }
    }
}

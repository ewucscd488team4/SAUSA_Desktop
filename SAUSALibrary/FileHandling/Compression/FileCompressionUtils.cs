using System;
using System.IO;
using System.IO.Compression;

namespace SAUSALibrary.FileHandling.Compression
{
    /// <summary>
    /// Class for handling project save file compression and extraction
    /// </summary>
    public class FileCompressionUtils
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
                if (Directory.Exists(workingFolder))
                {
                    if (File.Exists(fullProjectFilePath))
                    {
                        ZipFile.ExtractToDirectory(fullProjectFilePath, workingFolder);
                    }                    
                }
                else
                {
                    throw new DirectoryNotFoundException("Scratch folder does not exist!");
                }

            }
            else
            {
                throw new FileNotFoundException("Project file does not exist!");
            }

        }

        /// <summary>
        /// Zips everything in the given working directory and puts the new zip in the given project file path,
        /// using the given project file name
        /// </summary>
        /// <param name="workingDirectory"></param>
        /// <param name="fullSaveFilePath"></param>
        /// <param name="projectFilePath"></param>
        public static void SaveProject(string workingDirectory, string fullSaveFilePath)
        {            
            if (!Directory.Exists(workingDirectory))
            {
                throw new DirectoryNotFoundException("Directory to compress does not exist!");
            }
            if (string.IsNullOrEmpty(fullSaveFilePath))
            {
                throw new ArgumentOutOfRangeException("Project file name cannot be null or empty!");
            }
            if(File.Exists(fullSaveFilePath))
            {
                File.Delete(fullSaveFilePath);
                ZipFile.CreateFromDirectory(workingDirectory, fullSaveFilePath);
            } else {
                ZipFile.CreateFromDirectory(workingDirectory, fullSaveFilePath);
            }
            
        }
    }
}

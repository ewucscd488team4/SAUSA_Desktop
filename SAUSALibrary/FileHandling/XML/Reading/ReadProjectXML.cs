using SAUSALibrary.Models;
using System;
using System.IO;
using System.Xml;

namespace SAUSALibrary.FileHandling.XML.Reading
{
    public class ReadProjectXML
    {
        /// <summary>
        /// Reads the sqlite database associated with the project, and returns it as a model class.
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static ProjectSQLiteDatabaseModel ReadProjectDatabase(string fullFilePath)
        {
            ProjectSQLiteDatabaseModel model = new ProjectSQLiteDatabaseModel();

            try
            {
                using (XmlReader _Reader = XmlReader.Create(new FileStream(fullFilePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
                {
                    while (_Reader.Read())
                    {

                        if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Data"))
                        {
                            if (_Reader.HasAttributes)
                            {
                                model.DatabaseName = _Reader.GetAttribute("FileName");
                                model.DatabasePath = _Reader.GetAttribute("Path");
                            }
                        }

                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e);
                model.DatabaseName = "BAD FILE";
                model.DatabasePath = @"c:\";
            }


            return model;
        }

        /// <summary>
        /// Reads the project name from the project XML file and returns it as a string.
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static string ReadProjectName(string fullFilePath)
        {
            string model = "";

            try
            {
                using (XmlReader _Reader = XmlReader.Create(new FileStream(fullFilePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
                {
                    while (_Reader.Read())
                    {

                        if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Name"))
                        {
                            if (_Reader.HasAttributes)
                            {
                                model = _Reader.GetAttribute("Name");
                            }
                        }

                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e);
                model = "BAD FILE";
            }


            return model;
        }

        /// <summary>
        /// Reads the storage attributes from the project file and returns them as a model class.
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static ProjectStorageModel ReadProjectStorage(string fullFilePath)
        {
            ProjectStorageModel model = new ProjectStorageModel();

            try
            {
                using (XmlReader _Reader = XmlReader.Create(new FileStream(fullFilePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
                {
                    while (_Reader.Read())
                    {

                        if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Dimensions"))
                        {
                            if (_Reader.HasAttributes)
                            {
                                model.Length = _Reader.GetAttribute("Length");
                                model.Width = _Reader.GetAttribute("Width");
                                model.Height = _Reader.GetAttribute("Height");
                                model.Weight = _Reader.GetAttribute("Weight");
                            }
                        }

                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e);
                model.Length = "666";
                model.Width = "666";
                model.Height = "666";
                model.Weight = "666";
            }


            return model;
        }
    }
}

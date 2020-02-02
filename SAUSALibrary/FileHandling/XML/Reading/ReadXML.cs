using SAUSALibrary.Models;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace SAUSALibrary.FileHandling.XML.Reading
{
    /// <summary>
    /// Class for reading SAUSA specific XML files
    /// </summary>
    public class ReadXML
    {
        /// <summary>
        /// Gets the saved external database connection parameters from the project XML file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<ExternalDBModel> GetDatabaseModel(string filePath)
        {

            List<ExternalDBModel> models = new List<ExternalDBModel>();

            using (XmlReader _Reader = XmlReader.Create(new FileStream(filePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
            {

                while (_Reader.Read()) //while reader can read
                {
                    ExternalDBModel model = new ExternalDBModel();
                    if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Data")) //looks for xml parent node type of "project" (will look like <Data />
                    {
                        if (_Reader.HasAttributes) //if current line has attributes
                        {
                            //read each attribute in the line if it matches the definition 

                            model.Type = _Reader.GetAttribute("Type");
                            model.Server = _Reader.GetAttribute("Server");
                            model.Database = _Reader.GetAttribute("Database");
                            model.UserID = _Reader.GetAttribute("UserID");
                            model.PassWord = _Reader.GetAttribute("Password");
                            models.Add(model);
                        }
                    }
                }//end of while
            }
            return models;
        }

        /// <summary>
        /// Reads project history list from the settings file, and returns it as a list of last project models.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<LastProjectModel> ReadProjects(string filePath)
        {
            List<LastProjectModel> models = new List<LastProjectModel>();

            using (XmlReader _Reader = XmlReader.Create(new FileStream(filePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
            {
                while (_Reader.Read())
                {
                    LastProjectModel model = new LastProjectModel();
                    if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Project"))
                    {
                        if (_Reader.HasAttributes)
                        {
                            model.Name = _Reader.GetAttribute("Definition");
                            model.Path = _Reader.GetAttribute("Path");
                            model.Time = _Reader.GetAttribute("Time");
                            models.Add(model);
                        }
                    }
                }
            }
            return models;
        }

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
                                model.TableName = _Reader.GetAttribute("TableName");
                                model.FileName = _Reader.GetAttribute("FileName");
                            }
                        }

                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e);
                model.TableName = "BAD FILE";
                model.FileName = @"c:\";
            }
            return model;
        }

        /// <summary>
        /// Returns the present project database table name
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static string ReadProjectDBTableName(string fullFilePath)
        {
            string model = "";
            using (XmlReader _Reader = XmlReader.Create(new FileStream(fullFilePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
            {
                while (_Reader.Read())
                {

                    if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Data"))
                    {
                        if (_Reader.HasAttributes)
                        {
                            model = _Reader.GetAttribute("TableName");
                        }
                    }

                }
            }
            return model;
        }

        /// <summary>
        /// Returns the present project database file name
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static string ReadProjectDBFileName(string fullFilePath)
        {
            string model = "";
            using (XmlReader _Reader = XmlReader.Create(new FileStream(fullFilePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
            {
                while (_Reader.Read())
                {

                    if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Data"))
                    {
                        if (_Reader.HasAttributes)
                        {
                            model = _Reader.GetAttribute("FileName");
                        }
                    }

                }
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
        /// <param name="workingFolder"></param>
        /// <returns></returns>
        public static ProjectStorageModel ReadProjectStorage(string workingFolder, string projectXMLFile)
        {
            ProjectStorageModel model = new ProjectStorageModel();
            var fqFilePath = Path.Combine(workingFolder, projectXMLFile);

            try
            {
                using (XmlReader _Reader = XmlReader.Create(new FileStream(fqFilePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
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

        /// <summary>
        /// Reads settings from settings XML file and returns them in a settings model.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Settings model</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static SettingsModel ReadSettings(string filePath)
        {
            if (File.Exists(filePath))
            {
                SettingsModel model = new SettingsModel();

                using (XmlReader _Reader = XmlReader.Create(new FileStream(filePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
                {
                    while (_Reader.Read())
                    {

                        if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Setting"))
                        {
                            if (_Reader.HasAttributes)
                            {
                                model.SettingOne = _Reader.GetAttribute("LastUsedColorScheme");
                                model.SettingTwo = _Reader.GetAttribute("LastProjectSavedDirectory");
                                model.SettingThree = _Reader.GetAttribute("DefaultSaveDirectory");
                                //model.SettingFour = _Reader.GetAttribute(""); //for future settings values
                            }
                        }

                    }
                }
                return model;
            }
            else
            {
                throw new FileNotFoundException("XML to read not found!");
            }
        }

        /// <summary>
        /// Checks to see if a given attribute exists in the given settings file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool CheckSettingsForProject(string filePath, string attribute)
        {

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var result = (from ele in doc.Descendants("Projects") select ele).ToList();
                foreach (var item in result)
                {
                    if (item.Attributes(attribute).Any())
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                throw new FileNotFoundException("Path to XML file not found!");
            }
        }
    }
}

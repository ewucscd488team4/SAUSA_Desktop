using SAUSALibrary.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SAUSALibrary.FileHandling.XML.Reading
{
    public class ReadDatabaseXML
    {
        /// <summary>
        /// Gets the saved external database connection parameters from the project XML file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<DatabaseModel> GetDatabaseModel(string filePath)
        {

            List<DatabaseModel> models = new List<DatabaseModel>();

            using (XmlReader _Reader = XmlReader.Create(new FileStream(filePath, FileMode.Open), new XmlReaderSettings() { CloseInput = true }))
            {

                while (_Reader.Read()) //while reader can read
                {
                    DatabaseModel model = new DatabaseModel();
                    if ((_Reader.NodeType == XmlNodeType.Element) && (_Reader.Name == "Database")) //looks for xml parent node type of "project" (will look like <Database>
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
    }
}

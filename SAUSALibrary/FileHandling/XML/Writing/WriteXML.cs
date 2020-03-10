﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using SAUSALibrary.Defaults;

namespace SAUSALibrary.FileHandling.XML.Writing
{
    /// <summary>
    /// Class for writing to SAUSA related XML files
    /// </summary>
    public class WriteXML
    {

        /// <summary>
        /// Writes out a blank project XML file to to given fully qualified file name.
        /// </summary>
        /// <param name="workingFolder"></param>
        public static void WriteBlankXML(string workingFolder, string xmlFileName) //NOTE this includes the xml file we need to write
        {            
            var fqPath = Path.Combine(workingFolder, xmlFileName);

            if (!File.Exists(fqPath)) //ensure we don't have a file 
            {
                var xmlNode =
                    new XElement("Sausa",                               //root of xml node

                                new XElement("ProjName",                //1st inner child node
                                    new XElement("Name",                //data node
                                        new XAttribute("Name", "XXX")   //attribute in data node
                                    ),
                                ""), //end of 1st child node

                                new XElement("Storage",                 //2nd child node
                                    new XElement("Dimensions",          //data node
                                        new XAttribute("Length", "XXX"),//attribute 1
                                        new XAttribute("Width", "XXX"), //attribute 2
                                        new XAttribute("Height", "XXX"),//attribute 3
                                        new XAttribute("Weight", "XXX") //attribute 4
                                    ),
                                ""), //end of 2nd child node

                                new XElement("ExternalDatabase",        //3rd child node
                                    new XElement("Data",                //data node
                                        new XAttribute("Type", "XXX"),  //attribute 1
                                        new XAttribute("Server", "XXX"),//attribute 2
                                        new XAttribute("Database", "XXX"),//attribute 3
                                        new XAttribute("UserID", "XXX"),//attribute 4
                                        new XAttribute("Password","XXX")//attribute 5
                                    ),
                                ""), //end of 3rd child node

                                new XElement("Stacks",                  //4th child node
                                    new XElement("Data",                //data node
                                        new XAttribute("Tablename", "XXX"),//first attribute
                                        new XAttribute("Filename", "XXX")  //2nd attribute
                                    ),
                                ""),  //end of last child node

                    ""); //end of root node
                xmlNode.Save(fqPath);
            }
            else
            {
                //file exists, do nothing
            }

        }

        /// <summary>
        /// Writes given last project save path into given settings fully qualified settings file path.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="lastProjectSavePath"></param>
        public static void WritePathSetting(string workingFolder, string xmlFileName, string lastProjectSavePath)
        {
            var fqXMLFileName = Path.Combine(workingFolder, xmlFileName);
            if (File.Exists(fqXMLFileName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fqXMLFileName);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.SettingsSettingStructure);
                node.Attributes[1].Value = lastProjectSavePath;
                xmlDoc.Save(fqXMLFileName);
            }
            else
            {
                throw new FileNotFoundException("Given XML File does not exist!");
            }

        }

        /// <summary>
        /// Writes given theme into given fully qualified settings file path
        /// </summary>
        /// <param name="fullyQualifiedSettingsFilePath"></param>
        /// <param name="lastThemeUsed"></param>
        public static void WriteThemeSetting(string fullyQualifiedSettingsFilePath, string xmlFileName, string lastThemeUsed)
        {
            var fqXMLFileName = Path.Combine(fullyQualifiedSettingsFilePath, xmlFileName);
            if (File.Exists(fqXMLFileName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fqXMLFileName);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.SettingsSettingStructure);
                node.Attributes[0].Value = lastThemeUsed;
                xmlDoc.Save(fqXMLFileName);
            }
            else
            {
                throw new FileNotFoundException("Given XML File does not exist!");
            }
        }

        /// <summary>
        /// Sets the project name in the project XML file.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="projectName"></param>
        public static void SaveProjectName(string workingFolder, string xmlFileName)
        {
            var fqXMLFileName = Path.Combine(workingFolder, xmlFileName);
            string[] file = xmlFileName.Split('.');
            if(File.Exists(fqXMLFileName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fqXMLFileName);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectNameStructure);
                node.Attributes[0].Value = file[0]; //only one attribute in this node
                xmlDoc.Save(fqXMLFileName);
            } else
            {
                throw new FileNotFoundException("Given XML File does not exist!");
            }            
        }

        /// <summary>
        /// Sets the project data base table name and database file in the project XML file.
        /// </summary>
        /// <param name="dBaseFileName"></param>
        /// <param name="dBaseTableName"></param>
        public static void SaveDatabase(string workingFolder, string xmlFileName, string dBaseFileName)
        {
            var fqXMLFileName = Path.Combine(workingFolder, xmlFileName);
            string[] file = dBaseFileName.Split('.');
            if(File.Exists(fqXMLFileName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fqXMLFileName);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectStackDataStructure);
                node.Attributes[0].Value = file[0]; //database table name
                node.Attributes[1].Value = dBaseFileName; //database file name
                xmlDoc.Save(fqXMLFileName);
            } else
            {
                throw new FileNotFoundException("Given XML File does not exist!");
            }
            
        }

        /// <summary>
        /// Sets the storage area for the project XML file.
        /// DimensionData shall be organized thus: length, width, height, weight capacity.
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="dimensionsDataArray"></param>
        public static void SaveDimensions(string workingFolder, string xmlFileName, string[] dimensionsDataArray)
        {
            var fqXMLFileName = Path.Combine(workingFolder, xmlFileName);
            if (File.Exists(fqXMLFileName))
            {
                if (dimensionsDataArray.Length == 4)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fqXMLFileName);
                    XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectRoomDimensionsStructure);
                    node.Attributes[0].Value = dimensionsDataArray[0]; //length
                    node.Attributes[1].Value = dimensionsDataArray[1]; //width
                    node.Attributes[2].Value = dimensionsDataArray[2]; //height
                    node.Attributes[3].Value = dimensionsDataArray[3]; //weight capacity
                    xmlDoc.Save(fqXMLFileName);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Array must contain indices for length, width, height, and weight!");
                }
            } else
            {
                throw new FileNotFoundException("Given XML File does not exist!");
            }
        }

        /// <summary>
        /// Write external database values to the given project XML file
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="xmlFileName"></param>
        /// <param name="externalDBData"></param>
        public static void StoreExternalDBData(string workingFolder, string xmlFileName, string type, string[] externalDBData)
        {
            var fqXMLFileName = Path.Combine(workingFolder, xmlFileName);

            if (File.Exists(fqXMLFileName))
            {
                if (externalDBData.Length == 4)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fqXMLFileName);
                    XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectExternalDatabaseStructure);
                    node.Attributes[0].Value = type;              //type
                    node.Attributes[1].Value = externalDBData[1]; //server
                    node.Attributes[2].Value = externalDBData[2]; //database
                    node.Attributes[3].Value = externalDBData[3]; //userID
                    node.Attributes[4].Value = externalDBData[4]; //password
                    xmlDoc.Save(fqXMLFileName);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Array must contain indices for type, server, database, userID, and password");
                }
            }
            else
            {
                throw new FileNotFoundException("Given XML file does not exist!");
            }
        }
    }
}

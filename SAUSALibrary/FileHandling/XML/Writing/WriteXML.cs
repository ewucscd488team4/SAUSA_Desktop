using System;
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
        /// <param name="fullyQualifiedFileName"></param>
        public static void WriteBlankXML(string fullyQualifiedFileName) //NOTE this includes the xml file we need to write
        {
            if (!File.Exists(fullyQualifiedFileName)) //ensure we don't have a file 
            {
                var xmlNode =
                    new XElement("Sausa",                               //root of xml node
                                new XElement("ProjName",                //1st inner child node
                                    new XElement("Name",                //data node
                                        new XAttribute("Name", "XXX")   //attribute in data node
                                    ),
                                ""),
                                new XElement("Storage",                 //2nd child node
                                    new XElement("Dimensions",          //data node
                                        new XAttribute("Length", "XXX"),//attribute 1
                                        new XAttribute("Width", "XXX"), //attribute 2
                                        new XAttribute("Height", "XXX"),//attribute 3
                                        new XAttribute("Weight", "XXX") //attribute 4
                                    ),
                                ""),
                                new XElement("Stacks",                  //3rd child node
                                    new XElement("Data",                //data node
                                        new XAttribute("FileName", "XXX"),//first attribute
                                        new XAttribute("Path", "XXX")     //2nd attribute
                                    ),
                                ""),  //3rd inner child node
                    "");
                xmlNode.Save(fullyQualifiedFileName);
            }
            else
            {
                //file exists, do nothing
            }

        }

        /// <summary>
        /// Writes given last project save path into given settings fully qualified settings file path.
        /// </summary>
        /// <param name="fullyQualifiedSettingsFilePath"></param>
        /// <param name="lastProjectSavePath"></param>
        public static void WritePathSetting(string fullyQualifiedSettingsFilePath, string lastProjectSavePath)
        {
            if (File.Exists(fullyQualifiedSettingsFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fullyQualifiedSettingsFilePath);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.SettingsSettingStructure);
                node.Attributes[1].Value = lastProjectSavePath;
                xmlDoc.Save(fullyQualifiedSettingsFilePath);
            }
            else
            {
                //TODO throw error dialog if project file does not exist
            }

        }

        /// <summary>
        /// Writes given theme into given fully qualified settings file path
        /// </summary>
        /// <param name="fullyQualifiedSettingsFilePath"></param>
        /// <param name="lastThemeUsed"></param>
        public static void WriteThemeSetting(string fullyQualifiedSettingsFilePath, string lastThemeUsed)
        {
            if (File.Exists(fullyQualifiedSettingsFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fullyQualifiedSettingsFilePath);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.SettingsSettingStructure);
                node.Attributes[0].Value = lastThemeUsed;
                xmlDoc.Save(fullyQualifiedSettingsFilePath);
            }
            else
            {
                //TODO throw error dialog if project file does not exist
            }
        }

        /// <summary>
        /// Sets the project name in the project XML file.
        /// </summary>
        /// <param name="fullProjectXMLFilePath"></param>
        /// <param name="projectName"></param>
        public static void SaveProjectName(string fullProjectXMLFilePath, string projectName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fullProjectXMLFilePath);
            XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectNameStructure);
            node.Attributes[0].Value = projectName; //only one attribute in this node
            xmlDoc.Save(fullProjectXMLFilePath);
        }

        /// <summary>
        /// Sets the project database name and path in the project XML file.
        /// </summary>
        /// <param name="fullProjectXMLFilePath"></param>
        /// <param name="dBaseName"></param>
        public static void SaveDatabase(string fullProjectXMLFilePath, string dBaseName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fullProjectXMLFilePath);
            XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectDataStructure);
            node.Attributes[0].Value = dBaseName; //database name
            node.Attributes[1].Value = fullProjectXMLFilePath; //database path
            xmlDoc.Save(fullProjectXMLFilePath);
        }

        /// <summary>
        /// Sets the storage area for the project XML file.
        /// DimensionData shall be organized thus: length, width, height, weight capacity.
        /// </summary>
        /// <param name="fullProjectXMLFilePath"></param>
        /// <param name="dimensionsDataArray"></param>
        public static void SaveDimensions(string fullProjectXMLFilePath, string[] dimensionsDataArray)
        {
            if (dimensionsDataArray.Length == 4)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fullProjectXMLFilePath);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.ProjectDimensionsStructure);
                node.Attributes[0].Value = dimensionsDataArray[0]; //length
                node.Attributes[1].Value = dimensionsDataArray[1]; //width
                node.Attributes[2].Value = dimensionsDataArray[2]; //height
                node.Attributes[3].Value = dimensionsDataArray[3]; //weight capacity
                xmlDoc.Save(fullProjectXMLFilePath);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Array must contain indices for length, width, height, and weight!");
            }

        }

    }
}

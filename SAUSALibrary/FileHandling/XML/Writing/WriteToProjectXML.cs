using SAUSALibrary.Defaults;
using System.Xml;

namespace SAUSALibrary.FileHandling.XML.Writing
{
    public class WriteToProjectXML
    {
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
                node.Attributes[1].Value = dimensionsDataArray[1]; //length
                node.Attributes[2].Value = dimensionsDataArray[2]; //length
                node.Attributes[3].Value = dimensionsDataArray[3]; //length
                xmlDoc.Save(fullProjectXMLFilePath);
            }
            else
            {
                //TODO throw error if new storage area array is not the right size
            }

        }
    }
}

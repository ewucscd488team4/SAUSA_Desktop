using System.IO;
using System.Xml;
using SAUSALibrary.Defaults;

namespace SAUSALibrary.FileHandling.XML.Writing
{
    public class WriteSettingsXML
    {
        public static void WritePathSetting(string filePath, string newData)
        {
            if(File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.SettingsSettingStructure);
                node.Attributes[1].Value = newData;
                xmlDoc.Save(filePath);
            } else
            {
                //TODO throw error dialog if project file does not exist
            }

        }

        public static void WriteThemeSetting(string filePath, string newData)
        {
            if(File.Exists(filePath)) {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode node = xmlDoc.SelectSingleNode(XMLDataDefaults.SettingsSettingStructure);
                node.Attributes[0].Value = newData;
                xmlDoc.Save(filePath);
            } else
            {
                //TODO throw error dialog if project file does not exist
            }
        }
    }
}

using SAUSALibrary.Models;
using System.IO;
using System.Xml;
using System;
using System.Xml.Linq;
using System.Linq;

namespace SAUSALibrary.FileHandling.XML.Reading
{
    public class ReadSettingsXML
    {
        /// <summary>
        /// Reads settings from settings XML file and returns them in a settings model.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Settings model</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static SettingsModel ReadSettings(string filePath)
        {
            if(File.Exists(filePath))
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
            } else
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
            
            if(File.Exists(filePath))
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
            } else
            {
                throw new FileNotFoundException("Path to XML file not found!");
            }            
        }
    }
}

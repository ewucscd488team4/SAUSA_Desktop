using SAUSALibrary.Models;
using System.IO;
using System.Xml;
using System;

namespace SAUSALibrary.FileHandling.XML.Reading
{
    public class ReadSettingsXML
    {
        /// <summary>
        /// Reads the settings portion of the settings file and returns them in a settings model class.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static SettingsModel ReadSettings(string filePath)
        {
            SettingsModel model = new SettingsModel();

            try
            {
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
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e);
                model.SettingOne = "blank";
                model.SettingTwo = @"c:\";
                model.SettingThree = @"c:\";
            }

            return model;
        }
    }
}

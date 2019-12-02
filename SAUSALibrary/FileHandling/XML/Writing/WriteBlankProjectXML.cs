using System.IO;
using System.Xml.Linq;

namespace SAUSALibrary.FileHandling.XML.Writing
{
    public class WriteBlankProjectXML
    {
        public static void WriteBlankXML(string fullFilePath) //NOTE this includes the xml file we need to write
        {
            if (!File.Exists(fullFilePath)) //ensure we don't have a file 
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
                xmlNode.Save(fullFilePath);
            }
            else
            {
                //file exists, do nothing
            }

        }
    }
}

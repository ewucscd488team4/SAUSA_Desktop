using SAUSALibrary.Defaults;
using SAUSALibrary.Models;
using SAUSALibrary.FileHandling.Database.Reading;
using System.Collections.ObjectModel;
using System;

namespace CommandLineTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> of fields to add to the project database
            /*List<string> defField = new List<string>
            {
                "0", "0", "0","10", "20", "30", "100", "'Crate'"
            };*/

            //write a new database with the given file name
            //CreateNewSQLiteDatabase.CreateDatabase(@"C:\Users\Diesel\Documents\Sausa\","Hangerbay.sqlite");

            //add new line to the project database file
            //AddDataSQLite.AddSQLiteData(@"C:\Users\Diesel\Documents\Sausa\","Hangerbay.sqlite", defField);

            //write to project XML file and change an attribute
            //WriteToProjectXML.SaveProjectName(FilePathDefaults.SettingsFolder + @"\Test_Write_Settings.xml","ZZZ");

            //test writing to storage dimension part of project XML file
            /*string[] values =
            {
                "YYY","YYY","YYY","YYY"
            };
            WriteToProjectXML.SaveDimensions(FilePathDefaults.SettingsFolder + @"\Test_Write_Settings.xml", values);*/

            //test writing to database area of peoject XML file
            //WriteToProjectXML.SaveDatabase(FilePathDefaults.SettingsFolder + @"\Test_Write_Settings.xml", "ZZZ");

            //testing extracting a "full stack" from the given database
            /*ObservableCollection<StackModel> models = ReadSQLite.GetEntireStack(FilePathDefaults.DefaultSavePath, "Hangerbay.sqlite");
            foreach(StackModel model in models) {
                Console.WriteLine(model.Index + " "
                    + model.XPOS + " "
                    + model.YPOS + " "
                    + model.ZPOS + " "
                    + model.Length + " "
                    + model.Width + " "
                    + model.Height + " "
                    + model.Weight + " "
                    + model.CrateName + " ");
            }*/
        }
    }
}

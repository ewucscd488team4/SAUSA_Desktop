using SAUSALibrary.Defaults;
using SAUSALibrary.Models.Database;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.FileHandling.Database.Writing;
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
            //WriteSQLite.CreateDatabase(FilePathDefaults.DefaultSavePath, "HangerbayTest");

            //add new line to the project database file
            //WriteXML.SaveDatabase(FilePathDefaults.DefaultSavePath + @"\Hangerbay.xml", "Hangerbay", "Hangerbay.sqlite");

            //write to project XML file and change an attribute
            //WriteXML.SaveProjectName(FilePathDefaults.DefaultSavePath + @"\Hangerbay.xml","ZZZ");

            //test writing to storage dimension part of project XML file
            /*string[] values =
            {
                "YYY","YYY","YYY","YYY"
            };
            WriteXML.SaveDimensions(FilePathDefaults.DefaultSavePath + @"\Hangerbay.xml", values);*/

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

            //testing extracting the container list from the given database
            /*ObservableCollection<MiniStackModel> models = ReadSQLite.GetContainerListInfo(FilePathDefaults.ScratchFolder + @"\Hangerbay.sqlite", "Hangerbay.sqlite");
            foreach (MiniStackModel model in models)
            {
                Console.WriteLine("Index: " + model.Index + " Name: " + model.CrateName);
            }*/

            //write out test databse table column headers
            /*ProjectSQLiteDatabaseFieldNamesModel model;
            model = ReadSQLite.GetDatabaseFieldLabels(FilePathDefaults.DefaultSavePath, "Hangerbay");
            Console.WriteLine(model.name);*/

            string testDir = FilePathDefaults.DefaultSavePath + @"\Test.sqlite";
            string testDV = "Test.sqlite";
            ObservableCollection<IndividualDatabaseFieldModel> list = new ObservableCollection<IndividualDatabaseFieldModel>();

            IndividualDatabaseFieldModel field1 = new IndividualDatabaseFieldModel();
            IndividualDatabaseFieldModel field2 = new IndividualDatabaseFieldModel();
            IndividualDatabaseFieldModel field3 = new IndividualDatabaseFieldModel();
            IndividualDatabaseFieldModel field4 = new IndividualDatabaseFieldModel();

            field1.FieldName = "PartNum";
            field2.FieldName = "SerNum";
            field3.FieldName = "FGC";
            field4.FieldName = "Location";

            field1.FieldType = "TEXT";
            field2.FieldType = "TEXT";
            field3.FieldType = "TEXT";
            field4.FieldType = "TEXT";

            list.Add(field1);
            list.Add(field2);
            list.Add(field3);
            list.Add(field4);

            WriteSQLite.CreateProjectDatabase(testDir, testDV);
            WriteSQLite.PopulateCustomProjectDatabase(testDir, testDV, list);

            Console.WriteLine(testDir);

        }

        private static string convertSQLiteFileName(string filename)
        {
            string[] file = filename.Split('.');
            return file[0] + ".sqlite";
        }
    }
}

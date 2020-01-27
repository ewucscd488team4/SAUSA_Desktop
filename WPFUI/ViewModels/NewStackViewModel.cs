using SAUSALibrary.Models.Database;
using System.Collections.Generic;
using SAUSALibrary.DataProcessing;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.IO;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.Database.Writing;
using System.Windows;

namespace WPFUI.ViewModels
{
    public class NewStackViewModel : BaseModel
    {
        #region fields

        //project db file in the scratch folder        

        public string? FieldNameTextField { get; set; } = "Enter Field Name";

        public RelayCommand AddCustomFieldToCustomFieldList { get; }

        public RelayCommand<Window> WriteCustomFieldsToDatabase { get; private set; }

        public RelayCommand DeleteFieldFromCustomFieldList { get; }

        public List<IndividualDatabaseFieldModel>? DropDownFieldList { get; set; }

        public List<IndividualDatabaseFieldModel>? DefaultFieldList { get; set; }

        public ObservableCollection<IndividualDatabaseFieldModel>? NewDBFields { get; set; } = new ObservableCollection<IndividualDatabaseFieldModel>();

        private IndividualDatabaseFieldModel? _SelectedModelOnCustomnFieldList;

        public IndividualDatabaseFieldModel? SelectedModelOnCustomFieldList
        {
            get => _SelectedModelOnCustomnFieldList;
            set => Set(ref _SelectedModelOnCustomnFieldList, value);
        }

        #endregion

        //Constructor
        public NewStackViewModel()
        {            
            DefaultFieldList = GetDefaultFields(); //sets the default fields
            DropDownFieldList = GetFieldsFromEnum(); //sets the drop down fields in the List used to populate the list in the GUI
            SelectedModelOnCustomFieldList = new IndividualDatabaseFieldModel() { FieldName = "BLANK", FieldType = "BLANK" }; //don't know if this is needed or not, but leaving it in here anyway.
            AddCustomFieldToCustomFieldList = new RelayCommand(OnAddField); //sets up the command that adds a new field to the user defined database field list
            WriteCustomFieldsToDatabase = new RelayCommand<Window>(OnWriteFields); //sets up the command that writes default fields and any custom fields into the project database
            DeleteFieldFromCustomFieldList = new RelayCommand(OnDeleteField); //sets up the command that deletes a selected field in the custom field list out of the list.
        }

        private void OnAddField()
        {
            if (!FieldNameTextField.Contains("Enter Field Name") || !string.IsNullOrEmpty(FieldNameTextField))
            {
                SelectedModelOnCustomFieldList.FieldName = FieldNameTextField; //sets the model field name to whatever text was typed into the name box
                NewDBFields.Add(new IndividualDatabaseFieldModel() { FieldName = SelectedModelOnCustomFieldList.FieldName, FieldType = SelectedModelOnCustomFieldList.FieldType }); //adds the new Model to the custom field list with the selected item from the drop down box and the text in the name field.
                RaisePropertyChanged(nameof(NewDBFields)); //tells the View to update
            }
            else
            {
                //TODO throw error dialog that name field can't be empty
            }
        }

        private void OnWriteFields(Window window)
        {
            //TODO write all new database fields in the optional field list to the project database            

            //hard coded folder and database file
            WriteSQLite.PopulateCustomProjectDatabase(FilePathDefaults.ScratchFolder, "Test.sqlite", NewDBFields);

            string[] dbFile = GetProjectDB();

            //dynamic coded database file NOTE: trying to read ProjectSQLiteDBFile from the parent class gets a null, I dont know why
            //WriteSQLite.PopulateCustomProjectDatabase(FilePathDefaults.ScratchFolder, dbFile[0], NewDBFields);

            NewDBFields.Clear();            

            //turn on main window processing fields
            UnityWindowOnOff = System.Windows.Visibility.Visible;
            MainWindowFieldVisibility = System.Windows.Visibility.Visible;

            //turn on full menu options
            FullMenuOnOff = true;

            //initialize the parent class container list and set the "changed" flag
            ParentContainers = ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, "Test.sqlite");
            RaisePropertyChanged(nameof(ParentContainers));

            if(window != null) {
                window.Close();
            }
        }

        /// <summary>
        /// Delete the selected field out of the custom field list.
        /// </summary>
        private void OnDeleteField()
        {
            NewDBFields.Remove(SelectedModelOnCustomFieldList); //removes the clicked on field
            RaisePropertyChanged(nameof(NewDBFields)); //tells the View to update
        }

        #region helper_methods

        /// <summary>
        /// Gets the project db file from the scratch folder for writing
        /// </summary>
        /// <returns></returns>
        private string[] GetProjectDB()
        {
            var scratchFolder = FilePathDefaults.ScratchFolder;

            return Directory.GetFiles(scratchFolder, "*.sqlite");
        }

        /// <summary>
        /// Gets the data types from the DB field enum for populating the field drop down list
        /// </summary>
        /// <returns></returns>
        private List<IndividualDatabaseFieldModel> GetFieldsFromEnum()
        {
            return GetEnums.GetFieldsList();
        }

        /// <summary>
        /// Since SQLite fields are not type bound, this list is manually created for the user's sake.
        /// </summary>
        /// <returns></returns>
        private List<IndividualDatabaseFieldModel> GetDefaultFields()
        {
            List<IndividualDatabaseFieldModel> list = new List<IndividualDatabaseFieldModel>();
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "FIELD NAME", FieldType = "TYPE" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "ID", FieldType = "Integer" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Xpos", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Ypos", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Zpos", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Length", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Width", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Height", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Weight", FieldType = "Float" });
            list.Add(new IndividualDatabaseFieldModel() { FieldName = "Name", FieldType = "VarChar" });
            return list;
        }

        #endregion
    }
}

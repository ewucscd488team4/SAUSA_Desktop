using SAUSALibrary.Models.Database;
using System.Collections.Generic;
using SAUSALibrary.DataProcessing;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.IO;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.Database.Writing;

namespace WPFUI.ViewModels
{
    public class NewStackViewModel : BaseModel
    {
        #region fields

        private string[]? _ProjectDBFile;

        private string? _FullProjectDBFilePath;

        public string? NameField { get; set; } = "Type Name Here";

        public RelayCommand AddFieldToList { get; }

        public RelayCommand WriteFieldsToDatabase { get; }

        public RelayCommand DeleteFieldFromList { get; }

        public List<IndividualDatabaseFieldModel>? DropDownFields { get; set; }

        public List<IndividualDatabaseFieldModel>? DefaultFields { get; set; }

        public ObservableCollection<IndividualDatabaseFieldModel>? NewDBFields { get; set; } = new ObservableCollection<IndividualDatabaseFieldModel>();

        private IndividualDatabaseFieldModel? _Model;

        public IndividualDatabaseFieldModel? Model
        {
            get => _Model;
            set => Set(ref _Model, value);
        }

        #endregion

        //Constructor
        public NewStackViewModel()
        {
            _ProjectDBFile = GetProjectDB(); //gets the project db file from the scratch folder
            DefaultFields = GetDefaultFields(); //sets the default fields
            DropDownFields = GetFieldsFromEnum(); //sets the drop down fields in the List used to populate the list in the GUI
            Model = new IndividualDatabaseFieldModel() { FieldName = "BLANK", FieldType = "BLANK" }; //don't know if this is needed or not, but leaving it in here anyway.
            AddFieldToList = new RelayCommand(OnAddField); //sets up the command that adds a new field to the user defined database field list
            WriteFieldsToDatabase = new RelayCommand(OnWriteFields); //sets up the command that writes default fields and any custom fields into the project database
            DeleteFieldFromList = new RelayCommand(OnDeleteField); //sets up the command that deletes a selected field in the custom field list out of the list.
        }

        private void OnAddField()
        {
            if (!NameField.Contains("Type Name Here") || !string.IsNullOrEmpty(NameField))
            {
                Model.FieldName = NameField; //sets the model field name to whatever text was typed into the name box
                NewDBFields.Add(new IndividualDatabaseFieldModel() { FieldName = Model.FieldName, FieldType = Model.FieldType }); //adds the new Model to the custom field list with the selected item from the drop down box and the text in the name field.
                RaisePropertyChanged(nameof(NewDBFields)); //tells the View to update
            }
            else
            {
                //TODO throw error dialog that name field can't be empty
            }
        }

        private void OnWriteFields()
        {
            //TODO write all new database fields in the optional field list to the project database
            var fqDBFilePath = FilePathDefaults.ScratchFolder + ProjectDBInScratchFile;

            WriteSQLite.PopulateCustomProjectDatabase(fqDBFilePath, ProjectFileName, NewDBFields);

            //turn on main window processing fields
            UnityWindowOnOff = System.Windows.Visibility.Visible;
            MainWindowFieldVisibility = System.Windows.Visibility.Visible;

            //initialize the parent class container list and set the "changed" flag
            ParentContainers = ReadSQLite.GetEntireStack(fqDBFilePath, ProjectDBInScratchFile);
            RaisePropertyChanged(nameof(ParentContainers));
        }

        /// <summary>
        /// Delete the selected field out of the custom field list.
        /// </summary>
        private void OnDeleteField()
        {
            NewDBFields.Remove(Model); //removes the clicked on field
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

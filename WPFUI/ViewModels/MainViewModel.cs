using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SAUSALibrary.DataProcessing;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.Compression;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.Database.Writing;
using SAUSALibrary.Init;
using SAUSALibrary.Models;
using SAUSALibrary.Models.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using WPFUI.Views.ErrorViews;
using WPFUI.Views.FileViews;

namespace WPFUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Variable Declarations

        private const string FILE_FILTER = @"SAUSA files (*.sausa)|*sausa|All Files (*.*)|*.*";

        private const string SAUSA_FILE = ".sausa";

        private const string SQLITE_FILE = ".sqlite";

        private const string XML_FILE = ".xml";

        //full path to the project file EXAMPLE: "C:\Users\Diesel\Documents\Sausa\something.sousa"
        protected string? FullProjectSavePath { get; set; }

        //project file only EXAMPLE: "something.sousa"
        protected string? ProjectFileName { get; set; }

        //project XML file EXAMPLE: "something.XML"
        protected string? ProjectXMLFile { get; set; }

        //project sqlite database file EXAMPLE: "something.sqlite"
        protected string? ProjectSQLiteDBFile { get; set; }

        public string? FieldNameTextField { get; set; } = "Enter Field Name";

        private bool _NewProjectOnOff;

        public bool NewProjectOnOff
        {
            get => _NewProjectOnOff;
            set => Set(ref _NewProjectOnOff, value);
        }

        private bool _OpenProjectOnOff;

        public bool OpenProjectOnOff
        {
            get => _OpenProjectOnOff;
            set => Set(ref _OpenProjectOnOff, value);
        }

        private bool _NewRoomOnOff;

        public bool NewRoomOnOff
        {
            get => _NewRoomOnOff;
            set => Set(ref _NewRoomOnOff, value);
        }

        private bool _NewStackOnOff;

        public bool NewStackOnOff
        {
            get => _NewStackOnOff;
            set => Set(ref _NewStackOnOff, value);
        }

        private bool _FullMenuOnOff;

        public bool FullMenuOnOff
        {
            get => _FullMenuOnOff;
            set => Set(ref _FullMenuOnOff, value);
        }

        private Visibility _UnityWindowOnOff;

        public Visibility UnityWindowOnOff
        {
            get => _UnityWindowOnOff;
            set => Set(ref _UnityWindowOnOff, value);
        }

        private Visibility _CrateListVisibility;

        public Visibility CrateListVisibility
        {
            get => _CrateListVisibility;
            set => Set(ref _CrateListVisibility, value);
        }

        private Visibility _FieldListVisibility;

        public Visibility FieldListVisibility
        {
            get => _FieldListVisibility;
            set => Set(ref _FieldListVisibility, value);
        }
        #endregion

        #region Command Declerations
        public RelayCommand? AddCustomFieldToCustomFieldList { get; private set; }

        public RelayCommand<Window>? WriteCustomFieldsToDatabase { get; private set; }

        public RelayCommand<Window>? ApplyRoomDimensionstoProjectXML { get; private set; }

        public RelayCommand? DeleteFieldFromCustomFieldList { get; private set; }

        public RelayCommand? OpenProjectCommand { get; }

        public RelayCommand? NewProjectCommand { get; }

        public RelayCommand? NewStackCommand { get; }

        public RelayCommand? NewStorageCommand { get; }

        public RelayCommand? SaveCommand { get; }

        public RelayCommand? SaveAsCommand { get; }

        public RelayCommand? CloseCommand { get; }

        public RelayCommand? AddContainerToContainerListCommand { get; }

        public RelayCommand? DeleteContainerFromContainerListCommand { get; }

        #endregion

        #region Lists

        public List<IndividualDatabaseFieldModel>? DropDownFieldList { get; set; }

        public List<IndividualDatabaseFieldModel>? DefaultFieldList { get; set; }

        public ObservableCollection<FullStackModel>? Containers { get; set; } = new ObservableCollection<FullStackModel>();

        public ObservableCollection<IndividualDatabaseFieldModel>? NewDBFields { get; set; } = new ObservableCollection<IndividualDatabaseFieldModel>();

        #endregion

        #region Models

        private StackModel? _AddContainerModel;

        public StackModel? AddContainerModel
        {
            get => _AddContainerModel;
            set => Set(ref _AddContainerModel, value);
        }

        private FullStackModel? _MainWindowContainerListModel;

        public FullStackModel? MainWindowContainerListModel
        {
            get => _MainWindowContainerListModel;
            set => Set(ref _MainWindowContainerListModel, value);
        }

        private IndividualDatabaseFieldModel? _SelectedModelOnCustomnDBFieldList;

        public IndividualDatabaseFieldModel? SelectedModelOnCustomDBFieldList
        {
            get => _SelectedModelOnCustomnDBFieldList;
            set => Set(ref _SelectedModelOnCustomnDBFieldList, value);
        }

        private ProjectDBFieldModel? _FieldModel;

        public ProjectDBFieldModel? FieldModel
        {
            get => _FieldModel;
            set => Set(ref _FieldModel, value);
        }

        #endregion

        public MainViewModel()
        {
            ColdBoot();
            CommandInit();
        }

        #region Command Methods

        private void OnAddField()
        {
            if (!FieldNameTextField.Contains("Enter Field Name") || !string.IsNullOrEmpty(FieldNameTextField))
            {
                SelectedModelOnCustomDBFieldList.FieldName = FieldNameTextField; //sets the model field name to whatever text was typed into the name box
                NewDBFields.Add(new IndividualDatabaseFieldModel() { FieldName = SelectedModelOnCustomDBFieldList.FieldName, FieldType = SelectedModelOnCustomDBFieldList.FieldType }); //adds the new Model to the custom field list with the selected item from the drop down box and the text in the name field.
                RaisePropertyChanged(nameof(NewDBFields)); //tells the View to update
            }
            else
            {
                EmptyCustomDBFieldNameError error = new EmptyCustomDBFieldNameError();
                error.Show();
            }
        }

        private void OnWriteCustomDBFields(Window window)
        {
            //hard coded folder and database file
            WriteSQLite.PopulateCustomProjectDatabase(FilePathDefaults.ScratchFolder, "Test.sqlite", NewDBFields);

            //dynamic coded database file NOTE: trying to read ProjectSQLiteDBFile from the parent class gets a null, I dont know why
            //WriteSQLite.PopulateCustomProjectDatabase(FilePathDefaults.ScratchFolder, dbFile[0], NewDBFields);

            NewDBFields.Clear();

            //turn on main window processing fields
            PostApplyNewStackCustomFields();

            //turn on full menu options
            FullMenuOnOff = true;

            //initialize the parent class container list and set the "changed" flag
            Containers = ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, "Test.sqlite");
            RaisePropertyChanged(nameof(Containers));

            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Delete the selected field out of the custom field list.
        /// </summary>
        private void OnDeleteCustDBField()
        {
            NewDBFields.Remove(SelectedModelOnCustomDBFieldList); //removes the clicked on field
            RaisePropertyChanged(nameof(NewDBFields)); //tells the View to update
        }

        /// <summary>
        /// Language to open an existing project
        /// </summary>
        private void OnOpenProject()
        {
            OpenFileDialog openDlg = new OpenFileDialog
            {
                InitialDirectory = FilePathDefaults.DefaultSavePath,
                Filter = FILE_FILTER,
                DefaultExt = SAUSA_FILE
            };

            if (openDlg.ShowDialog() == true)
            {
                //parent class fields
                ProjectFileName = openDlg.SafeFileName;
                FullProjectSavePath = openDlg.FileName;
                ProjectSQLiteDBFile = ConvertToSQLiteFileName(ProjectFileName);
                ProjectXMLFile = ConvertToXMLFileName(ProjectFileName);

                //disable new project/open project because we have one already open
                NewProjectOnOff = false;
                NewRoomOnOff = false;
                NewStackOnOff = false;

                //enable full menu options                
                FullMenuOnOff = true;
                
                FileCompressionUtils.OpenProject(FullProjectSavePath, FilePathDefaults.ScratchFolder);

                //write project details to settings XML file, in the Projects child node


                //open list of stackmodel to populate the container list                
                Containers = ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);
                RaisePropertyChanged(nameof(Containers));

                //populate field list
                FieldModel = ReadSQLite.GetDatabaseFieldLabels(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);

                //initialize the attribute entry fields
                AddContainerModel = new StackModel();

                //change field visibility to enable use
                CrateListVisibility = Visibility.Visible;
                FieldListVisibility = Visibility.Visible;
                UnityWindowOnOff = Visibility.Visible;

                //TODO dump database to CSV file for unity window to read
            }
            else
            {
                //launch something went wrong window
                SomethingWentWrong wrong = new SomethingWentWrong();
                wrong.Show();
            }
        }

        /// <summary>
        /// Language to open a new project
        /// </summary>        
        private void OnNewProject()
        {
            SaveFileDialog openDlg = new SaveFileDialog
            {
                InitialDirectory = FilePathDefaults.DefaultSavePath,
                Filter = FILE_FILTER,
                DefaultExt = SAUSA_FILE
            };

            if (openDlg.ShowDialog() == true)
            {
                //parent class fields
                ProjectFileName = openDlg.SafeFileName;
                FullProjectSavePath = openDlg.FileName;
                ProjectSQLiteDBFile = ConvertToSQLiteFileName(ProjectFileName);
                ProjectXMLFile = ConvertToXMLFileName(ProjectFileName);

                //full qualified project database path in scratch folder
                //var fqDBFilePath = FilePathDefaults.ScratchFolder + ProjectSQLiteDBFile;

                //this is set to disabled so we can't open a new project again; we have one already open
                OpenProjectOnOff = false;
                NewProjectOnOff = false;

                //enable menu commands to make a new storage room and a new stack
                NewRoomOnOff = true;
                NewStackOnOff = true;

                //MenuState = true; //leave full menu options disabled, as we have nothing to act on with those menu options

                //set up blank project files to the scratch directory
                NewProjectInit.NewProjectDetailOperations(FilePathDefaults.ScratchFolder, ProjectXMLFile, ProjectSQLiteDBFile);
            }
        }

        /// <summary>
        /// Set up a new stack database including all database fields
        /// </summary>        
        private void OnNewStack()
        {
            NewStack newStack = new NewStack();
            //TODO move relevent view model stuff to this view model, and then new stack can work like I want it to
            newStack.DataContext = this;
            newStack.Show();
        }

        /// <summary>
        /// Set up a new store room to store a stack in.
        /// </summary>
        private void OnNewStoreroom()
        {
            NewRoom newRoom = new NewRoom(ProjectXMLFile);
            newRoom.DataContext = this;
            newRoom.Show();
            //TODO let 3d view know that room state has changed
        }

        /// <summary>
        /// Save project in current state.
        /// </summary>
        private void OnSaveProject()
        {
            //compress project XML and SQLite database to save directory
            FileCompressionUtils.SaveProject(FilePathDefaults.ScratchFolder, FullProjectSavePath);

            //TODO write NEW containers in container list to the project database when save menu dialog is involked.

            //TODO write new project save date to appropriate project in settings XML file
        }

        /// <summary>
        /// Open save as dialog to save a project in a directory of the user's discretion
        /// </summary>
        private void OnSaveAs()
        {
            SaveFileDialog saveDlg = new SaveFileDialog
            {
                InitialDirectory = FilePathDefaults.DefaultSavePath,
                Filter = FILE_FILTER,
                DefaultExt = SAUSA_FILE
            };

            if (saveDlg.ShowDialog() == true)
            {
                FullProjectSavePath = saveDlg.FileName;
                //compress working files in scratch folder to given save directory.
                FileCompressionUtils.SaveProject(FilePathDefaults.ScratchFolder, FullProjectSavePath);

                //write given save directory to settings file, LastProjectSavedDirectory attribute.
                //_Savepath set to given save directory, so that save command saves to the right place.

            }
        }

        /// <summary>
        /// Sets the menu to default state, closes the design windows, and deletes the files in the working directory.
        /// </summary>
        private void OnClose()
        {
            //set menu state
            OpenProjectOnOff = true; //so we can open a new project again.
            FullMenuOnOff = false;  //no open project, so all fields not relevent get disabled.
            NewRoomOnOff = false; //project is closed, so disable project related options
            NewStackOnOff = false;

            DirectoryInfo di = new DirectoryInfo(FilePathDefaults.ScratchFolder);
            //hide container list, container fields, and container text boxes
            CrateListVisibility = Visibility.Hidden;
            FieldListVisibility = Visibility.Hidden;

            //hide unity window
            UnityWindowOnOff = Visibility.Hidden;

            //clear the container list
            Containers.Clear();

            //update view
            RaisePropertyChanged(nameof(Containers));

            //clear scratch folder
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Add a container to the container list, and to the 3d Window for placement
        /// </summary>
        private void OnAddContainer()
        {
            if (ContainerFieldValidator())
            {
                //add new container to SQLite database
                Containers.Add(new FullStackModel(Containers.Count + 1, 0, 0, 0, AddContainerModel.Length, AddContainerModel.Width, AddContainerModel.Height, AddContainerModel.Weight, AddContainerModel.CrateName));
                //call update on listbox List to pull new list from SQLite database
                RaisePropertyChanged(nameof(Containers));
                //TODO add new container to project SQLite database when add button is pressed.

                //TODO add new container to 3d view when add button is pressed.
            }
            else
            {
                EmptyContainerError econtainerError = new EmptyContainerError();
                econtainerError.Show();
            }

        }

        /// <summary>
        /// Delete a container from the container list and the 3d window
        /// </summary>
        private void OnDeleteContainer()
        {
            //delete container from container List
            Containers.Remove(MainWindowContainerListModel);

            //call update on view
            RaisePropertyChanged(nameof(Containers));

            //TODO detele container from database when delete button is pressed

            //TODO delete container from 3d view when delete button is pressed
        }

        #endregion

        #region Init methods

        /// <summary>
        /// Sets up the view model to the default main window load state
        /// </summary>
        private void ColdBoot()
        {
            NewProjectOnOff = true;
            OpenProjectOnOff = true;
            NewStackOnOff = false;
            NewRoomOnOff = false;
            FullMenuOnOff = false;
            UnityWindowOnOff = Visibility.Hidden;
            CrateListVisibility = Visibility.Hidden;
            FieldListVisibility = Visibility.Hidden;
        }

        private void CommandInit()
        {
            AddCustomFieldToCustomFieldList = new RelayCommand(OnAddField); //sets up the command that adds a new field to the user defined database field list
            WriteCustomFieldsToDatabase = new RelayCommand<Window>(OnWriteCustomDBFields); //sets up the command that writes default fields and any custom fields into the project database
            DeleteFieldFromCustomFieldList = new RelayCommand(OnDeleteCustDBField); //sets up the command that deletes a selected field in the custom field list out of the list.
        }

        #endregion

        #region State change methods

        private void PostApplyNewStackCustomFields()
        {

        }

        #endregion

        #region Helper Methods

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

        private string ConvertToSQLiteFileName(string filename)
        {
            string[] file = filename.Split('.');
            return file[0] + SQLITE_FILE;
        }

        private string ConvertToXMLFileName(string filename)
        {
            string[] file = filename.Split('.');
            return file[0] + XML_FILE;
        }

        private bool ContainerFieldValidator()
        {
            if (AddContainerModel.Length <= 0)
                return false;
            else if (AddContainerModel.Width <= 0)
                return false;
            else if (AddContainerModel.Height <= 0)
                return false;
            else if (AddContainerModel.Weight <= 0)
                return false;
            else if (string.IsNullOrEmpty(AddContainerModel.CrateName))
                return false;
            return true;
        }

        #endregion
    }
}

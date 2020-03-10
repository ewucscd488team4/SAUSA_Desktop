using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SAUSALibrary.DataProcessing;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.Compression;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.Database.Writing;
using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.FileHandling.Text.Writing;
using SAUSALibrary.Init;
using SAUSALibrary.Models;
using SAUSALibrary.Models.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using WPFUI.Views.ErrorViews;
using WPFUI.Views.FileViews;
using SAUSALibrary.FileHandling.XML.Reading;
using System;
using System.Windows.Threading;
using System.Threading;

namespace WPFUI.ViewModels
{
    /// <summary>
    /// View model for main window, new room, edit external database, and new stack.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region string variable declarations

        //constants associated with SAUSA

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

        //text field that NewStack.xaml uses to accept a new database field name
        public string? NewStack_NewCustomDBFieldNameTextField { get; set; } = "Enter Field Name";

        //test field that ExportData.xaml, ImportData.xaml, and EditExternalDBAttributes.xaml uses
        //to show results of various operations
        private string? _ExternalDB_SuccessOrFail;

        public string? ExternalDB_SuccessOrFail
        {
            get => _ExternalDB_SuccessOrFail;
            set => Set(ref _ExternalDB_SuccessOrFail, value);
        }

        //external database connection settings in string array form
        //this is used to export and import data, AND to display data in the external database paremeters view
        private string[]? _ExternalDBConnectionFields;

        public string[]? ExternalDBConnectionFields
        {
            get => _ExternalDBConnectionFields;
            set => Set(ref _ExternalDBConnectionFields, value);
        }

        private string? _XStorageDimension;
        public string? XStorageDimension
        {
            get => _XStorageDimension;
            set => Set(ref _XStorageDimension, value);
        }

        private string? _YStorageDimension;
        public string? YStorageDimension
        {
            get => _YStorageDimension;
            set => Set(ref _YStorageDimension, value);
        }
        private string? _ZStorageDimension;
        public string? ZStorageDimension
        {
            get => _ZStorageDimension;
            set => Set(ref _ZStorageDimension, value);
        }
        private string? _WeightStorageMax;
        public string? WeightStorageMax
        {
            get => _WeightStorageMax;
            set => Set(ref _WeightStorageMax, value);
        }

        //this is used to keep track of the radio button selected in the external database paremeters view
        public bool[]? ExternalDataBaseTypeArray { get; } = new bool[] { true, false, false };

        #endregion

        #region Enable/Disable boolean values

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

        private bool _AddDelButtons;

        public bool AddDelButtons
        {
            get => _AddDelButtons;
            set => Set(ref _AddDelButtons, value);
        }

        private bool _SaveSaveAsClose;

        public bool SaveSaveAsClose
        {
            get => _SaveSaveAsClose;
            set => Set(ref _SaveSaveAsClose, value);
        }

        private bool _ExternalDBFieldsHaveBeenTested;

        public bool ExternalDBFieldsHaveBeenTested
        {
            get => _ExternalDBFieldsHaveBeenTested;
            set => Set(ref _ExternalDBFieldsHaveBeenTested, value);
        }
        #endregion

        #region Visibility values

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

        private Visibility _TestFieldVisibility;

        public Visibility TestFieldVisibility
        {
            get => _TestFieldVisibility;
            set => Set(ref _TestFieldVisibility, value);
        }
        #endregion

        #region Command Declerations
        public RelayCommand? AddCustomFieldToCustomFieldList { get; private set; }

        public RelayCommand<Window>? WriteCustomFieldsToDatabase { get; private set; }

        public RelayCommand<Window>? ApplyRoomDimensionstoProjectXML { get; private set; }

        public RelayCommand? DeleteFieldFromCustomFieldList { get; private set; }

        public RelayCommand? OpenProjectCommand { get; private set; }

        public RelayCommand? NewProjectCommand { get; private set; }

        public RelayCommand? NewStackCommand { get; private set; }

        public RelayCommand? NewStorageCommand { get; private set; }

        public RelayCommand? SaveCommand { get; private set; }

        public RelayCommand? SaveAsCommand { get; private set; }

        public RelayCommand? CloseCommand { get; private set; }

        public RelayCommand? LaunchExportCommand { get; private set; }

        public RelayCommand? LaunchImportCommand { get; private set; }

        public RelayCommand? ImportCommand { get; private set; }

        public RelayCommand? ExportCommand { get; private set; }

        public RelayCommand? AddContainerToContainerListCommand { get; private set; }

        public RelayCommand? DeleteContainerFromContainerListCommand { get; private set; }

        public RelayCommand<Window>? ExternalDB_ApplyDBSettingsCommand { get; private set; }

        public RelayCommand? ExternalDB_TestDatabaseSettings { get; private set; }

        public RelayCommand? ExternalDB_InitializeProjectTable { get; private set; }

        #endregion

        #region Lists

        public List<IndividualDatabaseFieldModel>? NewStack_DropDownDBFieldTypeList { get; set; }

        public List<IndividualDatabaseFieldModel>? NewStack_DefaultDBFieldList { get; set; }

        public ObservableCollection<FullStackModel>? Containers { get; set; } = new ObservableCollection<FullStackModel>();

        public ObservableCollection<IndividualDatabaseFieldModel>? NewStack_CustomDBFieldsList { get; set; } = new ObservableCollection<IndividualDatabaseFieldModel>();

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
            InitStorageFields();
            InitExternalDBFields();
        }

        #region Init methods

        /// <summary>
        /// Sets up the view model to the default main window load state
        /// </summary>
        private void ColdBoot()
        {
            //we need to open a project or make a new one, so make these two true
            NewProjectOnOff = true;
            OpenProjectOnOff = true;
            //set everything else to false
            NewStackOnOff = false;
            NewRoomOnOff = false;
            SaveSaveAsClose = false;
            FullMenuOnOff = false;
            AddDelButtons = false;
            //set everything to hidden
            UnityWindowOnOff = Visibility.Hidden;
            CrateListVisibility = Visibility.Hidden;
            FieldListVisibility = Visibility.Hidden;
            TestFieldVisibility = Visibility.Hidden;
        }

        /// <summary>
        /// Initialize all relaycommand menu and button press commands
        /// </summary>
        private void CommandInit()
        {
            AddCustomFieldToCustomFieldList = new RelayCommand(OnAddCustomFieldToCustomDBFieldList);    // 1 sets up the command that adds a new field to the user defined database field list
            WriteCustomFieldsToDatabase = new RelayCommand<Window>(OnWriteCustomDBFields);              // 2 sets up the command that writes default fields and any custom fields into the project database
            ApplyRoomDimensionstoProjectXML = new RelayCommand<Window>(OnApplyRoomDimensions);          // 3 sets up the command that applies storage dimensions to a new project storage room
            DeleteFieldFromCustomFieldList = new RelayCommand(OnDeleteCustDBField);                     // 4 sets up the command that deletes a selected field in the custom field list out of the list.
            OpenProjectCommand = new RelayCommand(OnOpenProject);                                       // 5 sets up the command that opens an existing project.
            NewProjectCommand = new RelayCommand(OnNewProject);                                         // 6 sets up the command that makes a new project.
            NewStackCommand = new RelayCommand(OnNewStack);                                             // 7 sets up the command that opens the view which defines the database fields for a new container stack.
            NewStorageCommand = new RelayCommand(OnOpenNewStoreroom);                                   // 8 sets up the command that opens the view which defines the storage dimensions for a new project storage room dimensions.
            SaveCommand = new RelayCommand(OnSaveProject);                                              // 9 sets up the command that saves the current open project as it current state.
            SaveAsCommand = new RelayCommand(OnSaveAs);                                                 //10 sets up the command that saves the current open project as a file name and a location of the user's choice.
            CloseCommand = new RelayCommand(OnClose);                                                   //11 sets up the command that closes the currently open project, abandoning any unsaved changes.
            LaunchImportCommand = new RelayCommand(OnLaunchImport);                                     //12 sets up the command that launches the import from external database fiew
            LaunchExportCommand = new RelayCommand(OnLaunchExport);                                     //13 sets up the command that launches the export to external database view
            ImportCommand = new RelayCommand(OnImport);                                                 //14 sets up the command that imports data from an external database
            ExportCommand = new RelayCommand(OnExport);                                                 //15 sets up the command that exports data to an external database
            ExternalDB_ApplyDBSettingsCommand = new RelayCommand<Window>(OnApplyExternalDBSettings);    //16 sets up the command that applies external database connection settings to the project SXML file
            ExternalDB_TestDatabaseSettings = new RelayCommand(OnTestExternalDBConnectionSettings);     //17 sets up the command that tests external database connection settings
            ExternalDB_InitializeProjectTable = new RelayCommand(OnInitDefaultDatabaseToExternalDB);    //18 sets up the command that initialized a table in the designated external database to export to
            AddContainerToContainerListCommand = new RelayCommand(OnAddContainer);                      //19 sets up the command that adds a new container to the container list on the main page view.
            DeleteContainerFromContainerListCommand = new RelayCommand(OnDeleteContainer);              //20 sets up the command that deletes the selected container from the container list on the main page view.            
        }

        /// <summary>
        /// Initialize the lists used with New Stack view
        /// </summary>
        private void InitializeNewStackLists()
        {
            NewStack_DefaultDBFieldList = GetDefaultDatabaseFields(); //sets the default fields
            NewStack_DropDownDBFieldTypeList = GetDropDownDBFIeldTypes(); //sets the drop down fields in the List used to populate the list in the GUI
            SelectedModelOnCustomDBFieldList = new IndividualDatabaseFieldModel() { FieldName = "BLANK", FieldType = "BLANK" }; //don't know if this is needed or not, but leaving it in here anyway.
        }

        /// <summary>
        /// Initialize the main window lists for use with adding new containers
        /// </summary>
        private void InitializeMainWindowFields()
        {
            //populate field list
            FieldModel = ReadSQLite.GetDatabaseFieldLabels(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);

            //initialize the attribute entry fields
            AddContainerModel = new StackModel();
        }

        /// <summary>
        /// initialize storage fields for use with the new room view
        /// </summary>
        private void InitStorageFields()
        {
            XStorageDimension = "0";
            YStorageDimension = "0";
            ZStorageDimension = "0";
            WeightStorageMax = "0";
        }

        /// <summary>
        /// Sets up the external DB connectivity fields for use in edit external DB attributes view
        /// </summary>
        private void InitExternalDBFields()
        {
            ExternalDBConnectionFields = new string[] { "Please", "Change", "Me", "Thanks" };
            ExternalDBFieldsHaveBeenTested = false;
            ExternalDB_SuccessOrFail = "Press Test to test your entries";
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds a custom DB field to the custom DB field list. Used in NewStack
        /// </summary>
        private void OnAddCustomFieldToCustomDBFieldList()
        {
            if (!NewStack_NewCustomDBFieldNameTextField.Contains("Enter Field Name") || !string.IsNullOrEmpty(NewStack_NewCustomDBFieldNameTextField))
            {
                SelectedModelOnCustomDBFieldList.FieldName = NewStack_NewCustomDBFieldNameTextField; //sets the model field name to whatever text was typed into the name box
                NewStack_CustomDBFieldsList.Add(new IndividualDatabaseFieldModel() { FieldName = SelectedModelOnCustomDBFieldList.FieldName, FieldType = SelectedModelOnCustomDBFieldList.FieldType }); //adds the new Model to the custom field list with the selected item from the drop down box and the text in the name field.
                RaisePropertyChanged(nameof(NewStack_CustomDBFieldsList)); //tells the View to update
            }
            else
            {
                EmptyCustomDBFieldNameError error = new EmptyCustomDBFieldNameError();
                error.Show();
            }
        }

        /// <summary>
        /// Writes the custom DB fields (if any) to the project DB. Used in NewStack
        /// </summary>
        /// <param name="window"></param>
        private void OnWriteCustomDBFields(Window window)
        {
            //write project database fields with defaults and any custom ones as defined by the user
            WriteSQLite.PopulateCustomProjectDatabase(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile, NewStack_CustomDBFieldsList);

            //write the project DB table name and DB file to the project XML file
            WriteXML.SaveDatabase(FilePathDefaults.ScratchFolder, ProjectXMLFile, ProjectSQLiteDBFile);

            //clear the custom field list
            NewStack_CustomDBFieldsList.Clear();

            //apply view state approprate to the current project state
            PostApplyNewStackCustomFields();

            //initialize the container list and set the "changed" flag
            Containers = ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);
            RaisePropertyChanged(nameof(Containers));

            //initialize container property list and the model used for adding containers to the container list
            InitializeMainWindowFields();

            if (window != null)
            {
                window.Close();
            }
        }

        private void OnApplyRoomDimensions(Window window)
        {
            if(RoomDimensionFieldValidator())
            {
                //create string array to write room dimensions 
                string[] NewRoomDimensions = { XStorageDimension, YStorageDimension, ZStorageDimension, WeightStorageMax };

                //send array to the write method
                WriteXML.SaveDimensions(FilePathDefaults.ScratchFolder, ProjectXMLFile, NewRoomDimensions);

                //TODO write room dimensions to the CSV file unity looks at
                WriteText.WriteRoomDimensionsToCSV(NewRoomDimensions);

                //set view state appropriate to project state
                NewProjectWithRoomNoStack();

                //close the window
                if (window != null)
                {
                    window.Close();
                }
            } else
            {
                NewRoomErrorDialog error = new NewRoomErrorDialog();
                error.Show();
            }
        }

        /// <summary>
        /// Delete the selected field out of the custom DB field list. Used in NewStack
        /// </summary>
        private void OnDeleteCustDBField()
        {
            NewStack_CustomDBFieldsList.Remove(SelectedModelOnCustomDBFieldList); //removes the clicked on field
            RaisePropertyChanged(nameof(NewStack_CustomDBFieldsList)); //tells the View to update
        }

        /// <summary>
        /// Language to open an existing project. Used in MainWindow
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
                
                //extract the save project to the scratch folder
                FileCompressionUtils.OpenProject(FullProjectSavePath, FilePathDefaults.ScratchFolder);

                //write project details to settings XML file, in the Projects child node
                //TODO -ignore- write opened project details to setting XML file

                //open list of stackmodel to populate the container list                
                Containers = ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);
                RaisePropertyChanged(nameof(Containers));

                //populate external database fields from the project xml file in case we want to export
                //TODO -Mark- grab external project database attributes from the project XML file and populate the model.

                //initialize container property list and the model used for adding containers to the container list
                InitializeMainWindowFields();

                //change field visibility to enable use
                OpenProjectState();

                //write out the CSV files for the unity window to initialize with
                WriteCSVForUnityInit(ProjectSQLiteDBFile, ProjectXMLFile);

                //prompt the unity window to load the project stack
                //TODO write CSV for project stack so unity can read it and display it.

                //Start the FileSystemWatcher!!!
                MonitorCSV();
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
                                
                //set view state appropriate to our current project state
                NewProjectNoRoomNoStack();

                //set up blank project files to the scratch directory
                NewProjectInit.NewProjectDetailOperations(FilePathDefaults.ScratchFolder, ProjectXMLFile, ProjectSQLiteDBFile);

                //write project name to project XML file
                WriteXML.SaveProjectName(FilePathDefaults.ScratchFolder, ProjectXMLFile);

                //and we are done with project set up
            }
        }

        /// <summary>
        /// Set up a new stack database including all database fields
        /// </summary>        
        private void OnNewStack()
        {
            NewStack newStack = new NewStack();
            InitializeNewStackLists();
            newStack.DataContext = this;
            newStack.Show();
        }

        /// <summary>
        /// Set up a new store room to store a stack in.
        /// </summary>
        private void OnOpenNewStoreroom()
        {
            NewRoom newRoom = new NewRoom();
            newRoom.DataContext = this;
            newRoom.Show();            
        }

        /// <summary>
        /// Save project in current state.
        /// </summary>
        private void OnSaveProject()
        {
            //compress project XML and SQLite database to save directory
            FileCompressionUtils.SaveProject(FilePathDefaults.ScratchFolder, FullProjectSavePath);

            //TODO write NEW containers in container list to the project database when save menu dialog is invoked.

            //TODO -ignore- write new project save date to appropriate project in settings XML file
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

                //TODO -mark- write given save directory to settings file, LastProjectSavedDirectory attribute.


                //TODO -ignore- update settings XML with new project save time
            }
        }

        /// <summary>
        /// Sets the menu to default state, closes the design windows, and deletes the files in the working directory.
        /// </summary>
        private void OnClose()
        {            
            //TODO tell unity window to clear room dimensions and any stacks when project is closed

            //clear the scratch folder
            DirectoryInfo di = new DirectoryInfo(FilePathDefaults.ScratchFolder);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            //set view state to cold boot
            ColdBoot();

            //clear the container list
            Containers.Clear();

            //update view
            RaisePropertyChanged(nameof(Containers));

            //TODO remove Close LOGGER
            writer.Close();
        }

        /// <summary>
        /// Import data to a project sqlite database from an already existing external database
        /// </summary>
        private void OnLaunchImport()
        {
            //grab expernal database items from project XML file if we have not done so already.
            //TODO read external DB attributes from project XML file and populate the array with them

            ImportData importData = new ImportData();
            importData.DataContext = this;
            importData.Show();
        }

        /// <summary>
        /// Export project sqlite database to a pre-existing external database
        /// </summary>
        private void OnLaunchExport()
        {
            //grab expernal database items from project XML file if we have not done so already.
            //TODO read external DB attributes from project XML file and populate the array with them

            ExportData exportData = new ExportData();
            exportData.DataContext = this;
            exportData.Show();
        }

        private void OnImport()
        {
            //TODO import data from external DB
        }

        private void OnExport()
        {
            //TODO export data to an external DB
        }



        /// <summary>
        /// Add a container to the container list, and to the 3d Window for placement
        /// </summary>
        /// 
        private void OnAddContainer()
        {
            if (ContainerFieldValidator())
            {

                //Get newStack from DB.
                ObservableCollection<FullStackModel> UpdatedStack = SAUSALibrary.FileHandling.Database.Reading.ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);

                //add new container to container list and call "update" on it
                Containers.Add(new FullStackModel(UpdatedStack[UpdatedStack.Count - 1].Index + 1, 0, 0, 0, AddContainerModel.Length, AddContainerModel.Width, AddContainerModel.Height, AddContainerModel.Weight, AddContainerModel.CrateName));
                RaisePropertyChanged(nameof(Containers));

                //TODO add new container to project SQLite database when add button is pressed.
                WriteSQLite.UpdateDatabasefromFullStackModel(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile, Containers);

                //add new container to 3d view when add button is pressed.
                WriteText.AddFullStackModeltoCSV(System.AppDomain.CurrentDomain.BaseDirectory, new FullStackModel(UpdatedStack[UpdatedStack.Count - 1].Index + 1, 100, 100, 100, AddContainerModel.Length, AddContainerModel.Width, AddContainerModel.Height, AddContainerModel.Weight, AddContainerModel.CrateName));
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
            //delete container from container List and call "update" on the list
            Containers.Remove(MainWindowContainerListModel);
            RaisePropertyChanged(nameof(Containers));
            
           //delete container from database when delete button is pressed
            WriteSQLite.UpdateDatabasefromFullStackModel(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile, Containers);

            //delete container from 3d view when delete button is pressed
            WriteText.WriteDatabasetoCSV(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);
        }

        /// <summary>
        /// Apply external DB settings to the project XML file; closes the view if the settings have been successfully tested and are still valid
        /// </summary>
        /// <param name="window"></param>
        private void OnApplyExternalDBSettings(Window window)
        {
            if(ExternalDBFieldsHaveBeenTested && TestExternalDBSettings(SelectedDBType))
            {
                WriteExternalDBSettingsToProjectXMLFile(SelectedDBType);
                if (window != null)
                {
                    window.Close();
                }
            } else
            {
                EmptyCustomDBFieldNameError error = new EmptyCustomDBFieldNameError();
                error.Show();
            }
        }

        /// <summary>
        /// Tests the given mysql connection paremeters and reports success if they work, fail if they do not.
        /// </summary>
        private void OnTestExternalDBConnectionSettings()
        {
            if (CheckDBFields()) //if default fields have changed
            {                
                if (TestExternalDBSettings(SelectedDBType)) 
                {
                    ExternalDB_SuccessOrFail = "TEST SUCCESSFUL";
                    ExternalDBFieldsHaveBeenTested = true;
                }
                else
                {
                    ExternalDB_SuccessOrFail = "TEST FAILED";
                }
                //set the text box to show the result text in to visible
                TestFieldVisibility = Visibility.Visible;
            }
            else //they haven't changed
            {
                EmptyCustomDBFieldNameError error = new EmptyCustomDBFieldNameError();
                error.Show();
            }
        }

        private void OnInitDefaultDatabaseToExternalDB()
        {
            //if settings work
            if (ExternalDBFieldsHaveBeenTested && TestExternalDBSettings(SelectedDBType))
            {
                if(SelectedDBType == 0)
                {
                    if(WriteExternalDB.SetUp_MSSQLDatabase(ExternalDBConnectionFields, FilePathDefaults.ScratchFolder, ProjectXMLFile))
                    {
                        ExternalDB_SuccessOrFail = "initialization successful";
                    }
                    ExternalDB_SuccessOrFail = "Set up went wrong.";
                }
                if(SelectedDBType == 1)
                {
                    if(WriteExternalDB.SetUp_MySQLDatabase(ExternalDBConnectionFields, FilePathDefaults.ScratchFolder, ProjectXMLFile))
                    {
                        ExternalDB_SuccessOrFail = "initialization successful";
                    }
                    ExternalDB_SuccessOrFail = "Set up went wrong.";
                }
                ExternalDB_SuccessOrFail = "I don't handle Oracle yet.";
            } else
            {
                EmptyCustomDBFieldNameError error = new EmptyCustomDBFieldNameError();
                error.Show();
            }            
        }

        #endregion

        #region State change methods

        /// <summary>
        /// Applies view state appropriate for the completion of the new project work flow
        /// </summary>
        private void PostApplyNewStackCustomFields()
        {
            NewProjectOnOff = false;
            OpenProjectOnOff = false;
            NewRoomOnOff = false;
            NewStackOnOff = false;
            SaveSaveAsClose = true;
            FullMenuOnOff = true;
            AddDelButtons = true;

            UnityWindowOnOff = Visibility.Visible;
            CrateListVisibility = Visibility.Visible;
            FieldListVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Applies view state appropriate for opening a saved project.
        /// </summary>
        private void OpenProjectState()
        {
            NewProjectOnOff = false;
            OpenProjectOnOff = false;
            NewRoomOnOff = false;
            NewStackOnOff = false;
            SaveSaveAsClose = true;
            FullMenuOnOff = true;
            AddDelButtons = true;

            UnityWindowOnOff = Visibility.Visible;
            CrateListVisibility = Visibility.Visible;
            FieldListVisibility = Visibility.Visible;
        }

        private void NewProjectNoRoomNoStack()
        {
            NewProjectOnOff = false;
            OpenProjectOnOff = false;
            NewRoomOnOff = true;
            NewStackOnOff = true;
            SaveSaveAsClose = true;
            FullMenuOnOff = false;
            AddDelButtons = false;

            UnityWindowOnOff = Visibility.Hidden;
            CrateListVisibility = Visibility.Hidden;
            FieldListVisibility = Visibility.Hidden;
        }

        private void NewProjectWithRoomNoStack()
        {
            NewProjectOnOff = false;
            OpenProjectOnOff = false;
            NewRoomOnOff = false;
            NewStackOnOff = true;
            FullMenuOnOff = false;
            AddDelButtons = false;

            UnityWindowOnOff = Visibility.Visible;
            CrateListVisibility = Visibility.Hidden;
            FieldListVisibility = Visibility.Hidden;
        }        

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the data types from the DB field enum for populating the field drop down list
        /// </summary>
        /// <returns></returns>
        private List<IndividualDatabaseFieldModel> GetDropDownDBFIeldTypes()
        {
            return GetEnums.GetFieldsList();
        }

        /// <summary>
        /// Since SQLite fields are not type bound, this list is manually created for the SAUSA user.
        /// </summary>
        /// <returns></returns>
        private List<IndividualDatabaseFieldModel> GetDefaultDatabaseFields()
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

        /// <summary>
        /// Makes sure we have valid (greater than zero) values for new room dimensions
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks to see if room dimensions have changed at all from the default zero; returns false if not
        /// </summary>
        /// <returns></returns>
        private bool RoomDimensionFieldValidator()
        {
            if (string.IsNullOrEmpty(XStorageDimension) || XStorageDimension is "0")
                return false;
            if (string.IsNullOrEmpty(YStorageDimension) || YStorageDimension is "0")
                return false;
            if (string.IsNullOrEmpty(ZStorageDimension) || ZStorageDimension is "0")
                return false;
            if (string.IsNullOrEmpty(WeightStorageMax) || WeightStorageMax is "0")
                return false;
            return true;
        }

        private void WriteCSVForUnityInit(string dbFile, string xmlFile)
        {
            var model = ReadXML.ReadProjectStorage(FilePathDefaults.ScratchFolder, xmlFile);
            string[] dimensions = { model.Length, model.Weight, model.Height, model.Weight };

            //write room dimensions to CSV file
            WriteText.WriteRoomDimensionsToCSV(dimensions);

            //write database to SCV File
            WriteText.WriteDatabasetoCSV(FilePathDefaults.ScratchFolder, dbFile);
        }

        public int SelectedDBType
        {
            get { return Array.IndexOf(ExternalDataBaseTypeArray, true); }
        }

        /// <summary>
        /// Tests external DB connection settings based upon given int representing type (set by radio buttons in external DB settings view)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool TestExternalDBSettings(int type)
        {
            if (type == 0)
            {
                //test MSSQL settings
                return WriteExternalDB.TestMSSQL_DBConnection(ExternalDBConnectionFields);
            }
            else if (type == 1)
            {
                //test MySQL settings
                return WriteExternalDB.TestMySQL_DBConnection(ExternalDBConnectionFields);
            }
            else
            {
                //test Oracle settings
                return false;
            }
        }

        /// <summary>
        /// Writes external database connectivity fields to the project XML file
        /// </summary>
        /// <param name="type"></param>
        private void WriteExternalDBSettingsToProjectXMLFile(int type)
        {
            if (type == 0)
            {
                //writes Microsoft SQL settings
                WriteXML.StoreExternalDBData(FilePathDefaults.ScratchFolder, ProjectXMLFile, "MSSQL", ExternalDBConnectionFields);
            }
            else if (type == 1)
            {
                //writes MySQL settings
                WriteXML.StoreExternalDBData(FilePathDefaults.ScratchFolder, ProjectXMLFile, "MySQL", ExternalDBConnectionFields);
            }
            else
            {
                //Writes Oracle settings
                WriteXML.StoreExternalDBData(FilePathDefaults.ScratchFolder, ProjectXMLFile, "Oracle", ExternalDBConnectionFields);
            }
        }

        /// <summary>
        /// Checks values against the default ones and returns false if any of them have not changed.
        /// </summary>
        /// <returns></returns>
        private bool CheckDBFields()
        {
            if (ExternalDBConnectionFields[0] is "Please")
            {
                return false;
            }
            else if (ExternalDBConnectionFields[1] is "Change")
            {
                return false;
            }
            else if (ExternalDBConnectionFields[2] is "Me")
            {
                return false;
            }
            else if (ExternalDBConnectionFields[3] is "Thanks")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        #endregion

        #region File System Watching CSV from Unity


        private static StreamWriter writer = new StreamWriter("FileSystemWatcherlog.txt", true);

        private void UpdateContainersfromNewStack(ObservableCollection<FullStackModel> NewStackModels)
        {
            Containers.Clear();
            foreach (FullStackModel stack in NewStackModels)
            {
                Containers.Add(stack);
            }
        }

        public void MonitorCSV()
        {

            FileSystemWatcher watcher = new FileSystemWatcher(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "EmbedTest", "ToGUI.csv");

            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = true;

            var switchThreadForFsEvent = (Func<FileSystemEventHandler, FileSystemEventHandler>)((FileSystemEventHandler handler) =>
            (object obj, FileSystemEventArgs e) => Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                    handler(obj, e))));

            Log(watcher.Path);
            watcher.Changed += switchThreadForFsEvent(OnChanged);
            watcher.Created += switchThreadForFsEvent(OnChanged);
            watcher.Deleted += switchThreadForFsEvent(OnChanged);


            watcher.EnableRaisingEvents = true;



            void OnChanged(object source, FileSystemEventArgs e)
            {
                Thread.Sleep(1000);
                //read from the CSV the new positions
                //update the current ObservableCollection of FullStackModels with new positions
                ObservableCollection<FullStackModel> newStack = SAUSALibrary.FileHandling.Text.Reading.ReadText.ComparePositionStackToCurrentStack(SAUSALibrary.FileHandling.Text.Reading.ReadText.ConvertCSVToPositionStack(), Containers);

                //update the local DB with the new info by sending in the newStack with updated positions
                WriteSQLite.UpdateDatabasefromFullStackModel(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile, newStack);

                //update the toUnity.csv by sending in the newStack with updated positions
                WriteText.WriteStackCollectiontoCSV(newStack);

                UpdateContainersfromNewStack(newStack);

                //Refresh and list the new containers in the GUI Window
                RaisePropertyChanged(Containers.Count.ToString());
            }

            static void Log(string message)
            {
                writer.WriteLine();

                string newmessage = System.DateTime.Now.ToString("MM-dd HH:mm:ss") + ": " + message;
                writer.Write(newmessage);
                writer.WriteLine();
            }

            static void LogStack(ObservableCollection<FullStackModel> LogThisStack)
            {
                foreach (FullStackModel LoggableStack in LogThisStack)
                {
                    writer.WriteLine("Name: " + LoggableStack.CrateName + " ID: " + LoggableStack.Index.ToString() + " XPOS: " + LoggableStack.XPOS.ToString() + " YPOS: " + LoggableStack.YPOS.ToString() + " ZPOS: " + LoggableStack.ZPOS.ToString() + " ");
                }
            }



        }
        #endregion
    }
}

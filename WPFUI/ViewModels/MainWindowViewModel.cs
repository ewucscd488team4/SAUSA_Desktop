using SAUSALibrary.Defaults;
using SAUSALibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SAUSALibrary.FileHandling.Compression;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.Database.Writing;
using SAUSALibrary.FileHandling.XML.Reading;
using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.Init;
using WPFUI.Views;
using WPFUI.Views.EditViews;
using WPFUI.Views.ErrorViews;
using WPFUI.Views.FileViews;
using System.IO;
using System.Windows;

namespace WPFUI.ViewModels
{
    public class MainWindowViewModel : BaseModel
    {        
        #region statement definitions

        private const string FILE_FILTER = @"SAUSA files (*.sausa)|*sausa|All Files (*.*)|*.*";

        private const string SAUSA_FILE = ".sausa";

        private const string SQLITE_FILE = ".sqlite";

        private const string XML_FILE = ".xml";

        public ObservableCollection<FullStackModel> Containers { get; set; } = new ObservableCollection<FullStackModel>();

        private bool _OpenProjectState;

        public bool OpenProjectState
        {
            get => _OpenProjectState;
            set => Set(ref _OpenProjectState, value);
        }            
        
        private bool _MenuState;
        public bool MenuState {
            get => _MenuState;
            set => Set(ref _MenuState, value);            
        }

        private bool _ProjectState;
        public bool ProjectState {
            get => _ProjectState;
            set => Set(ref _ProjectState, value);
        }

        private StackModel? _AddContainerModel;

        public StackModel? AddContainerModel
        {
            get => _AddContainerModel;
            set => Set(ref _AddContainerModel, value);
        }

        private FullStackModel? _ContainerListModel;

        public FullStackModel? ContainerListModel
        {
            get => _ContainerListModel;
            set => Set(ref _ContainerListModel, value);
        }

        private Visibility _FieldVisibility;

        public Visibility FieldVisibility
        {
            get => _FieldVisibility;
            set => Set(ref _FieldVisibility, value);
        }

        private ProjectDBFieldModel? _FieldModel;

        public ProjectDBFieldModel? FieldModel
        {
            get => _FieldModel;
            set => Set(ref _FieldModel, value);
        }

        #endregion

        #region commands

        public RelayCommand OpenProjectCommand { get; }

        public RelayCommand NewProjectCommand { get; }

        public RelayCommand NewStackCommand { get; }

        public RelayCommand NewStorageCommand { get; }

        public RelayCommand SaveCommand { get; }

        public RelayCommand SaveAsCommand { get; }

        public RelayCommand CloseCommand { get; }

        public RelayCommand AddContainerCommand { get; }

        public RelayCommand DeleteContainerCommand { get; }

        #endregion

        //constructor
        public MainWindowViewModel()
        {
            FullMenuOnOff = false; //it is assumed no project is open, ergo menu items are disabled by default
            NewRoomNewStackOnOff = false; //it is assumed no project is open, ergo menu items are disabled by default
            NewOpenProjectOnOff = true; //because SAUSA opens in a closed project state by default, this must be enabled
            MainWindowFieldVisibility = Visibility.Hidden;
            UnityWindowOnOff = Visibility.Hidden;
            OpenProjectCommand = new RelayCommand(OnOpenProject);
            NewProjectCommand = new RelayCommand(OnNewProject);
            NewStackCommand = new RelayCommand(OnNewStack);
            NewStorageCommand = new RelayCommand(OnNewStoreroom);
            SaveCommand = new RelayCommand(OnSaveProject);
            SaveAsCommand = new RelayCommand(OnSaveAs);
            CloseCommand = new RelayCommand(OnClose);
            AddContainerCommand = new RelayCommand(OnAddContainer);
            DeleteContainerCommand = new RelayCommand(OnDeleteContainer);            
        }

        #region command methods

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

            if(openDlg.ShowDialog() == true)
            {
                //parent class fields
                ProjectFileName = openDlg.SafeFileName;
                FullProjectSavePath = openDlg.FileName;
                ProjectSQLiteDBFile = ConvertToSQLiteFileName(ProjectFileName);
                ProjectXMLFile = ConvertToXMLFileName(ProjectFileName);

                //disable new project/open project because we have one already open
                NewOpenProjectOnOff = false;
                NewRoomNewStackOnOff = false;

                //enable full menu options                
                FullMenuOnOff = true;

                FileCompressionUtils.OpenProject(FullProjectSavePath, FilePathDefaults.ScratchFolder);

                //write project details to settings XML file, in the Projects child node


                //open list of stackmodel to populate the container list                
                ParentContainers = ReadSQLite.GetEntireStack(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);
                RaisePropertyChanged(nameof(ParentContainers));

                //populate field list
                FieldModel = ReadSQLite.GetDatabaseFieldLabels(FilePathDefaults.ScratchFolder, ProjectSQLiteDBFile);

                //initialize the attribute entry fields
                AddContainerModel = new StackModel();

                //change field visibility to enable use
                MainWindowFieldVisibility = Visibility.Visible;
                UnityWindowOnOff = Visibility.Visible;

                //TODO dump database to CSV file for unity window to read
            } else
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

            if(openDlg.ShowDialog() == true)
            {                
                //parent class fields
                ProjectFileName = openDlg.SafeFileName;
                FullProjectSavePath = openDlg.FileName;
                ProjectSQLiteDBFile = ConvertToSQLiteFileName(ProjectFileName);
                ProjectXMLFile = ConvertToXMLFileName(ProjectFileName);
                
                //full qualified project database path in scratch folder
                //var fqDBFilePath = FilePathDefaults.ScratchFolder + ProjectSQLiteDBFile;

                //this is set to disabled so we can't open a new project again; we have one already open
                OpenProjectState = false;
                NewOpenProjectOnOff = false;

                //enable menu commands to make a new storage room and a new stack
                ProjectState = true;
                NewRoomNewStackOnOff = true;

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
            //newStack.DataContext = this;
            newStack.Show();                        
        }

        /// <summary>
        /// Set up a new store room to store a stack in.
        /// </summary>
        private void OnNewStoreroom()
        {
            NewRoom newRoom = new NewRoom(ProjectXMLFile);
            //newRoom.DataContext = this;
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

            if(saveDlg.ShowDialog() == true)
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
            OpenProjectState = true; //so we can open a new project again.
            MenuState = false;  //no open project, so all fields not relevent get disabled.
            ProjectState = false; //project is closed, so disable project related options

            DirectoryInfo di = new DirectoryInfo(FilePathDefaults.ScratchFolder);
            //hide container list, container fields, and container text boxes
            FieldVisibility = Visibility.Hidden;

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
            if(ContainerFieldValidator())
            {
                //add new container to SQLite database
                Containers.Add(new FullStackModel(Containers.Count + 1, 0, 0, 0, AddContainerModel.Length, AddContainerModel.Width, AddContainerModel.Height, AddContainerModel.Weight, AddContainerModel.CrateName));
                //call update on listbox List to pull new list from SQLite database
                RaisePropertyChanged(nameof(Containers));
                //TODO add new container to project SQLite database when add button is pressed.

                //TODO add new container to 3d view when add button is pressed.
            } else
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
            Containers.Remove(ContainerListModel);

            //call update on view
            RaisePropertyChanged(nameof(Containers));

            //TODO detele container from database when delete button is pressed

            //TODO delete container from 3d view when delete button is pressed
        }

        #endregion

        #region supporting methods             
        
        private string ConvertToSQLiteFileName (string filename)
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

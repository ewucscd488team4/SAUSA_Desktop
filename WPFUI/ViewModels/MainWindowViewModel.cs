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
using System.IO;
using System.Windows;

namespace WPFUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region statement definitions

        private const string FILE_FILTER = @"SAUSA files (*.sausa)|*sausa|All Files (*.*)|*.*";

        private const string SAUSA_FILE = ".sausa";

        private const string SQLITE_FILE = ".sqlite";

        private const string XML_FILE = ".xml";

        private string? _ProjectFileName;

        private string? _ProjectSavePath;

        private string? _ProjectXMLFile;

        private string? _ProjectDB;

        public ObservableCollection<FullStackModel> Containers { get; set; } = new ObservableCollection<FullStackModel>();
        
        private bool _OpenProjectState;

        public bool OpenProjectState
        {
            get => _OpenProjectState;
            set => SetProperty(ref _OpenProjectState, value);
        }            
        
        private bool _MenuState;
        public bool MenuState {
            get => _MenuState;
            set => SetProperty(ref _MenuState, value);            
        }

        private bool _ProjectState;
        public bool ProjectState {
            get => _ProjectState;
            set => SetProperty(ref _ProjectState, value);
        }

        private StackModel _AddContainerModel;

        public StackModel AddContainerModel
        {
            get => _AddContainerModel;
            set => SetProperty(ref _AddContainerModel, value);
        }

        private FullStackModel _ContainerListModel;

        public FullStackModel ContainerListModel
        {
            get => _ContainerListModel;
            set => SetProperty(ref _ContainerListModel, value);
        }

        private Visibility _FieldVisibility;

        public Visibility FieldVisibility
        {
            get => _FieldVisibility;
            set => SetProperty(ref _FieldVisibility, value);
        }

        private ProjectDBFieldModel _FieldModel;

        public ProjectDBFieldModel FieldModel
        {
            get => _FieldModel;
            set => SetProperty(ref _FieldModel, value);
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
            MenuState = false; //it is assumed no project is open, ergo menu items are disabled by default
            ProjectState = false; //it is assumed no project is open, ergo menu items are disabled by default
            OpenProjectState = true; //because SAUSA opens in a closed project state by default, this must be enabled
            FieldVisibility = Visibility.Hidden;            
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
                //variables needed for methods used in this command
                _ProjectFileName = openDlg.SafeFileName;
                _ProjectSavePath = openDlg.FileName;
                _ProjectDB = ConvertToSQLiteFileName(_ProjectFileName);
                _ProjectXMLFile = ConvertToXMLFileName(_ProjectFileName);
                var fqDBFilePath = FilePathDefaults.ScratchFolder + _ProjectDB;                

                OpenProjectState = false; //this is disabled so we can't open a new project again, we have one already open
                ProjectState = true; //enable menu commands to make a new storage room and a new stack
                MenuState = true; //enable full menu options

                FileCompressionUtils.OpenProject(openDlg.FileName, FilePathDefaults.ScratchFolder);
                //write project details to settings file, Projects child node

                //open list of stackmodel to populate the container list                
                Containers = ReadSQLite.GetEntireStack(fqDBFilePath, _ProjectDB);                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Containers)));

                //populate field list
                FieldModel = ReadSQLite.GetDatabaseFieldLabels(fqDBFilePath, _ProjectDB);

                //initialize the attribute entry fields
                AddContainerModel = new StackModel();

                //change field visibility to enable use
                FieldVisibility = Visibility.Visible;                                

                //TODO populate 3d window with all existing containers in the project database
            } else
            {
                //TODO show error dialog that "something went wrong" opening the given project file
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
                //set up blank project files to scratch directory

                //NewProjectInit.NewProjectDetailOperations(openDlg.FileName, openDlg.SafeFileName);
                //WriteXML.WriteBlankXML(FilePathDefaults.ScratchFolder, openDlg.SafeFileName);

                //write blank sqlite database to scratch directory

                //write project name and path to settings file Projects child node
            }            
        }

        /// <summary>
        /// Set up a new stack database including all database fields
        /// </summary>        
        private void OnNewStack()
        {
            NewStack newStack = new NewStack();
            newStack.Show();            
            //TODO let view know state has changed
        }

        /// <summary>
        /// Set up a new store room to store a stack in.
        /// </summary>
        private void OnNewStoreroom()
        {
            NewRoom newRoom = new NewRoom(_ProjectXMLFile);
            newRoom.Show();
            //TODO let view know state has changed
        }

        /// <summary>
        /// Save project in current state.
        /// </summary>
        private void OnSaveProject()
        {
            //TODO Save Project           
            //compress project XML and SQLite database to save directory
            //FileCompressionUtils.SaveProject();
            //write new containers to the project database.
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
                //TODO save current working files to given new save file.
                //compress working files in scratch folder to given save directory.
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
            //clear the container list
            Containers.Clear();
            //update view
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Containers)));
            //clear scratch folder
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }                                    
            //TODO on close menu command turn off 3d view
        }

        /// <summary>
        /// Add a container to the container list, and to the 3d Window for placement
        /// </summary>
        private void OnAddContainer()
        {            
            //add new container to SQLite database
            Containers.Add(new FullStackModel(Containers.Count+1, 0, 0, 0, AddContainerModel.Length, AddContainerModel.Width, AddContainerModel.Height, AddContainerModel.Weight, AddContainerModel.CrateName));
            //call update on listbox List to pull new list from SQLite database
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Containers)));
            //TODO add new container to project SQLite database when add button is pressed.
            //TODO add new container to 3d view when add button is pressed.
        }

        /// <summary>
        /// Delete a container from the container list and the 3d window
        /// </summary>
        private void OnDeleteContainer()
        {
            //delete container from container List
            Containers.Remove(ContainerListModel);
            //call update on view
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Containers)));
            //TODO detele container from database when delete button is pressed
            //TODO delete container from 3d view when delete button is pressed
        }

        #endregion

        #region supporting methods

        private void SetProperty<T>(ref T field, T value,
            [CallerMemberName]string propertyName = null) //<- this is an optional parameter so we dont have to pass a value to use the method
        {
            if (!EqualityComparer<T>.Default.Equals(field, value)) //if using custom classes need to implement equals
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
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

        #endregion
    }
}

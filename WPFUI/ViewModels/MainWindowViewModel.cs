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
using WPFUI.Views;
using System.IO;
using System.Windows;

namespace WPFUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region statement definitions

        private const string FILE_FILTER = @"SDF files (*.sdf)|*sdf|All Files (*.*)|*.*";

        private const string SAUSA_FILE = "sdf";

        private string? _FileName;

        public ObservableCollection<MiniStackModel> Containers { get; } = new ObservableCollection<MiniStackModel>();

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

        private MiniStackModel _ContainerListModel;

        public MiniStackModel ContainerListModel
        {
            get => _ContainerListModel;
            set => SetProperty(ref _ContainerListModel, value);
        }

        private Visibility _ContainerListVisibility;

        public Visibility ContainerListVisibility
        {
            get => _ContainerListVisibility;
            set => SetProperty(ref _ContainerListVisibility, value);
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
            ContainerListVisibility = Visibility.Collapsed;
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

            //if 
            if(openDlg.ShowDialog() == true)
            {                
                _FileName = openDlg.SafeFileName;
                OpenProjectState = false; //this is disabled so we can't open a new project again, we have one already open
                ProjectState = true; //enable menu commands to make a new storage room and a new stack
                FileCompressionUtils.OpenProject(openDlg.FileName, FilePathDefaults.ScratchFolder);
                //write project to settings file recent project section

                //open list of ministackmodel to populate the container list
                ContainerListVisibility = Visibility.Visible;

                //grab crate attributes to populate new crate entry fields

                //TODO populate 3d window with all existing containers in the database
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
            OpenFileDialog openDlg = new OpenFileDialog
            {
                InitialDirectory = FilePathDefaults.DefaultSavePath,
                Filter = FILE_FILTER,
                DefaultExt = SAUSA_FILE
            };

            if(openDlg.ShowDialog() == true)
            {                
                //write blank project file to scratch directory
                //write blank sqlite database to scratch directory
                //write project name and path to settings file
            }            
        }

        /// <summary>
        /// 
        /// </summary>        
        private void OnNewStack()
        {
            NewStack newStack = new NewStack();
            newStack.Show();            
            //TODO let view know state has changed
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnNewStoreroom()
        {
            NewRoom newRoom = new NewRoom(_FileName);
            newRoom.Show();
            //TODO let view know state has changed
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnSaveProject()
        {
            //TODO Save Project           
            //compress project XML and SQLite database to save directory            
        }

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

            } else
            {
                //TODO maybe throw error dialog here?
            }
        }

        /// <summary>
        /// Sets the menu to default state, closes the design windows, and deletes the files in the working directory.
        /// </summary>
        private void OnClose()
        {
            //sets both enabled properties to false, and updates the window
            OpenProjectState = true; //so we can open a new project again
            MenuState = false; 
            ProjectState = false;
            DirectoryInfo di = new DirectoryInfo(FilePathDefaults.ScratchFolder);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            //TODO on close turn off list of containers by setting the list to empty and set the list to disabled
            //TODO on close turn off container attribute list by setting the attribute model to an empty one and disable the list.
            //TODO on colse turn off 3d view
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnAddContainer()
        {
            //add new container to SQLite database
            //call update on listbox List to pull new list from SQLite database
            //TODO add new container to 3d view
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDeleteContainer()
        {
            //delete container from container List
            //delete container from SQLite database
            //TODO delete container from 3d view
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

        #endregion
    }
}

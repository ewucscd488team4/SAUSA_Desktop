using GalaSoft.MvvmLight;
using SAUSALibrary.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFUI.ViewModels
{
    /// <summary>
    /// Base view model class that main window and new stack extend, thereby enabling field visibility to both child view models
    /// </summary>
    public class BaseModel : ViewModelBase
    {
        #region parent class variable definitions

        //full path to the project file EXAMPLE: "C:\Users\Diesel\Documents\Sausa\something.sousa"
        protected string? FullProjectSavePath { get; set; }

        //project file only EXAMPLE: "something.sousa"
        protected string? ProjectFileName { get; set; }

        //project XML file EXAMPLE: "something.XML"
        protected string? ProjectXMLFile { get; set; }

        //project sqlite database file EXAMPLE: "something.sqlite"
        protected string? ProjectSQLiteDBFile { get; set; }

        private bool _NewOpenProjectOnOff;

        public bool NewOpenProjectOnOff
        {
            get => _NewOpenProjectOnOff;
            set => Set(ref _NewOpenProjectOnOff, value);
        }

        private bool _NewRoomNewStackOnOff;

        public bool NewRoomNewStackOnOff
        {
            get => _NewRoomNewStackOnOff;
            set => Set(ref _NewRoomNewStackOnOff, value);
        }

        private bool _FullMenutOnOff;

        public bool FullMenuOnOff
        {
            get => _FullMenutOnOff;
            set => Set(ref _FullMenutOnOff, value);
        }

        private Visibility _UnityWindowOnOff;

        public Visibility UnityWindowOnOff
        {
            get => _UnityWindowOnOff;
            set => Set(ref _UnityWindowOnOff, value);
        }

        private Visibility _MainWindowFieldVisibility;

        public Visibility MainWindowFieldVisibility
        {
            get => _MainWindowFieldVisibility;
            set => Set(ref _MainWindowFieldVisibility, value);
        }

        public ObservableCollection<FullStackModel> ParentContainers { get; set; } = new ObservableCollection<FullStackModel>();

        #endregion
    }
}

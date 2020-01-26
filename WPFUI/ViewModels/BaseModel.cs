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
        protected string? FullProjectSavePath { get; set; }

        protected string? ProjectFileName { get; set; }

        protected string? ProjectXMLInScratchFolder { get; set; }

        protected string? ProjectDBInScratchFile { get; set; }

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

        public ObservableCollection<FullStackModel> ParentContainers { get; set; } = new ObservableCollection<FullStackModel>();

        #endregion
    }
}

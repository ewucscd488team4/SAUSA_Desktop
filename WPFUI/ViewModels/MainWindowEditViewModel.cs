using SAUSALibrary.Defaults;
using SAUSALibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace WPFUI.ViewModels
{
    public class MainWindowEditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region edit specific field declarations
        
        //TODO put relevent fields related to edit menu here

        #endregion

        #region edit commands

        public RelayCommand OpenStorageArea { get; }

        public RelayCommand OpenCrateAttributes { get; }

        public RelayCommand OpenDatabaseParemeters { get; }

        #endregion

        //constructor
        public MainWindowEditViewModel()
        {
            OpenStorageArea = new RelayCommand(OnOpenStorageAreaAttribs);
            OpenCrateAttributes = new RelayCommand(OnOpenCrateAttribs);
            OpenDatabaseParemeters = new RelayCommand(OnOpenDbaseParameters);
        }

        #region command methods

        private void OnOpenStorageAreaAttribs()
        {
            //TODO open edit storage attributes view
        }

        private void OnOpenCrateAttribs()
        {
            //TODO open crate attributes view
        }

        private void OnOpenDbaseParameters()
        {
            //TODO open database parameters view
        }

        #endregion

        #region supporting setProperty method

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

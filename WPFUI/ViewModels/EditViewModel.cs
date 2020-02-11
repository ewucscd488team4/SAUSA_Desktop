using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;

namespace WPFUI.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        #region fields

        public bool[] DataBaseArray { get; } = new bool[] { true, false, false };

        #endregion

        #region commands

        public RelayCommand<Window>? ApplySettings { get; private set; }

        #endregion

        public EditViewModel()
        {
            ApplySettings = new RelayCommand<Window>(OnApplySettings);
        }

        #region command methods

        private void OnApplySettings(Window window)
        {
            WriteDBSettings(SelectedDBType);

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion

        #region helper methods

        public int SelectedDBType
        {
            get { return Array.IndexOf(DataBaseArray, true); }
        }

        private void WriteDBSettings(int type)
        {
            if (type == 0)
            {
                //TODO write MSSQL settings
            }
            else if (type == 1)
            {
                //TODO write MySQL settings
            }
            else
            {
                //TODO write Oracle settings
            }
        }

        #endregion
    }
}

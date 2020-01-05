using SAUSALibrary.Defaults;
using SAUSALibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using WPFUI.Views;

namespace WPFUI.ViewModels
{
    public class MainWindowFileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region statement definitions

        public ObservableCollection<MiniStackModel> _StackContainers { get; } = new ObservableCollection<MiniStackModel>();

        public bool MenuState { get; set; }

        #endregion

        #region commands

        public RelayCommand OpenProjectCommand { get; }

        public RelayCommand NewProjectCommand { get; }

        public RelayCommand NewStackCommand { get; }

        public RelayCommand ExportCSVCommand { get; }

        public RelayCommand CloseCommand { get; }

        #endregion

        //constructor
        public MainWindowFileViewModel()
        {
            MenuState = false; //it is assumed no project is open, ergo menu items are disabled by default
            OpenProjectCommand = new RelayCommand(OnOpenProject);
            NewProjectCommand = new RelayCommand(OnNewProject);
            NewStackCommand = new RelayCommand(OnNewStack);
            ExportCSVCommand = new RelayCommand(OnExportCSV);
            CloseCommand = new RelayCommand(OnClose);
        }

        #region command methods

        private void OnOpenProject()
        {
            //TODO open a project here
        }

        private void OnNewProject()
        {
            //TODO make a new project here
        }

        private void OnClose()
        {
            //TODO close current project
            //TODOset menu state to disabled (MenuState = false)
        }

        private void OnNewStack()
        {
            NewStack newStack = new NewStack();
            newStack.Show();
        }

        private void OnNewRoom()
        {

        }

        private void OnExportCSV()
        {

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

        private void ChangeMenuState()
        {
            if (MenuState)
            {
                MenuState = false;
            }
            else
            {
                MenuState = true;
            }
        }

        #endregion
    }
}

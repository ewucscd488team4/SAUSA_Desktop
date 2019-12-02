using SAUSALibrary.Defaults;
using SAUSALibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace WPFUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region statement definitions

        ObservableCollection<MiniStackModel> People { get; } = new ObservableCollection<MiniStackModel>();

        public bool MenuState { get; set; }

        #endregion

        #region commands

        public ICommand OpenProjectCommand { get; }

        public ICommand NewProjectCommand { get; }

        public ICommand CloseCommand { get; }

        #endregion

        //constructor
        public MainWindowViewModel()
        {
            MenuState = false; //it is assumed no project is open, ergo menu items are disabled by default
            OpenProjectCommand = new Command(OnOpenProject);
            NewProjectCommand = new Command(OnNewProject);
            CloseCommand = new Command(OnClose);
        }

        #region command methods

        private void OnOpenProject()
        {
            //open a project here
        }

        private void OnNewProject()
        {
            //make a new project here
        }

        private void OnClose()
        {
            //close current project
            //set menu state to disabled (MenuState = false)
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

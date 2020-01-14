using GalaSoft.MvvmLight.Command;
using SAUSALibrary.DataProcessing;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.XML.Reading;
using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFUI.ViewModels
{
    public class PreferencesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Private field to hold the last saved directory string being extracted out of the settings file
        /// </summary>
        private string _NewFolder;

        /// <summary>
        /// Setter and getter for setting and getting the save directory. has event propertychangedevent handler wired to it
        /// </summary>
        public string NewFolder
        {
            get => _NewFolder;
            set => SetProperty(ref _NewFolder, value);
        }

        /// <summary>
        /// Private field to hold the light/dark setting string being extracted out of the settings file
        /// </summary>
        private string _LightDark;

        /// <summary>
        /// Setter and getter for setting and getting the light/dark setting. has event propertychangedevent handler wired to it
        /// </summary>
        public string LightDark
        {
            get => _LightDark;
            set => SetProperty(ref _LightDark, value);
        }

        /// <summary>
        /// Value for the text box
        /// </summary>
        private string _OnLoadFolder;

        public string OnLoadFolder
        {
            get => _OnLoadFolder;
            set => SetProperty(ref _OnLoadFolder, value);
        }

        /// <summary>
        /// Private List of themes from the theme enum class
        /// </summary>
        private List<ThemeModel> _Themes;

        /// <summary>
        /// Setter/getter for the themes list
        /// </summary>
        public List<ThemeModel> Themes
        {
            get { return _Themes; }
            set { _Themes = value; }
        }

        /// <summary>
        /// Single theme for use with selected item in combo box
        /// </summary>
        private ThemeModel _Theme;

        /// <summary>
        /// Setter/getter for single theme
        /// </summary>
        public ThemeModel Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }


        /// <summary>
        /// Xaml buttons point to this to execute a button click
        /// </summary>
        public RelayCommand ApplyChangesCommand { get; }

        /// <summary>
        /// Xaml buttons point to this to execute a button click
        /// </summary>
        public RelayCommand OpenFileSelectDialogCommand { get; }

        /// <summary>
        /// Constructor that instantiates both settings fields and turns on the button commands
        /// </summary>
        public PreferencesViewModel()
        {
            OnLoadFolder = FilePathDefaults.DefaultSavePath; //set to the default file save directory in settings
            NewFolder = "XXX";
            LightDark = GetLightDarkFromXML(); //gets default theme setting from settings file
            Themes = GetThemesFromEnumClass(); //gets list of setting types from settings enum class
            Theme = new ThemeModel(); //sets default ThemeModel (will have "blank")
            ApplyChangesCommand = new RelayCommand(OnChangeTheme); //wires up command
            OpenFileSelectDialogCommand = new RelayCommand(OnChangeSaveFolder); //wires up command
        }

        /// <summary>
        /// Method used to perform the actual button press actions
        /// </summary>
        private void OnChangeSaveFolder()
        {
            //opens a new directory dialog and saves the new directory
            //when I can figure out how to do that, I will wire that up here

            //old stuff

            //OpenFileDialog newDialog = new OpenFileDialog();
            //Nullable<bool> result = newDialog.ShowDialog();
            //if(result == true)
            //{
            //write chosen folder into NewFolder
            //}

        }

        /// <summary>
        /// Method used to perform the actual button press actions
        /// </summary>
        private void OnChangeTheme()
        {
            //save the new directory folder into the settings file
            LightDark = Theme.ThemeValue + " written";
            NewFolder = OnLoadFolder + " written";
            SetThemeXML(Theme.ThemeValue);
            SetDirectoryXML(OnLoadFolder);
        }

        private void SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value)) //if using custom classes need to implement equals
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Gets current light/dark setting from settings file and returns it
        /// </summary>
        /// <returns></returns>
        private string GetLightDarkFromXML()
        {
            SettingsModel model = ReadXML.ReadSettings(FilePathDefaults.SettingsFile);
            return model.SettingOne;
        }

        /// <summary>
        /// Gets current last saved directory from setting fine and returns it
        /// </summary>
        /// <returns></returns>
        private string GetSaveFolderFromXML()
        {
            SettingsModel model = ReadXML.ReadSettings(FilePathDefaults.SettingsFile);
            return model.SettingTwo;
        }

        /// <summary>
        /// Retrieves List of enums from enum class
        /// </summary>
        /// <returns></returns>
        private List<ThemeModel> GetThemesFromEnumClass()
        {
            return GetEnums.GetThemesList();
        }


        /// <summary>
        /// Saves data from light/dark setting into the settings file
        /// </summary>
        /// <param></param>
        private void SetThemeXML(string newData)
        {
            WriteXML.WriteThemeSetting(FilePathDefaults.SettingsFile, newData);
        }

        /// <summary>
        /// Saves the new last saved directory text box into the settings file
        /// </summary>
        private void SetDirectoryXML(string newData)
        {
            WriteXML.WritePathSetting(FilePathDefaults.SettingsFile, newData);
        }
    }
}

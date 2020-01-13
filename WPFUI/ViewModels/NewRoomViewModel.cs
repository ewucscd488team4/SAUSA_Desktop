using SAUSALibrary.Defaults;
using SAUSALibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SAUSALibrary.Models.Database;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using SAUSALibrary.FileHandling.Compression;
using SAUSALibrary.FileHandling.Database.Reading;
using SAUSALibrary.FileHandling.Database.Writing;
using SAUSALibrary.FileHandling.XML.Reading;
using SAUSALibrary.FileHandling.XML.Writing;
using System.IO;

namespace WPFUI.ViewModels
{
    public class NewRoomViewModel : INotifyPropertyChanged
    {
        private string[]? _projectDBFile;

        private string? fullProjectDBFilePath;

        public RelayCommand AddFieldToList { get; }
        
        public RelayCommand WriteFieldsToDatabase { get; }

        public FullStackModel? _MandatoryFields;

        public ObservableCollection<IndividualDatabaseFieldModel> Fields { get; set; } = new ObservableCollection<IndividualDatabaseFieldModel>();

        public NewRoomViewModel()
        {
            _projectDBFile = GetProjectDB();
            //FullStackModel = ReadSQLite.ReadAttributeNameType()
            AddFieldToList = new RelayCommand(OnAddField);
            WriteFieldsToDatabase = new RelayCommand(OnWriteFields);
        }

        private void OnAddField()
        {
            //TODO add new database field to the optional list of project database fields
        }

        private void OnWriteFields()
        {
            //TODO write all new database fields in the optional field list to the project database
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string[] GetProjectDB()
        {
            var scratchFolder = FilePathDefaults.ScratchFolder;
            
            return Directory.GetFiles(scratchFolder,"*.sqlite");
        }

        private void SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value)) //if using custom classes need to implement equals
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

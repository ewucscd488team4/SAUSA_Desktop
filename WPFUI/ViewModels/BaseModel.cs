using SAUSALibrary.Models;
using System.Collections.ObjectModel;

namespace WPFUI.ViewModels
{
    public class BaseModel
    {
        protected ObservableCollection<FullStackModel> Containers { get; set; } = new ObservableCollection<FullStackModel>();

        protected string? _ProjectFileName;

        protected string? _ProjectSavePath;

        protected string? _ProjectXMLFile;

        protected string? _ProjectDB;

    }
}

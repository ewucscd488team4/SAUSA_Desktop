using GalaSoft.MvvmLight;
using SAUSALibrary.Models;
using System.Collections.ObjectModel;

namespace WPFUI.ViewModels
{
    public class BaseModel : ViewModelBase
    {
        private bool _ParentProjectOpenState;

        public bool ParentProjectOpenState
        {
            get => _ParentProjectOpenState;
            set => Set(ref _ParentProjectOpenState, value);
        }

        private bool _NewProjectState;

        public bool NewProjectState
        {
            get => _NewProjectState;
            set => Set(ref _NewProjectState, value);
        }

        private bool _FullMenutState;

        public bool FullMenuState
        {
            get => _FullMenutState;
            set => Set(ref _FullMenutState, value);
        }
    }
}

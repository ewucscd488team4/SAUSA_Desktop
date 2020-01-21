using SAUSALibrary.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for NewStack.xaml
    /// </summary>
    public partial class NewStack : Window
    {
        private string? projectDB;

        private ObservableCollection<FullStackModel> containers;

        public NewStack()
        {
            InitializeComponent();
        }

        public NewStack(string? projectDBName, ObservableCollection<FullStackModel> incomingContainers)
        {
            projectDB = projectDBName;
            containers = incomingContainers;
            InitializeComponent();
        }

        private void Accept_Stack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using System.Windows;

namespace WPFUI.Views.EditViews
{
    /// <summary>
    /// Interaction logic for StorageAreaAttributes.xaml
    /// </summary>
    public partial class StorageAreaAttributes : Window
    {
        public StorageAreaAttributes()
        {
            InitializeComponent();
        }

        private void ApplyStorageAttribs_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

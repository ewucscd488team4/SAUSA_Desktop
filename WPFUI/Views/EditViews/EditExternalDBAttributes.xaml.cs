using System.Windows;

namespace WPFUI.Views.EditViews
{
    /// <summary>
    /// Interaction logic for EditExternalDBAttributes.xaml
    /// </summary>
    public partial class EditExternalDBAttributes : Window
    {
        public EditExternalDBAttributes()
        {
            InitializeComponent();
        }

        private void ApplyDatabaseParam_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

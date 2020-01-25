using System.Windows;

namespace WPFUI.Views.EditViews
{
    /// <summary>
    /// Interaction logic for EditStackAttributes.xaml
    /// </summary>
    public partial class EditStackAttributes : Window
    {
        public EditStackAttributes()
        {
            InitializeComponent();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

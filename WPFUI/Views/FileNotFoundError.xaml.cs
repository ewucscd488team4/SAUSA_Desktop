using System.Windows;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for FileNotFoundError.xaml
    /// </summary>
    public partial class FileNotFoundError : Window
    {
        public FileNotFoundError()
        {
            InitializeComponent();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            //closes the Window, all other processing happens in the view model
            this.Close();
        }
    }
}

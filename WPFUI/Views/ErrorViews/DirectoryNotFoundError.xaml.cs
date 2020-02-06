using System.Windows;

namespace WPFUI.Views.ErrorViews
{
    /// <summary>
    /// Interaction logic for DirectoryNotFoundError.xaml
    /// </summary>
    public partial class DirectoryNotFoundError : Window
    {
        public DirectoryNotFoundError()
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

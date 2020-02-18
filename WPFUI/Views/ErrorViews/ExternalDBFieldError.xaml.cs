
using System.Windows;

namespace WPFUI.Views.ErrorViews
{
    /// <summary>
    /// Interaction logic for ExternalDBFieldError.xaml
    /// </summary>
    public partial class ExternalDBFieldError : Window
    {
        public ExternalDBFieldError()
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

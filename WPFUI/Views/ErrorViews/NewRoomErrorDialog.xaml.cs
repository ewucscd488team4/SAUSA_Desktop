using System.Windows;

namespace WPFUI.Views.ErrorViews
{
    /// <summary>
    /// Interaction logic for NewRoomErrorDialog.xaml
    /// </summary>
    public partial class NewRoomErrorDialog : Window
    {
        public NewRoomErrorDialog()
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

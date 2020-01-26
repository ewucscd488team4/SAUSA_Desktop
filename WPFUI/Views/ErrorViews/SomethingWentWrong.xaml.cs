using System.Windows;

namespace WPFUI.Views.ErrorViews
{
    /// <summary>
    /// Interaction logic for SomethingWentWrong.xaml
    /// </summary>
    public partial class SomethingWentWrong : Window
    {
        public SomethingWentWrong()
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

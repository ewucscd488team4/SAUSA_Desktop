using System.Windows;

namespace WPFUI.Views.FileViews
{
    /// <summary>
    /// Interaction logic for OpenBuild.xaml
    /// </summary>
    public partial class OpenBuild : Window
    {
        public OpenBuild()
        {
            InitializeComponent();
        }

        private void Accept_Build(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

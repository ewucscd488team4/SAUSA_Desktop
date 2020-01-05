using System.Windows;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.XML.Reading;
using SAUSALibrary.FileHandling.XML.Writing;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for NewRoom.xaml
    /// </summary>
    public partial class NewRoom : Window
    {
        private string? fileName;

        public NewRoom()
        {
            InitializeComponent();
        }

        public NewRoom(string? fileName)
        {
            this.fileName = fileName;
        }

        private void Accept_Room(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

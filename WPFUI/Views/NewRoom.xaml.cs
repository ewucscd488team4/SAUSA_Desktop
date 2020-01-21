using System.Collections.ObjectModel;
using System.Windows;
using SAUSALibrary.Defaults;
using SAUSALibrary.FileHandling.XML.Reading;
using SAUSALibrary.FileHandling.XML.Writing;
using SAUSALibrary.Models;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for NewRoom.xaml
    /// </summary>
    public partial class NewRoom : Window
    {
        private string? _WorkingFile;

        public string? WorkingFile => _WorkingFile;

        public NewRoom()
        {
            InitializeComponent();
        }

        public NewRoom(string? incomingFileName)
        {            
            this._WorkingFile = incomingFileName;
            InitializeComponent();
        }

        private void CloseNewRoom_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void SetRoomDimensions(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(rLength.Text) || string.IsNullOrEmpty(rWidth.Text) || string.IsNullOrEmpty(rHeight.Text) || string.IsNullOrEmpty(rWeight.Text))
            {
                //TODO launch error dialog for enpty room dimension fields
            }
            else
            {
                string[] dimensions = new string[4];
                dimensions[0] = rLength.Text;
                dimensions[1] = rWidth.Text;
                dimensions[2] = rHeight.Text;
                dimensions[3] = rWeight.Text;
                WriteXML.SaveDimensions(FilePathDefaults.ScratchFolder + _WorkingFile, dimensions);
            }
        }
    }
}

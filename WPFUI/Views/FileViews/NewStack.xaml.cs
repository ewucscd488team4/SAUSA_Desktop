using SAUSALibrary.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFUI.Views.FileViews
{
    /// <summary>
    /// Interaction logic for NewStack.xaml
    /// </summary>
    public partial class NewStack : Window
    {        
        public NewStack()
        {
            InitializeComponent();
        }

        private void Accept_Stack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

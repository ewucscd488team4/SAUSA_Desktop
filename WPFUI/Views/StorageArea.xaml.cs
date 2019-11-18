using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for StorageArea.xaml
    /// </summary>
    public partial class StorageArea : Window
    {
        public StorageArea()
        {
            InitializeComponent();
        }

        private void ApplyStorageAttribs_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

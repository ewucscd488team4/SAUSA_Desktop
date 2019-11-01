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

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for default_main_window.xaml
    /// </summary>
    public partial class default_main_window : Window
    {
        public default_main_window()
        {
            InitializeComponent();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

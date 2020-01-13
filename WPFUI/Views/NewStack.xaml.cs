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
    /// Interaction logic for NewStack.xaml
    /// </summary>
    public partial class NewStack : Window
    {
        private string? projectDB;

        public NewStack()
        {
            InitializeComponent();
        }

        public NewStack(string? projectDBName)
        {
            projectDB = projectDBName;
            InitializeComponent();
        }

        private void Accept_Stack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

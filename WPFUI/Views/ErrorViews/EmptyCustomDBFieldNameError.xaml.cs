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

namespace WPFUI.Views.ErrorViews
{
    /// <summary>
    /// Interaction logic for EmptyCustomDBFieldNameError.xaml
    /// </summary>
    public partial class EmptyCustomDBFieldNameError : Window
    {
        public EmptyCustomDBFieldNameError()
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

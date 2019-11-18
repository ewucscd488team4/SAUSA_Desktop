using Microsoft.Win32;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly string SAUSA_DIR = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + @"\Sausa";
        readonly string TYPE_FILTER = @"SDF files (*.sdf)|*sdf|All Files (*.*)|*.*";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileNewBuild_Click(object sender, RoutedEventArgs e)
        {
            OpenBuild newBuild = new OpenBuild();
            newBuild.Show();
        }

        private void FileNewProject_Click(object sender, RoutedEventArgs e)
        {
            OpenProject newProject = new OpenProject();
            newProject.Show();
        }

        private void FileNewStack_Click(object sender, RoutedEventArgs e)
        {
            NewStack newStack = new NewStack();
            newStack.Show();
        }

        private void FileOpenDesign_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog openDlg = new OpenFileDialog
            {
                InitialDirectory = SAUSA_DIR,
                Filter = TYPE_FILTER,
                DefaultExt = "sdf"
                //FilterIndex = 2,
                //RestoreDirectory = true
        };            
            openDlg.ShowDialog();
        }

        private void FileOpenProject_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog openDlg = new OpenFileDialog
            {
                InitialDirectory = SAUSA_DIR,
                Filter = TYPE_FILTER,
                DefaultExt = "sdf"
                //FilterIndex = 2,
                //RestoreDirectory = true
            };
            openDlg.ShowDialog();
        }

        private void FileSaveAs_Click(object sender, RoutedEventArgs e)
        {            
            SaveFileDialog saveDlg = new SaveFileDialog
            {
                InitialDirectory = SAUSA_DIR,
                Filter=TYPE_FILTER,
                DefaultExt = "sdf"
            };
            saveDlg.ShowDialog();
        }

        private void FilePref_Click(object sender, RoutedEventArgs e)
        {
            Preferences prefs = new Preferences();
            prefs.Show();
        }

        private void EditDatabaseParam_Click(object sender, RoutedEventArgs e)
        {
            DatabaseParameters dataPrefs = new DatabaseParameters();
            dataPrefs.Show();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HelpHelp_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void HelpUpdates_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet", "Oops!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        
    }
}

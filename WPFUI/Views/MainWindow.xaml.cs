using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")] 
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        readonly string SAUSA_DIR = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + @"\Sausa";
        readonly string TYPE_FILTER = @"SDF files (*.sdf)|*sdf|All Files (*.*)|*.*";

        private Process process;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private const int WA_ACTIVE = 1;

        public MainWindow()
        {
            InitializeComponent();
            InitializePanel();
            DataContext = new MainWindowViewModel();
            try
            {
                process = new Process();
                process.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\EmbedTest\\Child.exe";
                process.StartInfo.Arguments = "-parentHWND " + unityPanel.Handle.ToInt32() + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                process.WaitForInputIdle();

                EnumChildWindows(unityPanel.Handle, WindowEnum, IntPtr.Zero);

                unityHWNDLabel.Content = "Unity HWND: 0x" + unityHWND.ToString("X8");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nCannot find Unity file: " + process.StartInfo.FileName);
            }
        }

        private void InitializePanel()
        {
            this.unityPanel.TabIndex = 1;
            this.unityPanel.TabStop = true;
            this.unityPanel.Resize += new System.EventHandler(this.UnityPanelResize);
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

        private void EditStorageAttribs_Click(object sender, RoutedEventArgs e)
        {
            StorageArea storageArea = new StorageArea();
            storageArea.Show();
        }

        private void EditDatabaseParam_Click(object sender, RoutedEventArgs e)
        {
            DatabaseParameters dataPrefs = new DatabaseParameters();
            dataPrefs.Show();
        }

        private void EditCrateAttribs_Click(object sender, RoutedEventArgs e)
        {
            CrateAttributes cratePrefs = new CrateAttributes();
            cratePrefs.Show();
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
            UpdateNotImplemented update = new UpdateNotImplemented();
            update.Show();
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            ActivateUnityWindow();
            return 0;
        }
        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }
        private void UnityPanelResize(object sender, EventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, unityPanel.Width, unityPanel.Height, true);
            ActivateUnityWindow();
        }
    }
}

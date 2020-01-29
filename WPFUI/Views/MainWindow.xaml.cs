using SAUSALibrary.Defaults;
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using WPFUI.ViewModels;
using WPFUI.Views.AboutViews;
using WPFUI.Views.EditViews;
using WPFUI.Views.ErrorViews;
using WPFUI.Views.FileViews;
using WPFUI.Views.WarningViews;

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
        
        private Process? process;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private const int WA_ACTIVE = 1;

        public MainWindow()
        {
            InitializeComponent();
            InitializePanel();
            
            try
            {
                process = new Process();
                process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\EmbedTest\\Child.exe";
                process.StartInfo.Arguments = "-parentHWND " + unityPanel.Handle.ToInt32() + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForInputIdle();

                EnumChildWindows(unityPanel.Handle, WindowEnum, IntPtr.Zero);

                process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nCannot find Unity file: " + process.StartInfo.FileName);
            }
            try
            {
                //Testing communication between Unity and Application
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\EmbedTest\\TestFile.csv", "1,100,5,100,10,10,30,1,Test");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nUnable to create communication file.");
            }
        }

        private void InitializePanel()
        {
            this.unityPanel.TabIndex = 1;
            this.unityPanel.TabStop = true;
            this.unityPanel.Resize += new EventHandler(this.UnityPanelResize);
        }

        private void FileNewDesign_Click(object sender, RoutedEventArgs e)
        {
            NewBuild newBuild = new NewBuild();
            newBuild.Show();
        }

        private void FileOpenDesign_Click(object sender, RoutedEventArgs e)
        {
            OpenBuild openBuild = new OpenBuild();
            openBuild.Show();
        }        

        private void FilePref_Click(object sender, RoutedEventArgs e)
        {
            Preferences prefs = new Preferences();
            prefs.Show();
        }

        private void EditStorageAttribs_Click(object sender, RoutedEventArgs e)
        {
            StorageAreaAttributes storageArea = new StorageAreaAttributes();
            storageArea.Show();
        }

        private void EditDatabaseParam_Click(object sender, RoutedEventArgs e)
        {
            EditExternalDBAttributes editDBAttributes = new EditExternalDBAttributes();
            editDBAttributes.Show();
        }

        private void EditCrateAttribs_Click(object sender, RoutedEventArgs e)
        {
            EditStackAttributes stackAttributes = new EditStackAttributes();
            stackAttributes.Show();
        }

        /// <summary>
        /// Clears the scratch folder and closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(FilePathDefaults.ScratchFolder);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
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

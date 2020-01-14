using SAUSALibrary.Defaults;
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using WPFUI.ViewModels;
using System.IO.Pipes;
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
            DataContext = new MainWindowViewModel();
            try
            {
                process = new Process();
                process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\EmbedTest\\Child.exe";
                process.StartInfo.Arguments = "-parentHWND " + unityPanel.Handle.ToInt32() + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                ////Piping Code
                //using (AnonymousPipeServerStream pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
                //{
                //    try
                //    {
                //        using (StreamWriter sw = new StreamWriter(pipeServer))
                //        {
                //            sw.AutoFlush = true;
                //            sw.WriteLine("SYNC");
                //            pipeServer.WaitForPipeDrain();
                //            sw.WriteLine("Hello");
                //        }
                //    }
                //    catch (IOException e)
                //    {

                //    }
                //}
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
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\EmbedTest\\TestFile.txt", "[10, 10, 20]");
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

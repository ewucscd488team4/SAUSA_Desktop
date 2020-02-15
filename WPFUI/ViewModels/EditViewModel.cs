using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SAUSALibrary.FileHandling.Database.Writing;
using System;
using System.Windows;

namespace WPFUI.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        #region test data

        private const string _SmarterASPTestServer = "MYSQL5022.site4now.net";

        private const string _SmarterASPTestDB = "db_a4733c_sausa";

        private const string _SmarterASPTestUserID = "a4733c_sausa";

        private const string _SmarterASPTestPassword = "Sausa#1test";

        private string[] _HardCodedTestDBFields = new string[] { _SmarterASPTestServer, _SmarterASPTestDB, _SmarterASPTestUserID, _SmarterASPTestPassword };

        #endregion

        #region fields

        private string? _TestSuccessOrFailString;

        public string? TestSuccessOrFailString
        {
            get => _TestSuccessOrFailString;
            set => Set(ref _TestSuccessOrFailString, value);
        }

        public bool[]? DataBaseArray { get; } = new bool[] { true, false, false };

        private string[]? _DBFieldData;

        public string[]? DBFieldData
        {
            get => _DBFieldData;
            set => Set(ref _DBFieldData, value);
        }

        private Visibility _TestFieldVisibility;

        public Visibility TestFieldVisibility
        {
            get => _TestFieldVisibility;
            set => Set(ref _TestFieldVisibility, value);
        }

        #endregion

        #region commands

        public RelayCommand<Window>? ApplyDatabaseSettings { get; private set; }
        public RelayCommand? TestDatabaseSettings { get; private set; }

        #endregion

        public EditViewModel()
        {
            TestFieldVisibility = Visibility.Hidden;
            ApplyDatabaseSettings = new RelayCommand<Window>(OnApplySettings);
            TestDatabaseSettings = new RelayCommand(OnTestDataBaseSettings);
            DBFieldData = new string[] { "WW", "XX", "YY", "ZZ" };
            TestSuccessOrFailString = "Press Test to test your entries";
        }

        #region command methods

        private void OnApplySettings(Window window)
        {
            WriteDBSettings(SelectedDBType);

            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Tests the given mysql connection paremeters and reports success if they work, fail if they do not.
        /// </summary>
        private void OnTestDataBaseSettings()
        {
            if (!CheckDBFields())
            {
                //if (WriteExternalDB.TestMySQL_DBConnection(DBFieldData)) //use field data from view
                if (WriteExternalDB.TestMySQL_DBConnection(_HardCodedTestDBFields)) //hard coded to work with smarterASP test database; for testing
                {
                    TestSuccessOrFailString = "TEST SUCCESSFUL";
                }
                else
                {
                    TestSuccessOrFailString = "TEST FAILED";
                }

                //set the text box to show the result text in to visible
                TestFieldVisibility = Visibility.Visible;


            }
            else
            {
                //TODO launch an error view complaining about the database fields
            }


        }

        #endregion

        #region helper methods

        public int SelectedDBType
        {
            get { return Array.IndexOf(DataBaseArray, true); }
        }

        private void WriteDBSettings(int type)
        {
            if (type == 0)
            {
                //TODO write MSSQL settings
            }
            else if (type == 1)
            {
                //TODO write MySQL settings
            }
            else
            {
                //TODO write Oracle settings
            }
        }

        private bool CheckDBFields()
        {
            if (DBFieldData[0] is "WW")
            {
                return false;
            }
            else if (DBFieldData[1] is "XX")
            {
                return false;
            }
            else if (DBFieldData[2] is "YY")
            {
                return false;
            }
            else if (DBFieldData[3] is "ZZ")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        #endregion
    }
}

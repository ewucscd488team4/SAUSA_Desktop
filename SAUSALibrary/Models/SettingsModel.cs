
namespace SAUSALibrary.Models
{
    /// <summary>
    /// Container for encapsulating settings read from the Settings.xml file
    /// </summary>
    public class SettingsModel
    {
        /// <summary>
        /// Theme setting
        /// </summary>
        public string SettingOne { get; set; }

        /// <summary>
        /// Last used save directory, can be overwritten by user programically
        /// </summary>
        public string SettingTwo { get; set; }

        /// <summary>
        /// Default save directory
        /// </summary>
        public string SettingThree { get; set; }

        //add more as more settings are designed
    }
}

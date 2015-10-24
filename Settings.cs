using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.Serialization;

namespace ScreenShifter
{
    public sealed class Settings
    {

        #region Properties

        [Category("Sound")]
        [DisplayName("Enabled")]
        [Description("Enabled or disables sounds")]
        [DefaultValue(true)]
        public bool SoundEnabled { get; set; }

        public enum SystemSoundsEnum
        {
            Asterisk,
            Beep,
            Exclamation,
            Hand,
            Question
        }

        [Category("Sound")]
        [DisplayName("Start")]
        [Description("Sound to play when upload starts")]
        [DefaultValue(SystemSoundsEnum.Beep)]
        public SystemSoundsEnum SoundStart { get; set; }

        [Category("Sound")]
        [DisplayName("Finish")]
        [Description("Sound to play when upload/save finishes")]
        [DefaultValue(SystemSoundsEnum.Asterisk)]
        public SystemSoundsEnum SoundFinish { get; set; }

        [Category("Sound")]
        [DisplayName("Error")]
        [Description("Sound to play when upload/save errors")]
        [DefaultValue(SystemSoundsEnum.Hand)]
        public SystemSoundsEnum SoundError { get; set; }

        [Category("Settings")]
        [DisplayName("Use Preview")]
        [Description("Show a preview window before uploading")]
        [DefaultValue(false)]
        public bool UsePreview { get; set; }

        [Category("Settings")]
        [DisplayName("Imgur API Key")]
        [Description("The Imgur API key used to upload images (shouldn't need to change this)")]
        [DefaultValue("7e3ab3a5ebdf427de734da52aa89ed79")]
        public string ImgurApiKey { get; set; }

        [Category("Settings")]
        [DisplayName("Settings File")]
        [Description("File name for the settings file")]
        [DefaultValue("settings.xml")]
        public string SettingsFile { get; set; }

        [Category("Settings")]
        [DisplayName("Auto Upload Clipboard")]
        [Description("Uploads any image data copied into the clipboard automatically")]
        [DefaultValue(false)]
        public bool UploadClipboard { get; set; }

        [Category("Image")]
        [DisplayName("Format")]
        [Description("Format to upload the image in")]
        [DefaultValue(Format.Jpeg)]
        public Format UploadImageFormat { get; set; }

        [Category("Image")]
        [DisplayName("Quality")]
        [Description("Value between 0 and 100 representing the quality of the image to upload (only applies to JPEG)")]
        [DefaultValue(85)]
        [Range(typeof(long), "0", "100")]
        public long UploadImageQuality { get; set; }

        [Category("Settings")]
        [DisplayName("Tray Minimise")]
        [Description("Minimise to system tray")]
        [DefaultValue(true)]
        public bool TrayMinimise { get; set; }

        [Category("Settings")]
        [DisplayName("Balloon Tips")]
        [Description("Show balloon tooltips from the systray icon for uploads (will only show when the window doesn't have focus or isn't visible)")]
        [DefaultValue(true)]
        public bool BalloonTips { get; set; }

        [Category("Settings")]
        [DisplayName("Save Images")]
        [Description("Save images instead of uploading them")]
        [DefaultValue(false)]
        public bool SaveImages { get; set; }

        [Category("Settings")]
        [DisplayName("Save Image Directory")]
        [Description("Path to save images to")]
        [DefaultValue(".")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string SaveImageDirectory { get; set; }

        [Category("Settings")]
        [DisplayName("Selection Colour")]
        [Description("The colour to use for the selection square")]
        [DefaultValue(typeof(Color), "Red")]
        [XmlIgnore]
        public Color SelectionColor { get; set; }

        [XmlElement("SelectionColor")]
        [Browsable(false)]
        public string SelectionColorHtml
        {
            get { return ColorTranslator.ToHtml(SelectionColor); }
            set { SelectionColor = ColorTranslator.FromHtml(value); }
        }

        [Category("Logs")]
        [DisplayName("Log To File")]
        [Description("Write log to file")]
        [DefaultValue(true)]
        public bool LogToFile { get; set; }

        [Category("Logs")]
        [DisplayName("Log Filename")]
        [Description("The filename of the log file")]
        [DefaultValue("uploads.log")]
        public string LogFilename { get; set; }

        public enum Format
        {
            Jpeg,
            Png
        }

        [Category("Hotkeys - Upload")]
        [DisplayName("Selection Shortcut")]
        [Description("The shortcut key to take a selection screenshot")]
        [DefaultValue(Keys.None)]
        public Keys SelectionHotkey { get; set; }

        [Category("Hotkeys - Upload")]
        [DisplayName("Clipboard Upload")]
        [Description("Uploads the image from the clipboard")]
        [DefaultValue(Keys.None)]
        public Keys ClipboardHotkey { get; set; }

        [Category("Hotkeys - Upload")]
        [DisplayName("Fullscreen")]
        [Description("Uploads the full screen")]
        [DefaultValue(Keys.None)]
        public Keys FullscreenHotkey { get; set; }

        [Category("Hotkeys - Upload")]
        [DisplayName("Current Window")]
        [Description("Uploads the current window")]
        [DefaultValue(Keys.None)]
        public Keys CurrentWindowHotkey { get; set; }

        [Category("Hotkeys - Upload")]
        [DisplayName("Cancel All Uploads")]
        [Description("Cancels all currently uploading images")]
        [DefaultValue(Keys.None)]
        public Keys CancelUploadsHotkey { get; set; }

        [Category("Settings")]
        [DisplayName("Save Window Bounds")]
        [Description("Saves the main window location and size")]
        [DefaultValue(true)]
        public bool SaveWindowBounds { get; set; }

        [Browsable(false)]
        public Rectangle DesktopBounds { get; set; }

        [Browsable(false)]
        [DefaultValue(FormWindowState.Normal)]
        public FormWindowState WindowState { get; set; }

        #endregion

        #region Singleton

        [XmlIgnore]
        private static volatile Settings instance;
        [XmlIgnore]
        private static object syncRoot = new Object();

        private Settings()
        {
            // set all the properties to their default value
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
            {
                var attr = (DefaultValueAttribute)property.Attributes[typeof(DefaultValueAttribute)];
                if (attr != null)
                {
                    property.SetValue(this, attr.Value);
                }
            }
        }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Settings();
                            instance.Load();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Saving & Loading

        public void Load()
        {
            var serializer = new XmlSerializer(this.GetType());
            try
            {
                using (var reader = XmlReader.Create(SettingsFile))
                {
                    instance = (Settings)serializer.Deserialize(reader);
                }
            }
            catch (Exception) { }
        }

        public void Save()
        {
            var serializer = new XmlSerializer(this.GetType());
            var s = new XmlWriterSettings();
            s.Indent = true;
            try
            {
                using (var writer = XmlWriter.Create(SettingsFile, s))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}
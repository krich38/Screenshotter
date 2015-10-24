using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Win32;

namespace ScreenShifter
{
    public partial class FormMain : Form
    {

        [STAThread]
        public static void Main()
        {
            try
            {
                // http://stackoverflow.com/questions/951856/is-there-an-easy-way-to-check-net-framework-version-using-c
                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                //int SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));
                if (Framework < 4.0)
                {
                    MessageBox.Show("This application requires .NET Framework 4.0, you are currently running " + Framework, ".NET Framework Version Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
            catch (Exception e)
            {
                if (MessageBox.Show("Unable to read information about currently installed .NET Framework version, error: " + e.Message + Environment.NewLine + "This application may not run correctly without 4.0 installed, continue anyway?", ".NET Framework Version Check", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                {
                    Application.Exit();
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        Dictionary<string, string> delete_urls = new Dictionary<string, string>();

        public FormMain()
        {
            InitializeComponent();
            Log("Session started");
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Icon = this.Icon;
            propertyGridSettings.SelectedObject = Settings.Instance;
            if (Settings.Instance.UploadClipboard)
                ClipboardListener.AddListener(this.Handle);
            foreach (var pi in typeof(Settings).GetProperties())
            {
                var value = pi.GetValue(Settings.Instance, null);
                if (value != null && value.GetType() == typeof(Keys))
                {
                    var key = (Keys)value;
                    if (!Hotkeys.RegisterHotKey(this, key))
                    {
                        Log("Unable to use shortcut: " + key);
                        pi.SetValue(Settings.Instance, Keys.None, null);
                    }
                }
            }
        }

        private void DoHide()
        {
            if (Settings.Instance.TrayMinimise)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;
            }
        }

        private void DoShow()
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Focus();            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMain_FormClosing(sender, null);
            Application.Exit();
        }

        private void propertyGridContextMenu_Opening(object sender, CancelEventArgs e)
        {
            GridItem item = propertyGridSettings.SelectedGridItem;
            resetToolStripMenuItem.Enabled = item.PropertyDescriptor.CanResetValue(propertyGridSettings.SelectedObject);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyDescriptor pd = propertyGridSettings.SelectedGridItem.PropertyDescriptor;
            var oldvalue = propertyGridSettings.SelectedGridItem.Value;
            pd.ResetValue(propertyGridSettings.SelectedObject);
            propertyGridSettings_PropertyValueChanged(sender, new PropertyValueChangedEventArgs(propertyGridSettings.SelectedGridItem, oldvalue));
            propertyGridSettings.Refresh();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            if (this.WindowState != FormWindowState.Minimized)
                DoHide();
            else
                DoShow();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            Settings.Instance.WindowState = this.WindowState;
            if (this.WindowState == FormWindowState.Minimized)
                DoHide();
        }

        private void propertyGridSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Value.GetType() == typeof(Keys))
            {
                Keys newKey = (Keys)e.ChangedItem.Value;
                if (Hotkeys.RegisterHotKey(this, newKey))
                {
                    Keys oldKey = (Keys)e.OldValue;
                    if (Hotkeys.RegisteredHotkeys.ContainsValue(oldKey))
                    {
                        int id = Hotkeys.RegisteredHotkeys.Where(kvp => kvp.Value == oldKey).FirstOrDefault().Key;
                        Hotkeys.UnRegisterHotKeys(this, id);
                        if (newKey == Keys.None)
                            Log("Removed hotkey for " + e.ChangedItem.Label + ": " + oldKey);
                        else
                            Log("Registered hotkey for " + e.ChangedItem.Label + ": " + newKey + ", removed: " + oldKey);
                    }
                    else
                    {
                        if (newKey != Keys.None)
                            Log("Registered new hotkey for " + e.ChangedItem.Label + ": " + newKey);
                    }
                }
                else
                {
                    Log("Unable to use shortcut for " + e.ChangedItem.Label + ": " + newKey);
                    PropertyDescriptor pd = propertyGridSettings.SelectedGridItem.PropertyDescriptor;
                    pd.ResetValue(propertyGridSettings.SelectedObject);
                    propertyGridSettings.Refresh();
                }
            }
            else
            {
                Log("Setting " + e.ChangedItem.Label + " = " + e.ChangedItem.Value);
                if (Settings.Instance.UploadClipboard && !ClipboardListener.IsListening)
                    ClipboardListener.AddListener(this.Handle);
                if (!Settings.Instance.UploadClipboard && ClipboardListener.IsListening)
                    ClipboardListener.RemoveListener(this.Handle);
            }
            Settings.Instance.Save();
        }

        private enum ImageSource
        {
            ClipboardHotkey,
            Selection,
            SelectionPaint,
            Fullscreen,
            DragDrop,
            CurrentWindow,
            ClipboardAuto,
        }

        private string GetFileName(ImageSource src, int i)
        {
            return string.Format("{0}{1}{2}{3}.{4}", Settings.Instance.SaveImageDirectory, Path.DirectorySeparatorChar, src.ToString(), i, Settings.Instance.UploadImageFormat.ToString());
        }

        private void ProcessImage(Image img, ImageSource src)
        {
            if (Settings.Instance.UsePreview)
            {
                new FormPreview(img, img2 => ProcessImageAfter(img2, src)).Show();
            }
            else
            {
                ProcessImageAfter(img, src);
            }
        }

        private void ProcessImageAfter(Image img, ImageSource src)
        {
            if (Settings.Instance.SaveImages)
            {
                int i = 0;
                string filename = GetFileName(src, i);
                while (File.Exists(filename)) 
                {
                    i++;
                    filename = GetFileName(src, i);
                }
                try
                {
                    img.Save(filename);
                    Log("Saved image to: " + filename);
                    PlaySound(Settings.Instance.SoundFinish);
                }
                catch (Exception e)
                {
                    Log("Error saving image to " + filename + ", " + e.Message);
                    PlaySound(Settings.Instance.SoundError);
                }
            }
            else
            {
                PlaySound(Settings.Instance.SoundStart);
                Utility.DoUploadImage(img, src.ToString(), this);
            }
        }

        private void PlaySound(Settings.SystemSoundsEnum sound)
        {
            if (!Settings.Instance.SoundEnabled)
                return;
            switch (sound)
            {
                case Settings.SystemSoundsEnum.Asterisk:
                    SystemSounds.Asterisk.Play();
                    break;
                case Settings.SystemSoundsEnum.Beep:
                    SystemSounds.Beep.Play();
                    break;
                case Settings.SystemSoundsEnum.Exclamation:
                    SystemSounds.Exclamation.Play();
                    break;
                case Settings.SystemSoundsEnum.Hand:
                    SystemSounds.Hand.Play();
                    break;
                case Settings.SystemSoundsEnum.Question:
                    SystemSounds.Question.Play();
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Hotkeys.WM_HOTKEY:
                    if (!Hotkeys.RegisteredHotkeys.ContainsKey(m.WParam.ToInt32()))
                        return;
                    Keys key = Hotkeys.RegisteredHotkeys[m.WParam.ToInt32()];
                    if (key == Settings.Instance.ClipboardHotkey)
                    {
                        if (Clipboard.ContainsImage())
                            ProcessImage(Clipboard.GetImage(), ImageSource.ClipboardHotkey);
                        else
                            Log("Clipboard doesn't contain an image!");
                    }
                    else if (key == Settings.Instance.SelectionHotkey)
                    {
                        new FormSelection(img => ProcessImage(img, ImageSource.Selection)).Show();
                    }
                    else if (key == Settings.Instance.FullscreenHotkey)
                    {
                        using (var img = Utility.GetImageFromScreen(Utility.GetRectangleFromScreen(true)))
                        {
                            ProcessImage(img, ImageSource.Fullscreen);
                        }
                    }
                    else if (key == Settings.Instance.CurrentWindowHotkey)
                    {
                        using(var img = Utility.GetImageFromScreen(Utility.GetRectangleFromScreen(false))) 
                        {
                            ProcessImage(img, ImageSource.CurrentWindow);
                        }
                    }
                    else if (key == Settings.Instance.CancelUploadsHotkey)
                    {
                        Log("Cancelling all uploads...");
                        foreach (var webclient in Utility.UploadingClients.Values)
                        {
                            try
                            {
                                webclient.CancelAsync();
                            }
                            catch (Exception) { }
                        }
                        Utility.UploadingClients.Clear();
                    }
                    return;

                case ClipboardListener.WM_CLIPBOARDUPDATE:
                    if (Clipboard.ContainsImage() && Settings.Instance.UploadClipboard)
                        ProcessImage(Clipboard.GetImage(), ImageSource.ClipboardAuto);
                    return;
            }

            base.WndProc(ref m);
        }

        public void UploaderClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                Utility.UploadingClients.Remove((int)e.UserState);
            }
            catch (Exception) { }
            if (e.Cancelled)
            {
                SetString((int)e.UserState, "Cancelled.");
                Log("(" + e.UserState + ") Cancelled.");
                return;
            }
            else if (e.Error != null)
            {
                SetString((int)e.UserState, "Error: " + e.Error.Message);
                Log("(" + e.UserState + ") Error: " + e.Error.Message);
                PlaySound(Settings.Instance.SoundError);
                return;
            }
            string result = System.Text.Encoding.UTF8.GetString(e.Result);
            XDocument doc;
            try
            {
                doc = XDocument.Parse(result);
            }
            catch (Exception ex)
            {
                Log("Error parsing imgur result, please check logfile for details (maybe imgur is down?)");
                Log(ex.ToString(), false);
                SetString((int)e.UserState, "Error.");
                PlaySound(Settings.Instance.SoundError);
                return;
            }
            if (doc.Element("error") != null)
            {
                string error_message = "Error: " + doc.Element("error").Element("message").Value;
                SetString((int)e.UserState, error_message);
                Log("(" + e.UserState + ") Error: " + error_message);
                Balloon(Utility.GetTime() + " (" + e.UserState + ") Error: ", error_message, ToolTipIcon.Error);
                PlaySound(Settings.Instance.SoundError);
            }
            else if (doc.Element("rsp") != null)
            {
                string original_image = doc.Element("rsp").Element("original_image").Value;
                string delete_page = doc.Element("rsp").Element("delete_page").Value;
                delete_urls.Add(original_image, delete_page);
                SetString((int)e.UserState, original_image);
                Log("(" + e.UserState + ") Uploaded: " + original_image + " - delete link: " + delete_page);
                Balloon(Utility.GetTime() + " (" + e.UserState + ") Uploaded! ", original_image);
                PlaySound(Settings.Instance.SoundFinish);
            }
            else
            {
                SetString((int)e.UserState, result);
                Log("(" + e.UserState + ") Unknown: " + result);
                Balloon(Utility.GetTime() + " (" + e.UserState + ") Unknown! ", result, ToolTipIcon.Error);
                PlaySound(Settings.Instance.SoundError);
            }
        }

        public void Balloon(string title, string text, ToolTipIcon icon = ToolTipIcon.Info, int timeout = 10)
        {
            if (Settings.Instance.BalloonTips && (this.WindowState == FormWindowState.Minimized || !this.Visible || !this.ContainsFocus))
            {
                notifyIcon.BalloonTipIcon = icon;
                notifyIcon.BalloonTipTitle = title;
                notifyIcon.BalloonTipText = text;
                notifyIcon.ShowBalloonTip(timeout);
            }
        }

        public void UploaderClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 50)
                SetString((int)e.UserState, "Uploading: " + (e.ProgressPercentage * 2) + "% " + Utility.FormatBytes(e.BytesSent) + "/" + Utility.FormatBytes(e.TotalBytesToSend));
            else
                SetString((int)e.UserState, "Uploaded, awaiting response ...");
        }

        private delegate void SetStringCallback(int index, string s);

        private void SetString(int index, string s)
        {
            if (this.listBoxImages.InvokeRequired)
            {
                SetStringCallback a = new SetStringCallback(SetString);
                this.Invoke(a, new object[] { index, s });
            }
            else
            {
                listBoxImages.Items[index] = Utility.GetTime() + " (" + index + ") " + s;
                //listBoxImages.SelectedIndex = index;
            }
        }

        private delegate void LogCallback(string s, bool printwindow);

        public void Log(string s, bool printwindow = true)
        {
            if (this.textBoxLog.InvokeRequired)
            {
                LogCallback d = new LogCallback(Log);
                this.Invoke(d, s, printwindow);
            }
            else
            {
                s = "[" + Utility.GetTime() + "] " + s;
                if (printwindow)
                    textBoxLog.Text += s + Environment.NewLine;
                try
                {
                    using (var writer = File.AppendText(Settings.Instance.LogFilename))
                    {
                        writer.WriteLine(s);
                    }
                }
                catch (Exception e)
                {
                    Settings.Instance.LogToFile = false;
                    textBoxLog.Text += "Error writing to logfile, " + e.Message + Environment.NewLine;
                    if (e.InnerException != null)
                    {
                        textBoxLog.Text += " - " + e.InnerException.Message;
                    }
                }
                if (printwindow)
                {
                    textBoxLog.SelectionStart = textBoxLog.Text.Length;
                    textBoxLog.ScrollToCaret();
                }
            }
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string s in files)
            {
                try
                {
                    var img = Image.FromFile(s);
                    Log("Uploading file: " + s);
                    ProcessImage(img, ImageSource.DragDrop);
                }
                catch (Exception)
                {
                    Log("Not a valid image: " + s);
                }
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hotkeys.UnRegisterAllHotKeys(this);
            ClipboardListener.RemoveListener(this.Handle);
            Settings.Instance.Save();
            Log("Session ended");
        }

        private void listBoxImages_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LaunchListBox(listBoxImages.SelectedItem.ToString());
        }

        private void LaunchListBox(string url, bool delete = false)
        {
            if (url == null || !url.Contains(' ') || !url.Contains(") http://i."))
                return;
            url = url.Substring(url.LastIndexOf(' ') + 1);
            if (delete)
            {
                if (!delete_urls.ContainsKey(url))
                {
                    Log("Unable to find delete link for url: " + url);
                    return;
                }
                Log("Launching delete url " + delete_urls[url] + " for " + url);
                url = delete_urls[url];
            }
            else
                Log("Launching: " + url);
            System.Diagnostics.Process.Start(url);
        }

        private void imagesContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            imagesContextMenuStrip.Enabled = listBoxImages.SelectedItem != null &&
                listBoxImages.SelectedItem.ToString().Contains(' ') &&
                !listBoxImages.SelectedItem.ToString().Contains(") Error:") &&
                !listBoxImages.SelectedIndex.ToString().Contains(") Cancelled");
            if (imagesContextMenuStrip.Enabled)
            {
                if (Utility.UploadingClients.ContainsKey(listBoxImages.SelectedIndex))
                    openToolStripMenuItem.Text = "Cancel";
                else
                    openToolStripMenuItem.Text = "Open";
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openToolStripMenuItem.Text == "Open")
                LaunchListBox(listBoxImages.SelectedItem.ToString());
            else
            {
                var client = Utility.UploadingClients.FirstOrDefault(kvp => kvp.Key == listBoxImages.SelectedIndex);
                Log("(" + client.Key + ") Cancelling...");
                client.Value.CancelAsync();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchListBox(listBoxImages.SelectedItem.ToString(), true);
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            if (notifyIcon.BalloonTipText.StartsWith("http://i."))
            {
                Process.Start(notifyIcon.BalloonTipText);
            }
        }

        private void textBoxLog_TextChanged(object sender, EventArgs e)
        {
            if (textBoxLog.TextLength > 10000)
            {
                textBoxLog.Text = "Clearing log buffer";
            }
        }

        private void copyStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = listBoxImages.SelectedItem.ToString();
            if (url == null || !url.Contains(' ') || !url.Contains(") http://i."))
                return;
            url = url.Substring(url.LastIndexOf(' ') + 1);
            Clipboard.SetText(url);
            Log("Copied: " + url);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Settings.Instance.SaveWindowBounds)
            {
                if (Settings.Instance.DesktopBounds != Rectangle.Empty && Utility.GetRectangleFromScreen(true).Contains(Settings.Instance.DesktopBounds.Location))
                    this.DesktopBounds = Settings.Instance.DesktopBounds;
                if (Settings.Instance.WindowState == FormWindowState.Minimized)
                    DoHide();
                else
                    this.WindowState = Settings.Instance.WindowState;
            }
        }

        private void FormMain_SizeLocationChanged(object sender, EventArgs e)
        {
            if (Settings.Instance.SaveWindowBounds)
            {
                if (this.Visible && this.WindowState == FormWindowState.Normal)
                    Settings.Instance.DesktopBounds = this.DesktopBounds;
            }
        }

    }
}
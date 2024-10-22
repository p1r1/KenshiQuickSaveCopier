using Gma.System.MouseKeyHook;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace KenshiSaveCopier
{
    public partial class Form1 : Form {


        private IKeyboardMouseEvents globalMouseHook;
        private string filePath; // Update with the correct file path
        private DateTime lastModifiedDate;
        private const int MAX_BACKUP_COUNT = 10; // Maximum number of backup directories to keep

        private MouseButtons triggerMouseButton = MouseButtons.Middle;
        private Keys triggerKey = Keys.None;

        public Form1() {
            InitializeComponent();

            //setups
            SetupComboBox();

            // Initialize the global mouse hook
            globalMouseHook = Hook.GlobalEvents();
            globalMouseHook.MouseDownExt += OnMouseDown;
            globalMouseHook.KeyDown += OnKeyDown;  // Register the key press event


            // Set the initial last modified date of the file
            if (File.Exists(filePath))
                lastModifiedDate = File.GetLastWriteTime(filePath);
        }


        // Method to load configuration from config.json
        private bool LoadConfig() {
            try {
                string configFilePath = "config.json"; // Path to the config.json file
                if (File.Exists(configFilePath)) {
                    string json = File.ReadAllText(configFilePath);
                    dynamic config = JsonConvert.DeserializeObject(json);
                    filePath = config.filePath;

                    label_info.Text = filePath;
                    return true;
                }
                else {
                    MessageBox.Show("Configuration file not found.");
                    return false;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error loading configuration: " + ex.Message);
                return false;
            }
        }

        // setup
        private void SetupComboBox() {
            // Populate the ComboBox with available trigger options (keys and mouse buttons)
            comboBoxTriggerKey.Items.AddRange(new object[] {
                "Middle Mouse Button",
                "F5",
                "H",
                "Caps Lock",
                "Mouse Button 4",
                "Mouse Button 5"
            });


            // Set default value to middle mouse button
            comboBoxTriggerKey.SelectedItem = "Middle Mouse Button";
        }

        // Main method to run the system tray application
        public void Run() {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        // Event handlers
        private async void OnMouseDown(object sender, MouseEventExtArgs e) {
            if (triggerMouseButton != MouseButtons.None && e.Button == triggerMouseButton) {
                await HandleBackupTrigger();
            }
        }
        private async void OnKeyDown(object sender, KeyEventArgs e) {
            if (triggerKey != Keys.None && e.KeyCode == triggerKey) {
                await HandleBackupTrigger();
            }
        }

        // main event for trigger press
        private async Task HandleBackupTrigger() {
            Console.WriteLine("Backup trigger pressed. Waiting 10 seconds...");
            await Task.Delay(10000); // Wait for 10 seconds

            if (File.Exists(filePath)) {
                DateTime currentModifiedDate = File.GetLastWriteTime(filePath);

                if (currentModifiedDate != lastModifiedDate) {
                    Console.WriteLine("File has been modified.");
                    CopyParentDirectory();
                    lastModifiedDate = currentModifiedDate; // Update the last modified date
                }
                else {
                    Console.WriteLine("File has not been modified.");
                }
            }
            else {
                Console.WriteLine("File not found.");
            }

            // Delete oldest directories if the number of backups exceeds maxBackupCount
            DeleteOldestBackups();
        }

        // Method to copy the parent directory
        private void CopyParentDirectory() {
            string parentDirectory = Path.GetDirectoryName(filePath);
            string destinationDirectory = Path.Combine(Path.GetDirectoryName(parentDirectory), $"{Path.GetFileName(parentDirectory)}_backup_{DateTime.Now:HH_mm_ss-dd_MM_yy}");

            Console.WriteLine($"Copying directory: {parentDirectory} to {destinationDirectory}");

            DirectoryCopy(parentDirectory, destinationDirectory, true);
            //backupDirectories.Add(destinationDirectory); // Track the new backup directory
            //MessageBox.Show($"Directory copied to {destinationDirectory}");
        }

        // Recursively copy the source directory to the destination
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }

            // Copy all files to the new directory
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // Copy subdirectories if requested
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        // Method to delete the oldest backup directories if there are more than 5
        private void DeleteOldestBackups() {
            try {
                string parentDirectory = Directory.GetParent(Path.GetDirectoryName(filePath)).FullName; // Get the parent directory
                                                                                                        //MessageBox.Show(parentDirectory);

                // Get all directories in the parent directory that contain "quicksave"
                var directories = Directory.GetDirectories(parentDirectory)
                                            .Where(dir => Path.GetFileName(dir).Contains("quicksave_backup", StringComparison.OrdinalIgnoreCase))
                                            .Select(dir => new DirectoryInfo(dir))
                                            .OrderBy(di => di.LastWriteTime) // Order by last write time (oldest first)
                                            .ToList();

                // If there are more than MAX_BACKUP_COUNT directories, delete the oldest ones
                if (directories.Count > MAX_BACKUP_COUNT) {
                    // Delete directories except for the newest MAX_BACKUP_COUNT
                    foreach (var dir in directories.Take(directories.Count - MAX_BACKUP_COUNT)) {
                        if (Directory.Exists(dir.FullName)) {
                            Directory.Delete(dir.FullName, true); // Delete the directory and its contents
                            Console.WriteLine($"Deleted backup folder: {dir.FullName}");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

        }

        private void comboBoxTriggerKey_SelectedIndexChanged(object sender, EventArgs e) {
            // Get the selected item from the ComboBox
            string selectedTrigger = comboBoxTriggerKey.SelectedItem.ToString();

            // Determine if the trigger is a mouse button or keyboard key
            if (selectedTrigger == "Middle Mouse Button") {
                triggerMouseButton = MouseButtons.Middle;
                triggerKey = Keys.None; // Disable keyboard trigger if mouse button is selected
            }
            else if (selectedTrigger == "F5") {
                triggerMouseButton = MouseButtons.None;
                triggerKey = Keys.F5;
            }
            else if (selectedTrigger == "H") {
                triggerMouseButton = MouseButtons.None;
                triggerKey = Keys.H;
            }
            else if (selectedTrigger == "Caps Lock") {
                triggerMouseButton = MouseButtons.None;
                triggerKey = Keys.CapsLock;
            }
            else if (selectedTrigger == "Mouse Button 4") {
                triggerMouseButton = MouseButtons.XButton1; // Mouse Button 4 (XButton1)
                triggerKey = Keys.None;
            }
            else if (selectedTrigger == "Mouse Button 5") {
                triggerMouseButton = MouseButtons.XButton2; // Mouse Button 5 (XButton2)
                triggerKey = Keys.None;
            }
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            if (globalMouseHook != null) {
                globalMouseHook.MouseDownExt -= OnMouseDown;
                globalMouseHook.Dispose();
            }

            notifyIcon1.Dispose();
        }


        #region notifyIcon
        private void Form1_Load(object sender, EventArgs e) {
            notifyIcon1.Visible = false;

            // not notify icon logic
            // Load filePath from config.json
            if (!LoadConfig()) {
                notifyIcon1.Visible = false;
                Application.Exit();
            }
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        #endregion

    }
}

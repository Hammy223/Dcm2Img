using FellowOakDicom;
using FellowOakDicom.Imaging;
using SixLabors.ImageSharp;
using System.IO;
using System.Linq;

namespace Dcm2Img {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            label1.Text = "";
            progressBar1.Visible = false;
        }

        private void ButtonSelectFolder_Click(object sender, EventArgs e) {
            folderBrowserDialog1.InitialDirectory = Application.StartupPath;
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e) {
            // Example usage: recursively find DICOM files in a directory and convert them to JPGs
            string startingDir = textBox1.Text;
            progressBar1.Visible = true;

            FindDicomFilesRecursively(startingDir);

        }

        private void LogMsg(string msg) {
            textBox2.Text += msg + Environment.NewLine;
        }

        /// <summary>
        /// Searches basePath for any .dcm files, and converts them to jpg. Then moves the dcm file to a backup folder while keeping the directory structure.
        /// </summary>
        /// <param name="basePath"></param>
        private void FindDicomFilesRecursively(string basePath) {
            var files = Directory.EnumerateFiles(basePath, "*.dcm", SearchOption.AllDirectories);
            string backupBasePath = Path.Combine(Path.GetDirectoryName(basePath), "OpenDentImagesDCMFiles", Path.GetFileName(basePath));

            new DicomSetupBuilder()
            .RegisterServices(s => s.AddFellowOakDicom().AddImageManager<ImageSharpImageManager>())
            .Build();


            // Extract valid DICOM files using fo-dicom
            progressBar1.Value = 0;
            progressBar1.Maximum = files.Count();
            int i = 0;

            foreach (var file in files) {
                 try {
                    // Ensure it's a DICOM file
                    var image = new DicomImage(file);
                    var fileInfo = new FileInfo(file);

                    string jpgPath = Path.ChangeExtension(fileInfo.FullName, ".jpg");

                    string backupRecursivePath = Path.GetFullPath(file).Substring(basePath.Length); //basePath minus where we are in the file search
                    string backupPath = backupBasePath + backupRecursivePath;
                    string backupPathNoFilename = Path.GetDirectoryName(backupPath);

                    progressBar1.Value++;
                    i++;
                    label1.Text = i.ToString() + " / " + progressBar1.Maximum.ToString();
                    LogMsg(backupRecursivePath);

                    ConvertDicom(fileInfo.FullName, jpgPath);

                    // Split the DCM backup folder path into individual folder names
                    string[] folders = backupPathNoFilename.Split(Path.DirectorySeparatorChar);
                    // Build the DCM backup path incrementally
                    string currentPath = "";
                    foreach (string folder in folders) {
                        currentPath = Path.Combine(currentPath, folder);
                        Directory.CreateDirectory(currentPath); // Create the folder if it doesn't exist
                    }

                    fileInfo.MoveTo(backupPath);

                 }
                 catch (DicomException ex) {
                    // Handle DICOM parsing errors
                    LogMsg($"Error parsing DICOM file '{file}': {ex.Message}");
                 }
                 catch (Exception ex) {
                    // Handle other exceptions
                    LogMsg($"Error processing file '{file}': {ex.Message}");
                 }

            }
            try {
                // Create or overwrite the file
                string filePath = Application.StartupPath + @"\log.txt";
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    writer.Write(textBox2.Text);
                }
                Console.WriteLine("Text saved successfully!");
            }
            catch (Exception ex) {
                Console.WriteLine("Error saving text: " + ex.Message);
            }
            MessageBox.Show("done");
            //label1.Text = "Done";
            //progressBar1.Visible = false;
        }

        // Function to convert a DICOM file to JPG
        private void ConvertDicom(string dicomPath, string jpgPath) {

            // Use fo-dicom's rendering capabilities
             try {
            DicomImage image = new DicomImage(dicomPath);
            // Ensure it's a suitable DICOM format for conversion
            //image.RenderImage().As<Bitmap>().Save(jpgPath);
            var sharpimage = image.RenderImage().AsSharpImage();
            sharpimage.SaveAsJpeg(jpgPath);
            sharpimage.Dispose();

               }
               catch (DicomException ex) {
                // Handle DICOM conversion errors
                LogMsg($"Error converting DICOM file '{dicomPath}' to JPG: {ex.Message}");
               }
               catch (Exception ex) {
                // Handle other exceptions
                LogMsg($"Error saving JPG file '{jpgPath}': {ex.Message}");
               }
        }
    }
}

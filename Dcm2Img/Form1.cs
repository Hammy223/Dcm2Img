using FellowOakDicom;
using FellowOakDicom.Imaging;
using SixLabors.ImageSharp;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dcm2Img {
    public partial class Form1 : Form {

        private CancellationTokenSource cancellationSource;
        public Form1() {
            InitializeComponent();
        }

        private void ButtonSelectFolder_Click(object sender, EventArgs e) {
            folderBrowserDialog1.InitialDirectory = Application.StartupPath;
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private async void ButtonStart_Click(object sender, EventArgs e) {
            //recursively find DICOM files in a directory and convert them to JPGs
            if (String.IsNullOrEmpty(textBox1.Text)) return;
          //  ButtonStart.Enabled = false;
           
           // try {
                cancellationSource = new CancellationTokenSource();
                Task task = Task.Run(() => FindDicomFilesRecursively(textBox1.Text, cancellationSource.Token));
               // if( task.GetAwaiter().GetResult() == task.IsCompleted) {
               //     ButtonStart.Enabled = true;
               // }
          //  }
           // catch (Exception ex) {
           //     MessageBox.Show(ex.Message);
           // }
           // finally {
           //     
            //}

            
        }

        private void LogMsg(string msg) {
            textBox2.Invoke(new MethodInvoker(delegate
            {
                textBox2.Text += msg + Environment.NewLine;
                textBox2.SelectionStart = textBox2.TextLength; // Set selection to the end of the text
                textBox2.ScrollToCaret(); // Scroll the view to make the caret visible

            }));
        }

        private void UpdateProgressBarValue(int i) {
            progressBar1.Value++;
            label1.Text = i.ToString() + " / " + progressBar1.Maximum.ToString();
        }

        /// <summary>
        /// Searches basePath for any .dcm files, and converts them to jpg. Then moves the dcm file to a backup folder while keeping the directory structure.
        /// </summary>
        /// <param name="basePath"></param>
        async Task FindDicomFilesRecursively(string basePath, CancellationToken cancellationToken) {
            var files = Directory.EnumerateFiles(basePath, "*.dcm", SearchOption.AllDirectories);
            string backupBasePath = Path.Combine(Path.GetDirectoryName(basePath), "OpenDentImagesDCMFiles", Path.GetFileName(basePath));

            new DicomSetupBuilder()
            .RegisterServices(s => s.AddFellowOakDicom().AddImageManager<ImageSharpImageManager>())
            .Build();


            // Extract valid DICOM files using fo-dicom
            progressBar1.Invoke(new MethodInvoker(delegate {
                progressBar1.Value = 0;
                progressBar1.Maximum = files.Count();
            }));
            int i = 0;

            string lastfile = "";

            foreach (var file in files) {
                try {
                    // Ensure it's a DICOM file
                    var imageDicom = new DicomImage(file);
                    var fileInfo_Dicom = new FileInfo(file);

                    // Get the filename and extension
                    string prefix = "_179_"; //definition of BW folder, opendental when prefix is added, it gets automatically added to this folder.
                    
                    string extension = ".jpg";

                    string backupRecursivePath = Path.GetFullPath(file).Substring(basePath.Length); //basePath minus where we are in the file search
                    string backupPath = backupBasePath + backupRecursivePath;
                    string backupPathNoFilename = Path.GetDirectoryName(backupPath);

                    i++;
                    if (progressBar1.InvokeRequired) {
                        progressBar1.Invoke(new MethodInvoker(delegate
                        {
                            UpdateProgressBarValue(i);
                        }));
                    }
                    else UpdateProgressBarValue(i);
                   
                    
                    LogMsg(backupRecursivePath);

                    //Get dicom file attributes (created, modified), and set those to the new JPG
                    var created = fileInfo_Dicom.CreationTime;
                    var modified = fileInfo_Dicom.LastWriteTime; //This is the actual xray date/time

                    // Create the new filename with the prefix
                    string formattedDate = modified.ToString("MM-dd-yyyy");
                    string fileNameWithoutExtension = formattedDate + " Sidexis " + i; //Path.GetFileNameWithoutExtension(fileInfo_Dicom.FullName);
                    
                    string jpgPath = Path.Combine(fileInfo_Dicom.DirectoryName, $"{prefix}{fileNameWithoutExtension}{extension}");

                    ConvertDicom(fileInfo_Dicom.FullName, jpgPath);

                    var fileInfo_JPG = new FileInfo(jpgPath);
                    fileInfo_JPG.CreationTime = created;
                    fileInfo_JPG.LastWriteTime = modified;

                    // Split the DCM backup folder path into individual folder names
                    string[] folders = backupPathNoFilename.Split(Path.DirectorySeparatorChar);
                    // Build the DCM backup path incrementally
                    string currentPath = "";
                    foreach (string folder in folders) {
                        currentPath = Path.Combine(currentPath, folder);
                        Directory.CreateDirectory(currentPath); // Create the folder if it doesn't exist
                    }

                    fileInfo_Dicom.MoveTo(backupPath);

                    if (cancellationToken.IsCancellationRequested) {
                        lastfile = fileInfo_Dicom.FullName;
                        break;
                    }

                        /* if (bCancelButtonPressed) {
                             lastfile = fileInfo.FullName; 
                             break;
                         }*/

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

            if (cancellationToken.IsCancellationRequested) {
                MessageBox.Show("Cancel pressed\n\nLast File:\n" + lastfile);
                
            }
            else {
                MessageBox.Show("done");
            }
            
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

        private void button1_Click(object sender, EventArgs e) {
            if (cancellationSource != null) {
                cancellationSource.Cancel();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Page = System.Windows.Controls.Page;

namespace BackgroundWorker
{
    /// <summary>
    /// Interaction logic for OpenFolder.xaml
    /// </summary>
    public partial class OpenFolder : Page
    {
        System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
        public OpenFolder()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            //bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        private void bw_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            FileInfo currentFile = e.UserState as FileInfo;
            int results = currentFile.Number;
            string fn = currentFile.FileName;
            numberOfFiles.Text = $"{e.ProgressPercentage+1}/{results}";

            
            //progressBar.Value = results;
            progressBar.Value = Math.Round((double)(e.ProgressPercentage+1) / results * 100) ;
            //progressBar.Value = e.ProgressPercentage;
            fileName.Text = fn;

        }

        private void bw_DoWork(object? sender, DoWorkEventArgs e)
        {
            countFileNumber(e.Argument.ToString());
        }

        private void btnSelectFolder_Clicked(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the folder to calculate number of files in this folder";
                //dlg.SelectedPath = Text;
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolder.Text = dlg.SelectedPath;
                }
            }

            bw.RunWorkerAsync(selectedFolder.Text.Trim());
            //count number of files under the folder path


            //if (!string.IsNullOrEmpty(selectedFolder.Text.Trim()))
            //    countFileNumber(selectedFolder.Text.Trim());

        }

        public class FileInfo
        {
            public int Number { get; set; }
            public string FileName { get; set; }
        }

        private void countFileNumber(string path)
        {
            // searches the current directory and sub directory
            var allFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            int fCount = allFiles.Length;

            //System.Windows.MessageBox.Show(path);
            // searches the current directory
            //int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            for (int x = 0; x < fCount; x++)
            {
                string fileName = System.IO.Path.GetFileName(allFiles[x]);
                //numberOfFiles.Text = fCount.ToString();
                bw.ReportProgress(x,new FileInfo{ Number = fCount, FileName = fileName });
                Thread.Sleep(100);
            }
        }
    }
}

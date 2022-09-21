using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


using System.ComponentModel;

namespace BackgroundWorker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        System.ComponentModel.BackgroundWorker bgWorker = new System.ComponentModel.BackgroundWorker();
        
        public MainWindow()
        {
            InitializeComponent();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;

            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.ProgressChanged += bgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //防止连续点击多次时抛出异常
            if(!bgWorker.IsBusy)
            { 
                bgWorker.RunWorkerAsync();
                btnStart.Content = "Stop";
            }
            else
            {
                btnStart.Content = "Start";
                bgWorker.CancelAsync();
            }
        }

        public void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <=100; i++)
            {
                //counter.Content = i.ToString();
                System.Diagnostics.Debug.WriteLine(i);
                bgWorker.ReportProgress(i);
                Thread.Sleep(100);
                if (bgWorker.CancellationPending)
                {
                    System.Diagnostics.Debug.WriteLine("Thread is now exiting...");
                    //以下e的参数会被传递到RunWorkerCompleted中执行
                    e.Cancel = true;
                    break;
                }
            }
        }
        public void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            counter.Content = e.ProgressPercentage;
            progressBar.Value = e.ProgressPercentage;
        }
        public void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                btnStart.Content = "Task Cancelled";
            else
                btnStart.Content = "Start Again";
        }
    }
}

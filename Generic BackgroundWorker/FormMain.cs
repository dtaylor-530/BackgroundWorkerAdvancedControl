using System;
using System.Collections.Generic;
using System.ComponentModel.Custom.Generic;
using System.Threading;
using System.Windows.Forms;

namespace GenericBackgroundWorker
{
    public partial class FormMain : Form
    {
        // The BackgroundWorker we're going to use
        private BackgroundWorker<string[], string, List<FileData>> fileWorker;
        // An array of filenames
        private string[] files;

        public FormMain()
        {
            InitializeComponent();

            // Set up array of psuedo file names
            files = FileData.CreateFileArray();

            // create the worker instance
            fileWorker = new BackgroundWorker<string[], string, List<FileData>>();

            // Subscribe to the events
            fileWorker.DoWork += new EventHandler<DoWorkEventArgs<string[], List<FileData>>>(fileWorker_DoWork);
            fileWorker.ProgressChanged += new EventHandler<ProgressChangedEventArgs<string>>(fileWorker_ProgressChanged);
            fileWorker.RunWorkerCompleted += new EventHandler<RunWorkerCompletedEventArgs<List<FileData>>>(fileWorker_RunWorkerCompleted);
        }

        private void fileWorker_DoWork(object sender, DoWorkEventArgs<string[], List<FileData>> e)
        {
            // a variable to hold the progress value
            int progress = 0;

            // Set the result to an instance of our list type
            e.Result = new List<FileData>(e.Argument.Length);

            // Iterate over array
            foreach (string file in e.Argument)
            {
                // Check if Cancel has been pressed
                if (fileWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                // Report progress
                fileWorker.ReportProgress(progress, file);
                // Simulate long operation
                Thread.Sleep(50);
                // Add file data to result and increment
                e.Result.Add(new FileData(file, DateTime.Now));
                progress += 2;
            }
            // Done so final report
            fileWorker.ReportProgress(progress, string.Empty);
        }
        private void fileWorker_ProgressChanged(object sender, ProgressChangedEventArgs<string> e)
        {
            // Update the UI
            labelProgress.Text = e.UserState;
            progressBar.Value = e.ProgressPercentage;
        }
        private void fileWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs<List<FileData>> e)
        {
            // Update the UI
            if (e.Cancelled)
            {
                labelProgress.Text = "Cancelled";
                progressBar.Value = 0;
            }
            else
                labelProgress.Text = "Done!";
            listBox.DataSource = e.Result;
            listBox.Enabled = true;
            buttonStart.Enabled = true;
            buttonCancel.Enabled = false;
            progressBar.Enabled = false;
            AcceptButton = buttonStart;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // Update the UI
            buttonCancel.Enabled = true;
            AcceptButton = buttonCancel;
            buttonStart.Enabled = false;
            listBox.DataSource = null;
            listBox.Enabled = false;

            // Run the worker
            fileWorker.RunWorkerAsync(files);
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Inform the worker we wish to cancel
            fileWorker.CancelAsync();
        }

      
    }

    public class FileData
    {


        public FileData(string filename, DateTime timestamp)
        {
            Filename = filename;
            Timestamp = timestamp;
        }
        public string Filename        {            get;            private set;        }
        public DateTime Timestamp        {            get;            private set;        }

        public override string ToString()
        {
            return string.Format("File: {0} Second: {1} MilliSecond: {2}", Filename, Timestamp.Second,Timestamp.Millisecond);
        }


        public static string[] CreateFileArray()
        {
            return new string[]{
                "00", "01", "02", "03", "04", "05", "06", "07",
                "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
                "10", "11", "12", "13", "14", "15", "16", "17",
                "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
                "20", "21", "22", "23", "24", "25", "26", "27",
                "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
                "30", "31"};
        }
    }
}

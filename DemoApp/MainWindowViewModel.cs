using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BackgroundWorkerUserControl;
using System.Diagnostics;


/* BackgroundWorker is ...
 *   ... meant to model a single task that you'd want to perform in the background, on a thread pool thread.  
 *   async/await is a syntax for asynchronously awaiting on asynchronous operations. 
 *   Those operations may or may not use a thread pool thread or even use any other thread. So, they're apples and oranges.
 */


namespace DemoApp
{

    public  class MainWindowViewModel
    {

        public BackgroundWorkerUserControl.BackgroundWorkerEx<WorkerArgument> BackgroundWorkerEx { get; set; }


        private static int iterations = 10;
        public int Iterations { get { return iterations; } }

        public MainWindowViewModel(Action<object , RunWorkerCompletedEventArgs> completedEvent,Action<object , ProgressChangedEventArgs> progressEvent)
        {
            BackgroundWorkerEx = new BackgroundWorkerEx<WorkerArgument>();
            BackgroundWorkerEx.Task( 10,  onProgressChanged:progressEvent, onFinished:completedEvent) ;

            BackgroundWorkerEx.MainMethod = MainWork;
            BackgroundWorkerEx.PreliminaryMethod = PreliminaryWork;

        }


        //private void onProgressChanged(object sender, ProgressChangedEventArgs arg)
        //{
        //     WorkerArgument userState = arg.UserState as WorkerArgument;
        //     Console.WriteLine(string.Format("Progress: {0}; Calculation result: {1}", arg.ProgressPercentage, userState.CalculationResult));
        //}

       
        //_worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //_worker.ProgressChanged += new ProgressChangedEventHandler(onProgressChanged);
        //_worker.WorkerReportsProgress = true;
        //_worker.RunWorkerAsync(new WorkerArgument { CalculationResult=-1, BaseNumber = 10 });


        public WorkerArgument MainWork(int arg)
        {
            return new WorkerArgument() { BaseNumber = arg, CalculationResult = 2 };
        }

        public  object PreliminaryWork() { return null; }

        


    }



    public class WorkerArgument
    {
        public int BaseNumber { get; set; }
        public double CalculationResult { get; set; }
    }



}

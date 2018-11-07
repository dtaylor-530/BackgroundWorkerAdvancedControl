//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.Custom.Generic;
//using System.Text;

//namespace BackgroundWorkerWrapper
//{
//    public class WorkerViewModel<T> : INotifyPropertyChanged where T : new()
//    {

//        private UtilityEnum.ProcessState state;
//        private int _progress;
//        double _delay = 1000;
//        private BackgroundWorkerEx<T> _worker;


//        public UtilityEnum.ProcessState State
//        {
//            get { return state; }
//            set
//            {
//                if (state == value)
//                    return;
//                else
//                {
//                    //var state = Converter.Main(value);
//                    if (state == UtilityEnum.ProcessState.Blocked)
//                        _worker.Pause();
//                    else if (state == UtilityEnum.ProcessState.Running)
//                        _worker.Process();
//                    else if (state == UtilityEnum.ProcessState.Terminated)
//                    {
//                        _worker.Cancel();
//                        Progress = 0;
             
//                    }
//                }


//                state = value;
//                OnPropertyChanged(nameof(State));
//            }
//        }



//        /// <summary>
//        /// Current Progress
//        /// </summary>
//        public int Progress
//        {
//            get { return _progress; }
//            set
//            {
//                if (_progress != value)
//                {
//                    _progress = value;
//                    OnPropertyChanged(nameof(Progress));
//                }
//            }
//        }



//        //delay for the main method
//        public double Delay { get { return _delay; } set {  _delay=value; _worker.ChangeDelay(value); OnPropertyChanged(nameof(Delay)); } } 



  


//        public WorkerViewModel(BackgroundWorkerEx<T> worker)
//        {
//            _worker = worker;
//            worker.ChangeDelay(_delay);

//        }



//        #region property change

//        public event PropertyChangedEventHandler PropertyChanged;


//        /// <summary>
//        /// Property Changed.
//        /// </summary>
//        /// <param name="propertyName">Property Name.</param>
//        protected void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//        #endregion property change

//    }
//}

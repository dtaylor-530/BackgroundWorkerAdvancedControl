using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace BackgroundWorkerUserControls.Controls
{


    public class ProgressTimeBar : ProgressBar
    {



        private static Style oStyle = null;
        //public System.Windows.Controls.Image MainImage { get; set; }
        TextBlock textBlockTime;
        TextBlock textBlockProgress;
        Button tButton;
        TextBlock tBox;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
          //  tButton = GetTemplateChild("PART_Button") as Button;
          //  tButton.Click += TButton_Click;

            tBox = GetTemplateChild("PART_Block") as TextBlock;
            tBox.PreviewMouseLeftButtonDown += TBox_PreviewMouseLeftButtonDown;
        }

        private void TBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            buttonIsClicked = !buttonIsClicked;
            UpdateBox();
        }

        bool buttonIsClicked = false;

        private void TButton_Click(object sender, RoutedEventArgs e)
        {
            buttonIsClicked = !buttonIsClicked;
            UpdateButton();
        }

        private void UpdateButton()
        {
        
            if (buttonIsClicked)
                tButton.Content = progress;
            else
                tButton.Content = runningTime;

        
        }


        private void UpdateBox()
        {

            if (buttonIsClicked)
                tBox.Text = progress;
            else
                tBox.Text= runningTime;


        }

        private string runningTime = "0";
        private string progress = "0";

        //public int RunningTime
        //{
        //    get { return runningTime; }
        //    set { runningTime = value; }
        //}

        //public TimeSpan TimeSpan
        //{
        //    get { return (TimeSpan)GetValue(TimeSpanSourceProperty); }
        //    set { SetValue(TimeSpanSourceProperty, value); }
        //}




        //private static void OnTimeSpanSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}



        //public static readonly DependencyProperty TimeSpanProperty =
        //    DependencyProperty.Register("TimeSpan", typeof(TimeSpan), typeof(ToggleButtonEx),
        //        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTimeSpanSourceChanged)));


        //public partial class MainWindow : Window
        //{
        //    public MainWindow()
        //    {
        //        InitializeComponent();

        //        CountingProgressBar cpb = new CountingProgressBar(30);
        //        LayoutRoot.Children.Add(cpb);
        //        cpb.ProgressCompletedEvent += () => { MessageBox.Show("Count Complete"); };
        //        cpb.Start();
        //    }
        //}




        //Thread timerThread;


        DispatcherTimer timer;
        int i = 0;
        //bool resetTimer;
        public event Action ExecutionFinished;

        private static int intervalinSecs = 1;
        //TextBlock blockTime_small;


        public ProgressTimeBar() : base()
        {
            Uri resourceLocater = new Uri("/BackgroundWorkerUserControls;component/Themes/ProgressTimeBar.xaml", System.UriKind.Relative);

            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            oStyle = resourceDictionary["ProgressTimeBarStyle"] as Style;
            Style = oStyle;




            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(intervalinSecs);
            timer.Tick += timer_Tick;

            ValueChanged += ProgressTimeBar_ValueChanged;
            ExecutionFinished += () => { timer.Stop(); };

        }

        private void ProgressTimeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                i = 0;
            }

            progress = String.Format("{0} %", e.NewValue);

            runningTime = TimeSpan.FromSeconds(i).ToString();

            UpdateBox();

            if (Value == Maximum)
                ExecutionFinished();




                //resetTimer = true;
        }




        private void timer_Tick(object source, EventArgs e)
        {

            //if (resetTimer)
            //    timer.Stop();
            //else
                i += intervalinSecs;


            //resetTimer = false;
        }

        





        //void SetTimer(int time)
        //{
        //    if (timerThread != null && timerThread.IsAlive)
        //        timerThread.Abort();

        //    timerThread = new Thread(() => 
        //    {
        //        for (int i = 0; i < time; ++i)
        //        {
        //            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //                {
        //                    Value += 1;
        //                }));
        //            Thread.Sleep(1000);
        //        }

        //        ProgressComplete();
        //    });
        //    timerThread.IsBackground = true;
        //}

        //public void Start()
        //{
        //    if (timerThread != null)
        //        timerThread.Start();
        //}
        //public void SetTime(int Time)
        //{
        //    Maximum = Time;
        //    SetTimer(Time);
        //}
        //public void Reset()
        //{
        //    Value = 0;
        //    SetTimer(Convert.ToInt32(Maximum));
        //}

        //public delegate void ProgressCompleted();
        //public event ProgressCompleted ProgressCompletedEvent;
        //private void ProgressComplete()
        //{
        //    if (ProgressCompletedEvent != null)
        //        ProgressCompletedEvent();
        //}
    }
}



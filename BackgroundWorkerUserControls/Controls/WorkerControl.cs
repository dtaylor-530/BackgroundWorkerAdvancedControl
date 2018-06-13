using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;

namespace BackgroundWorkerUserControls.Controls
{
    [ContentProperty("Content")]
    public class WorkerControl : Control,INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        private static Style oStyle = null;
        private ToggleButtonEx playPauseButton;
        private Button cancelButton;
        private Slider SliderControl;
         public ProgressTimeBar progressTimeBar { get; set; }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            playPauseButton = GetTemplateChild("PlayPause") as ToggleButtonEx;
            cancelButton = GetTemplateChild("Cancel") as Button;

            playPauseButton.Click += PlayPauseButton_Click;
            cancelButton.Click += CancelButton_Click;

            SliderControl = GetTemplateChild("DelaySlider") as Slider;

            SliderControl.ValueChanged += SliderControl_ValueChanged;
            progressTimeBar = GetTemplateChild("ProgressTimeBar") as ProgressTimeBar;
            progressTimeBar.ValueChanged += ProgressTimeBar_ValueChanged;
            progressTimeBar.ExecutionFinished += () => { CancelButton_Click(null, null); };
          
            //chkbx.Checked += Chkbx_Checked;

        }

        private void SliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetValue(DelayProperty,e.NewValue);
        }

        private void ProgressTimeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButton.IsEnabled = false;
            playPauseButton.Click -= PlayPauseButton_Click;
                    playPauseButton.IsChecked = false;
            SetValue(WorkerStateProperty, State.Stopped);
            playPauseButton.Click += PlayPauseButton_Click;
       
            // playPauseButton.IsChecked = true;

        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButton.IsEnabled = true;
          
            //ProgressTimeBar.ValueProperty
            SetValue(WorkerStateProperty, (sender as ToggleButtonEx).IsChecked == true ? State.Running : State.Paused);
        }

        //private void Chkbx_Checked(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Event Raised");
        //}


        public WorkerControl() : base()
        {
       
        
              
       


            Uri resourceLocater = new Uri("/BackgroundWorkerUserControls;component/Themes/WorkerControl.xaml", System.UriKind.Relative);

            
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            oStyle = resourceDictionary["WorkerBar"] as Style;
            Style = oStyle;



           // SliderControl.Style= resourceDictionary["WorkerBar"] as Style;
            //var ty = 3;


        }

     

        /// <summary>
        /// Gets or sets the orientation.
        /// <remarks>The default is Vertical.</remarks>
        /// </summary>
        /// <value>The orientation.</value>
        //[Category("Layout"), Description("Defines the directionality of the autolayout. Use vertical for a column first layout, horizontal for a row first layout.")]
        //public Orientation Orientation
        //{
        //    get { return (Orientation)GetValue(OrientationProperty); }
        //    set { SetValue(OrientationProperty, value); }
        //}




        ///// <summary>
        ///// Handled the redraw properties changed event
        ///// </summary>
        //private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}


        //public static readonly DependencyProperty OrientationProperty =
        //    DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Control),
        //        new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnOrientationChanged)));


        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }





        /// <summary>
        /// Gets or sets the WorkerState.
        /// </summary>
        /// <value>The WorkerState.</value>
        public State WorkerState
        {
            get { return (State)GetValue(WorkerStateProperty); }
            set { SetValue(WorkerStateProperty, value); }
        }




        /// <summary>
        /// Handled the redraw properties changed event
        /// </summary>
        private static void OnWorkerStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WorkerControl).WorkerState = (State)e.NewValue;
            (d as WorkerControl). RaisePropertyChanged("WorkerState");
        }


        public static readonly DependencyProperty WorkerStateProperty =
            DependencyProperty.Register("WorkerState", typeof(State), typeof(WorkerControl),
                new FrameworkPropertyMetadata(State.Stopped, new PropertyChangedCallback(OnWorkerStateChanged)));





        /// <summary>
        /// Gets or sets the WorkerState.
        /// </summary>
        /// <value>The WorkerState.</value>
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }




        /// <summary>
        /// Handled the redraw properties changed event
        /// </summary>
        private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as SliderControl).WorkerState = (State)e.NewValue;
            //(d as WorkerControl).RaisePropertyChanged("WorkerState");
          //  (d as WorkerControl).SetValue(DelayProperty, e.NewValue);
        }


        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(double), typeof(WorkerControl),
                new FrameworkPropertyMetadata(1000.00, new PropertyChangedCallback(OnDelayChanged)));


        /// <summary>
        /// Gets or sets the WorkerState.
        /// </summary>
        /// <value>The WorkerState.</value>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }



        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(int), typeof(WorkerControl),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WorkerControl).progressTimeBar.Value = (d as WorkerControl).Value;


        }
    }

}

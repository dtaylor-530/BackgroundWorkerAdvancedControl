using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BackgroundWorkerUserControl;


namespace BackgroundWorkerUserControl
{
    //public partial class MainWindowViewModel  :BackgroundWorkerEx

    //{ 

    //}



    /// <summary>
    /// View Model Base
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Property Changed Event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property Changed.
        /// </summary>
        /// <param name="propertyName">Property Name.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            // Get the current property changed event handler
            PropertyChangedEventHandler handler = PropertyChanged;

            // Raise the property changed event when the handler is not null
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}

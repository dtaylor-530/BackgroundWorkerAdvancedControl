using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorkerExtended
{

    enum WorkerState { Stopped = 2, Running = 1, Paused = 0 }



    static class Converter
    {

        public static WorkerState Main(bool? b)
        {
            if (b == true)
                return WorkerState.Running;
            else if (b == false)
                return WorkerState.Paused;
            else if (b == null)
                return WorkerState.Stopped;
            else
                throw new ArgumentOutOfRangeException("argument for converter should be true, false or null");

        }

    }


    //[ValueConversion(typeof(bool), typeof(bool))]
    //public class StateToNullableBooleanConverter : IValueConverter
    //{
    //    #region IValueConverter Members

    //    public object Convert(object value, Type targetType, object parameter,
    //        System.Globalization.CultureInfo culture)
    //    {
    //        if (targetType != typeof(bool?))
    //            throw new InvalidOperationException("The target must be a boolean");

    //        switch((State)(object)
    //        {


    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter,
    //        System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }

    //    #endregion
    //}


}


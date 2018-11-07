using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace BackgroundWorkerWrapper
{

    //public enum UtilityEnum.ProcessState { Stopped = 2, Running = 1, Paused = 0 }



    //static class Converter
    //{

    //    public static UtilityEnum.ProcessState Main(bool? b)
    //    {
    //        if (b == true)
    //            return UtilityEnum.ProcessState.Running;
    //        else if (b == false)
    //            return UtilityEnum.ProcessState.Paused;
    //        else if (b == null)
    //            return UtilityEnum.ProcessState.Stopped;
    //        else
    //            throw new ArgumentOutOfRangeException("argument for converter should be true, false or null");

    //    }

    //}


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


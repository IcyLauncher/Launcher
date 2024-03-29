﻿namespace IcyLauncher.Xaml.Helpers;

public class DateTimeFormatter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        ((DateTime)value).ToString((string)parameter);

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        DateTime.Parse((string)value);
}
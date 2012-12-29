﻿using System;
using System.Globalization;
using System.Windows.Data;
using Put.io.Core.Models;

namespace Put.io.Core.Converters
{
    public class StatusTypeIsIndeterminate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusType)
            {
                switch ((StatusType)value)
                {
                    case StatusType.InQueue:
                        return true;
                        break;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
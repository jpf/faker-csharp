﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Faker.Generators;

namespace Faker.Selectors
{
    /// <summary>
    /// Injects DateTime values between the specified To and From ranges...
    /// </summary>
    public class DateTimeSelector : TypeSelectorBase<DateTime>
    {
        public DateTimeSelector()
        {
            From = DateTime.UtcNow.AddDays(-5);
            To = DateTime.UtcNow.AddDays(5); ;
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        #region Overrides of TypeSelectorBase<DateTime>

        public override void Generate(object targetObject, PropertyInfo property)
        {
            property.SetValue(targetObject, DateTimes.GetDateTime(From, To), null);
        }

        #endregion
    }

    public class TimeStampSelector : TypeSelectorBase<long>
    {
        public TimeStampSelector()
        {
            From = DateTime.UtcNow.AddDays(-5);
            To = DateTime.UtcNow.AddDays(5); ;
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        private static readonly Regex _regex = new Regex(SpecialFieldsRegex.TimeStampRegex, RegexOptions.IgnoreCase);

        #region Overrides of TypeSelectorBase<long>

        public override bool CanBind(PropertyInfo field)
        {
            return _regex.IsMatch(field.Name);
        }
        
        public override void Generate(object targetObject, PropertyInfo property)
        {
            property.SetValue(targetObject, DateTimes.GetTimeStamp(From, To), null);
        }

        #endregion
    }
}

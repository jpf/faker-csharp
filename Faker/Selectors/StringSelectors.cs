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
    /// Type selector for generating standard strings
    /// </summary>
    public sealed class StringSelector : PrimitiveSelectorBase<string>
    {
        /// <summary>
        /// By default, we constrain the length of the string to be between 10 and 40 characters
        /// </summary>
        public StringSelector()
        {
            MaxSize = 40;
            MinSize = 10;
        }

        public int MaxSize { get; set; }
        public int MinSize { get; set; }

        #region Overrides of TypeSelectorBase<string>

        public override string Generate()
        {
            return Strings.GenerateAlphaNumericString(MinSize, MaxSize);
        }

        #endregion
    }

    /// <summary>
    /// Type selector for first names...
    /// </summary>
    public sealed class LastNameSelector : TypeSelectorBase<string>
    {
        private static readonly Regex _regex = new Regex(SpecialFieldsRegex.LastNameRegex, RegexOptions.IgnoreCase);

        #region Overrides of TypeSelectorBase<string>

        public override bool CanBind(PropertyInfo field)
        {
            return _regex.IsMatch(field.Name);
        }

        public override string Generate()
        {
            return Names.Last();
        }

        #endregion
    }

    /// <summary>
    /// Type selector for first names...
    /// </summary>
    public sealed class FirstNameSelector : TypeSelectorBase<string>
    {
        private static readonly Regex _regex = new Regex(SpecialFieldsRegex.FirstNameRegex, RegexOptions.IgnoreCase);

        #region Overrides of TypeSelectorBase<string>

        public override bool CanBind(PropertyInfo field)
        {
            return _regex.IsMatch(field.Name);
        }

        public override string Generate()
        {
            return Names.First();
        }

        #endregion
    }

    /// <summary>
    /// Selector used for populating full names for fields that accept full names
    /// </summary>
    public sealed class FullNameSelector : TypeSelectorBase<string>
    {
        private static readonly Regex _regex = new Regex(SpecialFieldsRegex.FullNameRegex, RegexOptions.IgnoreCase);

        public override bool CanBind(PropertyInfo field)
        {
            return _regex.IsMatch(field.Name);
        }

        public override string Generate()
        {
            return Names.FullName();
        }
    }

    public sealed class EmailSelector : TypeSelectorBase<string>
    {
        private static readonly Regex _regex = new Regex(SpecialFieldsRegex.EmailRegex, RegexOptions.IgnoreCase);

        #region Overrides of TypeSelectorBase<string>

        public override bool CanBind(PropertyInfo field)
        {
            return _regex.IsMatch(field.Name);
        }

        public override string Generate()
        {
            return EmailAddresses.Human();
        }

        #endregion
    }
}

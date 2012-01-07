﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Faker.Selectors;
using NUnit.Framework;

namespace Faker.Tests
{
    [TestFixture(Description = "Ensures that our type table behaves as expected")]
    public class TypeTableTests
    {
        #region Setup / Teardown
        #endregion

        #region Tests

        [Test(Description = "Should be able to add new typeselectors to a fresh type table")]
        public void Should_Add_Type_Selectors_For_Single_Type_To_TypeTable()
        {
            //Create a new TypeTable
            var table = new TypeTable();

            //Create some selectors that we're going to add
            var stringSelector1 = new StringSelector();
            var stringSelector2 = new FullNameSelector();
            var stringSelector3 = new EmailSelector();

            Assert.AreEqual(0, table.CountSelectors<string>(), "should have ZERO type selectors for type 'string' since we haven't added any yet");

            //Add the first selector (our default string selector)
            table.AddSelector<string>(stringSelector1);
            Assert.AreEqual(1, table.CountSelectors<string>(), "should have ONE type selectors for type 'string'");

            var firstselector = table.GetSelectors<string>().First();
            Assert.IsInstanceOf<StringSelector>(firstselector);

            //Add the second selector (the full name selector)
            table.AddSelector<string>(stringSelector2);
            Assert.AreEqual(2, table.CountSelectors<string>(), "should have TWO type selectors for type 'string'");

            firstselector = table.GetSelectors<string>().First();
            Assert.IsInstanceOf<FullNameSelector>(firstselector); //Oh snap, the new front of the line should be our full name selector!

            //Add the thrid selector (the email address selector)
            table.AddSelector<string>(stringSelector3);
            Assert.AreEqual(3, table.CountSelectors<string>(), "should have THREE type selectors for type 'string'");

            firstselector = table.GetSelectors<string>().First();
            Assert.IsInstanceOf<EmailSelector>(firstselector); //Oh snap, the new front of the line should be our full name selector!
        }

        [Test(Description = "Should be able to add type selectors for multiple types to the table")]
        public void Should_Add_Type_Selectors_For_Multiple_Types_To_TypeTable()
        {
            //Create a new TypeTable
            var table = new TypeTable();

            //Create some string selectors that we're going to add
            var stringSelector1 = new StringSelector();

            //Create some long selectors that we're going to use...
            var longSelector1 = new LongSelector();
            var longSelector2 = new TimeStampSelector();

            Assert.AreEqual(0, table.CountSelectors<string>(), "should have ZERO type selectors for type 'string' since we haven't added any yet");
            Assert.AreEqual(0, table.CountSelectors<long>(), "should have ZERO type selectors for type 'long' since we haven't added any yet");

            //Add the first and only string selector (our default string selector)
            table.AddSelector<string>(stringSelector1);
            Assert.AreEqual(1, table.CountSelectors<string>(), "should have ONE type selectors for type 'string'");
            Assert.AreEqual(0, table.CountSelectors<long>(), "should have ZERO type selectors for type 'long' since we haven't added any yet"); //Assert that we haven't added any long selectors yet

            var firstStringSelector = table.GetSelectors<string>().First();
            Assert.IsInstanceOf<StringSelector>(firstStringSelector);

            var currentLongSelector = table.GetSelectors<long>().FirstOrDefault();
            Assert.IsNull(currentLongSelector); //Since we haven't added any long selectors yet, this should return null

            //Add the first long selector (our default long selector)
            table.AddSelector<long>(longSelector1);
            Assert.AreEqual(1, table.CountSelectors<string>(), "should have ONE type selectors for type 'string'");
            Assert.AreEqual(1, table.CountSelectors<long>(), "should have ONE type selectors for type 'long'");

            firstStringSelector = table.GetSelectors<string>().First();
            Assert.IsInstanceOf<StringSelector>(firstStringSelector);

            currentLongSelector = table.GetSelectors<long>().FirstOrDefault();
            Assert.IsInstanceOf<LongSelector>(currentLongSelector);

            //Add the final long selector (our timestamp selector)
            table.AddSelector<long>(longSelector2);
            Assert.AreEqual(1, table.CountSelectors<string>(), "should have ONE type selectors for type 'string'");
            Assert.AreEqual(2, table.CountSelectors<long>(), "should have TWO type selectors for type 'long'");

            firstStringSelector = table.GetSelectors<string>().First();
            Assert.IsInstanceOf<StringSelector>(firstStringSelector);

            currentLongSelector = table.GetSelectors<long>().FirstOrDefault();
            Assert.IsInstanceOf<TimeStampSelector>(currentLongSelector);
        }

        #endregion
    }
}

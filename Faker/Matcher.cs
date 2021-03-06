﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Faker.Generators;
using Faker.Helpers;
using Faker.Selectors;

namespace Faker
{
    /// <summary>
    /// Class used to match a type selector from the TypeTable with the properties of an object
    /// </summary>
    public class Matcher
    {
        public TypeTable TypeMap { get; protected set; }

        /// <summary>
        /// Default constructor - uses the default TypeTable
        /// </summary>
        public Matcher():this(new TypeTable()){}

        /// <summary>
        /// Constructor which accepts a TypeTable as an argument
        /// </summary>
        /// <param name="table">an instantiated type table that can be accessed via the TypeMap property later</param>
        public Matcher(TypeTable table)
        {
            TypeMap = table;
        }

        /// <summary>
        /// Method matches all properties on a given class with a 
        /// </summary>
        /// <typeparam name="T">a class with a parameterless constructor (POCO class)</typeparam>
        /// <param name="targetObject">an instance of the class</param>
        public virtual void Match<T>(T targetObject) where T : new()
        {
            //Get all of the properties of the class
            var properties = typeof (T).GetProperties();

            ProcessProperties(properties, targetObject);
        }

        /// <summary>
        /// Method for iterating over all of the indivdual properties in for a given object
        /// </summary>
        /// <param name="properties">The set of properties available to an object instance</param>
        /// <param name="targetObject">The object against which type selectors will inject values</param>
        protected virtual void ProcessProperties(PropertyInfo[] properties, object targetObject)
        {
            //Iterate over the properties
            foreach (var property in properties)
            {
                if (!property.CanWrite) //Bail if we can't write to the property
                    continue;

                ProcessProperty(property, targetObject);
            }
        }

        /// <summary>
        /// Protected method used to implement our selector-matching strategy. Uses a greedy approach.
        /// </summary>
        /// <param name="property">The meta-data about the property for which we will be finding a match</param>
        /// <param name="targetObject">The object which will receive the property injection</param>
        protected virtual void ProcessProperty(PropertyInfo property, object targetObject)
        {
            //Get the type of the property
            var propertyType = property.PropertyType;

            //Determine if we have a selector-on-hand for this data type
            var selectorCount = TypeMap.CountSelectors(propertyType);

            //We have some matching selectors, so we'll evaluate and return the best match
            if(selectorCount > 0)
            {
                //Evaluate all of the possible selectors and find the first available match
                var selector = EvaluateSelectors(property, TypeMap.GetSelectors(propertyType));

                //We found a matching selector
                if(!(selector is MissingSelector))
                {
                    selector.Generate(targetObject, property); //Bind the property
                    return; //Exit
                }
            }

            //Check to see if the type is a class and has a default constructor
            if (propertyType.IsClass && propertyType.GetConstructor(Type.EmptyTypes) != null)
            {
                var subProperties = propertyType.GetProperties();

                //Create an instance of the underlying subclass
                var subClassInstance = Activator.CreateInstance(propertyType);

                //Match all of the properties on the subclass 
                ProcessProperties(subProperties, subClassInstance);

                //Bind the sub-class back onto the original target object
                property.SetValue(targetObject, subClassInstance, null);

                return; //Exit
            }

            //Check to see if the type is an array or any other sort of collection
            if(typeof(IList).IsAssignableFrom(propertyType))
            {
                //Get the underlying type used int he array
                var elementType = propertyType.GetElementType();

                //Get a number of elements we want to create 
                //Note: (between 0 and 10 for now)
                var elementCount = Numbers.Int(0, 10);
                
                //Create an instance of our target array
                IList arrayInstance = null;

                //If we're working with a generic list or any other sort of collection
                if(propertyType.IsGenericType)
                {
                    arrayInstance = (IList)GenericHelper.CreateGeneric(propertyType, elementType);
                }
                else
                {
                    arrayInstance = (IList) GenericHelper.CreateGeneric(typeof (List<>), elementType);
                }

                //Determine if there's a selector available for this type
                var hasSelector = TypeMap.CountSelectors(elementType) > 0;
                ITypeSelector selector = null;

                if(hasSelector)
                {
                    //selector = EvaluateSelectors(ele)
                }

                for(var i =0; i < elementCount; i++)
                {
                    //Create a new element instance
                    var element = Activator.CreateInstance(elementType);

                    if(hasSelector)
                    {
                        
                    }

                    //If the element type is a sub-class, then populate it recursively
                    if(elementType.IsClass)
                    {
                        var subProperties = propertyType.GetProperties();

                        //Populate all of the properties on this object
                        ProcessProperties(subProperties, element);
                    }

                    arrayInstance.Add(element);
                }
            }

        }

        /// <summary>
        /// Evaluates a set of selectors and grabs the first available match
        /// </summary>
        /// <param name="propertyType">The type for which we're trying to find a match</param>
        /// <param name="selectors">A list of selectors from the TypeTable</param>
        /// <returns>the first matching ITypeSelector instance we could find</returns>
        protected virtual ITypeSelector EvaluateSelectors(PropertyInfo propertyType, IEnumerable<ITypeSelector> selectors)
        {
            foreach(var selector in selectors)
            {
                //If the selector can bind
                if(selector.CanBind(propertyType))
                {
                    //Return it
                    return selector;
                }
            }

            //Otherwise, return a MissingSelector and let them know that we can't do it.
            return new MissingSelector();
        }
    }
}

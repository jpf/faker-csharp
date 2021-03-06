﻿using System;
using System.Reflection;

namespace Faker.Selectors
{
    public interface ITypeSelector
    {
        /// <summary>
        /// Settable priority which indicates the priority of this TypeSelector relative to the
        /// other selectors for this type. A fake can execute multiple strategies for selecting a value of a 
        /// given type
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// Determines if we can allow nulls for a given type
        /// </summary>
        /// <param name="canBeNull">If true, we can set nulls - false by default</param>
        void BeNull(bool canBeNull = false);

        /// <summary>
        /// Determines if this strategy can be successfully executed for this field.
        /// </summary>
        /// <param name="field">This method determines whether or not we will attempt to execute this strategy for a given type.</param>
        /// <returns>true if we can go forward with this strategy, false otherwise.</returns>
        bool CanBind(PropertyInfo field);

        /// <summary>
        /// Injects the generator function into the property
        /// </summary>
        /// <param name="targetObject">The target object designed for property injection</param>
        /// <param name="property">The meta-data for the current property we're testing</param>
        void Generate(object targetObject, PropertyInfo property);

        /// <summary>
        /// Injects the generator function into the property
        /// </summary>
        /// <param name="propertyObject">An instantiated property object to which we're assigning directly</param>
        void Generate(ref object propertyObject);

        /// <summary>
        /// The underlying Type used for this mapping
        /// </summary>
        Type TargetType { get; }
    }
}

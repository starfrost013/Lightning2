using System;
using System.Collections.Generic;
using System.Reflection; 
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// InstanceInfoProperty
    /// 
    /// March 9, 2021 (modified April 17, 2021: The Great Re-Namespacecing)
    /// 
    /// Defines a property for Instance Information.
    /// </summary>
    public class InstanceInfoProperty
    {
        /// <summary>
        /// The type of the property.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The name of this property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The .NET accessibility level of this property - see <see cref="InstanceAccessibility"/>.
        /// </summary>
        public InstanceAccessibility Accessibility { get; set; }

        /// <summary>
        /// Presently unimplemented: Determine if this property is deprecated.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Presently unimplemented: Determine if this property is experimental.
        /// </summary>
        public bool Experimental { get; set; }

    }
}

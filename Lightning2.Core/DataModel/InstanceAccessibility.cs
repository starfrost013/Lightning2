using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// InstanceAccessibility
    /// 
    /// April 15, 2021
    /// 
    /// Defines accessibility levels for Instances.
    /// </summary>
    public enum InstanceAccessibility
    {
        /// <summary>
        /// Publically accessibly by all
        /// </summary>
        Public = 0,

        /// <summary>
        /// Accessible by anything within the assembly.
        /// </summary>
        Internal = 1,

        /// <summary>
        /// Accessible by anything within the class.
        /// </summary>
        Private = 2
    }
}

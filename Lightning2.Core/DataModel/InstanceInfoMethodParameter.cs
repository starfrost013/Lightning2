using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// InstanceInfoMethodParameter
    /// 
    /// March 9, 2021 (modified April 17, 2021: The great re-namespacing)
    /// 
    /// Defines a method parameter for instance information. 
    /// </summary>
    public class InstanceInfoMethodParameter
    {
        public Type ParamType { get; set; }
        public string ParamName { get; set; }

        /// <summary>
        /// Convert .NET parameter information to lightning instanceinfomethodparameter
        /// </summary>
        /// <param name="PI"></param>
        /// <returns></returns>
        public static InstanceInfoMethodParameter FromParameterInfo(ParameterInfo PI)
        {
            InstanceInfoMethodParameter IIMP = new InstanceInfoMethodParameter();

            IIMP.ParamName = PI.Name;
            IIMP.ParamType = PI.ParameterType;

            return IIMP; 
        }
    }
}

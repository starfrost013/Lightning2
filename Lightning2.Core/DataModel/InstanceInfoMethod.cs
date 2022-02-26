using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Determines a method
    /// </summary>
    public class InstanceInfoMethod 
    {
        public string MethodName { get; set; }
        public InstanceInfoProperty Property { get; set; }
        public List<InstanceInfoMethodParameter> Parameters { get; set; }

        /// <summary>
        /// Convert .NET MethodInfo to Lightning instance information for methods.
        /// </summary>
        /// <param name="CurMethod"></param>
        /// <returns></returns>
        public static InstanceInfoMethod FromMethodInfo(MethodInfo CurMethod)
        {
            InstanceInfoMethod IIMX = new InstanceInfoMethod();

            IIMX.MethodName = CurMethod.Name;

            ParameterInfo[] PIPList = CurMethod.GetParameters();

            foreach (ParameterInfo PI in PIPList)
            {
                InstanceInfoMethodParameter IIMP = InstanceInfoMethodParameter.FromParameterInfo(PI);

                IIMX.Parameters.Add(IIMP);
            }

            return IIMX; 

        }

        public InstanceInfoMethod()
        {
            Parameters = new List<InstanceInfoMethodParameter>();
        }
        
    }
}

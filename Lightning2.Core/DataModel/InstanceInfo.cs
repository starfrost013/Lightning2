using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq; 
using System.Reflection; 
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// InstanceInfo class.
    /// 
    /// Holds metadata about an instance - its methods, properties, and their metadata. Used for the IDE. May be expanded to a ReflectionMetadata.xml
    /// 
    /// Converted from System.Reflection types at boot.
    /// 
    /// Translated to and from .NET System.Reflection types as required for the IDE and the datamodel serialiser. 
    /// 
    /// INTERNAL ONLY - USE ONLY FOR POPULATING IDE. NOT FOR SCRIPTS.
    /// </summary>
    public class InstanceInfo
    {
        /// <summary>
        /// The list of methods within the instance.
        /// </summary>
        public List<InstanceInfoMethod> Methods { get; set; }

        /// <summary>
        /// The list of properties within this instance.
        /// </summary>
        public List<InstanceInfoProperty> Properties { get; set; }

        /// <summary>
        /// Get InstanceInfo from a type.
        /// 
        /// fucking compiler stifling my generic type parameters again!
        /// </summary>
        /// <typeparam name="T">The DataModel-conformant type you wish to get the InstanceInfo from.</typeparam>
        /// <returns></returns>
        public static InstanceInfoResult FromType(Type TType) 
        {
            InstanceInfoResult IIR = new InstanceInfoResult();

            if (!TType.IsSubclassOf(typeof(Instance)))
            {
                // Successful is true by default
                IIR.FailureReason = "Type is not DataModel compliant!";
                return IIR;
            }
            else
            {
                // Get ALL properties; this allows for internal and private properties.
                MemberInfo[] MI = TType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                List<PropertyInfo> PIList = new List<PropertyInfo>();
                List<MethodInfo> MIList = new List<MethodInfo>();

                foreach (MemberInfo MemberInformation in MI)
                {
                    switch (MemberInformation.MemberType)
                    {
                        case MemberTypes.Property:

                            PropertyInfo CurrentPI = null;

                            CurrentPI = TType.GetProperty(MemberInformation.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                            if (CurrentPI == null) continue;

                            PIList.Add(CurrentPI);

                            Type PropertyType = CurrentPI.PropertyType;

                            // This code allows for things like color3.
                            // we check for properties that have their own members and if so, iterate through those
                            // 2021-03-11
                            MemberInfo[] PIInfo = PropertyType.GetMembers();

                            if (PIInfo.Length > 0)
                            {
                                //todo: cache type information
                                if (PropertyType.IsAssignableFrom(typeof(Instance)))
                                {
                                    // Prevents a stack overflow by preventing recursive instanceinfo parsing
                                    if (!InstanceInfo_CheckIfFiltered(MemberInformation.Name)) FromType(PropertyType);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }


                            continue;
                        case MemberTypes.Method:
                            MethodInfo CurrentMI = null;

                            Type[] Types = new Type[] { TType };

                            CurrentMI = TType.GetMethod(MemberInformation.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                            MIList.Add(CurrentMI);
                            continue;
                    }
                }

                // Process each method and add its parameters
                foreach (MethodInfo CurMethod in MIList)
                {
                    InstanceInfoMethod IIM = InstanceInfoMethod.FromMethodInfo(CurMethod);

                    IIR.InstanceInformation.Methods.Add(IIM);
                }

                //todo: instanceinfoproperty.frompropertyinfo
                foreach (PropertyInfo CurProperty in PIList)
                {
                    InstanceInfoProperty IIP = new InstanceInfoProperty();

                    IIP.Name = CurProperty.Name;
                    IIP.Type = CurProperty.PropertyType;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                    MethodInfo? GetMethodIX = CurProperty.GetGetMethod(true);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

                    if (GetMethodIX == null)
                    {
                        IIP.Accessibility = InstanceAccessibility.Public;
                    }
                    else
                    {
                        if (GetMethodIX.IsPrivate)
                        {
                            IIP.Accessibility = InstanceAccessibility.Private;
                        }
                        else if (GetMethodIX.IsAssembly)
                        {
                            IIP.Accessibility = InstanceAccessibility.Internal;
                        }
                        else
                        {
                            IIP.Accessibility = InstanceAccessibility.Public;
                        }

                    }


                    IIR.InstanceInformation.Properties.Add(IIP);

                }

                IIR.Successful = true;
                return IIR;
            }

        } 

        /// <summary>
        /// Prevents a stack overflow by preventing recursive instances
        /// </summary>
        /// <param name="PropertyName">The property name to check</param>
        /// <returns></returns>
        private static bool InstanceInfo_CheckIfFiltered(string PropertyName)
        {
            return (PropertyName.ContainsCaseInsensitive("Parent")
                || PropertyName.ContainsCaseInsensitive("Child")
                );
        }

        private static MemberInfoResult InstanceInfo_RemoveAmbiguousMatches(Type ThisType, MemberInfo[] MemberInformation)
        {
            MemberInfoResult IIR = new MemberInfoResult(); 

            List<MemberInfo> MIList = MemberInformation.ToList();

            for (int i = 0; i < MIList.Count; i++)
            {
                MemberInfo MI = MIList[i];

                for (int j = 0; j < MIList.Count; j++)
                {
                    MemberInfo MI2 = MIList[j];

                    if (MI2 == MI)
                    {
                        if (MI2.DeclaringType.IsSubclassOf(ThisType)
                            || MI2.DeclaringType == ThisType)
                        {

                            MIList.Remove(MI2);
                        }
                        else
                        {
                            MIList.Remove(MI); 
                        }


                    }
                }
            }

            IIR.Successful = true;
            IIR.MemberInfo = MIList;
            return IIR; 
        }

        public InstanceInfoMethod GetMethod(string MethodName)
        {
            foreach (InstanceInfoMethod IIM in Methods)
            {
                if (IIM.MethodName == MethodName)
                {
                    return IIM; 
                }
            }

            return null; // return null on failure/
        }
        
        public InstanceInfoProperty GetProperty(string PropertyName)
        {
            foreach (InstanceInfoProperty IIP in Properties)
            {
                if (IIP.Name == PropertyName)
                {
                    return IIP;
                }
            }

            return null; 
            
        }
        /// <summary>
        /// Gets the value of the <paramref name="Obj"/> Instance's <paramref name="PropertyName"/> property.
        /// </summary>
        /// <param name="PropertyName">The name of the property you wish to acquire the value of.</param>
        /// <param name="Obj">The object you wish to acquire the property value of. </param>
        /// <returns>The value for the object if successful, otherwise returns null.</returns>
        public object GetValue(string PropertyName, Instance Obj)
        {
            Type Typ = Obj.GetType();

            foreach (InstanceInfoProperty IIP in Properties)
            {
                if (IIP.Name == PropertyName)
                {
                    PropertyInfo PI = Typ.GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                    return PI.GetValue(Obj);
                }
            }

            ErrorManager.ThrowError("DataModel", "AttemptedToAcquireInvalidPropertyInfoException", $"Attempted to acquire the property name {PropertyName}, that is not in the type {Typ.Name}!");
            return null; 
        }

        public InstanceInfo()
        {
            Methods = new List<InstanceInfoMethod>();
            Properties = new List<InstanceInfoProperty>(); 
        }
    }
}

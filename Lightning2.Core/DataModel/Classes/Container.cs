using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Container
    /// 
    /// July 4, 2021
    /// 
    /// Defines a generic container of any Lightning object. Works in tandem with grouping. 
    /// </summary>
    public class Container : ControllableObject
    {
        internal override string ClassName => "Container";

        internal override InstanceTags Attributes => InstanceTags.Serialisable | InstanceTags.Instantiable | InstanceTags.Destroyable | InstanceTags.ShownInIDE | InstanceTags.ParentCanBeNull;
        public static Container FromListOfInstances(List<Instance> InstanceList)
        {

            Container Cnt = new Container();

            foreach (Instance Ins in InstanceList) Cnt.Children.Add(Ins);

            return Cnt;
        }

        public static Container FromInstanceCollection(InstanceCollection InstanceList)
        {

            Container Cnt = new Container();

            foreach (Instance Ins in InstanceList) Cnt.Children.Add(Ins);

            return Cnt;
        }
    }
}

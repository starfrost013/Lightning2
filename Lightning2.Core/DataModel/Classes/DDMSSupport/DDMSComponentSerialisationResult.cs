using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Lightning.Core.API
{
    public class DDMSComponentSerialisationResult : Result
    {
        public XmlDocument XmlDocument { get; set; }
    }
}

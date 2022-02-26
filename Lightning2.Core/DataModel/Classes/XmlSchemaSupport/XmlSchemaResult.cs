using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace Lightning.Core.API
{
    /// <summary>
    /// XmlSchemaResult
    /// 
    /// Result class for XML schemas.
    /// </summary>
    public class XmlSchemaResult : Result
    {
        public XmlSeverityType RSeverity { get; set; }
    }
}

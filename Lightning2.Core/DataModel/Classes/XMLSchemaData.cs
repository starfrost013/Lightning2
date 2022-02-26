using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// 2021-03-06
    /// 
    /// Non-instantiable
    /// 
    /// XMLSchemaData
    /// 
    /// Holds information about an XML schema.
    /// 
    /// 2021-03-06: Created.
    /// 2021-03-09: Added properties.
    /// 2021-03-30: Renamed to XMLSchemaData. Removed from DataModel.
    /// </summary>
    public class XmlSchemaData
    {
        public string SchemaPath { get; set; }
        public string XmlPath { get; set; }
    }
}

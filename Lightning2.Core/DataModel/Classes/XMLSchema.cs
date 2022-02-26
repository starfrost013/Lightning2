using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.IO; 
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Lightning.Core.API
{
    public class LightningXMLSchema
    {
        public XmlSchema Schema { get; set; }
        public XmlSchemaData XSI { get; set; }

        /// <summary>
        /// Get around the requirement for validationeventhandler to return void (The API had a spaz. Hold out! API!)
        /// </summary>
        private XmlSchemaResult __dumbhack { get; set; }
        public LightningXMLSchema()
        {
            XSI = new XmlSchemaData();

        }

        /// <summary>
        /// Validates XML located at <see cref="XmlSchemaData.XmlPath"/> against the schema <see cref="XmlSchemaData.SchemaPath"/>. These must both be set or an error will be thrown.
        /// </summary>
        /// <returns></returns>
        public XmlSchemaResult Validate()
        {
            XmlSchemaResult XSR = new XmlSchemaResult();

            if (XSI.SchemaPath == null
                || XSI.XmlPath == null)
            {
                XSR.FailureReason = "Invalid XmlReaderSettings!";
                XSR.RSeverity = XmlSeverityType.Error;
                return XSR;
            }
            else
            {

                string LWSchemaPath = PathUtil.GetLightningPath(XSI.SchemaPath);

                // hack to get around some stuff just as a test
                LWSchemaPath = LWSchemaPath.Replace("\\\\", "\\");

                if (!File.Exists(LWSchemaPath))
                {
                    string ErrorString = $"Cannot find the XML schema at {LWSchemaPath}!";

                    ErrorManager.ThrowError("XML Schema Validator", "CannotFindXmlFileOrSchemaException", ErrorString);
                    XSR.FailureReason = ErrorString;
                    return XSR;
                }
                else
                {
                    if (!File.Exists(XSI.XmlPath))
                    {
                        string ErrorString = $"Cannot find the XML file at {XSI.XmlPath}!";

                        ErrorManager.ThrowError("XML Schema Validator", "CannotFindXmlFileOrSchemaException", ErrorString);
                        XSR.FailureReason = ErrorString;
                        return XSR;
                    }
                    else
                    {
                        return Validate_DoValidate(); 
                    }
                }
            }

        }

        /// <summary>
        /// Actually performs the XML schema validation
        /// </summary>
        /// <returns></returns>
        private XmlSchemaResult Validate_DoValidate()
        {
            XmlSchemaResult XSR = new XmlSchemaResult(); 

            XmlReaderSettings XRS = new XmlReaderSettings();
            XRS.ValidationType = ValidationType.Schema;

            XRS.IgnoreComments = true;
            XRS.IgnoreWhitespace = true;
            XRS.ValidationEventHandler += Validate_OnFail;

            XmlReader XR = XmlReader.Create(XSI.XmlPath, XRS);

            // yes we have to do this.
            while (XR.Read())
            {

            }

            // check if we didn't fail (dumb hack)
            if (__dumbhack == null)
            {
                XSR.Successful = true;
                return XSR;
            }
            else
            {
                XSR.FailureReason = __dumbhack.FailureReason;

                // warning or error
                XSR.RSeverity = __dumbhack.RSeverity;
                return XSR;
            }
        }

        private void Validate_OnFail(object sender, ValidationEventArgs EventArgs)
        {
            __dumbhack = new XmlSchemaResult();
            __dumbhack.RSeverity = EventArgs.Severity;
            switch (EventArgs.Severity)
            {
                case XmlSeverityType.Warning:
                    string ValidationWarningReason = $"XML Validation warning: {EventArgs.Exception}";
                    Logging.Log(ValidationWarningReason, "XMLSchema", MessageSeverity.Warning);
                    __dumbhack.FailureReason = ValidationWarningReason;
                        
                    return;
                case XmlSeverityType.Error:
                    string ValidationErrorReason = $"XML Validation error! {EventArgs.Exception}";
                    Logging.Log(ValidationErrorReason, "XMLSchema", MessageSeverity.Error);
                    __dumbhack.FailureReason = ValidationErrorReason;

                    return;
            }
        }

    }
}

using Lightning.Core.API; 
using NuCore.Utilities;
using System; 
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Schema; // need to create our own xmlseveritytype equivalent
using System.Xml.Serialization;

namespace Lightning.Core
{
    /// <summary>
    /// Non-DataModel (Engine Core)
    /// 
    /// GlobalSettings
    /// 
    /// 2021-04-05
    /// 
    /// Holds global settings for the engine [DataModel] 
    /// 
    /// </summary>
    [XmlRoot("GlobalSettings")]
    public class GlobalSettings // not static for reasons
    {
        /// <summary>
        /// The path to the GlobalSettings XML.
        /// </summary>
        public static string GLOBALSETTINGS_XML_PATH = @"Content\EngineContent\GlobalSettings.xml";

        /// <summary>
        /// The path to the GlobalSettings XML schema.
        /// </summary>
        public static string GLOBALSETTINGS_XSD_PATH = @"Content\Schema\GlobalSettings.xsd";

        /// <summary>
        /// The schema version
        /// </summary>
        public static string GLOBALSETTINGS_XSD_SCHEMAVERSION = "0.2.0.0002";

        /// <summary>
        /// Has the GlobalSettings been loaded?
        /// </summary>
        public static bool GLOBALSETTINGS_LOADED { get; set; }

        /// <summary>
        /// A list of services to be started by the Service Control Manager at init time. Contains an optional startup order and list of strings. 
        /// </summary>
        public ServiceStartupCommandCollection ServiceStartupCommands { get; set; }

        /// <summary>
        /// The default splash screen
        /// </summary>
        public string DefaultSplashScreen { get; set; }

        /// <summary>
        /// The path to Lightning.xsd, the XML schema for LGX files.
        /// </summary>
        public string LightningXsdPath { get; set; }

        /// <summary>
        /// Boot splash path 
        /// </summary>
        public string BootSplashPath { get; set; }


        /// <summary>
        /// Serialises \EngineContent\GlobalSettings.xml to an instance of GlobalSettings. 
        /// </summary>
        /// <returns></returns>
        public static GlobalSettingsResult SerialiseGlobalSettings()
        {
            GlobalSettingsResult GSR = new GlobalSettingsResult();

            XmlSchemaResult XSR1 = SerialiseGlobalSettings_Validate();

            if (!XSR1.Successful)
            {
                switch (XSR1.RSeverity)
                {
                    // Throw a warning or fatalerror.
                    case XmlSeverityType.Warning:
                        string FailureReason = $"An error occurred during serialisation of settings. Some or all global engine settings may fail to load: {XSR1.FailureReason}";
                        ErrorManager.ThrowError("GlobalSettings Serialiser", "GlobalSettingsValidationWarningException", FailureReason);
                        GSR.FailureReason = FailureReason;

                        return GSR; 
                    case XmlSeverityType.Error:
                        string FailureReason2 = $"Failed to serialise GlobalSettings: {XSR1.FailureReason}!";
                        ErrorManager.ThrowError("GlobalSettings Serialiser", "GlobalSettingsValidationFailureException", FailureReason2);

                        GSR.FailureReason = FailureReason2;
                        return GSR; // this is a fatal error
                }

               
                
                return GSR; 
            }
            else
            {
                GlobalSettingsResult SSR = SerialiseGlobalSettings_Serialise();


                GSR.Successful = SSR.Successful;
                GSR.Settings = SSR.Settings;

                GLOBALSETTINGS_LOADED = true; 

                if (!SSR.Successful)
                {
                    ErrorManager.ThrowError("GlobalSettings Serialiser", "FailedToSerialiseGlobalSettingsException", $"Failed to serialise GlobalSettings: {SSR.FailureReason}", SSR.BaseException);
                    return GSR;
                }
                else
                {
                    
                    return GSR; 
                }
            }
        }

        private static XmlSchemaResult SerialiseGlobalSettings_Validate()
        {
            LightningXMLSchema LXMLS = new LightningXMLSchema();
            LXMLS.XSI.XmlPath = GLOBALSETTINGS_XML_PATH;
            LXMLS.XSI.SchemaPath = GLOBALSETTINGS_XSD_PATH;

            XmlSchemaResult XSR = LXMLS.Validate();

            return XSR;
        }

        /// <summary>
        /// Serialises GlobalSettings. 
        /// </summary>
        /// <returns>A <see cref="GlobalSettingsResult"/> object. The serialised GlobalSettings object is located within </returns>
        private static GlobalSettingsResult SerialiseGlobalSettings_Serialise() // genericresult for now?
        {
            GlobalSettingsResult GSR = new GlobalSettingsResult(); 

            XmlSerializer XS = new XmlSerializer(typeof(GlobalSettings));

            string XmlPath = GLOBALSETTINGS_XML_PATH;

            XmlReader XR = XmlReader.Create(XmlPath);

            try
            {
                // we are creating it as globalsettings so we can easily convert
                GSR.Settings = (GlobalSettings)XS.Deserialize(XR);
                GSR.Successful = true;
                return GSR;
            }
            catch (InvalidOperationException err)
            {
                GSR.FailureReason = $"Failed to serialise: {err}.\n\nInner exception (this may provide more information surrounding the issue): {err.InnerException}";
                GSR.BaseException = err; 
                return GSR; 
            }

        }

#if DEBUG
        public void ATest()
        {
            foreach (ServiceStartupCommand SSC in ServiceStartupCommands)
            {
                Logging.Log("ServiceStartupCommand:\n");
                Logging.Log($"Service Name: {SSC.ServiceName}");
                Logging.Log($"Start Order: {SSC.StartOrder}");
            }
        }

#endif

    }
}

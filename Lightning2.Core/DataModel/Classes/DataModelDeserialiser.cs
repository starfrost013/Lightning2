using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq; 
using System.Reflection; 
using System.Text;
using System.Xml;
using System.Xml.Linq; 
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Lightning.Core.API
{
    /// <summary>
    /// Dynamic DataModel Serialiser
    /// 
    /// Version 0.9.1
    /// 
    /// Created 2021-03-16
    /// Modified 2021-04-17 (Version 0.9.0: Add XML output capabilities - version 0.9.1: Move to Lightning.Core.API)
    /// 
    /// Dynamically serialises XML (.lgx files) to Lightning DataModel objects.
    /// </summary>
    public partial class DataModelDeserialiser : Instance
    {

        internal override string ClassName => "DataModelDeserialiser";

        internal override InstanceTags Attributes => InstanceTags.Instantiable | InstanceTags.Destroyable;
        /// <summary>
        /// An incredibly dumb hack to get around the compiler
        /// </summary>
        private static bool IsSuccessful { get; set; }

        /// <summary>
        /// The XML schema version. 
        /// </summary>
        public static string XMLSCHEMA_VERSION = "0.3.1.0007";

        public DataModel DDMS_Deserialise(string Path)
        {
            try
            {
                LightningXMLSchema LXMLS = new LightningXMLSchema();

                GlobalSettings GS = DataModel.GetGlobalSettings();

                LXMLS.XSI.SchemaPath = GS.LightningXsdPath;
                LXMLS.XSI.XmlPath = Path; 

                return DDMS_DoDeserialise(LXMLS, Path);
            }
            catch (DirectoryNotFoundException err)
            {
                ErrorManager.ThrowError(ClassName, "CannotFindLgxFileException", $"Cannot find the file {Path}\n\n{err}");

                return null; // temp? until we have nonstatic datamodel
            }
            catch (FileNotFoundException err)
            {
                ErrorManager.ThrowError(ClassName, "CannotFindLgxFileException", $"Cannot find the file {Path}\n\n{err}");

                return null; // temp? until we have nonstatic datamodel
            }
        }

        /// <summary>
        /// DDMS (Dynamic DataModel Serialiser)
        /// 
        /// Transform an XML document to a DataModel.
        /// 
        /// 2021-03-11
        /// 
        /// make ddmsserialisationresult?
        /// 
        /// </summary>
        /// <param name="Schema">The schema to utilise for serialisation.</param>
        /// <param name="Path">The path to the XML document to serialise. </param>
        /// <returns></returns>
        private DataModel DDMS_DoDeserialise(LightningXMLSchema Schema, string Path)
        {
            XmlSchemaResult XSR = DDMS_Validate(Schema);

            if (!XSR.Successful)
            {
                ErrorManager.ThrowError(ClassName, "ErrorValidatingLGXFileException", $"An error occurred validating the XML against the schema: {XSR.FailureReason}");
                return null;
            }

            Logging.Log($"DDMS: Reading {Path} and transforming to DataModel...");

            XDocument XD = XDocument.Load(Path);

            // Create the datamodel that we will be returning.
            DataModel DM = new DataModel();
            
            List<string> ValidComponents = new List<string>();

            foreach (int EVal in Enum.GetValues(typeof(DDMSComponents)))
            {
                // Death to Dumb Hacks (especially political ones)!
                string EvalString = Enum.GetName(typeof(DDMSComponents), EVal);

                if (EvalString == null)
                {
                    // TEMP - SERIALISATION - ERRORS.XML
                    return null;
                    // VTEMP - SERIALISATION - ERRORS.XML
                }
                else
                {
                    ValidComponents.Add(EvalString);
                }

            }

            // We have already determined the valid components,
            // so no error checking
            foreach (string ValidComponent in ValidComponents)
            {
                // Every file must have all compoennts
                DDMSComponents DDMSComp = (DDMSComponents)Enum.Parse(typeof(DDMSComponents), ValidComponent);
                Logging.Log($"Serialising file component: {DDMSComp}");
                DDMSDeserialisationResult DDSRMS = DDMS_SerialiseFileComponent(XD, DM, DDMSComp);

                if (DDSRMS.Successful)
                {
                    DM = DDSRMS.DataModel;
                    continue;
                }
                else
                {
                    Logging.Log($"Failed to serialise DDMS component - {DDSRMS.FailureReason}", $"DDMS Serialiser - {DDMSComp}", MessageSeverity.Error);
                    return null;
                }

            }

            DataModel.DATAMODEL_LASTXML_PATH = Path;
            DDMS_OnSucceed_NotifyAll(); 
            return DM; 
            
        }

        private void DDMS_OnSucceed_NotifyAll()
        {
            GetMultiInstanceResult GMIR = DataModel.GetChildren(true);

            foreach (Instance DMChild in GMIR.Instances) // if this fails you have bigger problems
            {
                if (DMChild.OnLoad != null)
                {
                    DMChild.OnLoad();
                }
            }
        }

        private XmlSchemaResult DDMS_Validate(LightningXMLSchema Schema)
        {
            XmlSchemaResult DDSR = Schema.Validate();

            if (!DDSR.Successful)
            {
                DDSR.FailureReason = $"XML schema validation failure: {DDSR.FailureReason}";
                return DDSR;
            }
            else
            {
                DDSR.Successful = true;
                return DDSR;
            }

          
            
        }

        /// <summary>
        /// Event handler for DDMS schema validation event
        /// </summary>
        /// <param name="sender">The sender of the DDMS schema validation event</param>
        /// <param name="e">The event arguments for the DDMS schema validation event - see <see cref="ValidationEventArgs"/>.</param>
        private void DDMS_Validate_OnFail(object sender, ValidationEventArgs e)
        {
            IsSuccessful = false; 
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
#if DEBUG
                    ErrorManager.ThrowError(ClassName, "DDMSSchemaValidationWarningException", $"Warning opening game: XML serialisation warning! {e.Message}\n\n{e.Exception}");
#endif
                    return;
                case XmlSeverityType.Error:
#if DEBUG
                    ErrorManager.ThrowError(ClassName, "DDMSSchemaValidationWarningException", $"Error opening game: XML serialisation failure. {e.Message}\n\n{e.Exception}");
#else
                    ErrorManager.ThrowError(ClassName, "DDMSSchemaValidationWarningException", $"The Game XML is invalid.");
#endif
                    return;

            }
        }

        /// <summary>
        /// Parses the DDMS file components.
        /// </summary>
        /// <param name="XM">The XmlReader to use for reading</param>
        /// <param name="DM">The DataModel to serialise to</param>
        /// <param name="Component">The DDMS Component to serialise</param>
        /// <returns></returns>
        private DDMSDeserialisationResult DDMS_SerialiseFileComponent(XDocument XD, DataModel DM, DDMSComponents Component)
        {
            DDMSDeserialisationResult DDSR = new DDMSDeserialisationResult();
            
            switch (Component)
            {
                case DDMSComponents.Metadata:
                    DDSR = DDMS_ParseMetadataComponent(XD, DM);

                    if (!DDSR.Successful)
                    {
                        ErrorManager.ThrowError(ClassName, "DDMSMetadataValidationError", DDSR.FailureReason);
                        return DDSR; 
                    }

                    return DDSR; 
                case DDMSComponents.Settings:
                    DDSR = DDMS_ParseSettingsComponent(XD, DM);
                    DDSR.DataModel = DM;

                    return DDSR;
                case DDMSComponents.Workspace:
                    DDSR = DDMS_ParseInstanceTreeComponent(XD, DM);
                    return DDSR;
            }

            return DDSR;


        }

        private bool DDMS_CheckCompatible(GameMetadata GM)
        {
            if (GM.MinimumLightningBuild == 0) // not specified
            {
                return true;
            }
            else
            {
                return (LVersion.Build >= GM.MinimumLightningBuild);
            }
            
        }

        /// <summary>
        /// Parses the metadata component of a DDSR-compliant file.
        /// 
        /// March 20, 2021
        /// </summary>
        /// <param name="XM"></param>
        /// <param name="DM"></param>
        /// <returns></returns>
        private DDMSDeserialisationResult DDMS_ParseMetadataComponent(XDocument XD, DataModel DM)
        {
            // Clear global datamodel state
            // Might need to make it non-static
            DataModel.Clear();

            DDMSDeserialisationResult DDSR = new DDMSDeserialisationResult();

            GameMetadata GM = (GameMetadata)DataModel.CreateInstance("GameMetadata");

            XElement XMetadataNode = XD.Root.Element("Metadata");

            if (XMetadataNode != null)
            {
                switch (XMetadataNode.NodeType)
                {
                    // blame the hp stream for my unproductive day 
                    // also i had to go to my dads
                    // 2021-03-20
                    case XmlNodeType.Element:

                        string ElementName = XMetadataNode.Name.LocalName;

                        Logging.Log($"Parsing element: {ElementName}", ClassName);

                        try
                        {
                            List<XElement> XMetadataContentNodes = XMetadataNode.Elements().ToList();

                            if (XMetadataContentNodes.Count != 0)
                            {
                                foreach (XElement XMetadataContentNode in XMetadataContentNodes)
                                {
                                    // Store the element name
                                    ElementName = XMetadataContentNode.Name.LocalName;
                                    string ElementValue = XMetadataContentNode.Value; 

                                    // this can almost certainly be renamed
                                    if (XmlUtil.CheckForValidXmlElementContent(XMetadataContentNode))
                                    {
                                        // Log it
                                        Logging.Log($"{ElementName}: {ElementValue}", ClassName);

                                        switch (ElementName)
                                        {
                                            // Author of this game
                                            case "Author":
                                                GM.Author = ElementValue;
                                                continue;
                                            // Game creation date
                                            case "CreationDate":
                                                GM.CreationDate = DateTime.Parse(ElementValue);
                                                continue;
                                            // DataModel schema version
                                            case "DMSchemaVersion":
                                                if (XMetadataContentNode.Value != XMLSCHEMA_VERSION)
                                                {
                                                    DDSR.FailureReason = $"Invalid schema version! Using version {ElementValue}, expected {XMLSCHEMA_VERSION}!";
                                                    return DDSR;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            // Minimum Lightning version
                                            case "MinimumLightningBuild":
                                                GM.MinimumLightningBuild = Convert.ToInt32(ElementValue);
                                                continue;
                                            // Game last modified date
                                            case "LastModifiedDate":
                                                GM.LastModifiedDate = DateTime.Parse(ElementValue);
                                                continue;
                                            // Game name
                                            case "Name":
                                                GM.GameName = ElementValue;
                                                continue;
                                            // Game revision ID
                                            case "RevisionID":
                                                GM.RevisionNumber = Convert.ToInt32(ElementValue);
                                                continue;
                                            case "Version":
                                                GM.Version = ElementValue;
                                                continue; 
                                        }
                                    }
                                    else
                                    {
                                        continue; 
                                    }
                                }
                            }
                            
                        }
                        catch (FormatException err)
                        {
                            string ErrDesc = $"Attempted to serialise invalid XML\n{err}";

                            DDSR.FailureReason = ErrDesc;
                            return DDSR;
                        }

                        if (!DDMS_CheckCompatible(GM)) 
                        {
                            DDSR.FailureReason = $"Incompatible Lightning version: Build {GM.MinimumLightningBuild} or later required, running build {LVersion.Build}!";
                            return DDSR;
                        }

                        DDSR.Successful = true;
                        DDSR.DataModel = DM;
                        return DDSR; 
                    

                }
            }

            return DDSR;
        }

        private DDMSDeserialisationResult DDMS_ParseSettingsComponent(XDocument XD, DataModel DM)
        {

            // Serialises the game settings for this LGX file (Lightning Game XML)
            DDMSDeserialisationResult DDSR = new DDMSDeserialisationResult();

            List<XElement> XSettingsTreeNodeList = XD.Root.Elements("Settings").ToList();

            GameSettings GS = (GameSettings)DataModel.CreateInstance("GameSettings");

            foreach (XElement XmlElement in XSettingsTreeNodeList)
            {
                // Skip all comments etc.
                switch (XmlElement.NodeType)
                {
                    case XmlNodeType.Element: // "Settings" element
                        List<XElement> SettingsElements = XmlElement.Elements().ToList();

                        foreach (XElement SettingElement in SettingsElements)
                        {
                            switch (SettingElement.NodeType)
                            {
                                case XmlNodeType.Element:
                                    GetGameSettingsResult GGSR = DDMS_ParseSettingsComponent_ParseSetting(SettingElement, GS);

                                    // Check that the result was successful
                                    if (GGSR.Successful)
                                    {
                                        GS = GGSR.GameSettings;
                                        continue; 
                                    }
                                    else
                                    {
                                        DDSR.FailureReason = $"Failed to load game settings: {GGSR.FailureReason}";
                                        return DDSR;
                                    }

                                default:
                                    continue; 
                            }
                        }

                        continue;
                    default:
                        // Ignore whitespaces
                        continue; 
                }
                
            }

            DDSR.Successful = true;
            return DDSR; 

        }

        private GetGameSettingsResult DDMS_ParseSettingsComponent_ParseSetting(XElement SettingsElement, GameSettings GS)
        {
            GetGameSettingsResult GGSR = new GetGameSettingsResult();

            // Parse an individual setting component.
            GameSetting NewSetting = (GameSetting)GS.AddChild("GameSetting");

            List<XElement> XSettingsElement = SettingsElement.Elements().ToList();

            foreach (XElement XSettingElement in XSettingsElement)
            {
                string ElementName = XSettingElement.Name.LocalName;

                string ElementValue = XSettingElement.Value;

                if (XmlUtil.CheckForValidXmlElementContent(XSettingElement))
                {
                    // could we store everything tempoarily and then convert? hmm? or is it redundant
                    switch (ElementName)
                    {
                        case "Name":

                            Logging.Log($"Loading Setting with Name: {ElementValue}...", ClassName);
                            NewSetting.SettingName = ElementValue;
                            NewSetting.Name = NewSetting.SettingName;
                            continue; 
                        case "Type": // The type of the 
                            try
                            {
                                // By default, load Lightning DataModel stuff
                                // If it contains a namespace name, load types from that namespace instead. Limit the namespaces we can load.

                                Type Typ; 

                                // If there's no namespace provided...
                                if (!ElementValue.Contains('.'))
                                {
                                    string TypeString = $"{DataModel.DATAMODEL_NAMESPACE_PATH}.{ElementValue}";

                                    Logging.Log($"Type: {TypeString}");

                                    Typ = Type.GetType(TypeString);
                                }
                                else
                                {
                                    if (XmlUtil.CheckIfValidTypeForInstantiation(ElementValue))
                                    {
                                        string TypeString = $"{ElementValue}";

                                        Logging.Log($"Type: {TypeString}");

                                        Typ = Type.GetType($"{ElementValue}");
                                    }
                                    else
                                    {
                                        GGSR.FailureReason = "Attempted to load a setting with a forbidden type (not in the System or Lightning.* namespaces)";
                                        return GGSR; 
                                    }
                                }


                                NewSetting.SettingType = Typ; 
                                continue;
                            }
                            catch (ArgumentNullException err)
                            {
#if DEBUG
                                GGSR.FailureReason = $"Attempted to load a setting with a non-existent type!\n\n{err}";
#else
                                GGSR.FailureReason = $"Attempted to load a setting with a non-existent type!";
#endif
                                return GGSR;
                            }
                            catch (TypeLoadException err)
                            {
#if DEBUG
                                GGSR.FailureReason = $"Attempted to load a setting with a type that does not exist!\n\n{err}";
#else
                                GGSR.FailureReason = $"Attempted to load a setting with a type that does not exist!";
#endif
                                return GGSR;
                            }
                        case "Value": // must be before Type!
                            try
                            {
                                Logging.Log($"Value: {ElementValue}");

                                Type ATyp = NewSetting.SettingType;

                                // If it's in the DataModel...
                                // this code is very redundant but whatever?
                                if (ATyp.IsSubclassOf(typeof(Instance)))
                                {
                                    // then add it to the datamodel
                                    // this isn't too good as it will result in crappy objects that I don't like polluting the workspace.
                                    NewSetting.SettingValue = DataModel.CreateInstance(NewSetting.SettingType.Name);
                                    continue; 

                                }
                                else
                                {
                                    TypeConverter TC = TypeDescriptor.GetConverter(ATyp);
                                    NewSetting.SettingValue = TC.ConvertFromString(ElementValue);
                                    continue; 
                                }
                            }
                            catch (NotSupportedException err)
                            {
#if DEBUG
                                GGSR.FailureReason = $"Attempted to load a setting with an invalid value!\n\n{err}";
#else
                                GGSR.FailureReason = $"Attempted to load a setting with an invalid value!";
#endif
                                return GGSR;
                            }

                    }
                }
                else
                {
                    GGSR.FailureReason = "Attempted to load Setting with invalid or zero-length content!";
                    return GGSR;
                }
            }

            // If it's successful...
            GGSR.Successful = true;
            GGSR.GameSettings = GS;
            return GGSR; 
        }


        private DDMSDeserialisationResult DDMS_ParseInstanceTreeComponent(XDocument XD, DataModel DM)
        {

            DDMSDeserialisationResult DDSR = new DDMSDeserialisationResult();

            XElement XInstanceTreeNode;

            List<XElement> XInstanceTreeNodeList = XD.Root.Elements("Workspace").ToList();

            if (XInstanceTreeNodeList.Count == 0)
            {
                DDSR.FailureReason = "Cannot find Workspace!";
                return DDSR; 
            }
            else
            {
                XInstanceTreeNode = XInstanceTreeNodeList[0];
            }

            List<XElement> XInstanceChildNodes = XInstanceTreeNode.Elements().ToList();

            // Loop through all child nodes.
            foreach (XElement XInstanceChildNode in XInstanceChildNodes)
            {
                switch (XInstanceChildNode.NodeType)
                {
                    case XmlNodeType.Element:
                        try
                        {
                            DDMSDeserialisationResult DDSR_Element = DDMS_SerialiseElementToDMObject(DM, XInstanceChildNode);
                            
                            if (DDSR_Element.Successful)
                            {
                                DDSR.DataModel = DDSR_Element.DataModel;
                            }
                            else
                            {
                                DDSR.FailureReason = $"An error occurred parsing a node: {DDSR_Element.FailureReason}";
                                return DDSR; 
                            }

                        }
                        catch (Exception err)
                        {
                            DDSR.FailureReason = $"DDMS: Conversion error: An exception occurred during the node parsing process:\n\n{err}";
                            return DDSR;
                        }
                   
                        continue;

                }
            }

            DDSR.Successful = true; 
            return DDSR; 
        }

        /// <summary>
        /// Private: Serialise an element to a DataModel object.
        /// </summary>
        /// <param name="DM"></param>
        /// <param name="XInstanceChildNode"></param>
        /// <returns></returns>
        private DDMSDeserialisationResult DDMS_SerialiseElementToDMObject(DataModel DM, XElement XInstanceChildNode, Instance Parent = null)
        {

            string XDataModelName = XInstanceChildNode.Name.LocalName;

            DDMSDeserialisationResult DDSR = new DDMSDeserialisationResult();
            
            // Namespace path to DataModel

            // Todo: EngineGlobal?
            Type XDR = Type.GetType($"{DataModel.DATAMODEL_NAMESPACE_PATH}.{XDataModelName}");

            if (XDR != null)
            {
                //bug: no checks
                //see: polymorphism
                //THIS IS NOT AN INSTANCE THIS IS WHATEVER CLASS WE HAVE JUST CREATED IT IS COMPILE-TIME AN INSTANCE BUT AT RUNTIME IT IS THE RESULT OF DATAMODEL.CREATEINSTANCE!

                // Start any services that are not running,
                // and if they are running, do nothing
                if (XDR.IsSubclassOf(typeof(Service)))
                {
                    Workspace Ws = DataModel.GetWorkspace();

                    GetInstanceResult GIR = Ws.GetFirstChildOfType("ServiceControlManager");

                    if (GIR.Successful)
                    {
                        ServiceControlManager SCM = (ServiceControlManager)GIR.Instance;

                        if (SCM.IsServiceRunning(XDataModelName))
                        {
                            DDSR.Successful = true;
                            DDSR.DataModel = DM;
                            return DDSR;
                        }
                        else
                        {
                            ServiceStartResult SSR = SCM.StartService(XDataModelName);

                            if (!SSR.Successful)
                            {
                                DDSR.FailureReason = $"Failed to start a service specified in the XML: {SSR.FailureReason}";
                                return DDSR;
                            }
                            else
                            {
                                DDSR.Successful = true;
                                return DDSR;
                            }
                        }
                    }
                    else
                    {
                        ErrorManager.ThrowError("DataModel", "ServiceControlManagerFailureException");
                        return DDSR; // this will not run as this is aftal aerror
                    }

                }

                Instance XDRInstance; 

                // April 9, 2021:
                // Implement nested serialisation for Children
                if (Parent == null)
                {
                    XDRInstance = (Instance)DataModel.CreateInstance(XDataModelName);
                }
                else
                {
                    XDRInstance = (Instance)Parent.AddChild(XDataModelName); 
                }
                
                // TODO: instantiationresult from datamodel.createinstance
                if (XDRInstance == null)
                {
                    // successful false by default
                    DDSR.FailureReason = "Object is not in the datamodel or the object is non-instantiable!";
                    return DDSR;
                }
                else
                {
                    List<XAttribute> XDMObjectAttributes = XInstanceChildNode.Attributes().ToList();

                    foreach (XAttribute XDMObjectAttribute in XDMObjectAttributes)
                    {
                        string XPropertyName = XDMObjectAttribute.Name.LocalName;

                        Logging.Log($"Parsing Attribute to DataModel: {XPropertyName}", ClassName);

                        // perform a kind of wizardry with InstanceInfo and classes

                        foreach (InstanceInfoProperty IIP in XDRInstance.Info.Properties)
                        {
                            // We have found the instance property that we want
                            if (XPropertyName == IIP.Name)
                            {
                                // WIZARD TIME
                                // Convert from string to arbitrary type! :D 

#pragma warning disable CS8632
                                // Updated to use TypeDescriptor for Vector2 (April 11, 2021)
                                TypeConverter CConvertedObjectTC = TypeDescriptor.GetConverter(IIP.Type);
                                object? CConvertedObject = CConvertedObjectTC.ConvertFromString(XDMObjectAttribute.Value);
#pragma warning restore CS8632
                                // TODO: NESTING
                                if (CConvertedObject != null)
                                {
                                    PropertyInfo PI = XDR.GetProperty(XPropertyName);

                                    if (PI == null)
                                    {
                                        ErrorManager.ThrowError(ClassName, "AttemptedToSetInvalidPropertyException", $"Attempted to set invalid property {XPropertyName} on an object of class {IIP.Type}");
                                        continue; 
                                    }

                                    if (PI.PropertyType.IsSubclassOf(typeof(Instance)))
                                    {
                                        Instance CInstanceObject = (Instance)CConvertedObject;

                                        if (!CInstanceObject.Attributes.HasFlag(InstanceTags.Serialisable)) // optimised (January 29, 2022)
                                        {
                                            // April 9, 2021: Don't fail on a non-serialisable object.
                                            DDSR.Successful = true;
                                            return DDSR;
                                        }
                                    }

                                    PI.SetValue(XDRInstance, CConvertedObject);

                                }
                                else
                                {
                                    DDSR.FailureReason = "DDMS: Conversion error: Unknown error";
                                    return DDSR;
                                }
                            }
                        }
                    }

                    List<XElement> XMetadataChildren = XInstanceChildNode.Elements().ToList();

                    if (XMetadataChildren.Count == 0)
                    {
                        DDSR.Successful = true;
                        DDSR.DataModel = DM;
                        return DDSR;
                    }
                    else
                    {
                        foreach (XElement MetadataChild in XMetadataChildren)
                        {
                            DDMS_SerialiseElementToDMObject(DM, MetadataChild, XDRInstance);
                        }
                    }
                }
            }
            else
            {
                DDSR.FailureReason = $"Error: attempted to instantiate invalid type {XDataModelName}!";
                return DDSR; 
            }

            DDSR.Successful = true;
            DDSR.DataModel = DM;
            // this should not run
            return DDSR; 
        }

    }
}

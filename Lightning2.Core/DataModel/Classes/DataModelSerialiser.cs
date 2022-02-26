using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO; 
using System.Text;
using System.Xml;

namespace Lightning.Core.API
{
    /// <summary>
    /// Dynamic DataModel Serialiser 0.x/1.x - DataModelSerialiser (DataModelDeserialiser class)
    /// 
    /// April 14, 2021
    /// 
    /// Implements the serialisation (saving) side of DDMS.
    /// </summary>
    public partial class DataModelDeserialiser
    {
        /// <summary>
        /// Saves a DDMS file using XML Serialisation.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public GenericResult DDMS_Serialise(string Path)
        {
            Logging.Log($"Saving DataModel to {Path}.", ClassName); 
            GenericResult GR = new GenericResult();

            if (DDMS_Serialise_FindRequiredComponents())
            {
                // create a new XmlDocument and add a new node
                // not using XDocument for writing as the api is fucked up the ass.
                XmlDocument XD = new XmlDocument();

                XmlNode XNRoot = XD.CreateElement("Lightning");
                XD.AppendChild(XNRoot);

                foreach (string FName in Enum.GetNames(typeof(DDMSComponents)))
                {
                    XmlNode XComponentNode = XD.CreateElement(FName);

                    XNRoot.AppendChild(XComponentNode);

                }

                Debug.Assert(XNRoot.HasChildNodes);

                foreach (XmlNode XComponentNode in XNRoot.ChildNodes)
                {
                    switch (XComponentNode.Name)
                    {
                        case "Metadata":
                            DDMSComponentSerialisationResult DDCSR_Metadata = DDMS_Serialise_SerialiseMetadataComponent(XD, XComponentNode);

                            if (!DDCSR_Metadata.Successful)
                            {
                                ErrorManager.ThrowError(ClassName, "FailedToSaveLgxException", $"Failed to save LGX file: Error parsing Metadata component: {DDCSR_Metadata.FailureReason}");
                            }
                            else
                            {
                                XD = DDCSR_Metadata.XmlDocument;
                            }

                            continue;
                        case "Settings":
                            DDMSComponentSerialisationResult DDCSR_Settings = DDMS_Serialise_SerialiseSettingsComponent(XD, XComponentNode);

                            if (!DDCSR_Settings.Successful)
                            {
                                ErrorManager.ThrowError(ClassName, "FailedToSaveLgxException", $"Failed to save LGX file: Error parsing Settings component: {DDCSR_Settings.FailureReason}");
                            }
                            else
                            {
                                XD = DDCSR_Settings.XmlDocument;
                            }

                            continue;
                        case "Workspace":
                            DDMSComponentSerialisationResult DDCSR_Workspace = DDMS_Serialise_SerialiseWorkspaceComponent(XD, XComponentNode);

                            if (!DDCSR_Workspace.Successful)
                            {
                                ErrorManager.ThrowError(ClassName, "FailedToSaveLgxException", $"Failed to save LGX file: Error parsing Workspace component: {DDCSR_Workspace.FailureReason}");
                            }
                            else
                            {
                                XD = DDCSR_Workspace.XmlDocument;
                            }

                            continue;
                    }
                }

                try
                {
                    XD.Save(Path);
                }
                catch (Exception ex) // check for any I/O errors, etc
                {
                    string FailureReason = $"Error occurred saving XML document: {ex}";
                    ErrorManager.ThrowError(ClassName, "FailedToSaveLgxException", FailureReason);

                    GR.FailureReason = FailureReason;
                    return GR;
                }

                DataModel.DATAMODEL_LASTXML_PATH = Path;
                GR.Successful = true;

                return GR; 
            }
            else
            {
                string ErrorString = "Failed to save LGX file: Invalid DataModel - must exit!";
                ErrorManager.ThrowError(ClassName, "InvalidDataModelCannotSaveLgxException", ErrorString);

                GR.FailureReason = ErrorString; 
                return GR; // will not run
            }

        }

        private bool DDMS_Serialise_FindRequiredComponents()
        {
            Workspace Ws = DataModel.GetWorkspace();

            GetInstanceResult GIR1 = Ws.GetFirstChildOfType("GameMetadata");
            GetInstanceResult GIR2 = Ws.GetFirstChildOfType("GameSettings");

            if (!GIR1.Successful || !GIR2.Successful
                || GIR1.Instance == null || GIR2.Instance == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private DDMSComponentSerialisationResult DDMS_Serialise_SerialiseMetadataComponent(XmlDocument XD, XmlNode XMetadataNode)
        {
            Logging.Log("Saving metadata...", ClassName);

            DDMSComponentSerialisationResult DDCSR = new DDMSComponentSerialisationResult();

            // we already checked
            Workspace Ws = DataModel.GetWorkspace();

            GetInstanceResult GIR = Ws.GetFirstChildOfType("GameMetadata");

            GameMetadata GMA = (GameMetadata)GIR.Instance;

            // Create the Metadata XML elements.
            XmlNode XDMSchemaVersion = XD.CreateElement("DMSchemaVersion");
            XmlNode XAuthor = XD.CreateElement("Author");
            XmlNode XDescription = XD.CreateElement("Description");
            XmlNode XGameName = XD.CreateElement("GameName"); 
            XmlNode XCreationDate = XD.CreateElement("CreationDate");
            XmlNode XLastModifiedDate = XD.CreateElement("LastModifiedDate");
            XmlNode XRevisionID = XD.CreateElement("RevisionID");
            XmlNode XVersion = XD.CreateElement("Version");

            XDMSchemaVersion.InnerText = XMLSCHEMA_VERSION;
            if (GMA.Author != null) XAuthor.InnerText = GMA.Author;
            if (GMA.Description != null) XDescription.InnerText = GMA.Description;

            if (GMA.Name == null || GMA.Name == "")
            {
                DDCSR.FailureReason = "Please name this Game!";
                return DDCSR; 
            }

            string CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (GMA.RevisionNumber == 0)
            {
                XCreationDate.InnerText = CurrentTime;
            }
            else
            {
                XCreationDate.InnerText = GMA.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            XLastModifiedDate.InnerText = CurrentTime;

            GMA.RevisionNumber += 1; 

            XRevisionID.InnerText = GMA.RevisionNumber.ToString();

            // Version is optional
            if (GMA.Version != null) XVersion.InnerText = GMA.Version;

            XMetadataNode.AppendChild(XDMSchemaVersion);
            if (XAuthor.InnerText != null
                && XAuthor.InnerText != "") XMetadataNode.AppendChild(XAuthor);
            if (XDescription.InnerText != null
                && XDescription.InnerText != "") XMetadataNode.AppendChild(XDescription);
            XMetadataNode.AppendChild(XCreationDate);
            XMetadataNode.AppendChild(XLastModifiedDate);
            XMetadataNode.AppendChild(XGameName); 
            XMetadataNode.AppendChild(XRevisionID);
            if (XVersion.InnerText != null
                && XVersion.InnerText != "") XMetadataNode.AppendChild(XVersion); // only append if used.

            Logging.Log("Saved metadata!", ClassName); 

            DDCSR.Successful = true;
            DDCSR.XmlDocument = XD;
            return DDCSR; 
        }

        private DDMSComponentSerialisationResult DDMS_Serialise_SerialiseSettingsComponent(XmlDocument XD, XmlNode XSettingsNode)
        {
            Logging.Log("Saving GameSettings...", ClassName); 

            DDMSComponentSerialisationResult DDCSR = new DDMSComponentSerialisationResult();

            Workspace Ws = DataModel.GetWorkspace();

            GetInstanceResult GIR = Ws.GetFirstChildOfType("GameSettings");

            // check already made
            GameSettings GS = (GameSettings)GIR.Instance;

            foreach (Instance Setting in GS.Children)
            {
                Type SettingType = Setting.GetType();

                // continue if not a gamesetting
                if (Setting.ClassName != "GameSetting"
                    || SettingType != typeof(GameSetting))
                {
                    continue; 
                }
                else
                {
                    GameSetting SettingToAdd = (GameSetting)Setting;

                    XmlNode XSettingNode = XD.CreateElement("Setting");

                    XmlNode XNameNode = XD.CreateElement("Name");
                    XmlNode XTypeNode = XD.CreateElement("Type");
                    XmlNode XValueNode = XD.CreateElement("Value");

                    XNameNode.InnerText = SettingToAdd.SettingName.ToString();
                    XValueNode.InnerText = SettingToAdd.SettingValue.ToString(); 

                    string XTypeNodeValueName = SettingToAdd.SettingType.FullName;

                    if (XmlUtil.CheckIfValidTypeForInstantiation(XTypeNodeValueName))
                    {
                        if (XTypeNodeValueName.ContainsCaseInsensitive("Lightning."))
                        {
                            XTypeNodeValueName.Replace("Lightning.", "");
                        }

                        XTypeNode.InnerText = XTypeNodeValueName;

                        XSettingNode.AppendChild(XNameNode);
                        XSettingNode.AppendChild(XTypeNode);
                        XSettingNode.AppendChild(XValueNode);

                        XSettingsNode.AppendChild(XSettingNode);

                        Logging.Log($"Saved the GameSetting with the name {XNameNode.InnerText} of type {XTypeNode.InnerText} with the value {XValueNode.InnerText}!");

                    }
                    else
                    {
                        DDCSR.FailureReason = "This Setting cannot be saved - the Type must be in the System or Lightning.* namespaces!";
                        return DDCSR;
                    }

                }
            }

            Logging.Log("Saved GameSettings!");

            DDCSR.Successful = true;
            DDCSR.XmlDocument = XD; 
            return DDCSR;
        }

        private DDMSComponentSerialisationResult DDMS_Serialise_SerialiseWorkspaceComponent(XmlDocument XD, XmlNode XWorkspaceNode)
        {
            DDMSComponentSerialisationResult DDCSR = new DDMSComponentSerialisationResult();

            Logging.Log("Saving Workspace...", ClassName);

            Workspace Ws = DataModel.GetWorkspace();

            foreach (Instance Ins in Ws.Children)
            {
                if (Ins.Attributes.HasFlag(InstanceTags.Archivable))
                {
                    if (Ins.Name != null)
                    {
                        Logging.Log($"Serialising ClassName: {Ins.ClassName}...");
                    }
                    else
                    {
                        Logging.Log($"Serialising ClassName: {Ins.ClassName} Name: {Ins.Name}...");
                    }

                    DDCSR = DDMS_Serialise_SerialiseDMObjectToElement(XD, XWorkspaceNode, Ins);

                    if (!DDCSR.Successful)
                    {
                        return DDCSR;
                    }
                }
            }

            Logging.Log("Saved Workspace!", ClassName);
            DDCSR.Successful = true;
            DDCSR.XmlDocument = XD;
            return DDCSR; 
        }

        private DDMSComponentSerialisationResult DDMS_Serialise_SerialiseDMObjectToElement(XmlDocument XD, XmlNode XWorkspaceNode, Instance Ins)
        {

            DDMSComponentSerialisationResult DDCSR = new DDMSComponentSerialisationResult();

            XmlNode XInstanceNode = XD.CreateElement(Ins.ClassName);

            List<InstanceInfoProperty> IIP = Ins.Info.Properties;

            foreach (InstanceInfoProperty IIPItem in IIP)
            {
                
                string PropertyName = IIPItem.Name;

                XmlAttribute XPropertyAttribute = XD.CreateAttribute(IIPItem.Name);

                object Value = Ins.Info.GetValue(PropertyName, Ins);
                object AttributesValue = (InstanceTags)Ins.Info.GetValue("Attributes", Ins);

                Debug.Assert(AttributesValue != null);

                if (Value == null
                    || IIPItem.Accessibility != InstanceAccessibility.Public) // only save public properties
                {
                    continue;
                }
                else
                {
                    TypeConverter TC = TypeDescriptor.GetConverter(Value.GetType());

                    try
                    {
                        XPropertyAttribute.Value = (string)TC.ConvertTo(null, null, Value, typeof(string));

                        if (XPropertyAttribute == null)
                        {
                            DDCSR.FailureReason = $"An error occurred converting the attribute {IIPItem.Name} of the DataModel-enabled type {Ins.ClassName} to a string in order to save it to a file.";
                            return DDCSR;
                        }
                        else
                        {
                            XPropertyAttribute.Value = XPropertyAttribute.Value.Cs2Xaml(); // convert illegal characters (April 29, 2021)

                            XInstanceNode.Attributes.Append(XPropertyAttribute);

                            Logging.Log($"Serialised Property {PropertyName} with value {XPropertyAttribute.Value}");
                        }

                    }
                    catch (NotSupportedException)
                    {
                        DDCSR.FailureReason = $"An error occurred converting the attribute {IIPItem.Name} of the DataModel-enabled type {Ins.ClassName} to a string in order to save it to a file: The property cannot be converted to a string!";
                        return DDCSR;
                    }

                }

            }

            XWorkspaceNode.AppendChild(XInstanceNode);

            if (Ins.Children.Count != 0)
            {
                foreach (Instance InsChild in Ins.Children)
                {
                    DDMS_Serialise_SerialiseDMObjectToElement(XD, XInstanceNode, InsChild);
                }
            }

            DDCSR.Successful = true;
            DDCSR.XmlDocument = XD;
            return DDCSR; 
        }


#if DEBUG
        public void ATest() => DDMS_Serialise("WritingTest.xml");

#endif
    }

}

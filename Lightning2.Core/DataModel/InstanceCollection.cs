using NuCore.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lightning.Core.API
{
    public class InstanceCollection : IEnumerable
    {
        public List<Instance> Instances { get; set; }

        public int Count => Instances.Count; 

        public InstanceCollection()
        {
            Instances = new List<Instance>(); 
        }

        public InstanceCollection(List<Instance> NInstances)
        {
            Instances = new List<Instance>(); 

            foreach (Instance Inx in NInstances)
            {
                Instances.Add(Inx); 
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public InstanceCollectionEnumerator GetEnumerator()
        {
            return new InstanceCollectionEnumerator(Instances);
        }

        /// <summary>
        /// Adds the object <paramref name="Obj"/> to the children of this Object. 
        ///
        /// This transparently works with any level in the hierarchy!
        /// 
        /// April 6, 2021
        /// </summary>
        /// <param name="Obj"></param>
        public void Add(object Obj, Instance Parent = null, ObjectCreated OCHandler = null)
        {
            // Get the types of the object and its parent.
            Type ObjType = Obj.GetType();

            if (ObjType.IsSubclassOf(typeof(Instance)))
            {
                Instance TestInstance = (Instance)Obj;

                Instance TestInstanceParent;

                if (Parent == null)
                {
                    TestInstanceParent = TestInstance.GetParent();
                }
                else
                {
                    TestInstanceParent = Parent;
                }

                // Check if tbis Instance has a parent. 
                if (TestInstanceParent == null)
                {
                    // this Instance is part of the DataModel Root
                    if (TestInstance.Attributes.HasFlag(InstanceTags.ParentCanBeNull))
                    {
                        Add_PerformAdd(Obj);
                        return; 
                    }
                    else
                    {
                        GetInstanceResult WorkSvc = DataModel.GetFirstChildOfType("Workspace");

                        if (!WorkSvc.Successful
                            || WorkSvc.Instance == null) 
                        {
                            Debug.Assert(WorkSvc.FailureReason != null);
                            ErrorManager.ThrowError("DataModel", "WorkspaceHasBeenDestroyedException");
                        }
                        else
                        {

                            Workspace Workspace = (Workspace)WorkSvc.Instance;

                            // Check for a Service, and redirect it to the SCM if it is one. 
                            // April 29, 2021
                            if (ObjType.IsSubclassOf(typeof(Service)))
                            {
                                GetInstanceResult ServiceControlManagerResult = Workspace.GetFirstChildOfType("ServiceControlManager");

                                if (!ServiceControlManagerResult.Successful
                                    || ServiceControlManagerResult.Instance == null)
                                {
                                    ErrorManager.ThrowError("DataModel", "ServiceControlManagerFailureException"); // Fatal Error
                                }
                                else
                                {
                                    ServiceControlManager SCM = (ServiceControlManager)ServiceControlManagerResult.Instance;

                                    Add_PerformAdd(Obj, SCM);
                                }
                            }
                            else
                            {
                                Add_PerformAdd(Obj, Workspace);
                                return;
                            }
                        }
                    }
                    
                }
                else // If it has a parent, add it to the parent. 
                {
                    Type ParentType = Parent.GetType();

                    // Instance children must be the same or child classes unless a flag is set in the instance' instancetags
                    if (ObjType == ParentType)
                    {
                        Add_PerformAdd(Obj, TestInstanceParent);
                        return; 
                    }
                    else
                    {
                        if (ObjType.IsSubclassOf(ParentType))
                        {
                            Add_PerformAdd(Obj, TestInstanceParent);
                            return;
                        }
                        else if (TestInstance.Attributes.HasFlag(InstanceTags.ParentCanBeNull))
                        {
                            Add_PerformAdd(Obj, TestInstanceParent);
                            return;
                        }
                        else
                        {

                            ErrorManager.ThrowError("DataModel", "CannotAddThatInstanceAsChildException", $"{ObjType.Name} cannot be a child of {ParentType.Name}!");
                            return;
                        }
                    }
                }
            }
            else
            {
                // throw an error
                ErrorManager.ThrowError("DataModel", "ThatIsNotAnInstancePleaseDoNotTryToAddItToTheDataModelException", $"Attempted to add an object of class {ObjType.Name} to the DataModel when it does not inherit from Instance!");
                return; 
            }

        }

        /// <summary>
        /// Adds an Instance to the InstanceCollection and then runs its OnCreate method. 
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="Parent"></param>
        private void Add_PerformAdd(object Obj, Instance Parent = null, ObjectCreated OCHandler = null)
        {
            // polymorphism mandates this being the instance we want.
            
            Instance NewInstance = (Instance)Obj;

            if (Parent == null)
            {
                Instances.Add(NewInstance);
            }
            else
            {
                
                Instance InstanceObj = NewInstance;
                InstanceObj.Parent = Parent; 
                Parent.Children.Instances.Add(NewInstance);
            }
    
            // todo: oncreate AND PASSING THIS TO DATAMODEL.CREATEINSTANCE!

            if (OCHandler != null)
            {
                OCHandler(this); 
            }

            NewInstance.OnCreate(); 
        }

        /// <summary>
        /// Removes an Object from the InstanceCollection. 
        /// 
        /// Checks for InstanceTags and that an object is in the DataModel. 
        /// 
        /// April 9, 2021
        /// </summary>
        /// <param name="Obj"></param>
        public void Remove(object Obj, Instance Parent = null)
        {
            Type ObjType = Obj.GetType();

            // Check for people passing idiotic things to this method.
            if (!ObjType.IsSubclassOf(typeof(Instance)))
            {
                ErrorManager.ThrowError("DataModel", "ThatIsNotAnInstancePleaseDoNotAttemptToRemoveItFromTheDataModelException");
                return; 
            }
            else
            {
                Instance TestInstance = (Instance)Obj;

                // Save the parent of this instance.
                Instance InstanceParent;

                if (Parent == null)
                {
                    InstanceParent = TestInstance.Parent;
                }
                else
                {
                    InstanceParent = Parent; 
                }
                

                // Check that the instance is destroyable. 
                if (!TestInstance.Attributes.HasFlag(InstanceTags.Destroyable))
                {
                    ErrorManager.ThrowError("DataModel", "CannotDestroyNonDestroyableInstanceException", $"{ObjType.Name} cannot be destroyed, as its InstanceTags do not include Destroyable.");
                    return; 
                }
                else
                {
                    if (TestInstance.OnDestroyed != null)
                    {
                        TestInstance.OnDestroyed(this);
                    }

                    if (InstanceParent == null) // If there is no parent, check ParentCanBeNull to see if it is inserted at the DataModel root or the Workspace.
                    {
                        if (TestInstance.Attributes.HasFlag(InstanceTags.ParentCanBeNull))
                        {
                            if (DataModel.Contains(TestInstance))
                            {
                                Remove_PerformRemove(TestInstance);
                            }
                            else
                            {
                                ErrorManager.ThrowError("DataModel", "AttemptedToRemoveInstanceThatIsNotPartOfDataModelException");
                                return; 
                            }

                                
                        }
                        else // Id 
                        {

                            Workspace WS = DataModel.GetWorkspace();
                            
                            if (WS.Children.Contains(TestInstance))
                            {
                                Remove_PerformRemove(TestInstance, WS);
                            }
                            else
                            {
                                ErrorManager.ThrowError("DataModel", "AttemptedToRemoveInstanceThatIsNotPartOfDataModelException");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (InstanceParent.Children.Contains(TestInstance))
                        {
                            Remove_PerformRemove(TestInstance, InstanceParent);
                        }
                        else
                        {
                            ErrorManager.ThrowError("DataModel", "AttemptedToRemoveInstanceThatIsNotAChildOfItsParentException");
                        }
                    }
                }

            }
        }

        private void Remove_PerformRemove(Instance ObjToRemove, Instance Parent = null)
        {
            // Remove all children of this Instance.
            ObjToRemove.RemoveAllChildren(); //TODO: move this

            if (Parent == null) // Parent will be passed as Workspace for those in the Workspace
            {
                InstanceCollection DMState = DataModel.GetState();
                DMState.Instances.Remove(ObjToRemove);
            }
            else
            {

                Parent.Children.Instances.Remove(ObjToRemove);
            }
        }

        /// <summary>
        /// Clear the InstanceCollection.
        /// </summary>
        public void Clear()
        {
            // Rewritten April 9, 2021 to ensure usage of our custom remove method
            for (int i = 0; i < Instances.Count; i++)
            {
                Instance Ins = Instances[i];

                // April 14, 2021: ACTUALLY use the custom remove method
                
                // hack
                if (Ins.Attributes.HasFlag(InstanceTags.Destroyable))
                {
                    Remove(Ins);
                }
                
            }
        }

        /// <summary>
        /// Ungroups an instance - places all of its children into the children of its Parent and destroys the Instance.
        /// </summary>
        public void Ungroup(Instance Ins, Instance Parent)
        {
            Logging.Log("Ungrouping...", "DataModel");
            foreach (Instance Child in Ins.Children)
            {
                // Move the child up one in the hierarchy.
                Parent.AddChildI(Child);

                Ins.RemoveChild(Child);
            }

            // we don't need to recurse, as all of the children of the children are already a part of the datamodel

            Remove(Ins); 
        }

        /// <summary>
        /// Direct check for finding a specific Instance in an InstanceCollection. 
        /// </summary>
        /// <param name="Obj">Does it contain Obj?</param>
        public bool Contains(Instance Obj) => Instances.Contains(Obj);

        /// <summary>
        /// Gets the first child of this Instance with ClassName <see cref="ClassName"/>
        /// </summary>
        /// <returns>A <see cref="GetInstanceResult"/> object. The Instance is <see cref="GetInstanceResult.Instance"/>.</returns>
        public GetInstanceResult GetFirstChildOfType(string ClassName)
        {
            GetInstanceResult GIR = new GetInstanceResult();

            foreach (Instance Child in Instances)
            {
                if (Child.ClassName == ClassName)
                {
                    GIR.Instance = Child;
                    GIR.Successful = true;
                    return GIR;
                }
            }

            GIR.FailureReason = $"This instance does not have a child of ClassName {ClassName}!";

            return GIR;
        }

        public GetInstanceResult GetFirstChildOfTypeT(Type Typ, bool AcceptInheritance = true)
        {
            GetInstanceResult GIR = new GetInstanceResult();

            foreach (Instance Child in Instances)
            {
                Type ChType = Child.GetType();

                if (ChType == Typ
                || (AcceptInheritance && ChType.IsSubclassOf(Typ)))
                {
                    GIR.Instance = Child;
                    GIR.Successful = true;
                    return GIR;
                }
            }

            GIR.FailureReason = $"This instance does not have a child of Type {Typ}!";

            return GIR;
        }

        /// <summary>
        /// Gets the last chil of this Instance with Class Name <see cref="ClassName"/>
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public GetInstanceResult GetLastChildOfType(string ClassName)
        {
            GetInstanceResult GIR = new GetInstanceResult();

            // create a list that we will use for the discovery of these instances. 
            List<Instance> MatchingInstances = new List<Instance>();

            foreach (Instance Child in Instances)
            {
                if (Child.ClassName == ClassName)
                {
                    MatchingInstances.Add(Child);
                }
            }

            // if we do not find any instances...
            if (MatchingInstances.Count == 0)
            {
                GIR.FailureReason = $"This instance does not have a child of ClassName {ClassName}!";
                return GIR;
            }
            else
            {
                GIR.Successful = true;
                GIR.Instance = MatchingInstances[MatchingInstances.Count - 1];
                return GIR;
            }
        }

        /// <summary>
        /// Indexer (April 29, 2021)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Instance this[int i] => Instances[i];

        public GetMultiInstanceResult GetAllChildrenOfType(string ClassName)
        {
            List<Instance> NewLI = new List<Instance>();

            return GetAllChildrenOfType_DoGetChildren(NewLI, ClassName);

        }

        private GetMultiInstanceResult GetAllChildrenOfType_DoGetChildren(List<Instance> InstanceList, string ClassName)
        {
            GetMultiInstanceResult GIR = new GetMultiInstanceResult();

            foreach (Instance ThisChild in this)
            {
                Type ParentType = Type.GetType($"{DataModel.DATAMODEL_NAMESPACE_PATH}.{ClassName}");
                Type ChildType = ThisChild.GetType();

                if (ThisChild.ClassName == ClassName
                    || ChildType.IsSubclassOf(ParentType))
                {
                    InstanceList.Add(ThisChild);
                }
            }

            GIR.Successful = true;
            GIR.Instances = InstanceList;
            return GIR;
        }


        /// <summary>
        /// Acquires the logical children of this object.
        /// </summary>
        /// <returns>A <see cref="GetMultiInstanceResult"/> object containing the success status of the method, with <see cref="GetMultiInstanceResult.Instances"/> containing the logical children of this object.</returns>
        public GetMultiInstanceResult GetChildren(bool Recursive = false)
        {
            GetMultiInstanceResult GMIR = new GetMultiInstanceResult();

            if (!Recursive)
            {
                foreach (Instance Child in this)
                {
                    GMIR.Instances.Add(Child);
                }

                GMIR.Successful = true;
                return GMIR;
            }
            else
            {
                return GetChildren_Recursive(GMIR); 
            }
        }

        private GetMultiInstanceResult GetChildren_Recursive(GetMultiInstanceResult GMIR, Instance Child = null)
        {
            if (Child == null)
            {
                foreach (Instance TChild in this)
                {
                    GMIR.Instances.Add(TChild);
                    if (TChild.Children.Count > 0) GetChildren_Recursive(GMIR, TChild);
                }
            }
            else
            {
                foreach (Instance TChild in Child.Children)
                {
                    GMIR.Instances.Add(TChild);
                    if (TChild.Children.Count > 0) GetChildren_Recursive(GMIR, TChild);
                }
            }


            return GMIR;
        }
    }

    public class InstanceCollectionEnumerator : IEnumerator
    {
        public List<Instance> Instances { get; set; }
        public Instance Current { get
            {
                try
                {
                    return Instances[Position];
                }
                catch (IndexOutOfRangeException)
                {
                    ErrorManager.ThrowError("DataModel", "AttemptedToAcquireInvalidInstanceException");
                    return null;
                }
            } 

        }

        object IEnumerator.Current
        {
            get
            {
                return (object)Current;
            }
        }

          
        private int Position = -1;

        public void Reset() => Position = -1;

        public bool MoveNext()
        {
            Position++;
            return (Position < Instances.Count);
        }

        public InstanceCollectionEnumerator(List<Instance> NewInstances) => Instances = NewInstances;

    }
}

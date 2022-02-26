using NuCore.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Lightning.Core
{

    public class ServiceStartupCommandCollection : IEnumerable
    {
        public List<ServiceStartupCommand> Commands { get; set; }

        public ServiceStartupCommandCollection()
        {
            Commands = new List<ServiceStartupCommand>();
        }

        public ServiceStartupCommandCollection(List<ServiceStartupCommand> NewCommands) // if we end up passing references to this a lot we are going to have to make this code worse :(
        {
            Commands = NewCommands; 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ServiceStartupCommandCollectionEnumerator GetEnumerator()
        {
            return new ServiceStartupCommandCollectionEnumerator(Commands);
        }

        /// <summary>
        /// Adds a <see cref="ServiceStartupCommand"/> to a <see cref="ServiceStartupCommandCollection"/>.
        /// </summary>
        /// <param name="Obj"></param>
        public void Add(object Obj)
        {
            if (Obj == null) ErrorManager.ThrowError("GlobalSettings Loader", "InternalSerialisationErrorException", "Attempted to serialise a null object.");

            if (Obj.GetType() == typeof(ServiceStartupCommand)) 
            {
                Add_PerformAdd(Obj);
            }
            else
            {
                ErrorManager.ThrowError("GlobalSettings Loader", "AttemptedToAddNonServiceStartupCommandToServiceStartupCommandCollectionException");
                return; 
            }
        }

        private void Add_PerformAdd(object Obj)
        {
            Commands.Add((ServiceStartupCommand)Obj); 
        }
    }

    public class ServiceStartupCommandCollectionEnumerator : IEnumerator
    {
        public int Position = -1;
        public void Reset() => Position = -1; 

        public bool MoveNext()
        {
            Position++;
            return (Position < Commands.Count);
        }

        public List<ServiceStartupCommand> Commands { get; set; }
        
        object IEnumerator.Current
        {
            get
            {
                return (object)Current;
            }
        }

        public ServiceStartupCommand Current
        {
            get
            {
                return Commands[Position];
            }
        }

        public ServiceStartupCommandCollectionEnumerator(List<ServiceStartupCommand> NCommands)
        {
            Commands = NCommands;
        }

    }
}

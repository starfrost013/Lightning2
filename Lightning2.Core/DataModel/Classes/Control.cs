using NuCore.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Control
    /// 
    /// April 14, 2021 (modified January 25, 2022: Add KeySym)
    /// 
    /// A control binding.
    /// </summary>
    public class Control : Instance
    {
        internal override string ClassName => "Control";
        internal override InstanceTags Attributes => InstanceTags.Archivable | InstanceTags.Destroyable | InstanceTags.Instantiable | InstanceTags.Serialisable;
        public SDL.SDL_Keysym KeyCode { get; set; }

        /// <summary>
        /// A string representation of this key
        /// </summary>
        public string KeySym => KeyCode.ToString();
        public bool Repeated { get; set; }
    }
}

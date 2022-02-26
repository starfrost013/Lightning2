using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// Metadata (inherits from SerialisableInstance)
    /// 
    /// Contains information about the file. 
    /// </summary>
    public class GameMetadata : Instance
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal override string ClassName => "GameMetadata";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal override InstanceTags Attributes => InstanceTags.Instantiable | InstanceTags.Destroyable | InstanceTags.ShownInIDE | InstanceTags.ShownInProperties;

        /// <summary>
        /// The author of this Game.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The description of this Game.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of this Game. 
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// The creation date of this Game.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The date this Game was last modified.
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// The revision number of this Game. 0 if a game has not been saved yet. 
        /// </summary>
        public int RevisionNumber { get; set; }

        /// <summary>
        /// An optional version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The minimum Lightning build for this game to run.
        /// </summary>
        public int MinimumLightningBuild { get; set; }
    }
}

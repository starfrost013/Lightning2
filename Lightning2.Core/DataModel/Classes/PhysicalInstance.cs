using NuCore.Utilities;
using NuRender; 
using NuRender.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// PhysicalInstance
    /// 
    /// April 9, 2021 (modified August 9, 2021)
    /// 
    /// Defines a physically rendered object in Lightning, with a Position, Size, and a Texture (stored as a logical child). Rendered every frame by RenderService.
    /// </summary>
    public class PhysicalInstance : Instance
    {
        /// <summary>
        /// <inheritdoc/> -- set to PhysicalInstance.
        /// </summary>
        internal override string ClassName => "PhysicalInstance";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal override InstanceTags Attributes => base.Attributes;

        /// <summary>
        /// Can this object collide?
        /// </summary>
        public bool CanCollide { get; set; }

        /// <summary>
        /// The position of this object in the world. 
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The size of this object.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The Z-index of this object.
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// The colour of this object. Not actually used by this class *specifically* but IS used by classes that inherit from this.
        /// </summary>
        public Color4 Colour { get; set; }

        // Texture is a child. 


        /// <summary>
        /// Gets or sets the colour of the border of this GuiElement. 
        /// </summary>
        public Color4 BorderColour { get; set; }

        /// <summary>
        /// Gets or sets the border width of this GuiElement - if it is set to zero, border drawing will be skipped as there is nothing to draw.
        /// </summary>
        public int BorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the border fill state. If true, the border will be filled. If false, it will not.
        /// </summary>
        public bool BorderFill { get; set; }

        /// <summary>
        /// Gets or sets the background colour of this GuiElement.
        /// </summary>
        public Color4 BackgroundColour { get; set; }

        /// <summary>
        /// Is this object at rest?
        /// </summary>
        public bool AtRest { get; set; }


        /// <summary>
        /// This is completely fucking bullshit but is required for screengui and it makes me sad
        /// </summary>
        internal bool ForceToScreen { get; set; }

        /// <summary>
        /// Ran on the spawning of an object, before it is rendered for the first time and after the initialisation of the renderer.
        /// </summary>
        public virtual void OnSpawn()
        {
            return; 
        }


        /// <summary>
        /// Click event handler.
        /// 
        /// Default event handler may be implemented by any Lightning class.
        /// 
        /// Scripts may modify the event handler function. 
        /// </summary>

        public MouseDownEvent Click { get; set; }

        /// <summary>
        /// Mouse enter event handler.
        /// 
        /// Default event handler may be implemented by any Lightning class.
        /// 
        /// Scripts may modify the event handler function. 
        /// </summary>
        public MouseEnterEvent OnMouseEnter { get; set; }

        /// <summary>
        /// Mouse leave event handler.
        /// 
        /// Default event handler may be implemented by any Lightning class.
        /// 
        /// Scripts may modify the event handler function. 
        /// </summary>
        public MouseLeaveEvent OnMouseLeave { get; set; }

        /// <summary>
        /// Mouse up event handler.
        /// 
        /// Default event handler may be implemented by any Lightning class.
        /// 
        /// Scripts may modify the event handler function. 
        /// </summary>
        public MouseUpEvent OnMouseUp { get; set; }

        /// <summary>
        /// Engine shutdown event handler.
        /// 
        /// Called on engine shutdown.
        /// </summary>
        public ShutdownEvent OnShutdown { get; set; }

        /// <summary>
        /// Collision start event handler.
        /// 
        /// Called on collision beginning.
        /// </summary>
        public CollisionStartEvent OnCollisionStart { get; set; }

        /// <summary>
        /// Collision end event handler.
        /// 
        /// Called on collision ending.
        /// </summary>
        public CollisionEndEvent OnCollisionEnd { get; set; }


        public AABB AABB
        {
            get
            {
                if (Position == null
                || Size == null)
                {
                    return null;
                }
                else
                {
                    return new AABB(Position, Size);
                }

            }
        }


        /// <summary>
        /// Determines if this object is anchored - if so, gravity is not active on it. (<see cref="PhysicsEnabled"/> must be set to true.)
        /// </summary>
        public bool Anchored { get; set; }

        /// <summary>
        /// Backing field for <see cref="_mass"/>
        /// </summary>
        private double _mass { get; set; }

        /// <summary>
        /// The mass of this object. (kg)
        /// </summary>
        public double Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                if (value == 0)
                {
                    InverseMass = 1; // prevents objects spazzing off to infinity...
                }
                else 
                {
                    InverseMass = 1 / value;
                }

                _mass = value;
            }

        }

        /// <summary>
        /// Inverse mass of this object.
        /// </summary>
        internal double InverseMass { get; private set; }

        /// <summary>
        /// The speed of this object. (m/s)
        /// </summary>
        internal Vector2 Velocity { get; set; }

        /// <summary>
        /// The elasticity of this object.
        /// </summary>
        public double Elasticity { get; set; }

        /// <summary>
        /// The maximum force that this object can tolerate before breaking. A random modifier will be applied to simulate real world effects.
        /// </summary>
        public double MaximumForce { get; set; }

        /// <summary>
        /// Determines if physics is enabled.
        /// </summary>
        public bool PhysicsEnabled { get; set; }

        /// <summary>
        /// The PhysicsController of this PhysicsObject - see <see cref="PhysicsController"/>.
        /// </summary>
        public PhysicsController PhysicsController { get; set; }

        /// <summary>
        /// Determines if this object is colliding with any objects at all..
        /// </summary>
        public bool IsColliding { get; set; }

        /// <summary>
        /// Defines the display viewport of this PhysicalInstance. BRUSHES ONLY
        /// </summary>
        public Vector2 DisplayViewport { get; set; }

        /// <summary>
        /// Determines if this PhysicalInstance is invisible.
        /// </summary>
        public bool Invisible { get; set; }

        /// <summary>
        /// The physics solidity of this object.
        /// </summary>
        public Solidity Solidity { get; set; }

        private bool PhysicalInstance_INITIALISED { get; set; }
        public override void OnCreate()
        {
            PhysicsController = new DefaultPhysicsController(); 
            if (Velocity == null) Velocity = new Vector2(0, 0);
            if (InverseMass == 0) InverseMass = 1; // hacky code test temp
        }

        /// <summary>
        /// This is called on each frame by the RenderService to tell this object to 
        /// render itself.
        /// 
        /// It has already been loaded, so the object is not required to load textures or anything similar.
        /// </summary>
        public virtual void Render(Scene SDL_Renderer, ImageBrush Tx, IntPtr RenderTarget)
        {
            if (!PhysicalInstance_INITIALISED)
            {
                PO_Init();
            }
            else 
            {
                Brush CBrush = GetBrush();

                if (CBrush == null)
                {
                    return; 
                }
                else
                {
                    CBrush.Render(SDL_Renderer, Tx, IntPtr.Zero); 
                }
            }

        }

        public virtual void OnClick(object Sender, MouseEventArgs EventArgs)
        {
            return; 
        }

        /// <summary>
        /// Applies an instantenous impulse force to this object if physics is enabled.
        /// </summary>
        /// <param name="Impulse">A <see cref="Vector2"/> containing the impulse force to apply to this PhysicalInstance.</param>
        public void ApplyImpulse(Vector2 Impulse)
        {
            if (Impulse == null)
            {
                ErrorManager.ThrowError(ClassName, "AttemptedToApplyInvalidImpulseException");
            }
            else
            {
                Velocity += Impulse; 
            }
        }

        internal void PO_Init()
        {
            Brush CBrush = GetBrush();

            PhysicalInstance_INITIALISED = true; 
        }

        /// <summary>
        /// INTERNAL: Gets the current brush.
        /// </summary>
        /// <returns></returns>
        internal Brush GetBrush()
        {
            GetInstanceResult GIR = GetFirstChildOfTypeT(typeof(Brush));

            if (GIR.Instance == null
            || !GIR.Successful)
            {
                return null; 
            }
            else
            {
                // Todo: not get stuff every frame :D
                Brush Brush = (Brush)GIR.Instance;
                
                return Brush; 
            }
            
        }

        /// <summary>
        /// Acquires the position of this PhysicalInstance relative to the <see cref="Camera"/> <paramref name="CCamera"/>
        /// </summary>
        /// <param name="CCamera">The camera you wish to obtain the relative position of this object to.</param>
        /// <returns>The position of this PhysicalInstance relative to the position of <paramref name="CCamera"/>.</returns>
        public Vector2 GetCameraPosition(Camera CCamera)
        {
            return new Vector2(Position.X - CCamera.Position.X, Position.Y - CCamera.Position.Y);
        }

    }
}

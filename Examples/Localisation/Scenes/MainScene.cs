using static LightningBase.SDL; // not required for project template
using LightningGL;
using static LightningGL.Lightning; // not required for project template
using System.Drawing;
using System.Numerics;

namespace BasicScene
{
    public class MainScene : Scene
    {
        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            // #[name] is used for localisation strings
            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Localised strings: (modify the CurrentLanguage setting in Engine.ini to change them!)"
                , "DebugFont", new(100, 50), Color.White)); // no fonts loaded so we use the debug font
            Lightning.Renderer.AddRenderable(new TextBlock("Text2", "Localised string 1: #[LOC_STRING_01]", "DebugFont", new(100, 100), Color.Yellow)); // no fonts loaded so we use the debug font
            Lightning.Renderer.AddRenderable(new TextBlock("Text3", "Localised string 2: #[LOC_STRING_02]", "DebugFont", new(100, 150), Color.Red)); // no fonts loaded so we use the debug font
            Lightning.Renderer.AddRenderable(new TextBlock("Text4", "Localised string 3: #[LOC_STRING_03]", "DebugFont", new(100, 200), Color.RebeccaPurple)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {

        }
    }
}

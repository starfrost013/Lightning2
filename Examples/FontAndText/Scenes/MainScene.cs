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

            // if you do not provide a path it will load from the system font directory
            // TrueType fonts only, you don't need to load bold/italic/etc fonts - drawtext has parameters for this
            Lightning.Tree.AddRenderable(new Font("Arial.ttf", 24, "Arial.24pt"));
            Lightning.Tree.AddRenderable(new Font("comic.ttf", 24, "ComicSans.24pt")); // comic.ttf is the file name for comic sans ms in Renderers
            Lightning.Tree.AddRenderable(new Font("segoeui.ttf", 24, "SegoeUI.24pt")); //v segoeui.ttf is the filename for segoe ui
            Lightning.Tree.AddRenderable(new Font("consola.ttf", 18, "Consolas.18pt")); // same for consolas...
            Lightning.Tree.AddRenderable(new Font("consola.ttf", 36, "Consolas.36pt"));

            // backgrounds etc supported
            Lightning.Tree.AddRenderable(new TextBlock("Text1", "Arial 24pt", "Arial.24pt", new Vector2(100, 150), Color.White, Color.Red)); // Arial.24pt, italic
            Lightning.Tree.AddRenderable(new TextBlock("Text2", "Comic Sans 24pt", "ComicSans.24pt", new Vector2(100, 200), Color.Blue));
            Lightning.Tree.AddRenderable(new TextBlock("Text3", "Segoe UI 24pt", "SegoeUI.24pt", new Vector2(100, 250), Color.White, Color.Yellow));
            Lightning.Tree.AddRenderable(new TextBlock("Text4", "Consolas 18pt", "Consolas.18pt", new Vector2(100, 300), Color.White, Color.Red));
            Lightning.Tree.AddRenderable(new TextBlock("Text5", "Consolas 36pt\nMultiline text!", "Consolas.36pt", new Vector2(100, 350), Color.White, Color.Red));
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {

        }
    }
}

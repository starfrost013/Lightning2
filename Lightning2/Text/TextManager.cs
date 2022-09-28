using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightningGL
{
    public class TextAssetManager : AssetManager<TextCacheEntry>
    {
        public override TextCacheEntry AddAsset(Renderer cRenderer, TextCacheEntry asset)
        {
            // temp code
            DrawText(cRenderer, asset.Text, asset.Font, asset.Position, Color.FromArgb(asset.Color.a, asset.Color.r, asset.Color.g, asset.Color.b),
                Color.FromArgb(asset.BackgroundColor.a, asset.BackgroundColor.r, asset.BackgroundColor.g, asset.BackgroundColor.b), asset.Style, asset.OutlineSize, -1,
                asset.SmoothingType, false);
            return asset;
        }


        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="cRenderer">The Window to draw this text to.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font.FriendlyName"/> of the <see cref="Font"/> to draw this text in.</param>
        /// <param name="position">The position to draw the text.</param>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">Optional: The background color of the text.</param>
        /// <param name="style">Optional: The <see cref="TTF_FontStyle"/> of the text.</param>
        /// <param name="outlineSize">Optional: Size of the font outline . Range is 1 to the font size.</param>
        /// <param name="lineLength">Optional: Maximum line length in pixels. Ignores newlines.</param>
        /// <param name="smoothingType">Optional: The <see cref="FontSmoothingType"/> of the text.</param>
        /// <param name="snapToScreen">Determines if the pixel will be drawn in world-relative space or camera-relative space.</param>
        public void DrawText(Renderer cRenderer, string text, string font, Vector2 position, Color foreground, Color background = default(Color),
            TTF_FontStyle style = TTF_FontStyle.Normal, int outlineSize = -1, int lineLength = -1, FontSmoothingType smoothingType = FontSmoothingType.Default,
            bool snapToScreen = false)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !snapToScreen)
            {
                position.X -= currentCamera.Position.X;
                position.Y -= currentCamera.Position.Y;
            }

            // Localise the string using Localisation Manager.
            text = LocalisationManager.ProcessString(text);

            // Get the font and throw an error if it's invalid
            Font curFont = FontManager.GetFont(font);

            if (curFont == null) _ = new NCException($"Attempted to acquire invalid font with name {font}", 39, "TextManager::DrawText font parameter is not a loaded font!", NCExceptionSeverity.FatalError);

            // Set the foreground color
            SDL_Color fgColor = new(foreground.R, foreground.G, foreground.B, foreground.A);

            // split the text into lines
            // add the length of each line to the text length
            string[] textLines = text.Split("\n");

            // default to entirely transparent background (if the user has specified shasded for some reason, we still need a BG color...)
            SDL_Color bgColor = default;

            int fontSizeX = -1;
            int fontSizeY = -1;

            // Draw the background
            if (background != default(Color)
                && smoothingType != FontSmoothingType.Shaded) // draw the background (which requires sizing the entire text)
            {
                // Set the background color
                bgColor = new SDL_Color(background.R, background.G, background.B, background.A);

                Vector2 fontSize = FontManager.GetTextSize(curFont, text);

                // get the number of lines
                int numberOfLines = textLines.Length;

                fontSizeX = (int)fontSize.X;
                fontSizeY = (int)fontSize.Y;

                int totalSizeX = fontSizeX;
                int totalSizeY = fontSizeY;

                for (int lineId = 0; lineId < numberOfLines - 1; lineId++) // -1 to prevent double-counting the first line
                {
                    if (lineLength < 0)
                    {
                        totalSizeY += fontSizeY;
                    }
                    else
                    {
                        totalSizeY += lineLength;
                    }
                }

                // get the size of the largest line
                if (numberOfLines > 1) totalSizeX = (int)FontManager.GetLargestTextSize(curFont, text).X;

                // camera-aware is false for this as we have already "pushed" the position, so we don't need to do it again.
                PrimitiveRenderer.DrawRectangle(cRenderer, position, new(totalSizeX, totalSizeY), Color.FromArgb(bgColor.a, bgColor.r, bgColor.g, bgColor.b), true, default, default, true);
            }

            // use the cached entry if it exists
            TextCacheEntry cacheEntry = GetEntry(font, text, fgColor, style, smoothingType, outlineSize, bgColor);

            // if it doesn't exist add it
            if (cacheEntry == null) cacheEntry = AddEntry(cRenderer, font, text, fgColor, style, smoothingType, outlineSize, bgColor);

            cacheEntry.UsedThisFrame = true;

            SDL_Rect fontSrcRect = new SDL_Rect(0, 0, fontSizeX, fontSizeY);
            SDL_FRect fontDstRect = new SDL_FRect(position.X, position.Y, fontSizeX, fontSizeY);

            foreach (TextCacheEntryLine line in cacheEntry.Lines)
            {
                fontSrcRect.w = (int)line.Size.X;
                fontSrcRect.h = (int)line.Size.Y;

                fontDstRect.w = fontSrcRect.w;
                fontDstRect.h = fontSrcRect.h;

                SDL_RenderCopyF(cRenderer.Settings.RendererHandle, line.Handle, ref fontSrcRect, ref fontDstRect);

                // increment by the line length
                if (lineLength < 0)
                {
                    fontDstRect.y += fontSizeY;
                }
                else
                {
                    fontDstRect.y += lineLength;
                }
            }

            // weird hack. if we don't do this weird stuff happens
            if (outlineSize > -1) TTF_SetFontOutline(curFont.Handle, -1);
        }


        internal TextCacheEntry AddEntry(Renderer cRenderer, string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            TextCacheEntry entry = TextCacheEntry.Render(cRenderer, font, text, color, style, type, outlineSize, bgColor);
            Assets.Add(entry);
            return entry;
        }

        internal void PurgeUnusedEntries()
        {
            // memory leaks are bad
            for (int entryId = 0; entryId < Assets.Count; entryId++)
            {
                TextCacheEntry entry = Assets[entryId];
                if (!entry.UsedThisFrame)
                {
                    NCLogging.Log($"Removing unused cached text (font={entry.Font}, text={entry.Text}, style={entry.Style}, " +
                        $"smoothing type={entry.SmoothingType}, bgcolor={entry.BackgroundColor}, outline size={entry.OutlineSize})");
                    DeleteEntry(entry);
                }
            }
        }

        #region Cache code

        internal TextCacheEntry GetEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            foreach (TextCacheEntry entry in Assets)
            {
                if (entry.Font == font
                    && entry.Text == text
                    && entry.Color.Equals(color)
                    && entry.Style == style
                    && entry.SmoothingType == type
                    && entry.OutlineSize == outlineSize
                    && entry.BackgroundColor.Equals(bgColor))
                {
                    return entry;
                }
            }

            return null;
        }

        internal void DeleteEntry(string font, string text,
            SDL_Color color, TTF_FontStyle style, FontSmoothingType type = FontSmoothingType.Default, int outlineSize = -1, SDL_Color bgColor = default)
        {
            TextCacheEntry fontEntry = GetEntry(font, text, color, style, type, outlineSize, bgColor);

            DeleteEntry(fontEntry);
        }

        internal void DeleteEntry(TextCacheEntry fontEntry)
        {
            if (fontEntry != null)
            {
                fontEntry.Unload();
                Assets.Remove(fontEntry);
            }
        }

        internal void UnloadAll()
        {
            foreach (TextCacheEntry entry in Assets)
            {
                entry.Unload();
            }
        }

        internal override void Update(Renderer cRenderer)
        {
            PurgeUnusedEntries();
            foreach (TextCacheEntry entry in Assets) entry.UsedThisFrame = false;
        }

        /// <summary>
        /// Internal: Shuts down the Font Manager.
        /// </summary>
        internal void Shutdown()
        {
            NCLogging.Log("Uncaching all cached text - shutdown requested");
            UnloadAll();
        }
        #endregion
    }
}

﻿using static NuCore.SDL2.SDL;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// MouseButton
    /// 
    /// August 11, 2022
    /// 
    /// Defines a mouse button
    /// </summary>
    public class MouseButton
    {
        /// <summary>
        /// The mouse button that is being pressed - see <see cref="SDL_MouseButton"/>
        /// </summary>
        public SDL_MouseButton Button { get; set; }

        /// <summary>
        /// The position of the mouse click.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The velocity of the mouse movement relative to the last mouse movement.
        /// Only used for mouse movement events.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The number of clicks of the mouse
        /// </summary>
        public int ClickCount { get; set; }

        /// <summary>
        /// Converts a SDL mouse button event (see <see cref="SDL_MouseButtonEvent"/> to a <see cref="MouseButton"/>
        /// </summary>
        /// <param name="mouseButton">The <see cref="SDL_MouseButtonEvent"/> to convert.</param>
        public static explicit operator MouseButton(SDL_MouseButtonEvent mouseButton)
        {
            return new MouseButton
            {
                Button = mouseButton.button,
                Position = new Vector2(mouseButton.x, mouseButton.y),
                ClickCount = mouseButton.clicks,
            };
        }

        /// <summary>
        /// Converts a SDL mouse motion event (see <see cref="SDL_MouseMotionEvent"/> to a <see cref="MouseButton"/>
        /// </summary>
        /// <param name="mouseButton">The <see cref="SDL_MouseMotionEvent"/> to convert.</param>
        public static explicit operator MouseButton(SDL_MouseMotionEvent mouseButton)
        {
            return new MouseButton
            {
                Button = (SDL_MouseButton)mouseButton.state,
                Position = new Vector2(mouseButton.x, mouseButton.y),
                Velocity = new Vector2(mouseButton.xrel, mouseButton.yrel),
            };
        }
    }
}
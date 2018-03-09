using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameRPG
{
    public class Button
    {
        public Texture2D TextureDefault;
        public Texture2D TexturePressed;
        public Rectangle Rectangle;
        public bool Pressed;
        public bool ButtonUp;
        public bool Visible;

        public Button (Rectangle nRectangle, Texture2D NewTextureDefault, Texture2D NewTexturePressed, bool NewVisible)
        {
            Rectangle = nRectangle;
            TextureDefault = NewTextureDefault;
            TexturePressed = NewTexturePressed;
            ButtonUp = Pressed = false;
            Visible = NewVisible;
        }

        public Button(Rectangle nRectangle, Texture2D NewTexture, bool NewVisible)
        {
            Rectangle = nRectangle;
            TextureDefault = NewTexture;
            TexturePressed = NewTexture;
            ButtonUp = Pressed = false;
            Visible = NewVisible;
        }

        public void Update(GameTime gameTime)
        {
            if (Visible)
            {
                ButtonUp = false;
                MouseState MouseState = Mouse.GetState();
                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (MouseState.X >= Rectangle.X && MouseState.Y >= Rectangle.Y &&
                        MouseState.X <= Rectangle.Width + Rectangle.X && MouseState.Y <= Rectangle.Height + Rectangle.Y)
                    {
                        Pressed = true;
                    }
                }
                if (MouseState.LeftButton != ButtonState.Pressed)
                {
                    if (Pressed)
                    {
                        Pressed = false;
                        ButtonUp = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            if (Visible)
            {
                if (!Pressed)
                    spriteBatch.Draw(TextureDefault, Rectangle, Color.White);
                else
                    spriteBatch.Draw(TexturePressed, Rectangle, Color.White);
            }
        }
    }
}

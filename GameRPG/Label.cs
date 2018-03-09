using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameRPG
{
    class Label
    {
        public SpriteFont Font { get; set; }
        public string Text;
        public Vector2 Position { get; set; }

    public Label(SpriteFont NewFont,Vector2 NewPosition,string NewText)
        {
            Font = NewFont;
            Text = NewText;
            Position = NewPosition;
        }

    public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color.Black);
        }

    public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.DrawString(Font, Text, Position, color);
        }
    }
}

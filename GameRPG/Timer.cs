using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameRPG
{
    class Timer : Label
    {
        public int MM { get; private set; }
        public int SS { get; private set; }
        public int MS { get; private set; }

        public Timer(SpriteFont NewFont, Vector2 NewPosition, string NewText) : base(NewFont,NewPosition,NewText)
        {
            Font = NewFont;
            Position = NewPosition;
            Text = NewText;
            MM = SS = MS = 0;
        }

        public void Update(GameTime gameTime)
        {
            MS = MS + (int)gameTime.ElapsedGameTime.Milliseconds;
            if (MS >= 1000)
            {
                MS = MS - 1000;
                SS++;
            }
            if (SS >= 60)
            {
                SS = SS - 60;
                MM++;
            }

            if (SS >= 10)
                Text = MM + ":" + SS;
            else
                Text = MM + ":0" + SS;

        }

    }
}

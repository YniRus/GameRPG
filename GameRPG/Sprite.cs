using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameRPG
{
    class Sprite
    {
        public Texture2D Texture;
        public Rectangle Rectangle;

        public Sprite(Texture2D NewTexture)
        {
            Texture = NewTexture;
        }

        public Sprite(Texture2D NewTexture, Rectangle NewRecrangle)
        {
            Texture = NewTexture;
            Rectangle = NewRecrangle;
        }
    }
}

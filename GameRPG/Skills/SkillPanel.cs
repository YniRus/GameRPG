using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameRPG
{
    class SkillPanel
    {
        public Rectangle Rectangle;

        public int SkillSize;

        public int MarginX;
        public int MarginY;

        public int SkillCount;

        public int MaxSkillCount;

        public SkillPanel (Rectangle nRectangle, int nMarginX, int nMarginY, int nSkillSize, int nMaxSkillCount)
        {
            Rectangle = nRectangle;
            MarginX = nMarginX;
            MarginY = nMarginY;
            SkillSize = nSkillSize;

            MaxSkillCount = nMaxSkillCount;
            SkillCount = 0;
        }

        public Rectangle GetRect()
        {
            return new Rectangle(
                Rectangle.X + (MarginX + SkillSize) * (SkillCount) + MarginX,
                Rectangle.Y + MarginY,
                SkillSize,
                SkillSize
                );
        }
    }
}

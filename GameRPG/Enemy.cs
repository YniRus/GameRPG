using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameRPG
{
    class Enemy : Person
    {
        public Enemy(int nHP, int nMP, int nMaxHP, int nMaxMP, int nAttack, int nDef, int nSpeed, int nAttackSpeed, Point nPosition, int nFrameWidth, int nFrameHeight) : base(nHP, nMP, nMaxHP, nMaxMP, nAttack, nDef, nSpeed, nAttackSpeed, nPosition, nFrameWidth, nFrameHeight)
        {
        }

        public void DoMove(GameWindow Window, GameTime gameTime)
        {
            if (State != 1)
            {
                State = 1;
                CurrentFrame = 0;
            }
            if (Rotate) Position.X -= Speed;
            else Position.X += Speed;
            Animate(gameTime);
        }
    }
}

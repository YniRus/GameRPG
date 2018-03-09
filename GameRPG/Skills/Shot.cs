using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GameRPG
{
    class Shot
    {
        public Effect[] Effect;

        public Texture2D Texture;
        public Rectangle Rectangle;

        public int Speed; //Скорость полета снаряда

        public int Distance; //Расстояние полета
        public int DistanseElapsed; //Пройденное расстояние
        public int StartPos;

        public bool Start; //Запущен ли снаряд
        public bool Active; //Летит
        public bool Success; //Попал или нет

        public bool Damaged;

        public bool Rotate;

        public SoundEffect Sound; //Звук попадания

        public Shot(Texture2D nTexture, Rectangle nRectangle, Effect[] nEffect, int nSpeed, int nDistance)
        {
            Texture = nTexture;
            Rectangle = nRectangle;
            Speed = nSpeed;
            Distance = nDistance;

            DistanseElapsed = 0;
            StartPos = 0;

            Effect = nEffect;
            Start = false;
            Success = false;
            Active = false;
            Damaged = false;
        }

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < Effect.Length; i++)
            {
                Effect[i].Update(gameTime);
            }

            if (Success)
            {
                Success = false;
                Active = false;
                for(int i =0; i < Effect.Length; i++)
                {
                    Effect[i].isStart = true;
                    Effect[i].Update(gameTime);
                }
            }

            if (Start)
            {
                StartPos = Rectangle.X;
                Start = false;
                Active = true;
            }
            else if (Active)
            {
                if(!Rotate) Rectangle.X += Speed;
                else Rectangle.X -= Speed;
                DistanseElapsed += Speed;
                if (DistanseElapsed > Distance)
                {
                    Active = false;
                    DistanseElapsed = 0;
                }
            }

            if(Damaged)
            {
                Start = false;
                Success = false;
                Active = false;
                Damaged = false;
                DistanseElapsed = 0;
                StartPos = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(Texture, Rectangle, Color.White);
            }
        }
    }
}

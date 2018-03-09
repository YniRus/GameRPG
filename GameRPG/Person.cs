using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GameRPG
{
    class Person
    {
        public int HP, MP; //Запас здоровья и маны
        public int MaxHP, MaxMP; //Запас здоровья и маны
        public int Attack, Def; //Показатели Атаки и Защиты
        public int Speed,AttackSpeed; //Скорость перемещения и скорость атаки

        public int FrameWidth; //Ширина одного кадра
        public int FrameHeight; // Высота одного кадра
        public int CurrentFrame; //Текущий показываемый кадр
        public int FrameCount; //Общее число кадров в спрайте
        public int CurrentTime; //Время с последней смены кадра
        public int Period; //Период смены кадров
        public int State; // текущее состояние объекта (0 - стоит, 1 - бежит)
        public Texture2D TextureStay; //Спрайт стояния
        public Texture2D TextureRun; // Спрайт бега
        public Texture2D TextureJump; // Спрайт прыжка
        public Texture2D TextureAttack; // Спрайт прыжка
        public Texture2D TextureDeath; // Спрайт прыжка
        public Rectangle Rectangle; // Прямоукольеник отрисовки
        public Point Position;// Верхняя левая точка прямоугольника отрисовки
        public bool Rotate;// Повернкть ли спрайт на право ( изначально смотрит налево)

        public bool onJump;// Находится ли персонаж в прыжке
        public bool isDeath;// Погиб ли персонаж
        public float JumpHeight; 
        public int JumpCountFrame;
        public int JumpFrame;

        public Person(int nHP,int nMP, int nMaxHP, int nMaxMP, int nAttack, int nDef, int nSpeed, int nAttackSpeed, Point nPosition, int nFrameWidth, int nFrameHeight)
        {
            HP = nHP;
            MP = nMP;
            MaxHP = nMaxHP;
            MaxMP = nMaxMP;
            Attack = nAttack;
            Def = nDef;
            Speed = nSpeed;
            AttackSpeed = nAttackSpeed;
            State = 0;
            CurrentFrame = 0;
            CurrentTime = 0;
            Period = 50;
            Position = nPosition;
            FrameWidth = nFrameWidth;
            FrameHeight = nFrameHeight;
            JumpCountFrame = 50;
            JumpFrame = 0;
            JumpHeight = FrameHeight*2 + 50;
        }

        public void Jump()
        {
            JumpFrame++;
            if (JumpFrame <= JumpCountFrame / 2)
            {
                Position.Y -= (int)(JumpHeight / JumpCountFrame);
            } else
            {
                Position.Y += (int)(JumpHeight / JumpCountFrame);
            }
            if (JumpFrame == JumpCountFrame)
            {
                onJump = false;
                State = 0;
                JumpFrame = 0;
            }   
        }

        public void Animate(GameTime gameTime)
        {
            int LocalPeriod = Period;
            if (State == 2) // Если персонаж атакует применяем скорость атаки
            {
                LocalPeriod = 1000 / AttackSpeed;
            }
            CurrentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (CurrentTime > LocalPeriod)
            {
                CurrentTime -= LocalPeriod;
                CurrentFrame++;
                if (CurrentFrame >= FrameCount && State != 3) CurrentFrame = 0;
                if (CurrentFrame >= FrameCount && State == 3) isDeath = true;
            }
        }

        public void DoAttack(GameWindow Window, GameTime gameTime)
        {
            if (State != 2)
            {
                State = 2;
                CurrentFrame = 0;
            }
            Animate(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == 0) //Стоит
            {
                FrameCount = TextureStay.Width / FrameWidth;
                Rectangle = new Rectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
                if (!Rotate)
                {
                    spriteBatch.Draw(TextureStay, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White);
                }
                else
                {
                    SpriteEffects effect = new SpriteEffects();
                    effect = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(TextureStay, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White, 0, Vector2.Zero, effect, 0);
                }
            }
            if (State == 1) //Бежит
            {
                FrameCount = TextureRun.Width / FrameWidth;
                Rectangle = new Rectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
                if (!Rotate)
                {
                    spriteBatch.Draw(TextureRun, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White);
                }
                else
                {
                    SpriteEffects effect = new SpriteEffects();
                    effect = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(TextureRun, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White, 0, Vector2.Zero, effect, 0);
                }
            }

            if (State == 2) // Атакует
            {
                FrameCount = TextureAttack.Width / FrameWidth;
                Rectangle = new Rectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
                if (!Rotate)
                {
                    spriteBatch.Draw(TextureAttack, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White);
                }
                else
                {
                    SpriteEffects effect = new SpriteEffects();
                    effect = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(TextureAttack, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White, 0, Vector2.Zero, effect, 0);
                }
            }

            if (State == 3) // Умирает
            {
                FrameCount = TextureDeath.Width / FrameWidth;
                Rectangle = new Rectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
                if (!Rotate)
                {
                    spriteBatch.Draw(TextureDeath, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White);
                }
                else
                {
                    SpriteEffects effect = new SpriteEffects();
                    effect = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(TextureDeath, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White, 0, Vector2.Zero, effect, 0);
                }
            }
            if (State == 4) //В прыжке
            {
                FrameCount = TextureJump.Width / FrameWidth;
                Rectangle = new Rectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
                if (!Rotate)
                {
                    spriteBatch.Draw(TextureJump, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White);
                }
                else
                {
                    SpriteEffects effect = new SpriteEffects();
                    effect = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(TextureJump, Rectangle,
                         new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight),
                         Color.White, 0, Vector2.Zero, effect, 0);
                }
            }
        }
    }
}

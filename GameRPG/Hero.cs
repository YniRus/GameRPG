using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameRPG
{
    class Hero : Person
    {
        public Hero(int nHP, int nMP, int nMaxHP, int nMaxMP, int nAttack, int nDef, int nSpeed, int nAttackSpeed, Point nPosition, int nFrameWidth, int nFrameHeight) : base(nHP, nMP, nMaxHP, nMaxMP, nAttack, nDef, nSpeed, nAttackSpeed, nPosition, nFrameWidth, nFrameHeight)
        {

        }
        public void Update(GameWindow Window, GameTime gameTime)
        {
            if (HP <= 0)
            {
                State = 3;
                Animate(gameTime);
            }
            if (State != 3)
            {
                KeyboardState kbState = Keyboard.GetState();
                //Нажата кнопка ВЛЕВО
                if (kbState.IsKeyDown(Keys.A))
                {
                    if (Rectangle.X > 0 && Main.ThisGame.CanRunLeft)
                    {
                        if(!onJump) State = 1;
                        Rotate = true;
                        Position.X -= Speed;
                        Animate(gameTime);
                    }
                    else if (State == 1)
                    {
                        State = 0;
                        CurrentTime = 0;
                        CurrentFrame = 0;
                    }
                }
                //Нажата кнопка ВПРАВО
                else if (kbState.IsKeyDown(Keys.D))
                {
                    if (Rectangle.X < Window.ClientBounds.Width - FrameWidth && Main.ThisGame.CanRunRight)
                    {
                        if (!onJump) State = 1;
                        Rotate = false;
                        Position.X += Speed;
                        Animate(gameTime);
                    }
                    else if(State == 1) { 
                        State = 0;
                        CurrentTime = 0;
                        CurrentFrame = 0;
                    }
                }                
                //Ничего не нажато
                else
                {
                    if (State != 0 && State != 3 && State != 2)
                    {
                        State = 0;
                        CurrentTime = 0;
                        CurrentFrame = 0;
                    }
                }
                //Нажат ПРОБЕЛ
                if (kbState.IsKeyDown(Keys.Space))
                {
                    if (Rectangle.X < Window.ClientBounds.Width - FrameWidth)
                    {
                        State = 4;
                        onJump = true;
                    }
                }

            }
        }
    }
}

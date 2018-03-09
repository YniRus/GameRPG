using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameRPG
{
    class ProgramTimer
    {
        private int SS;
        private int MS;

        private int Time;

        public bool inWork; //Работает
        public bool isEnd; //Законсил

        public bool Closed; //Закончил и больше не нужен

        public ProgramTimer(int nTime)
        {
            inWork = false;
            isEnd = false;
            Closed = false;

            Time = nTime;
            SS = MS = 0;
        }

        public int GetSS()
        {
            return SS;
        }

        public void Update(GameTime gameTime)
        {
            if (SS < Time)
            {
                inWork = true;
                isEnd = false;
            }
            else
            {
                isEnd = true;
                inWork = false;
            }

            if (inWork)
            {
                MS = MS + (int)gameTime.ElapsedGameTime.Milliseconds;
                if (MS >= 1000)
                {
                    MS = MS - 1000;
                    SS++;
                }
            }
        }

    }
}

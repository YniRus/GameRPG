using Microsoft.Xna.Framework;

namespace GameRPG
{
    class Effect
    {
        public string Param;
        public int Value;
        public string Target; //На кого применить [Hero] или [Enemy]

        public bool Deactive; //Нужно ли дективировать

        public bool isStart; //Запустить действие эффекта 
        public bool isEnd; //Запустить действие эффекта 

        public int Time; //Вермя дейстия
        private ProgramTimer TimeRemaind; //Остаток времени действия

        public Effect(string nParam, int nValue, string nTarget, int nTime, bool nDeactive)
        {
            Param = nParam;
            Value = nValue;
            Deactive = nDeactive;
            Target = nTarget;
            Time = nTime;

            isStart = false;
            isEnd = false;
        }

        public Effect(string nParam, int nValue,string nTarget)
        {
            Param = nParam;
            Value = nValue;
            Deactive = false;
            Target = nTarget;
            Time = 0;

            isStart = false;
            isEnd = false;
        }

        public void Update(GameTime gameTime)
        {
            if (isStart)
            {
                TimeRemaind = new ProgramTimer(Time);
            }

            if (TimeRemaind != null)
            {
                if(!TimeRemaind.Closed)TimeRemaind.Update(gameTime);
                if (TimeRemaind.isEnd && !isEnd)
                {
                    isEnd = true;
                    TimeRemaind.isEnd = false;
                    TimeRemaind.Closed = true;
                }

                if(isEnd)
                {
                    TimeRemaind.isEnd = false;
                    TimeRemaind.Closed = true;
                }
            }
        }

    }
}

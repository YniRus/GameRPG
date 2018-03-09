using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GameRPG
{
    class Skill
    {

        public Button Button; //Миниатюра скила

        public bool Active; //Активен ли(Находится не в КД и хватает маны)
        public bool inPanel; //Находится ли он на панели

        public Effect[] Effect;

        public string Description;

        public int KD; //КД
        public int TimeActive; //Длительность эффекта

        public ProgramTimer KDRemaind; //Остаток КД
        public ProgramTimer TimeActiveRemaind; //Остаток эффекта

        public Label LabelKD;
        public Label LabelTimeActive;

        public SoundEffect Sound;

        public Shot Shot;

        public Skill(string nDescription, Button nBtn, Effect[] nEffect, int nKD, int nTimeActive, SoundEffect nSound)
        {
            Button = nBtn;
            Effect = nEffect;
            Sound = nSound;

            Active = true;
            inPanel = false;

            Description = nDescription;

            KD = nKD;
            TimeActive = nTimeActive;

            LabelTimeActive = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(Button.Rectangle.X + Button.Rectangle.Width / 2 - 7, Button.Rectangle.Y + Button.Rectangle.Height / 2 - 15),
                "");

            LabelKD = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(Button.Rectangle.X + Button.Rectangle.Width / 2 - 7, Button.Rectangle.Y + Button.Rectangle.Height),
                "");

            Shot = null;
        }

        public Skill(string nDescription, Button nBtn, Shot nShot, Effect[] nEffect, int nKD, int nTimeActive, SoundEffect nSound)
        {
            Button = nBtn;
            Effect = nEffect;
            Sound = nSound;

            Active = true;
            inPanel = false;

            Description = nDescription;

            KD = nKD;
            TimeActive = nTimeActive;

            LabelTimeActive = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(Button.Rectangle.X + Button.Rectangle.Width / 2 - 7, Button.Rectangle.Y + Button.Rectangle.Height / 2 - 15),
                "");

            LabelKD = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(Button.Rectangle.X + Button.Rectangle.Width / 2 - 7, Button.Rectangle.Y + Button.Rectangle.Height),
                "");

            Shot = nShot;
        }

        public void CheckActive(GameTime gameTime, int HeroMP)
        {
            if(KDRemaind.isEnd && !KDRemaind.Closed)
            {
                Active = true;

                //Обновляем состояния скилов по запасу маны
                for (int i = 0; i < Effect.Length; i++)
                {
                    if (Effect[i].Param == "MP")
                    {
                        if (Effect[i].Value * (-1) > HeroMP) Active = false;
                        else Active = true;
                    }
                }

                KDRemaind.Closed = true;
            }

            if(TimeActiveRemaind.isEnd && !TimeActiveRemaind.Closed)
            {
                TimeActiveRemaind.Closed = true;
            }
        }

        public void Update(GameTime gameTime,int HeroMP)
        {
            if (inPanel) Button.Update(gameTime);

            for (int i = 0; i < Effect.Length; i++)
            {
                Effect[i].Update(gameTime);
            }

            if (!Active)
            {

                KDRemaind.Update(gameTime);
                TimeActiveRemaind.Update(gameTime);
                CheckActive(gameTime, HeroMP);

                if (KD - KDRemaind.GetSS() != 0)
                    LabelKD.Text = Convert.ToString(KD - KDRemaind.GetSS());
                else
                    LabelKD.Text = "";

                if (TimeActive - TimeActiveRemaind.GetSS() != 0)
                    LabelTimeActive.Text = Convert.ToString(TimeActive - TimeActiveRemaind.GetSS());
                else
                    LabelTimeActive.Text = "";
            }

            if (Button.ButtonUp && Active)
            {
                Active = false;

                Sound.Play();

                for (int i = 0; i < Effect.Length; i++)
                {
                    Effect[i].isStart = true;
                    Effect[i].Update(gameTime);
                }

                KDRemaind = new ProgramTimer(KD);
                TimeActiveRemaind = new ProgramTimer(TimeActive);

                if (Shot != null) Shot.Start = true;

                Sound.Play();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            if(inPanel) Button.Draw(spriteBatch, Window);

            if (Shot != null) Shot.Draw(spriteBatch);

            LabelKD.Draw(spriteBatch);
            LabelTimeActive.Draw(spriteBatch);
        }

    }
}

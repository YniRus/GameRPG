using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameRPG
{
    class SkillWindow
    {
        public int SkillPoint;

        Label SkillPointLabel;

        Skill[] Skill;

        Rectangle[] SavedPosition;
        Rectangle[] LocalPosition;

        int ActiveSkill;

        Button ButtonStart,ButtonLearn, ButtonReturn;

        int MarginX,MarginY;

        Texture2D Bg, SkillPointPanel;

        Texture2D Select; //Рамка вокруг скила
        int SelectRadius;

        Label Description;

        SoundEffect BuySkill, NotBuySkill;

        public SkillWindow(Skill[] nSkill, GameWindow Window)
        {
            SkillPoint = 1;

            MarginX = 20;
            MarginY = 20;

            SelectRadius = 3;

            Skill = nSkill;

            SavedPosition = new Rectangle[Skill.Length];
            LocalPosition = new Rectangle[Skill.Length];

            for (int i = 0; i < Skill.Length; i++)
            {
                SavedPosition[i] = Skill[i].Button.Rectangle;

                Skill[i].Button.Rectangle = new Rectangle(
                    MarginX + (MarginX + Skill[i].Button.Rectangle.Width) *i,
                    MarginY,
                    Skill[i].Button.Rectangle.Width,
                    Skill[i].Button.Rectangle.Height
                    );

                LocalPosition[i] = Skill[i].Button.Rectangle;
            }

            Bg = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Bg");
            Select  = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Skill/Select");
            SkillPointPanel = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Skill/SkillPoint");

            ButtonLearn = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    280,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Skill/Learn"),
                true
                );

            ButtonStart = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    580,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Start"),
                true
                );

            ButtonReturn = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    650,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Skill/Back"),
                true
                );

            ActiveSkill = 0;

            Description = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(20, 150),
                "");

            SkillPointLabel = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(1200, 27),
                "");

            BuySkill = Main.ThisGame.Content.Load<SoundEffect>("sound/BuySkill");
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Skill.Length; i++)
            {
                Skill[i].Button.Rectangle = LocalPosition[i];
                Skill[i].Button.Update(gameTime);
                if(Skill[i].Button.ButtonUp)
                {
                    Main.ThisGame.ButtonClick.Play();
                    ActiveSkill = i;
                }
            }

            ButtonStart.Update(gameTime);
            ButtonReturn.Update(gameTime);
            ButtonLearn.Update(gameTime);

            if (ButtonReturn.ButtonUp)
            {
                Main.ThisGame.ButtonClick.Play();
                Main.ThisGame.WindowState = WindowState.Start;
            }

            if (ButtonStart.ButtonUp)
            {

                for (int i = 0; i < Skill.Length; i++)
                {
                    Skill[i].Button.Rectangle = SavedPosition[i];
                }
                Main.ThisGame.ButtonClick.Play();
                Main.ThisGame.WindowState = WindowState.Battle;
                MediaPlayer.Play(Main.ThisGame.BattleSong);
            }

            if (ButtonLearn.ButtonUp)
            {
                
                if (SkillPoint > 0)
                {
                    BuySkill.Play();
                    Skill[ActiveSkill].inPanel = true;
                    SkillPoint--;
                } else
                {
                    Main.ThisGame.ButtonClick.Play();
                }
            }

            SkillPointLabel.Text = SkillPoint.ToString();
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(Bg, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

            spriteBatch.Draw(SkillPointPanel, new Rectangle(1000, 20, SkillPointPanel.Width, SkillPointPanel.Height), Color.White);
            SkillPointLabel.Draw(spriteBatch, Color.White);

            for (int i = 0; i < Skill.Length; i++)
            {
                if (ActiveSkill == i)
                {
                    spriteBatch.Draw(Select, new Rectangle(Skill[i].Button.Rectangle.X - SelectRadius, Skill[i].Button.Rectangle.Y - SelectRadius, Skill[i].Button.Rectangle.Width + SelectRadius * 2, Skill[i].Button.Rectangle.Height + SelectRadius * 2), Color.White);
                    Description.Text = Skill[i].Description;
                    Description.Draw(spriteBatch);
                }
                Skill[i].Button.Draw(spriteBatch, Window);
            }

            if (!Skill[ActiveSkill].inPanel) ButtonLearn.Draw(spriteBatch, Window);

            ButtonStart.Draw(spriteBatch, Window);
            ButtonReturn.Draw(spriteBatch, Window);
        }
    }
}

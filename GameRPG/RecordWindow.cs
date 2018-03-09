using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GameRPG
{
    class RecordWindow
    {
        Button ButtonReturn;
        Button ButtonClean;

        Texture2D Bg;
        Texture2D RecordPanel;
        Label RecordHeader;

        public Label[,] Record;

        int CurrentLine = 0;
        int LineCount = 0;

        int Speed = 4;
        bool SpeedBlock = false;

        public RecordWindow(GameWindow Window)
        {
            Bg = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Bg");
            RecordPanel = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Record/RecordPanel");

            RecordHeader = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(340, 205),
                "Имя                                Время                             Очки");

            LineCount = File.ReadAllLines("Content/Record.txt").Length;

            Record = new Label[LineCount, 3];

            for (int i = 0; i < LineCount; i++)
            {
                Record[i,0] = new Label(
                    Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                    new Vector2(340, 235 + i * 30),
                    "");

                Record[i, 1] = new Label(
                    Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                    new Vector2(610, 235 + i * 30),
                    "");

                Record[i, 2] = new Label(
                    Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                    new Vector2(885, 235 + i * 30),
                    "");
            }

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

            ButtonClean = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    120,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Record/Clean"),
                true
                );

            GetRecord();
        }

        private void GetRecord()
        {

            using (StreamReader File = new StreamReader("Content/Record.txt", Encoding.Default))
            {
                string Line;
                while ((Line = File.ReadLine()) != null)
                {
                    string[] Data = Line.Split(new char[] { ' ' });

                    if (Data.Length == 3)
                        for (int i = 0; i < 3; i++)
                            Record[CurrentLine, i].Text = Data[i];

                    CurrentLine++;
                }
            }
        }

        public void Update(GameTime gameTime,GameWindow Window)
        {
            if(Record.Length != 0) KeyboardManager(Window);

            ButtonReturn.Update(gameTime);
            ButtonClean.Update(gameTime);

            if (ButtonReturn.ButtonUp)
            {
                Main.ThisGame.ButtonClick.Play();
                Main.ThisGame.WindowState = WindowState.Start;
            }

            if (ButtonClean.ButtonUp)
            {
                Main.ThisGame.ButtonClick.Play();
                for (int i = 0; i < LineCount; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Record[i, j].Text = "";
                    }
                }

                using (FileStream fstream = File.Open("Content/Record.txt", FileMode.Create)) { }
            }
        }

        public void KeyboardManager(GameWindow Window)
        {
            KeyboardState kbState = Keyboard.GetState();

            if (Record[0, 0].Position.Y <  230)
                if (kbState.IsKeyDown(Keys.Down))
                {
                    for (int i = 0; i < LineCount; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Record[i, j].Position = new Vector2(Record[i, j].Position.X, Record[i, j].Position.Y + Speed);
                        }
                    }
                }

            if (Record[LineCount - 1, 0].Position.Y + 30 > RecordPanel.Height + 200)
                if (kbState.IsKeyDown(Keys.Up))
                {
                    for (int i = 0; i < LineCount; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Record[i, j].Position = new Vector2(Record[i, j].Position.X, Record[i, j].Position.Y - Speed);
                        }
                    }
                }
        }

        public void Draw(SpriteBatch spriteBatch,GameWindow Window)
        {
            spriteBatch.Draw(Bg, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

            spriteBatch.Draw(RecordPanel, new Rectangle((Window.ClientBounds.Width - RecordPanel.Width) / 2, 200, RecordPanel.Width, RecordPanel.Height), Color.White);
            RecordHeader.Draw(spriteBatch, Color.White);

            for (int i = 0; i < LineCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(Record[i,j].Position.Y + 30 <= RecordPanel.Height + 200 && Record[i, j].Position.Y >= 230)
                        Record[i,j].Draw(spriteBatch, Color.White);
                }
            }

            ButtonClean.Draw(spriteBatch, Window);
            ButtonReturn.Draw(spriteBatch, Window);


        }
    }
}

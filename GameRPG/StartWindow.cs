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
    class StartWindow
    {
        public Label InputName;

        private Keys[] lastPressedKeys;

        private Texture2D Bg;
        private Texture2D Tilte;
        private Texture2D InputNameTexture;

        private Button ButtonStart;
        private Button ButtonRecord;
        private Button ButtonExit;

        public StartWindow(GameWindow Window)
        {

            ButtonStart = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    510,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Start"),
                true
                );

            ButtonRecord = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    580,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Record"),
                true
                );

            ButtonExit = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    650,
                    400,
                    50
                    ),
                Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Exit"),
                true
                );

            Bg = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Bg");
            Tilte = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/Title");
            InputNameTexture = Main.ThisGame.Content.Load<Texture2D>("img/Windows/Start/InputName");

            InputName = new Label(
                Main.ThisGame.Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2((Window.ClientBounds.Width - InputNameTexture.Width) / 2 + 20, 256),
                "");

            lastPressedKeys = new Keys[0];

            GetUserName();
        }

        private void GetUserName()
        {
            using (FileStream fstream = File.Open("Content/UserName.txt", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string Name = Encoding.Default.GetString(array);
                InputName.Text = Name;
            }
        }

        private void SaveUserName()
        {
            using (FileStream fstream = new FileStream("Content/UserName.txt", FileMode.Create))
            {
                // преобразуем строку в байты
                byte[] array = Encoding.Default.GetBytes(InputName.Text);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }

        public void Update(GameTime gameTime)
        {
            KbUpdate();

            ButtonStart.Update(gameTime);
            ButtonRecord.Update(gameTime);
            ButtonExit.Update(gameTime);

            if(ButtonStart.ButtonUp)
            {
                Main.ThisGame.ButtonClick.Play();
                Main.ThisGame.WindowState = WindowState.Skill;
                SaveUserName();
            }

            if (ButtonRecord.ButtonUp)
            {
                Main.ThisGame.ButtonClick.Play();
                Main.ThisGame.WindowState = WindowState.Record;
                SaveUserName();
            }

            if (ButtonExit.ButtonUp)
            {
                Main.ThisGame.ButtonClick.Play();
                Main.ThisGame.Exit();
                SaveUserName();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(Bg, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

            spriteBatch.Draw(Tilte, new Rectangle((Window.ClientBounds.Width - Tilte.Width) / 2, 50, Tilte.Width, Tilte.Height), Color.White);

            spriteBatch.Draw(InputNameTexture, new Rectangle((Window.ClientBounds.Width - InputNameTexture.Width) / 2, 200, InputNameTexture.Width, InputNameTexture.Height), Color.White);
            InputName.Draw(spriteBatch,Color.White);

            ButtonStart.Draw(spriteBatch, Window);
            ButtonRecord.Draw(spriteBatch, Window);
            ButtonExit.Draw(spriteBatch, Window);
        }

        private void KbUpdate()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        private void OnKeyDown(Keys key)
        {

        }

        private char ToRussian(char Char)
        {
            switch (Char)
            {
                case 'Q': return 'Й';
                case 'W': return 'Ц';
                case 'E': return 'У';
                case 'R': return 'К';
                case 'T': return 'Е';
                case 'Y': return 'Н';
                case 'U': return 'Г';
                case 'I': return 'Ш';
                case 'O': return 'Щ';
                case 'P': return 'З';
                case (char)219: return 'Х';
                case (char)221: return 'Ъ';
                case 'A': return 'Ф';
                case 'S': return 'Ы';
                case 'D': return 'В';
                case 'F': return 'А';
                case 'G': return 'П';
                case 'H': return 'Р';
                case 'J': return 'О';
                case 'K': return 'Л';
                case 'L': return 'Д';
                case (char)186: return 'Ж';
                case (char)222: return 'Э';
                case 'Z': return 'Я';
                case 'X': return 'Ч';
                case 'C': return 'С';
                case 'V': return 'М';
                case 'B': return 'И';
                case 'N': return 'Т';
                case 'M': return 'Ь';
                case (char)188: return 'Б';
                case (char)190: return 'Ю';
                default: return ' ';
            }
        }

        private void OnKeyUp(Keys key)
        {

            if (key == Keys.Back && InputName.Text.Length != 0)
            {
                if(InputName.Text.Length != 0)
                    InputName.Text = InputName.Text.Remove(InputName.Text.Length - 1, 1);
            }
            else
            {
                if (InputName.Text.Length <= 15)
                {
                    if (key >= Keys.D0 && key <= Keys.D9)
                        InputName.Text += key.ToString()[1];
                    char Char = Convert.ToChar(key);
                    if (ToRussian(Char) != ' ')
                        InputName.Text += ToRussian(Char);
                }
            }


        }
    }
}

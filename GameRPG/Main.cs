using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Text;

namespace GameRPG
{

    public enum WindowState
    {
        Start = 0,
        Skill,
        Battle,
        Record,
    }
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Main ThisGame;

        public WindowState WindowState;

        string fmt = "000.";

        public bool CanRunLeft, CanRunRight;

        Sprite Bg;
        Sprite GoldPanel,TimePanel;
        Sprite WinMessage, DeathMessage;

        StartWindow StartWindow;
        SkillWindow SkillWindow;
        RecordWindow RecordWindow;

        Timer GameTimer;
        Label GoldCount;

        Label EndMessage;

        Label Level;
        int[] LevelEnd = { 2, 5, 6 };
        int CurrentLevel = 0;
        bool LevelIsEnd = false;
        bool GameIsEnd = false;

        Button ButtonSaveAndExit,ButtonContinue;

        struct PersonStat
        {
            public Label Title;
            public Label Stats;
        }
        PersonStat StatHero,StatEnemy;

        Hero Hero;
        Enemy Enemy;

        Song MainTheme, SoundWin;
        public Song BattleSong;
        SoundEffect HeroAttack, HeroDeath, EnemyAttack;
        public SoundEffect ButtonClick;

        bool SoundOnes = true;

        SkillPanel SkillPanel;
        Skill[] Skill;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "RPG";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            ThisGame = this;
            //graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            int WinWidth = Window.ClientBounds.Width;
            int WinHeight = Window.ClientBounds.Height;

            //Текстуры
            Bg = new Sprite(Content.Load<Texture2D>("img/Windows/Battle/Bg"), new Rectangle(0, 0, WinWidth, WinHeight));

            WinMessage =  new Sprite(Content.Load<Texture2D>("img/Windows/Battle/Win"), new Rectangle(0, 130, WinWidth, 150));
            DeathMessage = new Sprite(Content.Load<Texture2D>("img/Windows/Battle/Death"), new Rectangle(0, 130, WinWidth, 150));

            GoldPanel = new Sprite(Content.Load<Texture2D>("img/Windows/Battle/GoldPanel"));
            GoldPanel.Rectangle = new Rectangle(WinWidth - 10 - GoldPanel.Texture.Width, 10, GoldPanel.Texture.Width, GoldPanel.Texture.Height);

            TimePanel = new Sprite(Content.Load<Texture2D>("img/Windows/Battle/TimePanel"));
            TimePanel.Rectangle = new Rectangle(WinWidth - 2 * (10 + TimePanel.Texture.Width), 10, TimePanel.Texture.Width, TimePanel.Texture.Height);

            //Персонажи
            Hero = new Hero(
                20, //HP 
                20, //MP
                200, //MaxHP
                20,  //MaxMP
                10,  //Attack
                5, //Def
                6, //Speed
                20, //AttackSpeed
                new Point(0, 400), //Position
                128, //FrameWidth
                128 //FrameHeight
                );
            Hero.TextureStay = Content.Load<Texture2D>("img/person/hero/Stay");
            Hero.TextureRun = Content.Load<Texture2D>("img/person/hero/Run");
            Hero.TextureJump = Content.Load<Texture2D>("img/person/hero/Jump");
            Hero.TextureAttack = Content.Load<Texture2D>("img/person/hero/Attack");
            Hero.TextureDeath = Content.Load<Texture2D>("img/person/hero/Death");

            CanRunLeft = CanRunRight = true;

            Enemy = new Enemy(
                    40, //HP 
                    10, //MP
                    40, //MaxHP
                    10,  //MaxMP
                    5,  //Attack
                    2, //Def
                    3, //Speed
                    10, //AttackSpeed
                    new Point(800, 400), //Position
                    128, //FrameWidth
                    128 //FrameHeight
                    );
            Enemy.TextureStay = Content.Load<Texture2D>("img/person/mob1/Stay");
            Enemy.TextureRun = Content.Load<Texture2D>("img/person/mob1/Run");
            Enemy.TextureJump = Content.Load<Texture2D>("img/person/mob1/Jump");
            Enemy.TextureAttack = Content.Load<Texture2D>("img/person/mob1/Attack");
            Enemy.TextureDeath = Content.Load<Texture2D>("img/person/mob1/Death");


            //Кнопки
            ButtonSaveAndExit = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    350,
                    400,
                    50
                    ),
                Content.Load<Texture2D>("img/Windows/Battle/SaveAndExit"),
                false
                );

            ButtonContinue = new Button(
                new Rectangle(
                    (Window.ClientBounds.Width - 400) / 2,
                    280,
                    400,
                    50
                    ),
                Content.Load<Texture2D>("img/Windows/Battle/Continue"),
                false
                );


            //Label
            GameTimer = new Timer(Content.Load<SpriteFont>("fonts/GameTime"), new Vector2(TimePanel.Rectangle.X + 130, TimePanel.Rectangle.Y), "00:00");
            GoldCount = new Label(Content.Load<SpriteFont>("fonts/GameTime"), new Vector2(GoldPanel.Rectangle.X + 150, GoldPanel.Rectangle.Y), "0");
            Level = new Label(Content.Load<SpriteFont>("fonts/GameTime"), new Vector2(20, 10), "");

            StatHero.Title = new Label(
                Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(10, 560),
                "ХП     МП     Атака     Защита     Скорость     Ск. атаки"
                );

            StatHero.Stats = new Label(
                Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(10, 600),
                ""
                );

            StatEnemy.Title = new Label(
             Content.Load<SpriteFont>("fonts/Stat"),
             new Vector2(680, 560),
            "ХП     МП     Атака     Защита     Скорость     Ск. атаки"
            );

            StatEnemy.Stats = new Label(
                Content.Load<SpriteFont>("fonts/Stat"),
                new Vector2(680, 600),
                ""
                );

            //Skills
            SkillPanel = new SkillPanel(
                new Rectangle(0, 640, WinWidth, 720),
                20,0, //MarginX,Y
                48, //SkillSize
                10 //Кол-во скилов
                );

            Skill = new Skill[SkillPanel.MaxSkillCount];

            Skill[SkillPanel.SkillCount] = new Skill(
                "Положительный эффект \nНа 10 секунд Запас ХП +200. \nМанакост: 2   КД: 12 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/Skill_1"),
                    true
                    ),
                new Effect[] {
                    new Effect("MaxHP",200,"Hero",10,true),
                    new Effect("MP",-2,"Hero")
                },
                12, //КД
                10,
                Content.Load<SoundEffect>("sound/Armlet")
                );
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Положительный эффект \nНа 10 секунд Атака +25, Защита -5. Поглощает 20 HP. \nМанакост: 0   КД: 18 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/Frenzy"),
                    true
                    ),
                new Effect[] {
                    new Effect("Attack",25,"Hero",10,true),
                    new Effect("Def",-5,"Hero",10,true),
                    new Effect("HP",-20,"Hero")
                },
                18, //КД
                10, //Время действия
                Content.Load<SoundEffect>("sound/Mask_of_Madness")
                );
                SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Положительный эффект \nНа 100 секунд Атака +2. \nМанакост: 2   КД: 180 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/AttackPlus"),
                    true
                    ),
                new Effect[] {
                    new Effect("Attack",2,"Hero",100,true),
                    new Effect("MP",-2,"Hero")
                },
                180, //КД
                100,
                Content.Load<SoundEffect>("sound/Iron_Talon")
                );
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Положительный эффект \nНа 4 секунды Ск.атаки +40. \nМанакост: 4   КД: 50 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/Rage"),
                    true
                    ),
                new Effect[] {
                    new Effect("AttackSpeed",40,"Hero",4,true),
                    new Effect("MP",-4,"Hero")
                },
                50, //КД
                4,
                Content.Load<SoundEffect>("sound/Satanic")
                );
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Положительный эффект \nНа 12 секунд Скорость +6. \nМанакост: 5   КД: 12 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/Boost"),
                    true
                    ),
                new Effect[] {
                    new Effect("Speed",+6,"Hero",12,true),
                    new Effect("MP",-5,"Hero")
                },
                12, //КД
                12,
                Content.Load<SoundEffect>("sound/PhaseBoots")
                );
                SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Восстановление \nВосстанавливает 7 МП \nМанакост: 0   КД: 10 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/MP"),
                    true
                    ),
                new Effect[] {
                    new Effect("MP",7,"Hero")
                },
                10, //КД
                0,
                Content.Load<SoundEffect>("sound/MP")
                );
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Восстановление \nВосстанавливает 60 ХП \nМанакост: 6   КД: 12 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/HP"),
                    true
                    ),
                new Effect[] {
                    new Effect("HP",60,"Hero"),
                    new Effect("MP",-6,"Hero")
                },
                12, //КД
                0,
                Content.Load<SoundEffect>("sound/HP")
                );
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Энергитический заряд \nУрон: 20. Накладывает дебаф: Защита -5 на 2 секунды. \nМанакост: 2   КД: 4 секунды",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/ElementShot"),
                    true
                    ),
                new Shot(
                    Content.Load<Texture2D>("img/skills/ElementShotShot"),
                    new Rectangle(Hero.Position.X + Hero.Rectangle.Width, Hero.Position.Y + Hero.Rectangle.Height / 2 + 20, 32, 32),
                    new Effect[] {
                        new Effect("HP",-20,"Enemy"),
                        new Effect("Def",-5,"Enemy",2,true),
                    },
                    2, //Скорость
                    500 //Дистанция
                    ),
                new Effect[] {
                    new Effect("MP",-2,"Hero")
                },
                4, //КД
                0, //Время действия
                Content.Load<SoundEffect>("sound/voin_skill_1"));
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Энергитический заряд \nУрон: 50. Скорость: 10. Дальность: 200\nМанакост: 4   КД: 8 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/FireBlast"),
                    true
                    ),
                new Shot(
                    Content.Load<Texture2D>("img/skills/FireBlastShot"),
                    new Rectangle(Hero.Position.X + Hero.Rectangle.Width, Hero.Position.Y + Hero.Rectangle.Height / 2 + 20, 32, 32),
                    new Effect[] {
                        new Effect("HP",-50,"Enemy"),
                    },
                    10, //Скорость
                    200 //Дистанция
                    ),
                new Effect[] {
                    new Effect("MP",-4,"Hero")
                },
                8, //КД
                0, //Время действия
                Content.Load<SoundEffect>("sound/Dagon"));
            SkillPanel.SkillCount++;

            Skill[SkillPanel.SkillCount] = new Skill(
                "Энергитический заряд \nУрон: 5. Накладывает дебаф: Скорость: -2, Ск. атаки: -5 на 15 секунд \nМанакост: 2   КД: 18 секунд",
                new Button(
                    SkillPanel.GetRect(),
                    Content.Load<Texture2D>("img/skills/IseDager"),
                    true
                    ),
                new Shot(
                    Content.Load<Texture2D>("img/skills/IseDagerShot"),
                    new Rectangle(Hero.Position.X + Hero.Rectangle.Width, Hero.Position.Y + Hero.Rectangle.Height / 2 + 20, 32, 32),
                    new Effect[] {
                        new Effect("HP",-5,"Enemy"),
                        new Effect("Speed",-2,"Enemy",15,true),
                        new Effect("AttackSpeed",-5,"Enemy",15,true),
                    },
                    5, //Скорость
                    600 //Дистанция
                    ),
                new Effect[] {
                    new Effect("MP",-2,"Hero")
                },
                18, //КД
                0, //Время действия
                Content.Load<SoundEffect>("sound/Maelstrom"));
                        SkillPanel.SkillCount++;

            // Songs
            MainTheme = Content.Load<Song>("sound/Menu");
            BattleSong = Content.Load<Song>("sound/Battle");
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(MainTheme);

            //Sound Effect
            ButtonClick = Content.Load<SoundEffect>("sound/Click");
            HeroAttack = Content.Load<SoundEffect>("sound/HeroAttack");
            HeroDeath = Content.Load<SoundEffect>("sound/HeroDeath");
            EnemyAttack = Content.Load<SoundEffect>("sound/EnemyAttack");
            SoundWin = Content.Load<Song>("sound/SoundWin");

            //Окна
            WindowState = WindowState.Start;
            StartWindow = new StartWindow(Window);
            SkillWindow = new SkillWindow(Skill,Window);
            RecordWindow = new RecordWindow(Window);
        }

        protected override void UnloadContent()
        {

        }

        void CheckRotate()
        {
            if (Enemy.Position.X < Hero.Position.X + Hero.Rectangle.Width)
            {
                Enemy.Rotate = false;
            }
            else
            {
                Enemy.Rotate = true;
            }
        }

        void CheckRun()
        {

            if (Hero.Position.X < Enemy.Position.X)
            {
                CanRunLeft = true;
                CanRunRight = false;

                Hero.Rotate = false;
                Enemy.Rotate = true;
            }
            if (Hero.Position.X > Enemy.Position.X)
            {
                CanRunLeft = false;
                CanRunRight = true;

                Hero.Rotate = true;
                Enemy.Rotate = false;
            }

            if (Hero.onJump)
            {
                CanRunLeft = true;
                CanRunRight = true;
            }
        }

        void StatTextUpdate()
        {
            StatHero.Stats.Text = Hero.HP.ToString(fmt) + "    " + Hero.MP.ToString(fmt) + "      " + Hero.Attack.ToString(fmt) + "          " + Hero.Def.ToString(fmt) + "           " + Hero.Speed.ToString(fmt) + "               " + Hero.AttackSpeed.ToString(fmt);
            StatEnemy.Stats.Text = Enemy.HP.ToString(fmt) + "    " + Enemy.MP.ToString(fmt) + "      " + Enemy.Attack.ToString(fmt) + "          " + Enemy.Def.ToString(fmt) + "           " + Enemy.Speed.ToString(fmt) + "               " + Enemy.AttackSpeed.ToString(fmt);
        }

        void ParseEffect(Effect Effect, int AddEffect)
        {
            /*AddEffect:
             * 1 - Добавить эффект
             * -1 - Убрать эффект
            */
            if(Effect.Param == "HP")
            {
                if (Effect.Target == "Hero")
                {
                    Hero.HP += Effect.Value * AddEffect;
                    if (Hero.HP > Hero.MaxHP) Hero.HP = Hero.MaxHP;
                }
                if(Effect.Target == "Enemy")
                {
                    Enemy.HP += Effect.Value * AddEffect;
                    if (Enemy.HP > Enemy.MaxHP) Enemy.HP = Enemy.MaxHP;
                }
            }

            if (Effect.Param == "MP")
            {
                if (Effect.Target == "Hero")
                {
                    Hero.MP += Effect.Value * AddEffect;
                    if (Hero.MP > Hero.MaxMP) Hero.MP = Hero.MaxMP;
                }
                if (Effect.Target == "Enemy")
                {
                    Enemy.MP += Effect.Value * AddEffect;
                    if (Enemy.MP > Enemy.MaxMP) Enemy.MP = Enemy.MaxMP;
                }
            }

            if (Effect.Param == "MaxHP")
            {
                if (Effect.Target == "Hero")
                {
                    Hero.MaxHP += Effect.Value * AddEffect;
                    Hero.HP += Effect.Value * AddEffect;
                }
                if (Effect.Target == "Enemy")
                {
                    Enemy.MaxHP += Effect.Value * AddEffect;
                    Enemy.HP += Effect.Value * AddEffect;
                }
            }

            if (Effect.Param == "MaxMP")
            {
                if (Effect.Target == "Hero")
                {
                    Hero.MaxMP += Effect.Value * AddEffect;
                    Hero.MP += Effect.Value * AddEffect;
                }
                if (Effect.Target == "Enemy")
                {
                    Enemy.MaxMP += Effect.Value * AddEffect;
                    Enemy.MP += Effect.Value * AddEffect;
                }
            }

            if (Effect.Param == "Attack")
            {
                if (Effect.Target == "Hero")
                    Hero.Attack += Effect.Value * AddEffect;
                if (Effect.Target == "Enemy")
                    Enemy.Attack += Effect.Value * AddEffect;
            }

            if (Effect.Param == "Def")
            {
                if (Effect.Target == "Hero")
                    Hero.Def += Effect.Value * AddEffect;
                if (Effect.Target == "Enemy")
                    Enemy.Def += Effect.Value * AddEffect;
            }

            if (Effect.Param == "Speed")
            {
                if (Effect.Target == "Hero")
                    Hero.Speed += Effect.Value * AddEffect;
                if (Effect.Target == "Enemy")
                    Enemy.Speed += Effect.Value * AddEffect;
            }

            if (Effect.Param == "AttackSpeed")
            {
                if (Effect.Target == "Hero")
                    Hero.AttackSpeed += Effect.Value * AddEffect;
                if (Effect.Target == "Enemy")
                    Enemy.AttackSpeed += Effect.Value * AddEffect;
            }
        }


        void ShotUpdate(Shot Shot, GameTime gameTime)
        {
            Shot.Update(gameTime);

            if (!Shot.Active) { 
            Shot.Rectangle.X = Hero.Position.X;
            Shot.Rotate = Hero.Rotate;
            }

            Rectangle EnemyRect = new Rectangle(Enemy.Position.X, Enemy.Position.Y, Enemy.FrameWidth, Enemy.FrameHeight);

            if (Shot.Rectangle.Intersects(EnemyRect) && Shot.Active && !Shot.Damaged)
            {
                Shot.Success = true;
                Shot.Damaged = true;
            }

            for (int i = 0; i < Shot.Effect.Length; i++)
            {
                if (Shot.Effect[i].isStart)
                {
                    ParseEffect(Shot.Effect[i], 1);
                    Shot.Effect[i].isStart = false;
                }

                //Проверяем, нужно ли выполнить какие то действия
                if (Shot.Effect[i].isEnd && Shot.Effect[i].Deactive)
                {
                    ParseEffect(Shot.Effect[i], -1);
                    Shot.Effect[i].isEnd = false;
                }
            }
        }

        void BuffUpdate(Skill Skill, GameTime gameTime)
        {

            //обновляем состояние скилов
            Skill.Update(gameTime, Hero.MP);

            if (Skill.Shot != null)
            {
                ShotUpdate(Skill.Shot, gameTime);
            }

            for (int i = 0; i < Skill.Effect.Length; i++)
            {
                if (Skill.Effect[i].isStart)
                {
                    ParseEffect(Skill.Effect[i], 1);
                    Skill.Effect[i].isStart = false;
                }

                //Проверяем, нужно ли выполнить какие то действия
                if (Skill.Effect[i].isEnd && Skill.Effect[i].Deactive)
                {
                    ParseEffect(Skill.Effect[i], -1);
                    Skill.Effect[i].isEnd = false;
                }
            }
        }

        public void BattleUpdate(GameTime gameTime)
        {
            if(!GameIsEnd) Level.Text = "Уровень " + (CurrentLevel + 1).ToString();

            ButtonContinue.Update(gameTime);
            ButtonSaveAndExit.Update(gameTime);

            if(!LevelIsEnd) {

                ButtonContinue.Visible = false;
                ButtonSaveAndExit.Visible = false;

                if (Hero.isDeath)
                {
                    //Поражение

                    ButtonSaveAndExit.Visible = true;

                    if (ButtonSaveAndExit.ButtonUp)
                    {
                        SaveResult();
                        Exit();
                    }

                    Enemy.CurrentFrame = 0;
                    GameIsEnd = true;
                }
                else
                {
                    //Обновление состояния Бафов
                    for (int i = 0; i < Skill.Length; i++)
                        BuffUpdate(Skill[i], gameTime);

                    GameTimer.Update(gameTime);
                    Hero.Update(Window, gameTime);

                    if(Hero.State == 3)
                    {
                        if (SoundOnes)
                        {
                            MediaPlayer.Pause();
                            HeroDeath.Play();
                            SoundOnes = false;
                        }
                    }

                    if (Hero.onJump)
                    {
                        Hero.State = 4;
                        Hero.Jump();
                    }

                    //протиуник может умереть от шота
                    if (Enemy.HP <= 0 && Enemy.State != 3)
                    {
                        GoldCount.Text = (Convert.ToInt32(GoldCount.Text) + 1).ToString();
                        Enemy.State = 3;
                        Enemy.CurrentFrame = 0;
                        Enemy.CurrentTime = 0;
                    }

                    if (Enemy.State != 3)
                    {
                        CanRunLeft = CanRunRight = true;

                        // Взаимодействие персонажа и противника
                        if (Enemy.Rectangle.Intersects(Hero.Rectangle))
                        {
                            if (Hero.State == 1 || Hero.State == 0)
                            {
                                Hero.CurrentTime = 0;
                                Hero.CurrentFrame = 0;
                                Hero.State = 2;
                            }
                            CheckRun();
                            Enemy.DoAttack(Window, gameTime);
                            if (Hero.State == 2 || Hero.State == 0) Hero.DoAttack(Window, gameTime);
                            if (Enemy.CurrentFrame == 11 && Enemy.State == 2) //При завершении анимации заверщаем удар
                            {
                                EnemyAttack.Play();
                                if (Enemy.Attack >= Hero.Def)
                                    Hero.HP -= Enemy.Attack - Hero.Def;
                                else
                                    Hero.HP--;
                                
                                StatHero.Stats.Text = Hero.HP + "    " + Hero.MP + "       " + Hero.Attack + "            " + Hero.Def + "               " + Hero.Speed + "                 " + Hero.AttackSpeed;
                                Enemy.CurrentFrame = 0;
                            }
                            if (Hero.CurrentFrame == 11 && Hero.State == 2) //При завершении анимации заверщаем удар
                            {
                                HeroAttack.Play();
                                if (Hero.Attack >= Enemy.Def)
                                    Enemy.HP -= Hero.Attack - Enemy.Def;
                                else
                                    Enemy.HP--;
                                Hero.CurrentFrame = 0;
                                if (Enemy.HP <= 0)
                                {
                                    GoldCount.Text = (Convert.ToInt32(GoldCount.Text) + 1).ToString();
                                    Enemy.State = 3;
                                }
                            }
                        }
                        else
                        {
                            CheckRotate();
                            Enemy.DoMove(Window, gameTime);
                        }
                    }
                    else
                    {
                        Enemy.Animate(gameTime);
                    }
                }
            } else
            {
                //Уровень завершен
                if (SoundOnes)
                {
                    MediaPlayer.Play(SoundWin);

                    if (!GameIsEnd) ButtonContinue.Visible = true;
                    ButtonSaveAndExit.Visible = true;

                    SoundOnes = false;
                }

                if(ButtonContinue.ButtonUp)
                {
                    MediaPlayer.Play(MainTheme);
                    WindowState = WindowState.Skill;
                    SkillWindow.SkillPoint++;
                    LevelIsEnd = false;
                    SoundOnes = true;
                }

                if (ButtonSaveAndExit.ButtonUp)
                {
                    SaveResult();
                    Exit();
                }
            }

            //Обновлняем статы персонажекй
            StatTextUpdate();

            base.Update(gameTime);

            //Если противник умер и произошла анимация падения
            if (Enemy.State == 3 && Enemy.CurrentFrame == 11)
            {
                NextEnemy(gameTime);
            }
        }
        protected override void Update(GameTime gameTime)
        {
            //KeyboardManager(gameTime);
            switch (WindowState)
            {
                case WindowState.Start:
                    StartWindow.Update(gameTime);
                    break;
                case WindowState.Skill:
                    SkillWindow.Update(gameTime);
                    break;
                case WindowState.Battle:
                    BattleUpdate(gameTime);
                    break;
                case WindowState.Record:
                    RecordWindow.Update(gameTime,Window);
                    break;
            }
        }

        private void NextEnemy(GameTime gameTime)
        {

        if (!GameIsEnd)
            for (int i = 0; i < Skill.Length; i++)
            {
                for(int j = 0; j < Skill[i].Effect.Length; j++)
                {
                    if (Skill[i].Effect[j].Target == "Enemy")
                    {
                        Skill[i].Effect[j].isEnd = true;
                    }

                    if (Skill[i].Shot != null)
                        for (int k = 0; k < Skill[i].Shot.Effect.Length; k++)
                        {
                            if (Skill[i].Shot.Effect[k].Target == "Enemy")
                            {
                                Skill[i].Shot.Effect[k].isEnd = true;
                            }
                        }
                }
                BuffUpdate(Skill[i], gameTime);
            }


            if (!GameIsEnd)
            {
                if (Convert.ToInt32(GoldCount.Text) >= LevelEnd[CurrentLevel])
                {
                    //Переход на следующий уровень
                    LevelIsEnd = true;
                    CurrentLevel++;

                    if (CurrentLevel >= LevelEnd.Length)
                    {
                        GameIsEnd = true;
                    }
                }
                else
                    switch (Convert.ToInt32(GoldCount.Text))
                    {
                        case 1:
                            Enemy = new Enemy(
                                50, //HP 
                                10, //MP
                                50, //MaxHP
                                10,  //MaxMP
                                6,  //Attack
                                3, //Def
                                3, //Speed
                                10, //AttackSpeed
                                new Point(800, 400), //Position
                                128, //FrameWidth
                                128 //FrameHeight
                                );
                            Enemy.TextureStay = Content.Load<Texture2D>("img/person/mob1/Stay");
                            Enemy.TextureRun = Content.Load<Texture2D>("img/person/mob1/Run");
                            Enemy.TextureJump = Content.Load<Texture2D>("img/person/mob1/Jump");
                            Enemy.TextureAttack = Content.Load<Texture2D>("img/person/mob1/Attack");
                            Enemy.TextureDeath = Content.Load<Texture2D>("img/person/mob1/Death");
                            break;
                        case 2:
                            Enemy = new Enemy(
                                100, //HP 
                                10, //MP
                                100, //MaxHP
                                10,  //MaxMP
                                8,  //Attack
                                3, //Def
                                3, //Speed
                                14, //AttackSpeed
                                new Point(800, 400), //Position
                                128, //FrameWidth
                                128 //FrameHeight
                                );
                            Enemy.TextureStay = Content.Load<Texture2D>("img/person/mob2/Stay");
                            Enemy.TextureRun = Content.Load<Texture2D>("img/person/mob2/Run");
                            Enemy.TextureJump = Content.Load<Texture2D>("img/person/mob2/Jump");
                            Enemy.TextureAttack = Content.Load<Texture2D>("img/person/mob2/Attack");
                            Enemy.TextureDeath = Content.Load<Texture2D>("img/person/mob2/Death");
                            break;
                        case 3:
                            Enemy = new Enemy(
                                110, //HP 
                                10, //MP
                                110, //MaxHP
                                10,  //MaxMP
                                8,  //Attack
                                3, //Def
                                5, //Speed
                                14, //AttackSpeed
                                new Point(800, 400), //Position
                                128, //FrameWidth
                                128 //FrameHeight
                                );
                            Enemy.TextureStay = Content.Load<Texture2D>("img/person/mob2/Stay");
                            Enemy.TextureRun = Content.Load<Texture2D>("img/person/mob2/Run");
                            Enemy.TextureJump = Content.Load<Texture2D>("img/person/mob2/Jump");
                            Enemy.TextureAttack = Content.Load<Texture2D>("img/person/mob2/Attack");
                            Enemy.TextureDeath = Content.Load<Texture2D>("img/person/mob2/Death");
                            break;
                        case 4:
                            Enemy = new Enemy(
                                120, //HP 
                                10, //MP
                                120, //MaxHP
                                10,  //MaxMP
                                10,  //Attack
                                3, //Def
                                5, //Speed
                                12, //AttackSpeed
                                new Point(800, 400), //Position
                                128, //FrameWidth
                                128 //FrameHeight
                                );
                            Enemy.TextureStay = Content.Load<Texture2D>("img/person/mob2/Stay");
                            Enemy.TextureRun = Content.Load<Texture2D>("img/person/mob2/Run");
                            Enemy.TextureJump = Content.Load<Texture2D>("img/person/mob2/Jump");
                            Enemy.TextureAttack = Content.Load<Texture2D>("img/person/mob2/Attack");
                            Enemy.TextureDeath = Content.Load<Texture2D>("img/person/mob2/Death");
                            break;
                        case 5:
                            Enemy = new Enemy(
                                160, //HP 
                                10, //MP
                                160, //MaxHP
                                10,  //MaxMP
                                25,  //Attack
                                5, //Def
                                3, //Speed
                                15, //AttackSpeed
                                new Point(800, 400), //Position
                                128, //FrameWidth
                                128 //FrameHeight
                                );
                            Enemy.TextureStay = Content.Load<Texture2D>("img/person/mob3/Stay");
                            Enemy.TextureRun = Content.Load<Texture2D>("img/person/mob3/Run");
                            Enemy.TextureJump = Content.Load<Texture2D>("img/person/mob3/Jump");
                            Enemy.TextureAttack = Content.Load<Texture2D>("img/person/mob3/Attack");
                            Enemy.TextureDeath = Content.Load<Texture2D>("img/person/mob3/Death");
                            break;

                    }
            }
        }

        public void KeyboardManager(GameTime gameTime)
        {

            //Закрытие игры
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kbState = Keyboard.GetState();
        }

        public void SaveResult()
        {
            using (FileStream fstream = new FileStream("Content/Record.txt", FileMode.Append))
            {
                // преобразуем строку в байты
                byte[] array = Encoding.Default.GetBytes(StartWindow.InputName.Text + " " + GameTimer.Text + " " + GoldCount.Text + Environment.NewLine);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }
        }

        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            if(WindowState == WindowState.Start)
            {
                StartWindow.Draw(spriteBatch, Window);
            }

            if (WindowState == WindowState.Skill)
            {
                SkillWindow.Draw(spriteBatch, Window);
            }

            if (WindowState == WindowState.Battle)
            {
                spriteBatch.Draw(Bg.Texture, Bg.Rectangle, Color.White);
                spriteBatch.Draw(GoldPanel.Texture, GoldPanel.Rectangle, Color.White);
                spriteBatch.Draw(TimePanel.Texture, TimePanel.Rectangle, Color.White);

                for (int i = 0; i < Skill.Length; i++)
                    Skill[i].Draw(spriteBatch, Window);

                Hero.Draw(spriteBatch);
                if(!LevelIsEnd) Enemy.Draw(spriteBatch);

                GameTimer.Draw(spriteBatch, Color.White);
                GoldCount.Draw(spriteBatch, Color.White);
                Level.Draw(spriteBatch, Color.White);

                if(LevelIsEnd ||  GameIsEnd)
                    if(Hero.isDeath)
                        spriteBatch.Draw(DeathMessage.Texture, DeathMessage.Rectangle, Color.White);
                    else
                        spriteBatch.Draw(WinMessage.Texture, WinMessage.Rectangle, Color.White);

                StatHero.Title.Draw(spriteBatch);
                StatHero.Stats.Draw(spriteBatch);

                StatEnemy.Title.Draw(spriteBatch);
                StatEnemy.Stats.Draw(spriteBatch);

                ButtonContinue.Draw(spriteBatch, Window);
                ButtonSaveAndExit.Draw(spriteBatch, Window);
            }

            if (WindowState == WindowState.Record)
            {
                RecordWindow.Draw(spriteBatch, Window);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}

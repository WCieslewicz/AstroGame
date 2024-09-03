using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Input.Touch;
using System.Threading;
using System.Diagnostics.Metrics;

namespace CieslewiczAstro2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rakieta gracz;
        private Meteor wróg;
        private Meteor wrógDrugi;
        private Texture2D rakieta;
        private Texture2D meteor;
        private Texture2D control;
        private Texture2D niebo;
        private Texture2D pocisk;
        private DateTime timeOfGameOver;
        private SpriteFont wykrytoKolizje, wykrytoKolizjePocisk;
        public int score = 0;
        private bool isGameOver = false;
        SoundEffectInstance wybuchRaz;
        SoundEffect wybuch;

        enum States
        {
            GameOver,
            Game,
        }

        States _state;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
            _state = States.Game;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            niebo = Content.Load<Texture2D>("niebo");
            rakieta = Content.Load<Texture2D>("AnimRakiety");
            meteor = Content.Load<Texture2D>("meteor");
            pocisk = Content.Load<Texture2D>("pocisk2D");
            control = Content.Load<Texture2D>("control");
            wybuch = Content.Load<SoundEffect>("wybuch");
            wróg = new Meteor(meteor, 10);
            wrógDrugi = new Meteor(meteor, 20);
            gracz = new Rakieta(rakieta, pocisk);
            wybuchRaz = wybuch.CreateInstance();
        }
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_state)
            {
                case States.Game:
                    wróg.Update();
                    wrógDrugi.Update();
                    gracz.LotPocisku();
                    if (wróg.Kolizja(gracz) || wrógDrugi.Kolizja(gracz))
                    {
                        wybuchRaz.Play();
                        isGameOver = true;
                        _state = States.GameOver;
                        timeOfGameOver = DateTime.Now;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.X))
                    {
                        Exit();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        gracz.MoveU();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        gracz.MoveD();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        gracz.MoveL();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        gracz.MoveR();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        gracz.Wystrzel();
                    }

                    TouchCollection mscaDotknięte = TouchPanel.GetState();
                    foreach (TouchLocation dotyk in mscaDotknięte)
                    {
                        Vector2 pozDotyku = dotyk.Position;
                        if (dotyk.State == TouchLocationState.Moved)
                        {
                            if (Math.Pow(pozDotyku.X - 110, 2) + (Math.Pow(pozDotyku.Y - 645, 2)) <= 40 * 40)
                            {
                                gracz.MoveU();
                            }
                            if (Math.Pow(pozDotyku.X - 110, 2) + (Math.Pow(pozDotyku.Y - 740, 2)) <= 40 * 40)
                            {
                                gracz.MoveD();
                            }
                            if (Math.Pow(pozDotyku.X - 60, 2) + (Math.Pow(pozDotyku.Y - 690, 2)) <= 40 * 40)
                            {
                                gracz.MoveL();
                            }
                            if (Math.Pow(pozDotyku.X - 160, 2) + (Math.Pow(pozDotyku.Y - 690, 2)) <= 40 * 40)
                            {
                                gracz.MoveR();
                            }
                        }
                        if (dotyk.State == TouchLocationState.Pressed)
                        {
                            if (Math.Pow(pozDotyku.X - 375, 2) + (Math.Pow(pozDotyku.Y - 695, 2)) <= 40 * 40)
                            {
                                gracz.Wystrzel();
                            }
                        }
                    }
                    break;
                case States.GameOver:
                    if ((DateTime.Now - timeOfGameOver).TotalSeconds >= 3)
                    {
                        wybuchRaz.Stop();
                        Thread.Sleep(500);
                        Exit();
                    }
                    break;
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(niebo, new Vector2(0, 0), Color.White);
            gracz.Draw(rakieta, _spriteBatch);
            wróg.Draw(_spriteBatch);
            wrógDrugi.Draw(_spriteBatch);
            _spriteBatch.Draw(control, new Vector2(0, 583), Color.White);
            if (isGameOver == true)
            {
                
                _spriteBatch.DrawString(wykrytoKolizje, "Koniec gry!", new Vector2(90, 300), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

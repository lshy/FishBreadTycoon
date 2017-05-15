using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FishBreadTycoon.UI;

using System.Diagnostics;

namespace FishBreadTycoon
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        SpriteManager spManger = new SpriteManager();

        Vector2[] posList;
        Vector2[] posMouse;
        Vector2 teelPos = Vector2.Zero;

        FishBread[] breads;
        Player player1;

        KeyboardState oldState;
        KeyboardState newState;

        MouseState mouseState;
        MouseState prevmouseState;

        Texture2D bg = null;
        Texture2D sprite;

        Random random;

        private int mouseX;
        private int mouseY;

        private int whatCursur;

        private int termClickTime;

        TimerManager timerManager;
        LongClickTimer longClickTimer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;


            posList = new Vector2[9];
            posMouse = new Vector2[3];

            player1 = new Player();

            random = new Random();
            termClickTime = 0;
            timerManager = new TimerManager();

            breads = new FishBread[9];
            player1.CursorType = (int)CURSOR_TYPE.HAND;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            breads = new FishBread[9];

            // TODO: Add your initialization logic here

            int k = 0;
            for (int i = 0; i < 9; i++)
            {
                breads[i] = new FishBread();
                if (i % 3 == 0)
                {
                    k++;
                }

                teelPos = new Vector2((i % 3) * 200 + 300, 200 * k - 100);
                posList[i] = teelPos;
            }

            posMouse[0] = new Vector2(14, 137);
            posMouse[1] = new Vector2(70, 100);
            posMouse[2] = new Vector2(123, 3);

            oldState = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spManger.SetBatch(spriteBatch);
            bg = new Texture2D(GraphicsDevice, 100, 100);
            bg = Content.Load<Texture2D>("background");

            spManger.AddSprite(TEEL_STATE.OJ_IDLE, Content, "Sprite/teel/teel/teel0000");
            spManger.AddSprite(TEEL_STATE.OJ_BASEING, Content, "Sprite/teel/base/teel", (int)SPRITE_COUNT.SP_BASEING);
            spManger.AddSprite(TEEL_STATE.OJ_BASE, Content, "Sprite/teel/base/teel0009");
            spManger.AddSprite(TEEL_STATE.OJ_PATING, Content, "Sprite/teel/pat/teel", (int)SPRITE_COUNT.SP_PATING);
            spManger.AddSprite(TEEL_STATE.OJ_PAT, Content, "Sprite/teel/pat/teel0002");
            spManger.AddSprite(TEEL_STATE.OJ_REVERSEING, Content, "Sprite/teel/reverse/teel", (int)SPRITE_COUNT.SP_REVERSING);
            spManger.AddSprite(TEEL_STATE.OJ_FINISHED, Content, "Sprite/teel/reverse/teel0016");
            spManger.AddSprite(TEEL_STATE.OJ_FINISHING, Content, "Sprite/teel/finishing/teel", (int)SPRITE_COUNT.SP_FINISHING);
            spManger.AddSprite(TEEL_STATE.OJ_BURNING, Content, "Sprite/teel/burn/teel", (int)SPRITE_COUNT.SP_BURNING);
            sprite = Content.Load<Texture2D>("hand");

            //TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            prevmouseState = mouseState;
            mouseState = Mouse.GetState();

            oldState = newState;
            newState = Keyboard.GetState();

            mouseX = mouseState.X;
            mouseY = mouseState.Y;


            ClickEvent(gameTime);
            TeelAnimationCheck(gameTime);


            timerManager.process((int)gameTime.TotalGameTime.TotalMilliseconds);

            UpdateInput();

            // TODO: Add your update logic here



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            spriteBatch.Draw(bg, new Vector2(0, 0), new Color(255, 255, 255, 255));//background

            for (int i = 0; i < 9; i++)
            {
                spManger.Draw(breads[i].State, breads[i].Start, posList[i], Color.White);
            }


            switch (player1.CursorType)
            {
                case (int)CURSOR_TYPE.KETTLE:
                    whatCursur = 0;
                    break;
                case (int)CURSOR_TYPE.PAT:
                    whatCursur = 1;
                    break;
                case (int)CURSOR_TYPE.HAND:
                    whatCursur = 2;
                    break;
            }
            spriteBatch.Draw(sprite, new Vector2(Mouse.GetState().X - (int)posMouse[whatCursur].X, Mouse.GetState().Y - (int)posMouse[whatCursur].Y), Color.White);


            spriteBatch.End();


            base.Draw(gameTime);
        }

        private void ClickEvent(GameTime gameTime)
        {
            switch (player1.CursorType)
            {
                case (int)CURSOR_TYPE.HAND:
                case (int)CURSOR_TYPE.PAT:

                    ShortClick(gameTime);

                    break;
                case (int)CURSOR_TYPE.KETTLE:

                    if (mouseState.LeftButton == ButtonState.Pressed && prevmouseState.LeftButton == ButtonState.Released)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (new Rectangle((int)posList[i].X, (int)posList[i].Y, 200, 200).Contains(mouseX, mouseY))
                            {
                                if (breads[i].State == TEEL_STATE.OJ_IDLE)
                                {
                                    breads[i].End = CalcCount(TEEL_STATE.OJ_BASEING);
                                    breads[i].State = TEEL_STATE.OJ_BASEING;
                                    breads[i].IsAnimate = true;
                                    timerManager.AddTimer(new LongClickTimer(breads[i], (int)gameTime.TotalGameTime.TotalMilliseconds));
                                }
                            }
                        }

                    }
                    break;
            }
        }

        private void ShortClick(GameTime gametime)
        {
            for (int i = 0; i < 9; i++)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevmouseState.LeftButton == ButtonState.Released)
                {
                    if (new Rectangle((int)posList[i].X, (int)posList[i].Y, 200, 200).Contains(mouseX, mouseY))
                    {
                        if (!breads[i].IsAnimate && breads[i].State != TEEL_STATE.OJ_IDLE)
                        {
                            if ((player1.CursorType == (int)CURSOR_TYPE.PAT && breads[i].State == TEEL_STATE.OJ_BASE)
                                  || (player1.CursorType == (int)CURSOR_TYPE.HAND && (breads[i].State == TEEL_STATE.OJ_PAT || (breads[i].State == TEEL_STATE.OJ_FINISHED))))
                            {
                                breads[i].State++;
                                breads[i].Start = 0;
                                breads[i].End = CalcCount(breads[i].State);
                                if (breads[i].End != 0)
                                {
                                    breads[i].IsAnimate = true;
                                }
                            }
                        }
                    }
                }
            }
        }




        private void TeelAnimationCheck(GameTime gameTime)
        {
            int time = (int)gameTime.TotalGameTime.TotalMilliseconds;
            
            for (int i = 0; i < 9; i++)
            {

                if (breads[i].EndTime != 0 && breads[i].EndTime < time / 1000)
                {
                    breads[i].State = TEEL_STATE.OJ_BURNING;
                    breads[i].Start = 0;
                    breads[i].End = CalcCount(TEEL_STATE.OJ_BURNING);
                    breads[i].EndTime = 0;
                    breads[i].IsAnimate = true;
                }


                // chage anmation time
                if ((player1.CursorType == (int)CURSOR_TYPE.KETTLE && time % 10 == 0)
                    || (player1.CursorType == (int)CURSOR_TYPE.PAT && time % 10 == 0)
                        || (player1.CursorType == (int)CURSOR_TYPE.HAND && time % 2 == 0))
                {
                    if (breads[i].Start == breads[i].End)
                    {
                        breads[i].Start = 0;
                    }
                    else
                    {
                        breads[i].Start++;
                    }
                }

                if (breads[i].IsAnimate && breads[i].Start == breads[i].End)
                {
                    if (breads[i].State == TEEL_STATE.OJ_FINISHING)
                    {
                        breads[i].State = TEEL_STATE.OJ_IDLE;
                        breads[i].EndTime = 0;
                        player1.BreadCount++;
                    }
                    else if (breads[i].State == TEEL_STATE.OJ_BURNING)
                    {
                        breads[i].State = TEEL_STATE.OJ_IDLE;
                    }
                    else
                    {
                        breads[i].State++;
                    }

                    if (breads[i].State == TEEL_STATE.OJ_PAT)
                    {
                        breads[i].EndTime = time / 1000 + random.Next(3, 7);
                    }

                    breads[i].Start = 0;
                    breads[i].End = 0;
                    breads[i].IsAnimate = false;
                }
            }

        }

        private int CalcCount(TEEL_STATE state)
        {
            switch (state)
            {
                case TEEL_STATE.OJ_BASEING:
                    return (int)SPRITE_COUNT.SP_BASEING - 1;
                case TEEL_STATE.OJ_PATING:
                    return (int)SPRITE_COUNT.SP_PATING - 1;
                case TEEL_STATE.OJ_REVERSEING:
                    return (int)SPRITE_COUNT.SP_REVERSING - 1;
                case TEEL_STATE.OJ_FINISHING:
                    return (int)SPRITE_COUNT.SP_FINISHING -1;
                case TEEL_STATE.OJ_BURNING:
                    return (int)SPRITE_COUNT.SP_BURNING - 1;
            }
            return 0;
        }

        private void UpdateInput()
        {

            if (oldState.IsKeyDown(Keys.Q) && newState.IsKeyUp(Keys.Q))
            {

                // If not down last update, key has just been pressed.
                //if (!oldState.IsKeyDown(Keys.Q)
                player1.CursorType = (int)CURSOR_TYPE.KETTLE;
                sprite = Content.Load<Texture2D>("banjuk");
            }
            if (oldState.IsKeyDown(Keys.W) && newState.IsKeyUp(Keys.W))
            {

                // If not down last update, key has just been pressed.
                //if (!oldState.IsKeyDown(Keys.W)

                player1.CursorType = (int)CURSOR_TYPE.PAT;
                sprite = Content.Load<Texture2D>("pat");
            }
            if (oldState.IsKeyDown(Keys.E) && newState.IsKeyUp(Keys.E))
            {
                // If not down last update, key has just been pressed.
                //if (!oldState.IsKeyDown(Keys.E)
                player1.CursorType = (int)CURSOR_TYPE.HAND;
                sprite = Content.Load<Texture2D>("hand");
            }
        }
    }
}

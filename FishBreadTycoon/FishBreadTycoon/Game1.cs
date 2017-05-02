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
        Vector2 teelPos = Vector2.Zero;

        FishBread[] breads;
        Player player1;

        MouseState mouseState;
        MouseState prevmouseState;

        Texture2D bg = null;

        Random random;

        private int mouseX;
        private int mouseY;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;


            posList = new Vector2[9];
            player1 = new Player();

            random = new Random();
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

                teelPos = new Vector2((i % 3) * 200+300, 200 * k-100);
                posList[i] = teelPos;
            }

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
            spManger.AddSprite(TEEL_STATE.OJ_BASEING, Content, "Sprite/teel/base/teel", 10);
            spManger.AddSprite(TEEL_STATE.OJ_BASE, Content, "Sprite/teel/base/teel0009");
            spManger.AddSprite(TEEL_STATE.OJ_PATING, Content, "Sprite/teel/pat/teel", 3);
            spManger.AddSprite(TEEL_STATE.OJ_PAT, Content, "Sprite/teel/pat/teel0002");
            spManger.AddSprite(TEEL_STATE.OJ_REVERSEING, Content, "Sprite/teel/reverse/teel", 4);
            spManger.AddSprite(TEEL_STATE.OJ_BURNING, Content, "Sprite/teel/burn/teel", 5);

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

            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            

            TeelAnimationCheck(gameTime);
            ClickEvent(gameTime);
            

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
            spriteBatch.End();

           
            base.Draw(gameTime);
        }

        private void ClickEvent(GameTime gameTime)
        {
            for (int i = 0; i < 9; i++)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevmouseState.LeftButton == ButtonState.Released)
                {
                    if (new Rectangle((int)posList[i].X, (int)posList[i].Y, 200, 200).Contains(mouseX, mouseY))
                    {
                        if (breads[i].State + 1 != TEEL_STATE.OJ_END)
                        {
                            breads[i].State++;
                            breads[i].Start = 0;
                            breads[i].End = CalcCount(breads[i].State);
                        }
                        else
                        {
                            breads[i].State = TEEL_STATE.OJ_IDLE;

                            breads[i].Start = 0;
                            breads[i].End = CalcCount(breads[i].State);

                        }
                    }
                }
                if (mouseState.RightButton == ButtonState.Pressed && prevmouseState.RightButton == ButtonState.Pressed)
                {
                    if (new Rectangle((int)posList[i].X, (int)posList[i].Y, 200, 200).Contains(mouseX, mouseY))
                    {
                        System.Windows.Forms.MessageBox.Show(i + 1 + "�� �ؾ ������");
                        Debug.Print(""+ player1.BreadCount);
                    }
                }
            }
        }

        private void TeelAnimationCheck(GameTime gameTime)
        {
            int time = (int)gameTime.TotalGameTime.TotalMilliseconds;
            
            for (int i = 0; i < 9; i++)
            {

                if(breads[i].EndTime != 0 && breads[i].EndTime < time / 1000)
                {
                    breads[i].State = TEEL_STATE.OJ_BURNING;
                    breads[i].Start = 0;
                    breads[i].End = CalcCount(TEEL_STATE.OJ_BURNING);
                    breads[i].EndTime = 0;
                }



                if (time % 10 == 0)
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

                if (breads[i].State == TEEL_STATE.OJ_BASEING && breads[i].Start == breads[i].End)
                {
                    breads[i].State = TEEL_STATE.OJ_BASE;

                    breads[i].Start = 0;

                    breads[i].End = 0;
                }
                else if (breads[i].State == TEEL_STATE.OJ_PATING && breads[i].Start == breads[i].End)
                {
                    breads[i].State = TEEL_STATE.OJ_PAT;

                    breads[i].Start = 0;

                    breads[i].End = 0;
                    breads[i].EndTime = time/1000 + random.Next(5, 7);
                }
                else if (breads[i].State == TEEL_STATE.OJ_REVERSEING && breads[i].Start == breads[i].End)
                {
                    breads[i].State = TEEL_STATE.OJ_IDLE;
                    breads[i].Start = 0;
                    breads[i].End = 0;
                    breads[i].EndTime = 0;
                    player1.BreadCount++;
                }
                else if (breads[i].State == TEEL_STATE.OJ_BURNING && breads[i].Start == breads[i].End)
                {
                    breads[i].State = TEEL_STATE.OJ_IDLE;
                    breads[i].Start = 0;
                    breads[i].End = 0;
                    breads[i].EndTime = 0;
                }
            }

        }

        private int CalcCount(TEEL_STATE state)
        {
            switch (state)
            {
                case TEEL_STATE.OJ_BASEING:
                    return 9;
                case TEEL_STATE.OJ_PATING:
                    return 2;
                case TEEL_STATE.OJ_REVERSEING:
                    return 3;
                case TEEL_STATE.OJ_BURNING:
                    return 4;
            }
            return 0;
        }
    }
}
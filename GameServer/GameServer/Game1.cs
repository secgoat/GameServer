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

using GameServer.Screens;

namespace GameServer
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        //add the different screens you want here
        StartScreen startScreen;
        ActionScreen actionScreen;
        BaseGameScreen activeScreen;
        PopUpScreen popUpScreen;


        //textures for menu backgrounds etc.
        Texture2D popUpTexture;
        Texture2D actionScreentexture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("font");

            popUpTexture = Content.Load<Texture2D>("quitscreen");
            actionScreentexture = Content.Load<Texture2D>("greenmetal");

            //initialize and start the scrrens
            startScreen = new StartScreen(this, spriteBatch, spriteFont);
            Components.Add(startScreen);
            startScreen.Hide();

            actionScreen = new ActionScreen(this, spriteBatch, actionScreentexture);
            Components.Add(actionScreen);
            actionScreen.Hide();

            popUpScreen = new PopUpScreen(this, spriteBatch, spriteFont, popUpTexture);
            Components.Add(popUpScreen);
            popUpScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();


        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (activeScreen == startScreen)
            {
                HandleStartScreen();
            }

            else if (activeScreen == actionScreen)
            {
                HandleActionScreen();
            }
            else if (activeScreen == popUpScreen)
            {
                HandlePopUpScreen();
            }

            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }

        private bool CheckKey(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        private void HandleStartScreen()
        {
            if (CheckKey(Keys.Enter))
            {
                if (startScreen.SelectedIndex == 0)
                {
                    activeScreen.Hide();
                    activeScreen = actionScreen;
                    activeScreen.Show();
                }

                if (startScreen.SelectedIndex == 3)
                {
                    this.Exit();
                }
            }
        }
        private void HandleActionScreen()
        {
            if (CheckKey(Keys.F1))
            {
                //activeScreen.Hide();
                activeScreen.Enabled = false;
                activeScreen = popUpScreen;
                activeScreen.Show();
            }
        }
        private void HandlePopUpScreen()
        {
            if (CheckKey(Keys.Enter))
            {
                if (popUpScreen.SelectedIndex == 0)
                {
                    activeScreen.Hide();
                    actionScreen.Hide();
                    activeScreen = startScreen;
                    activeScreen.Show();
                }
                if (popUpScreen.SelectedIndex == 1)
                {
                    activeScreen.Hide();
                    activeScreen = actionScreen;
                    activeScreen.Show();
                }
            }
        }

    }
}

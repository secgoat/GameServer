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
using GameServer.Server;
using GameServer.Client;

using FuchsGUI;
using Lidgren.Network;
using System.Threading;

namespace GameServer
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont, formFont;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        //add the different screens you want here 
        StartScreen startScreen; //basic start menu / first screen they see
        NetworkGameSelectScreen networkScreen; //the screen / menu to choose to join or host a network game
        JoinNetworkGameScreen joinGameScreen;
        ActionScreen actionScreen; // right now this is the "Game" screen //TODO: change this to the Client
        BaseGameScreen activeScreen; //just a way to keep track of which screen is currently active
        PopUpScreen popUpScreen; // this is right now just a screen that says ar eyou sure you want to quit


        //textures for menu backgrounds etc.
        Texture2D popUpTexture;
        Texture2D actionScreentexture;
        Texture2D blankBlackTexture;

        //Network pieces
        
        Client.Client client;
        Server.Server server;
        //NetClient client;
        //NetServer server;
        Thread serverThread;

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
            formFont = Content.Load<SpriteFont>("FormFont");

            popUpTexture = Content.Load<Texture2D>("quitscreen");
            actionScreentexture = Content.Load<Texture2D>("greenmetal");
            blankBlackTexture = Content.Load<Texture2D>("black");
           
            //initialize and start the screens
            startScreen = new StartScreen(this, spriteBatch, spriteFont);
            Components.Add(startScreen);
            startScreen.Hide();

            networkScreen = new NetworkGameSelectScreen(this, spriteBatch,spriteFont, blankBlackTexture);
            Components.Add(networkScreen);
            networkScreen.Hide();

            joinGameScreen = new JoinNetworkGameScreen(this, spriteBatch, formFont, blankBlackTexture);
            Components.Add(joinGameScreen);
            joinGameScreen.ButtonClicked += new JoinNetworkGameScreen.ClickEvent(HandleJoinGameScreenButtons);
            joinGameScreen.Hide();
            
            actionScreen = new ActionScreen(this, spriteBatch, actionScreentexture);
            Components.Add(actionScreen);
            actionScreen.Hide();

            popUpScreen = new PopUpScreen(this, spriteBatch, spriteFont, popUpTexture);
            Components.Add(popUpScreen);
            popUpScreen.Hide();



            activeScreen = startScreen;
            activeScreen.Show();

            IsMouseVisible = true;


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

            else if (activeScreen == networkScreen)
            {
                HandleNetworkSelectScreen();
            }

            else if (activeScreen == joinGameScreen)
            {
                //not sure i need this as I am using delegates instead?
                //HandleJoinGameScreen();
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            client.Shutdown("bye");
            server.Shutdown("Bye");
            base.OnExiting(sender, args);
        }

        void StartLocalServer()
        {
            
            string configName = "LocalGame";
            server = new Server.Server(this, spriteBatch, configName);
            Components.Add(server);
            //server.Hide();
            StartClient(configName);
        }

        void StartClient(string configName)
        {
            client = new Client.Client(this, spriteBatch, configName);
            Components.Add(client);
            activeScreen.Hide();
            activeScreen = client;
            activeScreen.Show();
        }

        private bool CheckKey(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        private void HandleStartScreen()
        {
            if (CheckKey(Keys.Enter))
            {
                if (startScreen.SelectedIndex == 0) //start
                {
                    StartLocalServer();
                    //activeScreen.Hide();
                    //activeScreen = actionScreen;
                    //activeScreen.Show();
                }

                if (startScreen.SelectedIndex == 1) //Multiplayer game
                {
                    activeScreen.Hide();
                    activeScreen = networkScreen;
                    activeScreen.Show();
                }

                if (startScreen.SelectedIndex == 2) //quit
                {
                    this.Exit();
                }
            }
        }

        private void HandleNetworkSelectScreen()
        {
            if(CheckKey(Keys.Enter))
            {
                if (networkScreen.SelectedIndex == 0)
                {
                    //Load Host Screen
                    //for now just start the defualt server with defualt port
                    //TODO: allow users to specify port number and maybe password on the host a game page
                }
                if (networkScreen.SelectedIndex == 1)
                {
                    //load join screen
                    activeScreen.Hide();
                    activeScreen = joinGameScreen;
                    activeScreen.Show();
                }
                if (networkScreen.SelectedIndex == 2)
                {
                    //go back to startScreen
                    activeScreen.Hide();
                    activeScreen = startScreen;
                    activeScreen.Show();
                }
            }
        }

        private void HandleJoinGameScreenButtons(Control sender)
        {
            if (sender.Name == "ScanLan")
            {
                //TODO: do a network autodisovery
                StartClient("LocalGame");
                Console.WriteLine("SCAN LAN!");
            }
            if (sender.Name == "Connect")
            {
                //TODO: grab ip and port and try a discover known peers
                Console.WriteLine("Connect to {0}:{1}", joinGameScreen.Address, joinGameScreen.Port);
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

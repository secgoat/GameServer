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

using MonoGameServer.Screens;
using MonoGameServer.Server;
using MonoGameServer.Client;

using FuchsGUI;
using Lidgren.Network;
using System.Threading;

namespace MonoGameServer
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
        HostNetworkGameScreen hostGameScreen;
        ActionScreen actionScreen; // right now this is the "Game" screen //TODO: change this to the Client
        BaseGameScreen activeScreen; //just a way to keep track of which screen is currently active
        PopUpScreen popUpScreen; // this is right now just a screen that says ar eyou sure you want to quit


        //textures for menu backgrounds etc.
        Texture2D popUpTexture;
        Texture2D actionScreentexture;
        Texture2D blankBlackTexture;

        //Network pieces
        
        Client.Client client;
        Client.Client.GameType gameType; //use this enum to tell client if local, join lan or hosted game

        Server.Server server;
        string gameConfigName;

        public Game1()
        {
            try
            {
                graphics = new GraphicsDeviceManager(this);
            }
            catch { }
            Content.RootDirectory = "Content";
            gameConfigName = "GameServer";
        }


        protected override void Initialize()
        {
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

            hostGameScreen = new HostNetworkGameScreen(this, spriteBatch, spriteFont, blankBlackTexture);
            Components.Add(hostGameScreen);
            hostGameScreen.ButtonClicked += new HostNetworkGameScreen.ClickEvent(HandleHostGameScreenButtons);
            hostGameScreen.Hide();
            
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

            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
        
        protected override void OnExiting(object sender, EventArgs args)
        {
            if(client != null)
                client.Shutdown("bye");

            if(server != null)
                server.Shutdown("Bye");
            base.OnExiting(sender, args);
        }

        void StartServer(Client.Client.GameType gameType)
        {
            //TODO: allow users hosting network game to choose max number of players
            int maxConnections = 0;
            if (gameType == Client.Client.GameType.local)
                maxConnections = 1;
            else
                maxConnections = 10;

            server = new Server.Server(this, spriteBatch, gameConfigName, maxConnections);
            Components.Add(server);
            //server.Hide();
            StartClient();
        }

        void StartClient()
        {
            client = new Client.Client(this, spriteBatch, gameConfigName, this.gameType);
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
                    gameType = Client.Client.GameType.local;
                    StartServer(gameType);
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
                    gameType = Client.Client.GameType.hosted;
                    activeScreen.Hide();
                    activeScreen = hostGameScreen;
                    activeScreen.Show();
                    //Load Host Screen
                    //for now just start the defualt server with defualt port
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
                //TODO: Only start client if local game is found
                gameType = Client.Client.GameType.scanLan;
                StartClient();
                Console.WriteLine("SCAN LAN!");
            }
            if (sender.Name == "Connect") //Join a network game by address
            {
                gameType = Client.Client.GameType.hosted;
                //TODO: grab ip and port and try a discover known peers
                Console.WriteLine("Connect to {0}:{1}", joinGameScreen.Address, joinGameScreen.Port);
            }
        }

        private void HandleHostGameScreenButtons(Control sender)
        {
            if (sender.Name == "Start")
            {
                gameType = Client.Client.GameType.hosted;
                StartServer(gameType);
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

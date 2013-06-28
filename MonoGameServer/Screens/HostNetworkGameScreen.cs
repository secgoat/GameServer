using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FuchsGUI;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MonoGameServer.Screens
{
    class HostNetworkGameScreen : BaseGameScreen
    {

        Texture2D buttonTexture, textboxTexture, background, formBackground; //for gui components

        //Fuchs GUI components
        Form hostGameForm;
        Button startButton, backButton;
        Label maxConnectionsLabel, portLabel;
        TextBox textBoxMaxConnections, textBoxPort;

        Rectangle imageRectangle;


        //delegates to send events back to the main game1 window
        public delegate void ClickEvent(Control sender);
        public event ClickEvent ButtonClicked;

        public int MaxConnections { get; set; }
        public int Port { get; set; }
        

        public HostNetworkGameScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D background)
            : base(game, spriteBatch)
        {
            
            this.background = background;
            formBackground = game.Content.Load<Texture2D>("alienmetal");
            hostGameForm = new Form("Host", "Host a Game", new Rectangle(121, 200, 425, 200), formBackground, spriteFont, Color.White);
            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");
            textboxTexture = game.Content.Load<Texture2D>("textboxTexture");

            //figure out the width and heigh of the text on the buttons
            Vector2 startButtonSize;
            startButtonSize = spriteFont.MeasureString("Start Game");
            startButton = new Button("Start", "Start Game", new Rectangle(19, 160, (int)startButtonSize.X + 10, (int)startButtonSize.Y + 10), buttonTexture, spriteFont, Color.White);
            backButton = new Button("BackButton", @"Back", new Rectangle(19, 180, 95, 23), buttonTexture, spriteFont, Color.White);
            
            textBoxMaxConnections = new TextBox("MaxConnections", "10", 100, new Rectangle(19, 76, 60, 20), textboxTexture, spriteFont, Color.Black);
            maxConnectionsLabel = new Label("maxConnectionsLabel", @"Max Connections", new Vector2(19, 51), spriteFont, Color.White, 0, 0);
           
            textBoxPort = new TextBox("Port", "14242", 8, new Rectangle(19, 125, 60, 20), textboxTexture, spriteFont, Color.Black);
            portLabel = new Label("portLabel", @"Port", new Vector2(19, 100), spriteFont, Color.White, 0, 0);

            hostGameForm.AddControl(startButton);
            hostGameForm.AddControl(backButton);
            hostGameForm.AddControl(textBoxMaxConnections);
            hostGameForm.AddControl(maxConnectionsLabel);
            hostGameForm.AddControl(textBoxPort);
            hostGameForm.AddControl(portLabel);

            startButton.onClick += new EHandler(ButtonClick);
            backButton.onClick += new EHandler(ButtonClick);

            imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        

        }

       

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            hostGameForm.Update(mouseState, keyboardState);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(background, imageRectangle, Color.White);
            hostGameForm.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        void ButtonClick(Control sender)
        {
            if (ButtonClicked != null)
            {
                this.MaxConnections = Int32.Parse(textBoxMaxConnections.Text);
                this.Port = Int32.Parse(textBoxPort.Text);
                this.ButtonClicked(sender);
            }
        }

    }
}

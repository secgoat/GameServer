using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FuchsGUI;
using Microsoft.Xna.Framework.Input;

namespace MonoGameServer.Screens
{
    class HostNetworkGameScreen : BaseGameScreen
    {

        Texture2D buttonTexture, textboxTexture, background, formBackground; //for gui components

        //Fuchs GUI components
        Form hostGameForm;
        Button startButton;
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
            hostGameForm = new Form("Host", "Host a Game", new Rectangle(100, 100, 350, 350), formBackground, spriteFont, Color.White);

            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");
            textboxTexture = game.Content.Load<Texture2D>("textboxTexture");

            //figure out the width and heigh of the text on the buttons
            Vector2 startButtonSize;
            startButtonSize = spriteFont.MeasureString("Start Game");
            //lanButton = new Button("ScanLan", "Scan LAN", new Rectangle(32, 30, (int)lanButtonSize.X + 10, (int)lanButtonSize.Y + 10), buttonTexture, spriteFont, Color.White);
            startButton = new Button("Start", "Start Game", new Rectangle(32, 151, (int)startButtonSize.X + 10, (int)startButtonSize.Y + 10), buttonTexture, spriteFont, Color.White);
            textBoxMaxConnections = new TextBox("MaxConnections", "10", 100, new Rectangle(32, 110, 232, 20), textboxTexture, spriteFont, Color.Black);
            textBoxPort = new TextBox("Port", "14242", 8, new Rectangle(288, 110, 60, 20), textboxTexture, spriteFont, Color.Black);

            //hostGameForm.AddControl(lanButton);
            hostGameForm.AddControl(startButton);
            hostGameForm.AddControl(textBoxMaxConnections);
            hostGameForm.AddControl(textBoxPort);

            //lanButton.onClick += new EHandler(ButtonClick);
            startButton.onClick += new EHandler(ButtonClick);


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
            //lanButton.Draw(spriteBatch);
            //sendButton.Draw(spriteBatch);
            //textBoxIP.Draw(spriteBatch);
            //textBoxPort.Draw(spriteBatch);

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

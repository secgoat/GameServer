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
    class JoinNetworkGameScreen : BaseGameScreen
    {
        Texture2D buttonTexture, textboxTexture, background, formBackground; //for gui components

        //Fuchs GUI components
        Form connectionMethodForm;
        Button sendButton, lanButton;
        TextBox textBoxIP, textBoxPort;

        Rectangle imageRectangle;


        //delegates to send events back to the main game1 window
        public delegate void ClickEvent(Control sender);
        public event ClickEvent ButtonClicked;

        public string Address { get; set; }
        public string Port { get; set; }
        

        public JoinNetworkGameScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D background)
            : base(game, spriteBatch)
        {
            this.background = background;
            formBackground = game.Content.Load<Texture2D>("alienmetal");
            Vector2 formSize = new Vector2(350, 350);
            Vector2 center = new Vector2((this.windowSize.Width - formSize.X) / 2, (this.windowSize.Height - formSize.Y) / 2);
            connectionMethodForm = new Form("Connect", "Connection Metod", new Rectangle((int)center.X, (int)center.Y,(int) formSize.X, (int)formSize.Y), formBackground, spriteFont, Color.White);
            
            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");
            textboxTexture = game.Content.Load<Texture2D>("textboxTexture");

            //figure out the width and heigh of the text on the buttons
            Vector2 lanButtonSize, connectButtonSize;
            lanButtonSize = spriteFont.MeasureString("Scan Lan");
            connectButtonSize = spriteFont.MeasureString("Connect");
            lanButton = new Button("ScanLan", "Scan LAN", new Rectangle(32,30,(int)lanButtonSize.X + 10, (int)lanButtonSize.Y + 10), buttonTexture, spriteFont, Color.White);
            sendButton = new Button("Connect", "Connect", new Rectangle(32,151,(int)connectButtonSize.X + 10, (int)connectButtonSize.Y + 10), buttonTexture, spriteFont, Color.White);
            textBoxIP = new TextBox("Address", "",100,new Rectangle(32,110,232,20), textboxTexture, spriteFont, Color.Black);
            textBoxPort = new TextBox("Port", "", 8, new Rectangle(288,110,60,20), textboxTexture, spriteFont, Color.Black);

            connectionMethodForm.AddControl(lanButton);
            connectionMethodForm.AddControl(sendButton);
            connectionMethodForm.AddControl(textBoxIP);
            connectionMethodForm.AddControl(textBoxPort);

            lanButton.onClick += new EHandler(ButtonClick);
            sendButton.onClick += new EHandler(ButtonClick);

            
            imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            connectionMethodForm.Update(mouseState, keyboardState);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(background, imageRectangle, Color.White);
            connectionMethodForm.Draw(spriteBatch);
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
                this.Address = textBoxIP.Text;
                this.Port = textBoxPort.Text;
                this.ButtonClicked(sender);
                
            }
        }


    }
}

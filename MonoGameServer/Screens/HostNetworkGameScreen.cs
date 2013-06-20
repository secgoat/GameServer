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

    public sealed class HostGameForm : Form
    {
        ContentManager contentManager;

        #region Fields

        public TextBox PortBox;
        public TextBox MaxConnectionsBox;
        public Button connectButton;
        public Label portLabel;
        public Label addressLabel;

        #endregion

        public HostGameForm(Texture2D texture, SpriteFont font, Color foreColor, ContentManager contentManager)
            : base("HostGameControls", "Host A Game", new Rectangle(121, 200, 425, 200), texture, font, foreColor)
        {
            this.contentManager = contentManager;
            this.AddControls();
        }

        void AddControls()
        {
            Texture2D texTextBox = contentManager.Load<Texture2D>(@"textboxTexture");
            Texture2D texButton = contentManager.Load<Texture2D>(@"buttonTexture");


            PortBox = new TextBox("Port", @"", 32767, "14242", new Rectangle(19, 125, 60, 20), texTextBox, font, new Color(0, 0, 0));
            this.AddControl(PortBox);

            MaxConnectionsBox = new TextBox("MaxConnections", @"", 32767, "10", new Rectangle(19, 76, 60, 20), texTextBox, font, new Color(0, 0, 0));
            this.AddControl(MaxConnectionsBox);

            connectButton = new Button("connectButton", @"Start Game", new Rectangle(19, 160, 75, 23), texButton, font, new Color(0, 0, 0));
            this.AddControl(connectButton);
           

            portLabel = new Label("portLabel", @"Port", new Vector2(16, 109), font, new Color(0, 0, 0), 0, 0);
            this.AddControl(portLabel);

            addressLabel = new Label("addressLabel", @"Max Connections", new Vector2(16, 60), font, new Color(0, 0, 0), 0, 0);
            this.AddControl(addressLabel);


        }
    }

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
            hostGameForm = new Form("Host", "Host a Game", new Rectangle(121, 200, 425, 200), formBackground, spriteFont, Color.White);
            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");
            textboxTexture = game.Content.Load<Texture2D>("textboxTexture");

            //figure out the width and heigh of the text on the buttons
            Vector2 startButtonSize;
            startButtonSize = spriteFont.MeasureString("Start Game");
            startButton = new Button("Start", "Start Game", new Rectangle(19, 160, (int)startButtonSize.X + 10, (int)startButtonSize.Y + 10), buttonTexture, spriteFont, Color.White);
            textBoxMaxConnections = new TextBox("MaxConnections", "10", 100, new Rectangle(19, 76, 60, 20), textboxTexture, spriteFont, Color.Black);
            textBoxPort = new TextBox("Port", "14242", 8, new Rectangle(19, 125, 60, 20), textboxTexture, spriteFont, Color.Black);

            hostGameForm.AddControl(startButton);
            hostGameForm.AddControl(textBoxMaxConnections);
            hostGameForm.AddControl(textBoxPort);

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

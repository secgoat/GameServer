using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FuchsGUI;

namespace GameServer.Screens
{
    class JoinNetworkGameScreen : BaseGameScreen
    {
        MenuComponent menuComponent;
        Texture2D buttonTexture, textboxTexture, background; //for gui components

        //Fuchs GUI components
        Button sendButton, lanButton;
        TextBox textBoxIP, textBoxPort;

        Rectangle imageRectangle;

        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }

        public JoinNetworkGameScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D background)
            : base(game, spriteBatch)
        {
            this.background = background;

            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");
            textboxTexture = game.Content.Load<Texture2D>("textboxTexture");

            lanButton = new Button("ScanLan", "Scan LAN", new Rectangle(32,30,75,23), buttonTexture, spriteFont, Color.White);
            sendButton = new Button("Connect", "Connect", new Rectangle(32,151,75,23), buttonTexture, spriteFont, Color.White);
            textBoxIP = new TextBox("Address", "",100,new Rectangle(32,110,232,20), textboxTexture, spriteFont, Color.Black);
            textBoxPort = new TextBox("Port", "", 8, new Rectangle(288,110,60,20), textboxTexture, spriteFont, Color.Black);
            string[] menuItems = { "Start Game","Network Game", "Exit"};
            menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
            Components.Add(menuComponent);
            imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(background, imageRectangle, Color.White);
            lanButton.Draw(spriteBatch);
            sendButton.Draw(spriteBatch);
            textBoxIP.Draw(spriteBatch);
            textBoxPort.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}

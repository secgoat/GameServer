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
    public class NetworkGameSelectScreen : BaseGameScreen
    {
       
        //delegates to control button presses
        public delegate void ClickEvent(Control sender);
        public event ClickEvent ButtonClicked;

        public Form networkGameTypeSelectForm;
        public Button hostGameButton, joinGameButton, backButton;
        Texture2D formBackground, buttonTexture;

        public NetworkGameSelectScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D backgorund)
            : base(game, spriteBatch)
        {
            Vector2 formSize = new Vector2(150, 200);
            Vector2 center = new Vector2((this.windowSize.Width - formSize.X) / 2, (this.windowSize.Height - formSize.Y) / 2);
            formBackground = game.Content.Load<Texture2D>("alienmetal");
            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");

            networkGameTypeSelectForm = new Form("GameTypeMenu", "Network Game", new Rectangle((int)center.X, (int)center.Y, (int)formSize.X, (int)formSize.Y), formBackground, spriteFont, Color.White);
            backButton = new Button("BackButton", @"Back", new Rectangle(27, 132, 95, 23), buttonTexture, spriteFont, Color.White);
            joinGameButton = new Button("JoinGame", @"Join Game", new Rectangle(27, 90, 95, 23), buttonTexture, spriteFont, Color.White);
            hostGameButton = new Button("HostGame", @"Host Game", new Rectangle(27, 42, 95, 23), buttonTexture, spriteFont, Color.White);
           

            networkGameTypeSelectForm.AddControl(hostGameButton);
            networkGameTypeSelectForm.AddControl(joinGameButton);
            networkGameTypeSelectForm.AddControl(backButton);

            hostGameButton.onClick  += new EHandler(ButtonClick);
            joinGameButton.onClick  += new EHandler(ButtonClick);
            backButton.onClick      += new EHandler(ButtonClick);
            
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            networkGameTypeSelectForm.Update(mouseState, keyboardState);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            networkGameTypeSelectForm.Draw(this.spriteBatch);
            base.Draw(gameTime);
        }

        void ButtonClick(Control sender)
        {
            if (ButtonClicked != null)
                this.ButtonClicked(sender);
        }
    }
}

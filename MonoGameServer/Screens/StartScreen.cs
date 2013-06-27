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
    public class StartScreen : BaseGameScreen
    {
      
        //delegates to control button presses
        public delegate void ClickEvent(Control sender);
        public event ClickEvent ButtonClicked;

        //main form components
        public Form mainMenuForm;
        public Button quitGameButton, networkGameButton, startGameButton;
        Texture2D formBackground;
        Texture2D buttonTexture;



        public StartScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont)
            : base(game, spriteBatch)
        {

            //screenSize = game.GraphicsDevice.Viewport.Bounds;
            //TAKE THE WIDTH AND HEIGHT MINUS THE MAIN FORM WIDTH AND HEIGHT DIVEDED BY 2 TO GET CENTER ON X AND Y
            Vector2 formSize = new Vector2(150, 200);
            Vector2 center = new Vector2((this.windowSize.Width - formSize.X) / 2, (this.windowSize.Height - formSize.Y) / 2);
            
            formBackground = game.Content.Load<Texture2D>("alienmetal");
            buttonTexture = game.Content.Load<Texture2D>("buttonTexture");
            
            mainMenuForm = new Form("MainMenu", "Main Menu", new Rectangle((int)center.X, (int)center.Y, (int)formSize.X, (int)formSize.Y), formBackground, spriteFont, Color.White);
            quitGameButton = new Button("QuitGame", @"Quit Game", new Rectangle(27, 132, 95, 23), buttonTexture, spriteFont, Color.White);
            networkGameButton = new Button("NetworkGame", @"Network Game",new Rectangle(27, 90, 95, 23), buttonTexture, spriteFont, Color.White);
            startGameButton = new Button("StartGame", @"Start Game", new Rectangle(27, 42, 95, 23), buttonTexture, spriteFont, Color.White);

            mainMenuForm.AddControl(startGameButton);
            mainMenuForm.AddControl(networkGameButton);
            mainMenuForm.AddControl(quitGameButton);

            startGameButton.onClick += new EHandler(ButtonClick);
            networkGameButton.onClick += new EHandler(ButtonClick);
            quitGameButton.onClick += new EHandler(ButtonClick);

        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyoardState = Keyboard.GetState();
            mainMenuForm.Update(mouseState, keyoardState);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Draw(image, imageRectangle, Color.White);
            mainMenuForm.Draw(this.spriteBatch);
            base.Draw(gameTime);
        }

        void ButtonClick(Control sender)
        {
            if (ButtonClicked != null)
                this.ButtonClicked(sender);
            //what do here?
        }
    }
}

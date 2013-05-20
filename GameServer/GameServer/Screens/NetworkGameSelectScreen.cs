using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameServer.Screens
{
    public class NetworkGameSelectScreen : BaseGameScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;

        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }

        public NetworkGameSelectScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D backgorund)
            : base(game, spriteBatch)
        {
            image = backgorund;
            string[] menuItems = { "Host Game", "Join Game", "Back" };

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
            spriteBatch.Draw(image, imageRectangle, Color.White);
            base.Draw(gameTime);
        }
    }
}

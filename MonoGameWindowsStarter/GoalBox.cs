using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public class GoalBox : IBoundable
    {
        const int BOX_SIZE = 50;

        public int X, Y;
        Texture2D texture;
        public BoundingRectangle Bounds => new BoundingRectangle(X, Y, BOX_SIZE, BOX_SIZE);

        public GoalBox()
        {
        }

        public void LoadContent(ContentManager Content)
        {
            this.texture = Content.Load<Texture2D>("pixel");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(X, Y, BOX_SIZE, BOX_SIZE), Color.Firebrick);
        }
    }
}

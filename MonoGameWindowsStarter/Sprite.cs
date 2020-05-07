using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameWindowsStarter
{
    public class Sprite
    {
        private Rectangle source;
        private Texture2D texture;

        public int Width => source.Width;
        public int Height => source.Height;

        public Sprite(Rectangle source, Texture2D texture)
        {
            this.texture = texture;
            this.source = source;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, source, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, source, color);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, source, color);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, source, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
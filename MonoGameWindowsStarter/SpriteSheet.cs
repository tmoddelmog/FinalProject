using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public class SpriteSheet
    {
        Texture2D sheet;
        Sprite[] sprites;
        public int Count => sprites.Length;

        public SpriteSheet(Texture2D texture, int width, int height, int offset = 0, int gutter = 0)
        {
            sheet = texture;
            var columns = (texture.Width - offset) / (width + gutter);
            var rows = (texture.Height - offset) / (height + gutter);
            sprites = new Sprite[rows * columns];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    sprites[y * columns + x] = new Sprite(new Rectangle(
                        x * (width + gutter) + offset,
                        y * (height + gutter) + offset,
                        width,
                        height
                        ), texture);
                }
            }
        }

        public Sprite this[int index]
        {
            get => sprites[index];
        }
    }
}

using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public class BoundingRectangle
    {
        public float X, Y, Width, Height;

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            this.X = position.X;
            this.Y = position.Y;
            this.Width = width;
            this.Height = height;
        }

        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return !(this.X > other.X + other.Width
                  || this.X + this.Width < other.X
                  || this.Y > other.Y + other.Height
                  || this.Y + this.Height < other.Y);
        }


        public static implicit operator Rectangle(BoundingRectangle br)
        {
            return new Rectangle(
                (int)br.X,
                (int)br.Y,
                (int)br.Width,
                (int)br.Height);
        }
    }
}
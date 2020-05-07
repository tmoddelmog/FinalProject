using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    enum PlayerAnimState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    enum VerticalState
    {
        Ground,
        Jumping,
        Falling
    }

    class Player : IBoundable
    {
        Game1 game;
        const int FRAME_RATE = 300,
                  JUMP_TIME = 800;
        public int currentFrame = 0,
                   speed = 3;
        TimeSpan jumpTimer,
                 animationTimer;
        PlayerAnimState animationState = PlayerAnimState.Idle;
        VerticalState verticalState = VerticalState.Ground;
        SpriteEffects spriteEffects = SpriteEffects.None;
        Color color = Color.White;
        Vector2 origin = new Vector2(15, 31);
        public Vector2 Position = new Vector2(40, 450);
        public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.2f * origin, 40, 40);
        Sprite[] frames;
        public int JumpHeight = 250;

        public Player(Game1 game, IEnumerable<Sprite> frames)
        {
            this.game = game;
            this.frames = frames.ToArray();
            this.animationState = PlayerAnimState.WalkingRight;
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var viewport = game.GraphicsDevice.Viewport;

            if (Position.X > 960) Position.X = 960;

            // Vertical movement
            switch (verticalState)
            {
                case VerticalState.Ground:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        verticalState = VerticalState.Jumping;
                        jumpTimer = new TimeSpan(0);
                    }
                    break;
                case VerticalState.Jumping:
                    jumpTimer += gameTime.ElapsedGameTime;
                    // Simple jumping with platformer physics
                    Position.Y -= (JumpHeight / (float)jumpTimer.TotalMilliseconds);
                    if (jumpTimer.TotalMilliseconds >= JUMP_TIME) verticalState = VerticalState.Falling;
                    break;
                case VerticalState.Falling:
                    Position.Y += speed;
                    if (Position.Y > 450)
                    {
                        Position.Y = 450;
                        verticalState = VerticalState.Ground;
                    }
                    break;
            }

            // Horizontal movement
            if (keyboard.IsKeyDown(Keys.Left))
            {
                if (verticalState == VerticalState.Jumping || verticalState == VerticalState.Falling)
                    animationState = PlayerAnimState.JumpingLeft;
                else animationState = PlayerAnimState.WalkingLeft;
                Position.X -= speed;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                if (verticalState == VerticalState.Jumping || verticalState == VerticalState.Falling)
                    animationState = PlayerAnimState.JumpingRight;
                else animationState = PlayerAnimState.WalkingRight;
                Position.X += speed;
            }
            else
            {
                animationState = PlayerAnimState.Idle;
            }

            // Apply animations
            switch (animationState)
            {
                case PlayerAnimState.Idle:
                    currentFrame = 0;
                    animationTimer = new TimeSpan(0);
                    break;

                case PlayerAnimState.JumpingLeft:
                    spriteEffects = SpriteEffects.None;
                    currentFrame = 7;
                    break;

                case PlayerAnimState.JumpingRight:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    currentFrame = 7;
                    break;

                case PlayerAnimState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 9 & 10
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

                case PlayerAnimState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 9 & 10
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;
            }

            Debug.WriteLine($"X {Position.X}");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }
    }
}

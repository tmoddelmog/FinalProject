using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace MonoGameWindowsStarter
{
    enum Level { level1, level2, level3, hiddenlevel }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int SCREEN_WIDTH = 960, SCREEN_HEIGHT = 540;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level levelState = Level.level1;
        int count = 0;
        Texture2D gameover;

        // player
        SpriteSheet sheet;
        Player player;

        // goal boxes
        GoalBox goalBox = new GoalBox();
        Point[] boxPoints = new Point[3];
        GoalBox hiddenBox = new GoalBox();

        // background
        Texture2D[] background = new Texture2D[4];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gameover = Content.Load<Texture2D>("game over");

            // player
            var texture = Content.Load<Texture2D>("CavemanSpriteSheet");
            sheet = new SpriteSheet(texture, 25, 32);
            var playerFrames = from index in Enumerable.Range(0, 16) select sheet[index];
            player = new Player(this, playerFrames);

            // goal boxes
            UpdateBoxes(boxPoints);
            goalBox.LoadContent(Content);
            hiddenBox.LoadContent(Content);
            hiddenBox.X = -1000;
            hiddenBox.Y = 450;

            // background
            background[0] = Content.Load<Texture2D>("forest");
            background[1] = Content.Load<Texture2D>("snow");
            background[2] = Content.Load<Texture2D>("city");
            background[3] = Content.Load<Texture2D>("pixel");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);

            
            var boxPointIndex = -99;

            switch (levelState)
            {
                case Level.level1:
                    boxPointIndex = 0;
                    player.JumpHeight = 0;
                    if (player.Bounds.CollidesWith(goalBox.Bounds))
                    {
                        player.Position.X = 935;
                        player.Position.Y = 450;
                        levelState = Level.level2;
                        count++;
                    }
                    if (player.Bounds.CollidesWith(hiddenBox.Bounds))
                    {
                        player.Position.X = 40;
                        player.Position.Y = 450;
                        levelState = Level.hiddenlevel;
                        count += 11;
                    }
                    break;

                case Level.level2:
                    boxPointIndex = 1;
                    player.JumpHeight = 1500;
                    if (player.Bounds.CollidesWith(goalBox.Bounds))
                    {
                        player.Position.X = 40;
                        player.Position.Y = 450;
                        levelState = Level.level3;
                        count++;
                    }
                    break;

                case Level.level3:
                    boxPointIndex = 2;
                    player.JumpHeight = 1500;
                    if (player.Bounds.CollidesWith(goalBox.Bounds))
                    {
                        player.Position.X = 40;
                        player.Position.Y = 450;
                        UpdateBoxes(boxPoints);
                        levelState = Level.level1;
                        count++;
                    }
                    break;

                case Level.hiddenlevel:
                    boxPointIndex = 1;
                    player.JumpHeight = 1500;
                    if (player.Bounds.CollidesWith(goalBox.Bounds))
                    {
                        count++;
                    }
                    break;

                default: break;
            }

            goalBox.X = boxPoints[boxPointIndex].X;
            goalBox.Y = boxPoints[boxPointIndex].Y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            var backgroundIndex = -99;

            switch (levelState)
            {
                case Level.level1:
                    backgroundIndex = 0;
                    break;

                case Level.level2:
                    backgroundIndex = 1;
                    break;

                case Level.level3:
                    backgroundIndex = 2;
                    break;

                case Level.hiddenlevel:
                    backgroundIndex = 3;
                    break;

                default:
                    backgroundIndex = 0;
                    break;
            }

            // background
            spriteBatch.Draw(background[backgroundIndex], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

            // player
            player.Draw(spriteBatch);

            // goal box
            goalBox.Draw(spriteBatch);

            // game over
            if (count > 11)
            {
                player.Position.X = 40;
                player.Position.Y = 450;
                player.JumpHeight = 0;
                player.speed = 0;
                spriteBatch.Draw(gameover, new Vector2((SCREEN_WIDTH-307) / 2 , (SCREEN_HEIGHT-79) / 2), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void UpdateBoxes(Point[] points)
        {
            var rando = new Random();
            points[0] = new Point(rando.Next(0, 920), 400);
            points[1] = new Point(rando.Next(0, 920), rando.Next(10, 400));
            points[2] = new Point(rando.Next(0, 920), rando.Next(10, 400));
        }
    }
}

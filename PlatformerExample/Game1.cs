using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace PlatformerExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        private bool flagReached = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        Player player;
        private SpriteFont font;
        List<Platform> platforms;
        AxisList world;
        Texture2D flag;
        BoundingRectangle recFlag;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            platforms = new List<Platform>();
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
            recFlag.Width = 50;
            recFlag.Height = 50;
            recFlag.X = 350;
            recFlag.Y = 30;

               
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if VISUAL_DEBUG
            VisualDebugging.LoadContent(Content);
#endif
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("DefaultFont");
            // TODO: use this.Content to load your game content here
            var t = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(t, 21, 21, 3, 2);

            // Create the player with the corresponding frames from the spritesheet
            var playerFrames = from index in Enumerable.Range(19, 30) select sheet[index];
            player = new Player(playerFrames);
            flag = Content.Load<Texture2D>("flag");
            // Create the platforms
            platforms.Add(new Platform(new BoundingRectangle(0, 450, 280, 30), sheet[3]));
            platforms.Add(new Platform(new BoundingRectangle(1, 150, 105, 30), sheet[3]));
            platforms.Add(new Platform(new BoundingRectangle(80, 300, 105, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(160, 200, 42, 21), sheet[3]));
            platforms.Add(new Platform(new BoundingRectangle(180, 75, 280, 30), sheet[3]));
            platforms.Add(new Platform(new BoundingRectangle(279, 220, 105, 30), sheet[3]));
            platforms.Add(new Platform(new BoundingRectangle(281, 460, 1000, 30), sheet[3]));
            platforms.Add(new Platform(new BoundingRectangle(400, 400, 84, 21), sheet[2]));

            platforms.Add(new Platform(new BoundingRectangle(484, 340, 84, 21), sheet[2]));
            platforms.Add(new Platform(new BoundingRectangle(550, 250, 84, 21), sheet[2]));
            platforms.Add(new Platform(new BoundingRectangle(640, 300, 84, 21), sheet[2]));



            // Add the platforms to the axis list
            world = new AxisList();
            foreach (Platform platform in platforms)
            {
                world.AddGameObject(platform);
            }
            Debug.WriteLine(world.count);
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

            // Check for platform collisions
            var platformQuery = world.QueryRange(player.Bounds.X, player.Bounds.X + player.Bounds.Width);
            player.CheckForPlatformCollision(platformQuery);
            if (player.Bounds.CollidesWith(recFlag))
            {
                flagReached = true;
            }
                
                
            
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
            if (flagReached)
            {
                spriteBatch.DrawString(font, "You Win", new Vector2(20, 20), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Reach the flag!", new Vector2(20, 20), Color.White);
            }
            // Draw the platforms 
            platforms.ForEach(platform =>
            {
                platform.Draw(spriteBatch);
            });

            // Draw the player
            player.Draw(spriteBatch);
            
            // Draw an arbitrary range of sprites
            for(var i = 17; i < 30; i++)
            {
                sheet[i].Draw(spriteBatch, new Vector2(i*25, 25), Color.White);
            }

            spriteBatch.Draw(flag, recFlag, Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

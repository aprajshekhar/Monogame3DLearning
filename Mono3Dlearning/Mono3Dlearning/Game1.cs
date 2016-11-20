using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mono3Dlearning
{  
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        Model alienModel;

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
            alienModel = Content.Load<Model>("robot");
            // TODO: use this.Content to load your game content here
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (var mesh in alienModel.Meshes)
            {
                foreach (BasicEffect bEffect in mesh.Effects)
                {

                    bEffect.EnableDefaultLighting();
                    bEffect.PreferPerPixelLighting = true;
                    bEffect.World = Matrix.Identity;

                    var cameraPosition = new Vector3(0, 8, 0);
                    var cameraUpPosition = Vector3.UnitZ;
                    var cameraLookAt = Vector3.Zero;

                    bEffect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUpPosition);

                    var cameraNear = 1;
                    var cameraFar = 200;

                    float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
                    float fieldOfView = MathHelper.PiOver4;
                    bEffect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, cameraNear, cameraFar);
                }
                mesh.Draw();
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

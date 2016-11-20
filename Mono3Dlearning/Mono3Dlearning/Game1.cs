using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mono3Dlearning
{  
    public class Game1 : Game
    {

        //FOr controlling the width/Height and other graphics related topic.
        GraphicsDeviceManager graphics;

        // For rendering any sprites.
        SpriteBatch spriteBatch;

        //Created an instance of Model to load 3D model in game.
        Model alienModel;

        //Creating a vertex Array for creating floor.
        VertexPositionTexture[] vPTexture;

        //Creating BasicEffect instance which will hold parameters for rendering such as position and lighting
        BasicEffect effects;

        //The Tiles Texture
        Texture2D checkerboardTexture;
        Vector3 cameraPosition = new Vector3(0, 40, 20);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {            
            vPTexture = new VertexPositionTexture[6];
            vPTexture[0].Position = new Vector3(-20, -20, 0);
            vPTexture[1].Position = new Vector3(-20, 20, 0);
            vPTexture[2].Position = new Vector3(20, -20, 0);
            vPTexture[3].Position = vPTexture[1].Position;
            vPTexture[4].Position = new Vector3(20, 20, 0);
            vPTexture[5].Position = vPTexture[2].Position;
          
            int repetitions = 10;

            vPTexture[0].TextureCoordinate = new Vector2(0, 0);
            vPTexture[1].TextureCoordinate = new Vector2(0, repetitions);
            vPTexture[2].TextureCoordinate = new Vector2(repetitions, 0);

            vPTexture[3].TextureCoordinate = vPTexture[1].TextureCoordinate;
            vPTexture[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            vPTexture[5].TextureCoordinate = vPTexture[2].TextureCoordinate;
          
            effects = new BasicEffect(graphics.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            alienModel = Content.Load<Model>("robot");
            using (var stream = TitleContainer.OpenStream("Content/checkerboard.png"))
            {
                checkerboardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Clearing Color in the game after each update of draw to remove unwanted color left in the screen.
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawGround();
            DrawAlienModel(new Vector3(0, 0,5));
            DrawAlienModel(new Vector3(0, -10, 5));
            DrawAlienModel(new Vector3(0, 10, 5));
            DrawAlienModel(new Vector3(-10, 0, 5));
            DrawAlienModel(new Vector3(10, 0, 5));

            base.Draw(gameTime);
        }
        //Drawing Ground
        void DrawGround()
        {
            var cameraLookAt = Vector3.Zero;
            var cameraUp = Vector3.UnitZ;

            effects.View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUp);

            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;
            effects.Projection =  Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            effects.TextureEnabled = true;
            effects.Texture = checkerboardTexture;

            foreach(var pass in effects.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
            // The array of verts that we want to render
            vPTexture,
            // The offset, which is 0 since we want to start 
            // at the beginning of the floorVerts array
            0,
            // The number of triangles to draw
            2);
            }
        }

        //Drawing alien model
        void DrawAlienModel(Vector3 position)
        {
            foreach (var mesh in alienModel.Meshes)
            {
                foreach (BasicEffect bEffect in mesh.Effects)
                {

                    bEffect.EnableDefaultLighting();
                    bEffect.PreferPerPixelLighting = true;
                    bEffect.World = Matrix.CreateTranslation(position);

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
        }
    }
}

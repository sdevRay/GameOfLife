using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameOfLife
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public const int CellSize = 10; // cell pixel width/height
		public const int CellsX = 100;
		public const int CellsY = 50;

		public static bool Paused = true;

		public static Texture2D Pixel;
		public static SpriteFont Font;

		public static Vector2 ScreenSize;

		public Grid grid;

		public KeyboardState keyboardState, lastKeyboardState;
		public const int UPS = 20; // updates per second
		public const int FPS = 60;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);

			ScreenSize = new Vector2(CellsX, CellsY) * CellSize;

			graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
			graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;

			IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			grid = new Grid();

			keyboardState = lastKeyboardState = Keyboard.GetState(); // this is a neat way to inialize mutliple variables with the same thing
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Font = Content.Load<SpriteFont>("Font");

			Pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
			Pixel.SetData(new Color[] { Color.White });
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
			keyboardState = Keyboard.GetState();

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
				Paused = !Paused;

			if (keyboardState.IsKeyDown(Keys.Back) && lastKeyboardState.IsKeyUp(Keys.Back))
				grid.Clear();

			// TODO: Add your update logic here
			base.Update(gameTime);

			grid.Update(gameTime);

			lastKeyboardState = keyboardState;
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			if(Paused)
				GraphicsDevice.Clear(Color.Red);
			else
				GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();

			if (Paused)
			{
				string paused = "Paused";
				spriteBatch.DrawString(Font, paused, ScreenSize / 2, Color.Gray, 0f, Font.MeasureString(paused) / 2, 1f, SpriteEffects.None, 0f);
			}
			grid.Draw(spriteBatch);
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}

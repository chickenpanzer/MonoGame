using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using System.Collections.Generic;

namespace MonoGameTest
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<SpriteBase> sprites = new List<SpriteBase>();

		SpriteBase sb = null;
		AmobeaSprite ams = null;

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

			Constants.ScreenHeight =  graphics.PreferredBackBufferHeight;
			Constants.ScreenWidth = graphics.PreferredBackBufferWidth;

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
			var texture = this.Content.Load<Texture2D>("Icon");
			sb = new SpriteBase(texture, Vector2.Zero, new KeyboardMover(5f), 10f, Color.White, 1f);
			sb.TextureScale = 0.2f;

			ams = new AmobeaSprite(texture, new Vector2(100f,100f), new RandomMover(1f,10), 5f, Color.Blue, 0.5f);
			ams.TextureScale = 0.5f;

			sprites.Add(sb);
			sprites.Add(ams);

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
			foreach (var sprite in sprites.ToArray())
			{
				sprite.Update(gameTime, sprites);
			}

			//Remove dead sprites from List
			sprites.RemoveAll(s => !s.IsAlive);

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
			spriteBatch.Begin(SpriteSortMode.FrontToBack);

			foreach (var sprite in sprites)
			{
				sprite.Draw(gameTime, spriteBatch);
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

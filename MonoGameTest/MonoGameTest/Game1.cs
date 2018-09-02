using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Windows;

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

		Level level = null;
		Player player = null;
		SpriteFont font = null;

		#region Penumbra
		// Store reference to lighting system.
		PenumbraComponent penumbra;

		// Create sample light source and shadow hull.
		Light light = new PointLight
		{
			Scale = new Vector2(300f), // Range of the light source (how far the light will travel)
			ShadowType = ShadowType.Solid, // Will not lit hulls themselves
			Color = Color.MonoGameOrange
		};
		Hull hull = new Hull(new Vector2(1.0f), new Vector2(-1.0f, 1.0f), new Vector2(-1.0f), new Vector2(1.0f, -1.0f))
		{
			Position = new Vector2(200f, 240f),
			Scale = new Vector2(10f)
		};
		#endregion


		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 320;
			graphics.PreferredBackBufferHeight = 320;

			Content.RootDirectory = "Content";

			//Penumbra
			// Create the lighting system and add sample light and hull.
			penumbra = new PenumbraComponent(this);
			penumbra.Lights.Add(light);
			//penumbra.Hulls.Add(hull);
			penumbra.Hulls.Add(new Hull(new Vector2(64f, 64f), new Vector2(96f, 64f), new Vector2(96f, 128f), new Vector2(64f, 128f)));
			//Penumbra

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

			Constants.ScreenHeight =  graphics.PreferredBackBufferHeight -32;
			Constants.ScreenWidth = graphics.PreferredBackBufferWidth -32;

			// Initialize the lighting system.
			penumbra.Initialize();

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

			font = this.Content.Load<SpriteFont>("BasicFont");

			player = new Player(this.Content.Load<Texture2D>("dwarf"), new Vector2(32f,32f), new KeyboardMover(32), 32);
			player.Layer = 1f;

			level = new Level(this);
			level.LoadLevel("LevelTest.xml");
			level.Begin(player);	

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


			//Penumbra
			// Animate light position and hull rotation.
			light.Position = player.Position;
				//new Vector2(400f, 240f) + // Offset origin
				//new Vector2( // Position around origin
				//	(float)Math.Cos(gameTime.TotalGameTime.TotalSeconds),
				//	(float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 240f;
			//hull.Rotation = MathHelper.WrapAngle(-(float)gameTime.TotalGameTime.TotalSeconds);





			// TODO: Add your update logic here
			if (player.Health > 0)
			{
				level.Update(gameTime, sprites);
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

			// Everything between penumbra.BeginDraw and penumbra.Draw will be
			// lit by the lighting system.
			penumbra.BeginDraw();

			spriteBatch.Begin(SpriteSortMode.FrontToBack);
			level.Draw(gameTime, spriteBatch);

			string score = string.Format("Score : {0} / Heath : {1} / Def : {2}", player.Score, player.Health, player.Defense);

			spriteBatch.End();

			penumbra.Draw(gameTime);

			spriteBatch.Begin();

			spriteBatch.DrawString(font, score, Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

			if (player.Health <= 0)
			{
				spriteBatch.DrawString(font, "GAME OVER", new Vector2((Constants.ScreenWidth / 2) - 32, Constants.ScreenHeight / 2), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
			}

			spriteBatch.End();



			base.Draw(gameTime);
		}
	}
}

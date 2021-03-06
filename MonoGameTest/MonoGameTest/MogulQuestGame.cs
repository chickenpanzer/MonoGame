﻿using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MogulQuest
{

	enum GameState
	{
		GameRunning,
		ChangingLevel
	}

	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	/// 
	public class MogulQuestGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<SpriteBase> sprites = new List<SpriteBase>();

		Level level = null;
		Player player = null;
		SpriteFont font = null;

		private GameState _gameState = GameState.GameRunning;

		private Camera _camera;
		private Transition _transition;

		#region Penumbra
		// Store reference to lighting system.
		PenumbraComponent penumbra;
		public PenumbraComponent Penumbra { get => penumbra; }

		// Create sample light source and shadow hull.
		Light light = new PointLight
		{
			Scale = new Vector2(400f), // Range of the light source (how far the light will travel)
			ShadowType = ShadowType.Solid, // Will not lit hulls themselves
			Color = Color.MonoGameOrange
		};

		#endregion


		public MogulQuestGame()
		{
			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 800,
				PreferredBackBufferHeight = 600
			};

			Content.RootDirectory = "Content";

			//Penumbra
			// Create the lighting system and add sample light and hull.
			penumbra = new PenumbraComponent(this);
			penumbra.Lights.Add(light);
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

			Constants.ScreenHeight = graphics.PreferredBackBufferHeight - 32;
			Constants.ScreenWidth = graphics.PreferredBackBufferWidth - 32;

			// Initialize the lighting system.
			penumbra.Initialize();
			penumbra.Debug = false;

			//Init Camera
			_camera = new Camera();

			//Transition
			_transition = new Transition(() => _gameState = GameState.GameRunning, LoadLevel,this.Content.Load<Texture2D>("transition"));

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

			//Fonts
			font = this.Content.Load<SpriteFont>("BasicFont");

			//Player
			player = new Player(this.Content.Load<Texture2D>("death"), new Vector2(32f, 32f), new KeyboardMover(32), 32);
			player.Layer = 1f;

			//Level
			level = new Level(this, this.Penumbra);

			string selectedLevelFile = null;

			//********************************
			// Level selection in debug Mode *
			//********************************
#if DEBUG

			var win = new OpenFileDialog();
			win.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			win.Filter = "Fichiers level (*.xml)|*.xml";

			win.ShowDialog();

			var fullPath = win.FileName;
			selectedLevelFile = Path.GetFileName(fullPath);
#endif
			string filename = (string.IsNullOrEmpty(selectedLevelFile) ? "Enter.xml" : selectedLevelFile);

			level.LoadLevel(filename);
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

			if (_gameState == GameState.ChangingLevel)
			{
				_transition.Reset();
				_transition.Update(gameTime, null);
			}
			else
			{

				//Penumbra
				// Player light position : center player
				light.Position = new Vector2(player.Position.X + 16, player.Position.Y + 16);

				//If player is alive
				if (player.Health > 0)
				{
					level.Update(gameTime, sprites);
				}

				//Check level victory conditions if any
				level.CheckVictoryConditions();

				//Load Next Level
				if (level.LevelCompleted)
				{
					_gameState = GameState.ChangingLevel;
				}
			}

			_camera.Follow(player);

			base.Update(gameTime);
		}


		private void LoadLevel()
		{
			string nextLevel = level.NextLevel;

			//reset penumbra lights exept player light
			Penumbra.Lights.Clear();
			Penumbra.Lights.Add(light);

			//reset penumbra hulls
			Penumbra.Hulls.Clear();

			//Load level
			level = new Level(this, this.Penumbra);
			level.LoadLevel(nextLevel);
			level.Begin(player);

		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Everything between penumbra.BeginDraw and penumbra.Draw will be
			// lit by the lighting system.
			penumbra.BeginDraw();

			spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);

			level.Draw(gameTime, spriteBatch);

			string score = string.Format("Score : {0} / Heath : {1} / Def : {2}", player.Score, player.Health, player.Defense);

			spriteBatch.End();

			penumbra.Transform = _camera.Transform;
			penumbra.Draw(gameTime);

			//Begin spriteBatch
			spriteBatch.Begin();

			//Draw text
			spriteBatch.DrawString(font, score, Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

			//Game over message when player HP <= 0
			if (player.Health <= 0)
			{
				spriteBatch.DrawString(font, "GAME OVER", new Vector2((Constants.ScreenWidth / 2) - 32, Constants.ScreenHeight / 2), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
			}

			spriteBatch.End();

			//Level transition - draw changelevel animation
			if (_gameState == GameState.ChangingLevel)
			{
				spriteBatch.Begin();

				if (_transition != null)
					_transition.Draw(gameTime, spriteBatch);

				spriteBatch.End();

			}

			//then, draw everithing else !
			base.Draw(gameTime);
		}
	}
}

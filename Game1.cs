using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FinalGame.Particles;

namespace FinalGame
  {
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    public SpriteBatch SpriteBatch
    {
      get { return spriteBatch; }
    }

    SpriteManager spriteManager;
    public SpriteManager SpriteManager
    {
      get { return spriteManager; }
    }
    
    SpriteFont scoreFont;
    Texture2D backgroundImage;
    public Random rnd { get; private set; }
    
    public int currentScore = 0;
    public float scoreMultiplyer = 1;

    ExplosionParticleSystem explosion;
    ExplosionSmokeParticleSystem smoke;
    
    int numberLivesRemaining = 3;
    public int NumberLivesRemaining
    {
      get { return numberLivesRemaining; }
      set 
        {
          numberLivesRemaining = value;
          if (numberLivesRemaining == 0)
          {
            currentGameState = GameState.GameOver;
            spriteManager.Enabled = false;
            spriteManager.Visible = false;
          }
        }
    }

    enum GameState { Start, InGame, GameOver };
    GameState currentGameState = GameState.Start;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      graphics.PreferredBackBufferHeight = 768;
      graphics.PreferredBackBufferWidth = 1024;
      rnd = new Random();
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
      explosion = new ExplosionParticleSystem(this, 3, @"Images\explosion");
      smoke = new ExplosionSmokeParticleSystem(this, 3, @"Images\smoke");
      spriteManager = new SpriteManager(this, explosion, smoke);

      Components.Add(spriteManager);
      Components.Add(explosion);
      Components.Add(smoke);

      spriteManager.Enabled = true;
      spriteManager.Visible = true;

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
      scoreFont = Content.Load<SpriteFont>(@"Fonts\Verdana");

      // Load background image
      backgroundImage = Content.Load<Texture2D>(@"images\background");
      
      // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
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
      switch (currentGameState)
      {
        case GameState.Start:
          if (Keyboard.GetState().GetPressedKeys().Length > 0)
          {
            currentGameState = GameState.InGame;
            spriteManager.Enabled = true;
            spriteManager.Visible = true;
          }
          break;

        case GameState.InGame:
          break;
        case GameState.GameOver:
          if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            Exit();
          break;
      }
    // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      switch (currentGameState)
      {
        case GameState.Start:
        {
          GraphicsDevice.Clear(Color.Black);

          // Draw text for intro splash screen
          spriteBatch.Begin();
          string text = "Use direction keys to move, and spacebar to fire. \nAt 1000 points, you will evolve. Again at 5000 points.\n\nSome enemies will move erratically and some will chase. Some will split in two.";
          spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(text).X / 2),
              (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(text).Y - 60 / 2)), Color.White);

          spriteBatch.End();
          break;
        }
        case GameState.InGame:
        {
          GraphicsDevice.Clear(Color.White);
          
          // TODO: Add your drawing code here
          spriteBatch.Begin();
          spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,
              Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

          spriteBatch.DrawString(scoreFont, "Score: " + currentScore + " Score Multiplyer: " + scoreMultiplyer, new Vector2(10, 10), Color.White, 0, Vector2.Zero,
              1, SpriteEffects.None, 1);
              
          spriteBatch.End();
          
          base.Draw(gameTime);
          break;
        }
        case GameState.GameOver:
        {
          GraphicsDevice.Clear(Color.Black);

          spriteBatch.Begin();
          string gameover = "Game Over!";
          spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).Y / 2),
            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2)), Color.White);

          gameover = "Your score: " + currentScore;
          spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).Y / 2),
            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 30), Color.White);

          gameover = "Press ENTER to exit!";
          spriteBatch.DrawString(scoreFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameover).Y / 2),
            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameover).Y / 2) + 60), Color.White);

          spriteBatch.End();
          break;
        }
      }
    }  

    public void AddScore(float score)
    {
      currentScore += Convert.ToInt16(score);
    }

  }
}

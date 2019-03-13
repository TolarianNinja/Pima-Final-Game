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
  /// This is a game component that implements IUpdateable.
  /// </summary>
  public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
  {
    SpriteBatch spriteBatch;
    UserControlledSprite player;
    
    List<Sprite> livesList = new List<Sprite>();
    List<Sprite> spriteList = new List<Sprite>();
    public List<Shots> shotList = new List<Shots>();
    public List<Shots> enemyShotList = new List<Shots>();

    int enemySpawnMinMilliseconds = 800;
    int enemySpawnMaxMilliseconds = 5000;
    int enemyMinSpeed = -1;
    int enemyMaxSpeed = -4;
    int nextSpawnTime = 0;
    
    float scoreMulti = 0;
    int multiCount;
    int randomScore = 150;
    int firstSplitScore = 25;
    int splitScore = 75;
    int chaseScore = 200;
    
    Texture2D[] form = new Texture2D[3];
    Vector2 currentPlayerPosition;
    Vector2 currentSplitPosition;
    
    public Texture2D firstShot;
    public Texture2D secondShot;
    public Texture2D thirdShot;
    public Texture2D enemyShot;
    
    ExplosionParticleSystem explosion;
    ExplosionSmokeParticleSystem smoke;


    public SpriteManager(Game1 game, ExplosionParticleSystem explosion, ExplosionSmokeParticleSystem smoke)
      : base(game)
      {
        this.explosion = explosion;
        this.smoke = smoke;
      }
    
    public SpriteManager(Game1 game) : base(game)
    {
    }
    

    /// <summary>
    /// Allows the game component to perform any initialization it needs to before starting
    /// to run.  This is where it can query for any required services and load content.
    /// </summary>
    public override void Initialize()
    {
      // TODO: Add your initialization code here

      base.Initialize();
    }

    /// <summary>
    /// Allows the game component to update itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
      form[0] = Game.Content.Load<Texture2D>(@"Images\form1");
      form[1] = Game.Content.Load<Texture2D>(@"Images\form2");
      form[2] = Game.Content.Load<Texture2D>(@"Images\form3");
      
      player = new UserControlledSprite(form[0], new Vector2(0, 50), new Point(75, 27), 2, new 
          Point(0, 0), new Point(4, 2), new Vector2(3, 3), ((Game1)Game), 0);

      firstShot = Game.Content.Load<Texture2D>(@"images\shot1");
      secondShot = Game.Content.Load<Texture2D>(@"images\shot2");
      thirdShot = Game.Content.Load<Texture2D>(@"images\shot3");
      enemyShot = Game.Content.Load<Texture2D>(@"images\eshot");

      for (int i = 0; i < ((Game1)Game).NumberLivesRemaining; ++i)
      {
        int offset = 10 + i * 30;
        livesList.Add(new PrimaryEnemy(Game.Content.Load<Texture2D>(@"images\life"), new Vector2(offset, 35),
            new Point(34, 28), 10, new Point(1, 1), new Point(4, 2), Vector2.Zero, ((Game1)Game), 0));
      }

      base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
      // TODO: Add your update code here
      nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
      if (nextSpawnTime < 0)
      {
        SpawnEnemy();

        // Reset Spawn Timer
        ResetSpawnTime();
      }
      UpdateSprites(gameTime);

      // Changes form of player based on score
      if (((Game1)Game).currentScore > 1000 && player.TextureImage == form[0])
      {
        currentPlayerPosition = GetPlayerPosition();
        player = new UserControlledSprite(form[1], currentPlayerPosition, new Point(76, 70), 2, new 
              Point(0, 0), new Point(4, 2), new Vector2(4, 4), ((Game1)Game), 0);
      }
      
      if (((Game1)Game).currentScore > 5000 && player.TextureImage == form[1])
      {
        currentPlayerPosition = GetPlayerPosition();
        player = new UserControlledSprite(form[2], currentPlayerPosition, new Point(100, 100), 2, new
            Point(0, 0), new Point(4, 2), new Vector2(5, 5), ((Game1)Game), 0);

      }
      
      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack,
              SaveStateMode.None);
      
      // Draw the player
      player.Draw(gameTime, spriteBatch);
      
      // Draw all sprites
      foreach (Sprite s in spriteList)
        s.Draw(gameTime, spriteBatch);
        
      foreach (Shots sh in shotList)
        sh.Draw(gameTime, spriteBatch);
      
      foreach (Shots ei in enemyShotList)
        ei.Draw(gameTime, spriteBatch);
      
      foreach (Sprite life in livesList)
        life.Draw(gameTime, spriteBatch);
        
      spriteBatch.End();
      
      base.Draw(gameTime);
    }
    
    public Vector2 GetPlayerPosition()
    {
      return player.GetPosition;
    }

    private void SpawnEnemy()
    {
      Vector2 speed = Vector2.Zero;
      Vector2 position = Vector2.Zero;

      // Default frame size
      Point frameSize = new Point(75, 75);

      position = new Vector2((int)Game.Window.ClientBounds.Width, ((Game1)Game).rnd.Next(0, 
                Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));
      
      switch(((Game1)Game).rnd.Next(3))
      {
        case 0:
        {
          speed = new Vector2(((Game1)Game).rnd.Next(enemyMaxSpeed, enemyMinSpeed), 0);
          spriteList.Add(new PrimaryEnemy(Game.Content.Load<Texture2D>(@"Images\primary"), position, new
              Point(75, 58), 2, new Point(1, 1), new Point(1, 1), speed, ((Game1)Game), randomScore));
          break;
        }
        case 1:
        {
          speed = new Vector2(((Game1)Game).rnd.Next(enemyMaxSpeed, enemyMinSpeed), 0);
          spriteList.Add(new SecondaryEnemy(Game.Content.Load<Texture2D>(@"Images\secondary"), position, new
              Point(75, 58), 2, new Point(1, 1), new Point(4, 2), speed, ((Game1)Game), firstSplitScore));
          break;
        }
        case 2:
        {
          speed = new Vector2(((Game1)Game).rnd.Next(enemyMaxSpeed, enemyMinSpeed), ((Game1)Game).rnd.Next(enemyMaxSpeed, enemyMinSpeed));
          spriteList.Add(new TertiaryEnemy(Game.Content.Load<Texture2D>(@"Images\tertiary"), position, new
              Point(73, 73), 2, new Point(1, 1), new Point(4, 2), speed, ((Game1)Game), chaseScore));
          break;
        }
        default:
        {
          break;
        }
      }      

    }

    private void ResetSpawnTime()
    {
      nextSpawnTime = ((Game1)Game).rnd.Next(
        enemySpawnMinMilliseconds,
        enemySpawnMaxMilliseconds);
    }
    
    public void UpdateSprites(GameTime gameTime)
    {
      // Update player
      player.Update(gameTime, Game.Window.ClientBounds);

      // Update all sprites
      for (int i = 0; i < spriteList.Count; i++)
      {
        Sprite s = spriteList[i];

        s.Update(gameTime, Game.Window.ClientBounds);
        
        for (int idx = 0; idx < shotList.Count; idx++)
        {
          Shots sh = shotList[idx];

          sh.Update(gameTime, Game.Window.ClientBounds);
          
          // Handling for player shots
          if (sh.collisionRect.Intersects(s.collisionRect))
          {
            multiCount++;
            if (multiCount == 26)
            {
              scoreMulti++;
              multiCount = 11;
            }
            else if (multiCount == 10)
            {
              scoreMulti++;
            } 
            ((Game1)Game).scoreMultiplyer = 1 + scoreMulti;
            ((Game1)Game).AddScore(s.scoreValue * ((Game1)Game).scoreMultiplyer);
            
            if (s is SecondaryEnemy)
            {
              currentSplitPosition = s.GetPosition;
              Split();
            }
            explosion.AddParticles(s.GetPosition);
            smoke.AddParticles(s.GetPosition);
            spriteList.RemoveAt(i);
            if (player.TextureImage != form[2])
              shotList.RemoveAt(idx);
          }

          if (sh.IsOutOfBounds(Game.Window.ClientBounds))
          {
            shotList.RemoveAt(idx);
          }
        }
        
        // Handling for enemy shots
        for (int ei = 0; ei < enemyShotList.Count; ei++)
        {
          Shots esh = enemyShotList[ei];
          
          esh.Update(gameTime, Game.Window.ClientBounds);
          
          if (esh.collisionRect.Intersects(player.collisionRect))
          {
            explosion.AddParticles(player.GetPosition);
            smoke.AddParticles(player.GetPosition);
            enemyShotList.RemoveAt(ei);
            PlayerDeath();
          }
          
          if (esh.IsOutOfBounds(Game.Window.ClientBounds))
          {
            enemyShotList.RemoveAt(ei);
          }
        }
        
        // Collision detection
        if (s.collisionRect.Intersects(player.collisionRect))
        {
          explosion.AddParticles(s.GetPosition);
          smoke.AddParticles(s.GetPosition);
          spriteList.RemoveAt(i);
          PlayerDeath();
        }
        
        // Remove sprites that leave the screen
        if (s.IsOutOfBounds(Game.Window.ClientBounds))
        {
          spriteList.RemoveAt(i);
          i--;
        }
      }
      for (int i = 0; i < livesList.Count; i++)
      {
        Sprite life = livesList[i];
        life.Update(gameTime, Game.Window.ClientBounds);
      }
    }

    public void Split()
    {
      Vector2 position = currentSplitPosition;

      Vector2 speed = new Vector2(-3, 1);
      spriteList.Add(new SplitEnemy(Game.Content.Load<Texture2D>(@"Images\split"), position, 
                new Point(55, 38), 2, new Point(0, 0), new Point(4, 2), speed, ((Game1)Game), splitScore));

      speed = new Vector2(-3, -1);
      spriteList.Add(new SplitEnemy(Game.Content.Load<Texture2D>(@"Images\split"), position, 
                new Point(55, 38), 2, new Point(0, 0), new Point(4, 2), speed, ((Game1)Game), splitScore));
    }
    
    public void PlayerDeath()
    {
      livesList.RemoveAt(livesList.Count - 1);
      --((Game1)Game).NumberLivesRemaining;
      multiCount = 0;
      scoreMulti = 0;
      
      for (int i = 0; i < spriteList.Count; i++)
      {
        Sprite sp = spriteList[i];
        explosion.AddParticles(sp.GetPosition);
        smoke.AddParticles(sp.GetPosition);
        spriteList.RemoveAt(i);
      }

      for (int i = 0; i < shotList.Count; i++)
      {
        Sprite sh = shotList[i];
        shotList.RemoveAt(i);
      }
      
      for (int i = 0; i < enemyShotList.Count; i++)
      {
        Sprite esh = enemyShotList[i];
        enemyShotList.RemoveAt(i);
      }
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalGame
{
  class PrimaryEnemy : Sprite // Random movement
  {
    int moveTimer = 700;
    float distanceLeft = 250;
    bool firstMove = false;
    bool moveMade = false;
    
    public PrimaryEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, game, scoreValue)
            {
              this.game = game;
            }

    public PrimaryEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, game, scoreValue, millisecondsPerFrame)
            {
              this.game = game;
            }

    public override Vector2 direction
    {
      get { return speed; }
    }

    public override void Update(GameTime gameTime, Rectangle clientBounds)
    {
      distanceLeft -= Math.Abs(direction.X + direction.Y);
      if (distanceLeft >= 0)
      {
        if (firstMove && !moveMade)     
          MoveRandom(ref speed, ref moveMade, position, clientBounds);
          
        position += speed;
      }
      else
      {
        if (moveTimer > 0)
          moveTimer -= gameTime.ElapsedGameTime.Milliseconds;
        else
        {
          distanceLeft = 150;
          moveTimer = 500;
          Fire(gameTime);
          moveMade = false;
          if (!firstMove)
            firstMove = true;
        }
      }
      base.Update(gameTime, clientBounds);
    }
    
    public override void Fire(GameTime gameTime)
    {
      Vector2 shotPosition = GetPosition + new Vector2(-10, 22);
      Vector2 shotSpeed = new Vector2(-2, 0);
      game.SpriteManager.enemyShotList.Add(new Shots(game.SpriteManager.enemyShot, shotPosition, new Point(15, 14), 0,
              new Point(1, 1), new Point(4, 2), shotSpeed, ((Game1)game), 0));
    }
    
    public static void MoveRandom(ref Vector2 speed, ref bool moveMade, Vector2 position, Rectangle clientBounds)
    {
      Random random = new Random();
      if (position.Y - 175 < 0)
        speed = new Vector2(0, 3);
      else if (position.Y + 175 > clientBounds.Height)
        speed = new Vector2(0, -3);
      else
      {
        switch(random.Next(3))
        {
          case 0:
          {
            speed = new Vector2(0, -3);
            break;
          }
          case 1:
          {
            speed = new Vector2(-3, 0);
            break;
          }
          case 2:
          {
            speed = new Vector2(0, 3);
            break;
          }
          default:
          {
            break;
          }
        }
      }
      moveMade = true;  
    }

  }
}

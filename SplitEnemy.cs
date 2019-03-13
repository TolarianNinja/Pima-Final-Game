using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalGame
{
  class SplitEnemy : Sprite // Random movement
  {
    int moveTimer = 700;
    float distanceLeft = 250;
    
    public SplitEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, game, scoreValue)
            {
              this.game = game;
            }

    public SplitEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
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
  }
}
    
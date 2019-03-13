using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FinalGame
  {
  class TertiaryEnemy : Sprite // The tracker
    {
      int moveTimer = 700;
      float distanceLeft = 250;
      Vector2 playerLocation;
      Vector2 fireTarget;
      
      public TertiaryEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
              int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue)
              : base(textureImage, position, frameSize, collisionOffset, currentFrame,
              sheetSize, speed, game, scoreValue)
              {
                this.game = game;
              }

      public TertiaryEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
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
        playerLocation = game.SpriteManager.GetPlayerPosition();
        if (distanceLeft >= 0)
        {
          if (playerLocation.X < position.X)
            position.X -= Math.Abs(speed.X);
          else if (playerLocation.X > position.X)
            position.X += Math.Abs(speed.X);

          if (playerLocation.Y < position.Y)
            position.Y -= Math.Abs(speed.Y);
          else if (playerLocation.Y > position.Y)
            position.Y += Math.Abs(speed.Y);
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
        fireTarget = game.SpriteManager.GetPlayerPosition();
        Vector2 shotPosition = GetPosition + new Vector2(-5, 22);
        Vector2 fireTrajectory;
        Vector2 shotSpeed = new Vector2(0, 0);
        float max;

        fireTrajectory.X = shotPosition.X - fireTarget.X;
        fireTrajectory.Y = shotPosition.Y - fireTarget.Y;

        max = Math.Max(fireTrajectory.X, fireTrajectory.Y);

        fireTrajectory.X = fireTrajectory.X / max;
        fireTrajectory.Y = fireTrajectory.Y / max;        

        shotSpeed -= fireTrajectory;

        game.SpriteManager.enemyShotList.Add(new Shots(game.SpriteManager.enemyShot, shotPosition, new Point(15, 14), 0,
                new Point(1, 1), new Point(4, 2), shotSpeed, ((Game1)game), 0));
      }
    }
  }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace FinalGame
{
  class UserControlledSprite : Sprite
  {
   int fireDelay;
   public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, game, scoreValue)
            {
            }

    public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, game, scoreValue, millisecondsPerFrame)
            {
              this.game = game;
            }
            
    public override Vector2 direction
    {
      get
      {
        Vector2 inputDirection = Vector2.Zero;
        
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
          inputDirection.X -= 1;
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
          inputDirection.X += 1;
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
          inputDirection.Y -= 1;
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
          inputDirection.Y += 1;
          
        return inputDirection * speed;
      }
    }

    public override void Update(GameTime gameTime, Rectangle clientBounds)
    {
      position += direction;
      
      if (position.X < 0)
        position.X = 0;
      if (position.Y < 0)
        position.Y = 0;
      if (position.X > clientBounds.Width - frameSize.X)
        position.X = clientBounds.Width - frameSize.X;
      if (position.Y > clientBounds.Height - frameSize.Y)
        position.Y = clientBounds.Height - frameSize.Y;

      fireDelay += gameTime.ElapsedGameTime.Milliseconds;
      if (Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        if (fireDelay > 200)
        {
          Fire(gameTime);
          fireDelay = 0;
        }
      }
  
      
      base.Update(gameTime, clientBounds);
    }
      
    public override void Fire(GameTime gameTime)
    {
      Vector2 shotPosition = GetPosition + new Vector2(70, 22);
      if (game.currentScore < 1000)
      {
        Vector2 shotSpeed = new Vector2(3, 0);
        game.SpriteManager.shotList.Add(new Shots(game.SpriteManager.firstShot, shotPosition, new Point(11, 7), 0,
              new Point(1, 1), new Point(4, 2), shotSpeed, ((Game1)game), 0));
      }
      else if (game.currentScore > 1000 && game.currentScore < 5000)
      {
        Vector2 shotSpeed = new Vector2(2, 0);
        game.SpriteManager.shotList.Add(new Shots(game.SpriteManager.secondShot, shotPosition, new Point(14, 14), 0,
              new Point(1, 1), new Point(1, 1), shotSpeed, ((Game1)game), 0));
      }
      else
      {
        Vector2 shotSpeed = new Vector2(2, 0);
        game.SpriteManager.shotList.Add(new Shots(game.SpriteManager.thirdShot, shotPosition, new Point(14, 14), 0,
              new Point(1, 1), new Point(4, 2), shotSpeed, ((Game1)game), 0));
      }
    }
  }
}

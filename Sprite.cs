using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FinalGame.Particles;


namespace FinalGame
{
  public abstract class Sprite
  {
    public Game1 game;
    Texture2D textureImage;
    public Texture2D TextureImage
    {
      get { return textureImage; }
      set { textureImage = value; }
    }
    protected Vector2 position;
    public Vector2 Position
    {
      get { return position; }
      set { position = value; }
    }
    protected Point frameSize;
    int collisionOffset;
    Point currentFrame;
    Point sheetSize;
    int timeSinceLastFrame = 0;
    int millisecondsPerFrame;
    protected Vector2 speed;
    public Vector2 Speed
    {
      get { return speed; }
      set { speed = value; }
    }
    const int defaultMillisecondsPerFrame = 16;
    public int scoreValue;

    
    public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
          Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue) : this(textureImage, position, frameSize,
          collisionOffset, currentFrame, sheetSize, speed, game, scoreValue, defaultMillisecondsPerFrame)
          {
          }
    public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
          Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue, int millisecondsPerFrame)
          {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.game = game;
            this.scoreValue = scoreValue;
          }
    
         
    public virtual void Update(GameTime gameTime, Rectangle clientBounds)
    {
      timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
      if(timeSinceLastFrame > millisecondsPerFrame)
      {
        timeSinceLastFrame = 0;
        currentFrame.X++;
        if (currentFrame.X >= sheetSize.X)
        {
          currentFrame.X = 0;
          currentFrame.Y++;
          if (currentFrame.Y >= sheetSize.Y)
            currentFrame.Y = 0;
        }
      }
    }
    
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X,
          currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,
          1f, SpriteEffects.None, 0);
    }
    
    public abstract Vector2 direction
    {
      get;
    }
    
    public Rectangle collisionRect
    {
      get
      {
        return new Rectangle((int)position.X + collisionOffset, (int)position.Y + collisionOffset,
               frameSize.X - (collisionOffset * 2), frameSize.Y - (collisionOffset * 2));
      }
    }

    public bool IsOutOfBounds(Rectangle clientRect)
    {
      if (position.X < -frameSize.X || position.X > clientRect.Width ||
        position.Y < -frameSize.Y || position.Y > clientRect.Height)
        {
          return true;
        }
      return false;
    }

    public Vector2 GetPosition
    {
      get { return position; }
    }
    
    public virtual void Fire(GameTime gameTime)
    {
    }
  }
}
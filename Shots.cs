using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalGame
{
  public class Shots : Sprite
  {
    
    public Shots(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, game, scoreValue)
            {
            }

    public Shots(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, Game1 game,
            int scoreValue, int millisecondsPerFrame)
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
      position += direction;
        
      base.Update(gameTime, clientBounds);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FinalGame
  {
  namespace Particles
    {
    public class ExplosionSmokeParticleSystem : ParticleSystem
      {
      public ExplosionSmokeParticleSystem(Game1 game, int howManyEffects, string textureFilename)
        : base(game, howManyEffects, textureFilename)
        {
        }
      protected override void InitializeConstants()
        {
        minInitialSpeed = 20;
        minInitialSpeed = 90;

        minAcceleration = -30;
        maxAcceleration = -60;

        minLifetime = 1f;
        maxLifetime = 3f;

        minScale = .5f;
        maxScale = 1f;

        minNumParticles = 30;
        maxNumParticles = 50;

        minRotationSpeed = -MathHelper.PiOver4;
        maxRotationSpeed = MathHelper.PiOver4;

        spriteBlendMode = SpriteBlendMode.AlphaBlend;

        DrawOrder = AlphaBlendDrawOrder;
        }
      }
    }
  }
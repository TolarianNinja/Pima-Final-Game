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
    public class ExplosionParticleSystem : ParticleSystem
      {
      public ExplosionParticleSystem(Game1 game, int howManyEffects, string textureFilename)
        : base(game, howManyEffects, textureFilename)
        {
        }
      protected override void InitializeConstants()
        {
        minInitialSpeed = 40;
        maxInitialSpeed = 300;

        minAcceleration = 0;
        maxAcceleration = 0;

        minLifetime = .5f;
        maxLifetime = 1.0f;

        minScale = .3f;
        maxScale = .8f;

        minNumParticles = 10;
        maxNumParticles = 15;

        minRotationSpeed = -MathHelper.PiOver4;
        maxRotationSpeed = MathHelper.PiOver4;

        spriteBlendMode = SpriteBlendMode.Additive;

        DrawOrder = AdditiveDrawOrder;
        }

      public override void InitializeParticle(Particle p, Vector2 where)
        {
        base.InitializeParticle(p, where);

        p.acceleration = -p.velocity / p.lifetime;
        }
      }
    }
  }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Indigo.Graphics;
using Atmo2.AI;
using Atmo2.Sprites;
using Atmo2.Worlds;

namespace Atmo2.Entities
{
    public class Enemy : Actor, Indigo.Loaders.IOgmoNodeHandler
    {
        public Spritemap spritemap;
        public IAI ai;

        private float prevX;

        // Only use this contructor with the OgmoLoader
        public Enemy() { }

        public Enemy(Spritemap spritemap, IAI ai, float x, float y)
            : base(x, y)
        {
            this.spritemap = spritemap;
            this.ai = ai;
            this.AddComponent<Image>(this.spritemap);
        }


        public void NodeHandler(XmlNode entity)
        {
            switch(entity.Name)
            {
                case "EnemyWalker":
                    {
                        this.spritemap = SpritemapConstructor.makeWalker();
                        this.SetHitbox(spritemap.Width, spritemap.Height, (int)spritemap.OriginX, (int)spritemap.OriginY);
                        this.ai = new AIWalker(GameWorld.World, this, int.Parse(entity.Attributes["speed"].Value));
                        this.AddComponent<Image>(this.spritemap);
                        break;
                    }
                default: throw new Exception("Attempted to create an unknown Enemy through Ogmo.");
            }
        }

        public override void Update()
        {
            base.Update();

            ai.update();
            this.spritemap.FlippedX = this.X < prevX;
            prevX = this.X;
        }
    }
}

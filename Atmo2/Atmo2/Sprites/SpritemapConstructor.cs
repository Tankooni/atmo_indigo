﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;

namespace Atmo2.Sprites
{
    public static class SpritemapConstructor
    {
        public static Spritemap makeWalker()
        {
            var map = new Spritemap(Library.Get<Texture>(
                "content/image/JulepSpice.png"), 20, 38);

            map.OriginX = map.Width / 2;
            map.OriginY = map.Height;

			map.Add("idle", FP.MakeFrames(0, 1), 1, true);
			map.Play("idle");
            return map;
        }
    }
}

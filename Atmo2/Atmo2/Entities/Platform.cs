using Indigo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Entities
{
	public class Platform : Solid
	{
		public Platform(float x, float y, int width, int height)
			: base(x, y, Image.CreateRect(width, height, new Color(0xFF0000)))
		{
			Width = width;
			Height = height;
		}

		public override void Update()
		{
			base.Update();
			Move(0.25f, 0);
		}
	}
}

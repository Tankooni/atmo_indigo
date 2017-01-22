using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo;
using Utility;
using Atmo2.Worlds;
using Utility.Audio;
using Indigo.Graphics;

namespace Atmo2
{
	class Game : Engine
	{
		static void Main(string[] args)
		{
			var game = new Game();
			game.Run();
		}

		public Game() :
			base(640, 480, 60)
		{
			Engine.Screen.Title = "Atmo2";
			Engine.Console.Enable();
			//Engine.Console.MirrorToSystemOut = true;
			Engine.Console.ToggleKey = Indigo.Inputs.Keyboard.Tilde;

			Engine.Screen.ClearColor = new Color(0x000000);
			Engine.Screen.Resize(1280, 860);

			Library.AddPath("../../../");

			Indigo.Inputs.Mouse.CursorVisible = true;
			AudioManager.Init(1);
			Engine.World = new GameWorld();
		}
	}
}

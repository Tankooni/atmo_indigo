using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo;
using Utility;
using Atmo2.Worlds;

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
			base(1280, 720, 60)
		{
			FP.Screen.Title = "Atmo2";
			FP.Console.Enable();
			FP.Console.MirrorToSystemOut = true;
			FP.Console.ToggleKey = Indigo.Inputs.Keyboard.Tilde;

			Indigo.Inputs.Mouse.CursorVisible = true;
			//AudioManager.Init(1);
			//AudioManager.PlayMusic("B_piano_mid");
			FP.World = new GameWorld();
		}
	}
}

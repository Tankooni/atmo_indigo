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
			base(320, 240, 60)
		{
			FP.Screen.Title = "Atmo2";
			FP.Console.Enable();
			FP.Console.MirrorToSystemOut = true;
			FP.Console.ToggleKey = Indigo.Inputs.Keyboard.Tilde;

			FP.Screen.ClearColor = new Color(0xAAAAAA);

			Indigo.Inputs.Mouse.CursorVisible = true;
			AudioManager.Init(0);
			AudioManager.PlayMusic("B_piano_mid");
			FP.World = new GameWorld();
		}
	}
}

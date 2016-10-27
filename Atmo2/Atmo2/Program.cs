﻿using System;
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
			FP.Screen.Resize(640, 480);

			Indigo.Inputs.Mouse.CursorVisible = true;
			AudioManager.Init(1);
			FP.World = new GameWorld();
		}
	}
}

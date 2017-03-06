using System;
using System.Linq;
using Indigo;
using Atmo2.Worlds;
using Utility.Audio;
using Indigo.Graphics;

namespace Atmo2
{
	public sealed class Game : Engine
	{
	    private readonly EngineConfiguration config;

		public Game(EngineConfiguration config) :
			base(640, 480, 60)
		{
            if (config == null)
                throw new ArgumentNullException("config");

		    this.config = config;

			Engine.Screen.Title = "Atmo2";
			Engine.Console.Enable();
			//Engine.Console.MirrorToSystemOut = true;
			Engine.Console.ToggleKey = Indigo.Inputs.Keyboard.Tilde;

			Engine.Screen.ClearColor = new Color(0x000000);
			Engine.Screen.Resize(1280, 860);

			Library.AddPath("../../../");
			Library.AddPath("./");

			Indigo.Inputs.Mouse.CursorVisible = true;
			AudioManager.Init(config.MuteAudio ? 0 : 1);
			Engine.World = new GameWorld();
		}

        public static void Main(string[] args)
        {
            var config = EngineConfiguration.FromCommandLine(args);

            var game = new Game(config);
            game.Run();
        }
    }

    public sealed class EngineConfiguration
    {
        public bool MuteAudio { get; private set; }

        private EngineConfiguration()
        {
            // NOTE Here to force factory pattern
        }

        public static EngineConfiguration FromCommandLine(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            return new EngineConfiguration
            {
                MuteAudio = args.Any(x => x == "--mute-audio")
            };
        }
    }
}

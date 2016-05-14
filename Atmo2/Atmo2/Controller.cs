using Indigo.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2
{
	public class Controller
	{
		public static Func<bool> Jump = () => { return Keyboard.X.Pressed; };
		public static Func<bool> Dash = () => { return Keyboard.C.Pressed; };

		public static Func<bool> Up = () => { return Keyboard.Up.Down; };
		public static Func<bool> Down = () => { return Keyboard.Down.Down; };
		public static Func<bool> Left = () => { return Keyboard.Left.Down; };
		public static Func<bool> Right = () => { return Keyboard.Right.Down; };

		public static Func<bool> Start = () => { return Keyboard.Escape.Pressed; };
		public static Func<bool> Select = () => { return Keyboard.Return.Pressed; };
	}
}

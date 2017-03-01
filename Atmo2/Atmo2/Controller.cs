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
		public static Func<bool> Attack = () => { return false;/*Keyboard.Z.Pressed;*/ };

		public static Func<bool> UpHeld = () => { return Keyboard.Up.Down; };
		public static Func<bool> DownHeld = () => { return Keyboard.Down.Down; };
		public static Func<bool> LeftHeld = () => { return Keyboard.Left.Down; };
		public static Func<bool> RightHeld = () => { return Keyboard.Right.Down; };

		public static Func<bool> UpPressed = () => { return Keyboard.Up.Pressed; };
		public static Func<bool> DownPressed = () => { return Keyboard.Down.Pressed; };
		public static Func<bool> LeftPressed = () => { return Keyboard.Left.Pressed; };
		public static Func<bool> RightPressed = () => { return Keyboard.Right.Pressed; };

		public static Func<bool> Start = () => { return Keyboard.Escape.Pressed; };
		public static Func<bool> Select = () => { return Keyboard.Return.Pressed; };
	}
}

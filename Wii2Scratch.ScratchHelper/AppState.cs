using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wii2Scratch.ScratchHelper
{
	internal static class AppState
	{
		private static List<WiiController> s_wiiControllers = new List<WiiController>();

		public static List<WiiController> WiiControllers
		{
			get
			{
				return s_wiiControllers;
			}
		}
	}
}

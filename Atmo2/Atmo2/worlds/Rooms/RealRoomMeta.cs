using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Audio;

namespace Atmo2.Worlds.Rooms
{
	public class RealRoomMeta
	{
		//Don't change the capitilization here, it is needed for reflection. Sorry
		public float width { get; set; }
		public float height { get; set; }
		public string roomMusic { get; set; }

		//public List<AudioLayer> MusicList { get; set; }

		public RealRoomMeta Init()
		{
			//MusicList = new List<AudioLayer>();
			//var isVol = 0;
			//AudioLayer audioLayer = null;
			//foreach (string stringVal in roomMusic.Split(';'))
			//{
			//	if (isVol % 2 == 0)
			//	{
			//		MusicList.Add(audioLayer = new AudioLayer { ChannelName = stringVal });
			//	}
			//	else if (isVol % 2 == 1)
			//	{
			//		audioLayer.Volume = int.Parse(stringVal);
			//	}
			//	isVol++;
			//}
			return this;
		}
	}
}

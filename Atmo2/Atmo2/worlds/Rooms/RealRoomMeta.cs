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
		public float Width = 0;
		public float Height = 0;
		public string RoomMusic = "";
		public List<AudioLayer> MusicList = new List<AudioLayer>();

		public RealRoomMeta Init()
		{
			var isVol = 0;
			AudioLayer audioLayer = null;
			foreach (string stringVal in RoomMusic.Split(';'))
			{
				if (isVol % 2 == 0)
				{
					MusicList.Add(audioLayer = new AudioLayer { ChannelName = stringVal });
				}
				else if (isVol % 2 == 1)
				{
					audioLayer.Volume = int.Parse(stringVal);
				}
				isVol++;
			}
			return this;
		}
	}
}

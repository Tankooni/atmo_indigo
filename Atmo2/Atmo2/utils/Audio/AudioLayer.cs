using System;
using Indigo.Audio;
using Indigo;

namespace Utility.Audio
{
	public class AudioLayer
	{
		public Sound SoundChannel;
		public string ChannelName;
		public Double StartPosition;
		public Double LoopPosition;
		public float Volume
		{
			get { return volume; }
			set { FP.Clamp(value, 0, 1); }
		}
		private float volume;

		public AudioLayer()
		{

		}
		public AudioLayer(string channelName, float volume = 1, Sound soundchannel = null, double startPosition = 0, double loopPosition = 0)
		{
			ChannelName = channelName;
			Volume = volume;
			SoundChannel = soundchannel;
			StartPosition = startPosition;
			LoopPosition = loopPosition;
		}
	}
}

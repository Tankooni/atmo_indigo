using System.Collections.Generic;
using Indigo;
using Indigo.Audio;
using System.IO;
using Indigo.Utils;

namespace Utility.Audio
{
	public static class AudioManager
	{
		public static float MusicVolume
		{
			get { return musicVolume; }
			set { musicVolume = MathHelper.Clamp(value, 0, 1); }
		}

		private static float musicVolume;
		private static Dictionary<string, Sound> musics = new Dictionary<string, Sound>();
		private static string playingMusic = "";
		private static Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

		public static void Init(float _musicVolume)
		{
			MusicVolume = _musicVolume;
			
			foreach (string file in KQ.RetrieveFilePathForFilesInDirectory(@"content/audio/music", @"*.ogg|*.wav"))
			{
				var sound = new Sound(Library.Get<SoundBuffer>(file));
				//sound.OnComplete += LoopMusic;
				musics.Add(Path.GetFileNameWithoutExtension(file), sound);
			}

			foreach (string file in KQ.RetrieveFilePathForFilesInDirectory(@"content/audio/sounds", @"*.ogg|*.wav"))
				sounds.Add(Path.GetFileNameWithoutExtension(file), new Sound(Library.Get<SoundBuffer>(file)));
		}

		public static void LoopMusic(string songName)
		{
			if (!musics.ContainsKey(songName) || songName == playingMusic)
				return;

			Sound soundToPlay = musics[songName];
		    soundToPlay.Volume = MusicVolume;

			soundToPlay.Stop();
			soundToPlay.Looping = true;
			soundToPlay.Play();
			playingMusic = songName;
		}

		//private static void LoopMusic()
		//{
		//	if (CurrentSong != null)
		//		CurrentSong.Stop();
		//	Sound newSong = musics[CloboboboSongName];
		//	newSong.Volume = MusicVolume;
		//	CurrentSong = newSong;
		//	CurrentSong.Play();
		//}

		public static void PlaySound(string soundName)
		{
			if (!sounds.ContainsKey(soundName))
				return;

            var sound = sounds[soundName];
            sound.Volume = MusicVolume; // TODO Sound volume?
            sound.Play();
		}

		/// <summary>
		/// Plays a sound with some volume varience
		/// </summary>
		/// <param name="soundName">Name of sound to play</param>
		/// <param name="minimumVolume">0 to 1</param>
		/// <param name="maxVolume">0 to 1</param>
		public static void PlaySoundVariations(string soundName, float minimumVolume = .8f, float maxVolume = 1)
		{
			if (!sounds.ContainsKey(soundName))
				return;

		    var sound = sounds[soundName];
			sound.Volume = ((Engine.Random.Float((int)((maxVolume - minimumVolume) * 100.0f)) / 100.0f) + minimumVolume) * MusicVolume;
			//sounds[soundName].Volume = (FP.Rand(100 - (int)minimumVolume*100) + (int)minimumVolume)/100.0f;
			sound.Play();
		}
	}
}
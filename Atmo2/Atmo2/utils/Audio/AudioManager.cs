using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo;
using Indigo.Audio;
using System.IO;
using System.Threading;

namespace Utility.Audio
{
	public static class AudioManager
	{
		public static float MusicVolume
		{
			get { return musicVolume; }
			set { musicVolume = FP.Clamp(value, 0, 1); }
		}
		private static float musicVolume;
		private static Dictionary<string, Sound> musics = new Dictionary<string, Sound>();
		private static List<string> playingMusic = new List<string>();
		private static Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

		public static void Init(float musicVolume)
		{
			MusicVolume = musicVolume;

			foreach (string file in KQ.RetrieveFilePathForFilesInDirectory(@"./content/audio/music", @"*.ogg|*.wav"))
			{
				var sound = new Sound(Library.Get<SoundBuffer>(file));
				//sound.OnComplete += LoopMusic;
				musics.Add(Path.GetFileNameWithoutExtension(file), sound);
			}

			foreach (string file in KQ.RetrieveFilePathForFilesInDirectory(@"./content/audio/sounds", @"*.ogg|*.wav"))
				sounds.Add(Path.GetFileNameWithoutExtension(file), new Sound(Library.Get<SoundBuffer>(file)));
		}

		public static void LoopMusic(string soundName, Double startPosition, Double loopPosition)
		{
			Console.WriteLine("Start Looping: {0}, {1}, {2}", soundName, startPosition, loopPosition);
			Sound soundToPlay;
			musics.TryGetValue(soundName, out soundToPlay);
			if (soundToPlay == null)
				return;
			soundToPlay.Stop();
			soundToPlay.ClearOnComplete();
			soundToPlay.OnComplete += () =>
			{
				Console.WriteLine("OnComplete: {0}, {1}, {2}", soundName, startPosition, loopPosition);
				LoopMusic(soundName, loopPosition, loopPosition);
			};
			soundToPlay.Play(startPosition);
			//soundToPlay.Position = startPosition;
			playingMusic.Add(soundName);
		}

		public static void SetLayersPlaying(List<AudioLayer> musicList)
		{
			bool foundSound;
			List<AudioLayer> soundsToSync = new List<AudioLayer>();

			AudioLayer[] newSoundsArr = new AudioLayer[musicList.Count()];
			musicList.CopyTo(newSoundsArr);
			var newSounds = newSoundsArr.ToList();

			if (playingMusic.Any())
			{
				for (int i = playingMusic.Count() - 1; i >= 0; i--)
				{
					var oldSound = playingMusic[i];
					foundSound = false;
					var oldSoundChannel = musics[oldSound];
					if (newSounds.Any())
					{
						for (int j = newSounds.Count() - 1; j >= 0; j--)
						{
							var newSound = newSounds[j];
							if (newSound.ChannelName == oldSound)
							{
								soundsToSync.Add(newSound);
								foundSound = true;
								newSounds.Remove(newSound);
								FP.LogFormat("Need to sync {0}", newSound.ChannelName);
								break;
							}
						}
					}
					if (!foundSound)
					{
						oldSoundChannel.Stop();
						playingMusic.Remove(oldSound);
						FP.LogFormat("Stopped {0}", oldSound);
					}
				}
			}

			var firstTrackName = playingMusic.FirstOrDefault();
			Double position = 0;
			if (firstTrackName != null)
				position = musics[firstTrackName].Position;

			for (int i = 0; i < newSounds.Count(); i++)
			{
				var newSoundLayer = newSounds[i];
				FP.LogFormat("Starting {0} at {1}", newSoundLayer.ChannelName, position);
				LoopMusic(newSoundLayer.ChannelName, position, newSoundLayer.LoopPosition);
			}
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
			sounds[soundName].Play();

		}

		/// <summary>
		/// Plays a sound with some volume varience
		/// </summary>
		/// <param name="soundName">Name of sound to play</param>
		/// <param name="minimumVolume">0 to 1</param>
		/// <param name="maxVolume">0 to 1</param>
		public static void PlaySoundVariations(string soundName, float minimumVolume, float maxVolume)
		{
			sounds[soundName].Volume = (FP.Random.Float((int)((maxVolume - minimumVolume) * 100.0f)) / 100.0f) + minimumVolume;
			//sounds[soundName].Volume = (FP.Rand(100 - (int)minimumVolume*100) + (int)minimumVolume)/100.0f;
			sounds[soundName].Play();
		}
	}
}
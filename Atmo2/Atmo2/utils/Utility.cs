using Indigo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public class KQ
	{
		public const string CollisionTypeSolid = "Solid";
		public const string CollisionTypePlayer = "Player";

		public static string CurrentMusic { get; internal set; }
		public static float MusicVolume = 1;



		/// <summary>
		/// Returns all files in a folder using one or more search filters.
		/// Treats filters as or's
		/// </summary>
		/// <param name="path">Path to folder to search</param>
		/// <param name="filters">A list of filters speparated by |</param>
		/// <returns></returns>
		public static string[] RetrieveFilePathForFilesInDirectory(string path, string filters)
		{
			
			List<string> files = new List<string>();
			foreach (string filter in filters.Split('|'))
				files.AddRange(Library.GetFilenames(path, filter));
			return files.Select(x => x.Replace(@"\", "/")).ToArray();
		}

		public static List<Type> GetTypeFromAllAssemblies<T>()
		{
			var typeList = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; ++i)
			{
				var types = assemblies[i].GetTypes();
				for (int j = 0; j < types.Length; ++j)
				{
					var t = types[j];
					if (typeof(T).IsAssignableFrom(t))
						typeList.Add(t);
				}
			}

			return typeList;
		}
	}
}

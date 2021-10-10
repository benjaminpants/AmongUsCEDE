using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using AmongUsCEDE.Core;

namespace AmongUsCEDE.Utilities
{
	static class ResourcesManager
	{
		private static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

		public static string TexturesPath;

		public static Texture2D GetTexture(string id, string file_path, bool cache = false) //AVOID CACHING. Its an option here to increase reload times for frequently used assets, but anything outside of UI probably shouldn't be cached
		{
			if (Textures.TryGetValue(id, out Texture2D tex)) return tex;
			Texture2D texture2D = null;
			if (File.Exists(file_path))
			{
				byte[] data = File.ReadAllBytes(file_path);
				texture2D = new Texture2D(2, 2);
				texture2D.LoadRawTextureData(data);
			}
			else
			{
				return Texture2D.whiteTexture; //dont want the game exceptioning everytime a texture is missing.
			}
			if (cache)
			{
				Textures.Add(id,texture2D);
			}
			return texture2D;
		}
	
		public static void LoadAllTextures()
		{
			string AttemptedDir = Path.Combine(CEExtensions.GetGameDirectory(), "Resources");
			if (Directory.Exists(AttemptedDir))
			{
				TexturesPath = Path.Combine(AttemptedDir, "Textures");
				if (Directory.Exists(TexturesPath))
				{
					if (Directory.Exists(Path.Combine(TexturesPath, "AutoCache")))
					{
						foreach (string path in Directory.GetFiles(Path.Combine(TexturesPath, "AutoCache")))
						{
							//DebugLog.ShowMessage("caching:" + path);
							GetTexture(new FileInfo(path).Name,path,true); //caches the texture
						}
					}
				}
				else
				{
					Application.Quit(1);
				}
			}
			else
			{
				Application.Quit(1);
			}
		}
	}
}

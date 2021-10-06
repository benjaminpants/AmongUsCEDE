using System;
using System.Collections.Generic;
using System.Text;
using MoonSharp.Interpreter;

namespace AmongUsCEDE.Core
{
	public class Gamemode
	{
		public string Name;
		public string DisplayName;
		public List<Role> Roles = new List<Role>();
		public CodeScript Script;

		public Gamemode()
		{

		}

		public Gamemode(string name, string display)
		{
			Name = name;
			DisplayName = display;
		}


	}

	public enum ScriptLanguage
	{
		Lua,
		Javascript,
		Undefined
	}

	public class CodeScript
	{
		public string FileLocation;
		public ScriptLanguage Language;
		public object Script;

		public CodeScript()
		{
			Language = ScriptLanguage.Undefined;
		}

		public CodeScript(Script scr)
		{
			Language = ScriptLanguage.Lua;
			Script = scr;
		}


		public CodeScript(ScriptLanguage lang)
		{
			Language = lang;
		}
	}
}

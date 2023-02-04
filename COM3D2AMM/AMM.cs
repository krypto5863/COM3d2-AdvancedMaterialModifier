using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using COM3D2.AdvancedMaterialModifier.UI;
using COM3D2API;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

namespace COM3D2.AdvancedMaterialModifier
{
	[BepInPlugin("AdvancedMaterialModifier", "AdvancedMaterialModifier", "2.0")]
	[BepInDependency("org.bepinex.plugins.unityinjectorloader", BepInDependency.DependencyFlags.SoftDependency)]
	public class Amm : BaseUnityPlugin
	{
		internal static Dictionary<string, MaterialGroup> Controls { get; private set; } = new Dictionary<string, MaterialGroup>
		{
			{ "global", new MaterialGroup("Global") },
			{ "body", new MaterialGroup("Body") },
			{ "head", new MaterialGroup("Head") },
			{ "hair", new MaterialGroup("Hair") },
			{ "clothes", new MaterialGroup("Clothes") },
			{ "acc", new MaterialGroup("Accessories") },
			{ "bg", new MaterialGroup("Background") },
			{ "prop", new MaterialGroup("Props") },
			{ "mps", new MaterialGroup("MPS Props") }
		};

		public static bool EnabledGui { get; internal set; }

		internal static Amm This;
		private const string IconBase64 = "iVBORw0KGgoAAAANSUhEUgAAABwAAAAcCAYAAAByDd+UAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAAL9SURBVEhL7ZZZSFRRGMe/uS5NOFAao5GRlplomkhYuZPZmENJEhhJBRESBhbBhCToU1lU6kulk1LolKa4NS5jM+OkpibSJlpkWmFvEYaKS7N4b+ece2acGz0EzfXJHxzO9//u8j/LXT4JAMjSM/LDg0NiU+bmfnpZrb88OJZlUP6/kTAM6+m51jL8Vjfwsq9mBKWmsGFAfmF7Xmi4MoecJQKMxGZ7P6KvGB3pr3aLic9KOJyuuskB45JZ/Q18b7lvcDRavUkmMiotluXc3ekxUcFbxlgsi55Uiw5+PhhXPSD/AvZaMTM7q4YuR2A4Pz8NDU8KqOKZmf4ODXWFYLNZaGaZlsar0N/7iMS6tlJobblBYmcWFmagploFLMsSLTDs7qqEFz1V8GqohWbwuzMFxmd3wdB5h2Z4Zmd/QGNdAXybHCZ6YnwQ+no1YDYvEG1H11aCWjHKzxMtMHxuVMPRYwWkd2ZbUDTUai5TxaNrLYb4xNNU8YSGJYHJUE4Vz9OmazTicRh+nhiCNVIZ7E/JhrGPfWQp7HAcB8ojKsHM27W3ICQskSqeiMhUwcCM+jI4niVcZoehSV8OMXEnSIxH3qG9TWIM+kLAAUUOmvl9ovVoeTOzrpPYGZvVDOjPA0ODjURXVZ4n1/lv3kk0xmHY2/0Q6h7nwalMCXTp76FWRo8AoM8fyH0DwUvmDV+/vIZuYwUo0nLpUSH8wNTw7k0HJCWfBanUC12/vK/EsMf0AHbvyQBNPedo3j7+8GlsgJyEZ4hRHMqF+pp8CAreC+g/B+ySjeSd8fbZBOvWbyRLrki7QHJ4wHaIoba5COISTpKEHTy6jlZ+WVl2ifTbd+yDxcVZx43+fCLtpCovkn5LwC7SO58nOZNdXpJ88NwlqkXFZFCXOvZwpVg1dDkMLuVoLDrYi/HwkFqpFh1cozIymQ//GRcZXJvigthNLg+0MBKz1c9va5SYtemH0U51reZKEyn1UduACuIIXKPistGVpT7eMryK42MDRm1z0ehv1F8t5wInIwIAAAAASUVORK5CYII=";

		private static readonly SaveFileDialog FileSave = new SaveFileDialog();
		private static readonly OpenFileDialog FileOpen = new OpenFileDialog();

		internal new static ManualLogSource Logger { get; private set; }

		internal static ConfigEntry<bool> HotKeyEnabled;
		internal static ConfigEntry<KeyboardShortcut> HotKey;

		//concerned with setting the playing field for DeRim to operate.
		private void Awake()
		{
			DontDestroyOnLoad(this);

			This = this;

			Logger = base.Logger;

			HotKeyEnabled = Config.Bind("HotKey", "1. Enable HotKey", false, "Use a hotkey to open AdvancedMaterialModifier.");
			HotKey = Config.Bind("HotKey", "2. HotKey", new KeyboardShortcut(KeyCode.F4, KeyCode.LeftControl, KeyCode.LeftAlt), "HotKey to open AdvancedMaterialModifier with.");

			var harmony = Harmony.CreateAndPatchAll(typeof(VanillaHarmonyPatch));
			try
			{
				harmony.PatchAll(typeof(MpsHarmonyPatch));
			}
			catch
			{
				Logger.LogWarning("MPS could not be patched! Likely not loaded.");
			}

			SystemShortcutAPI.AddButton("AdvancedMaterialModifier", () =>
			{
				EnabledGui = !EnabledGui;

				SaveConfig();
			}, "Open/Close GUI", Convert.FromBase64String(IconBase64));

			LoadConfig();

			Logger.LogDebug("Declaring war on Rim Lighting!!");
		}

		private void Update()
		{
			if (HotKeyEnabled.Value && HotKey.Value.IsDown())
			{
				EnabledGui = !EnabledGui;
			}
		}

		private void OnGUI()
		{
			if (EnabledGui)
			{
				MainGui.DisplayGui();
			}
		}

		internal static void SaveConfig(bool withPrompt = false)
		{
			string path = null;

			if (withPrompt)
			{
				FileSave.Filter = "json files (*.json)|*.json";
				FileSave.InitialDirectory = Paths.GameRootPath;
				FileSave.ShowDialog();

				if (!String.IsNullOrEmpty(FileSave.FileName))
				{
					path = FileSave.FileName;
				}
			}
			else
			{
				path = Paths.ConfigPath + "\\AdvancedMaterialModifier.json";
			}

			if (path.IsNullOrWhiteSpace())
			{
				var saveSer = JsonConvert.SerializeObject(Controls, Formatting.Indented);
				if (path != null) File.WriteAllText(path, saveSer);
			}
		}

		internal static void LoadConfig(bool withPrompt = false)
		{
			string path = null;

			if (withPrompt)
			{
				FileOpen.Filter = "json files (*.json)|*.json";
				FileOpen.InitialDirectory = Paths.GameRootPath;
				FileOpen.ShowDialog();

				if (!string.IsNullOrEmpty(FileOpen.FileName))
				{
					path = FileOpen.FileName;
				}
			}
			else
			{
				path = Paths.ConfigPath + "\\AdvancedMaterialModifier.json";
			}

			if (File.Exists(path))
			{
				// ReSharper disable once AssignNullToNotNullAttribute, it's not true!
				var loadSer = File.ReadAllText(path);
				Controls = JsonConvert.DeserializeObject<Dictionary<string, MaterialGroup>>(loadSer);
			}
		}
	}
}
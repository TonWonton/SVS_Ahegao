#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

using LogLevel = BepInEx.Logging.LogLevel;


namespace SVS_Ahegao
{
	[BepInProcess(PROCESS_NAME)]
	[BepInPlugin(GUID, PLUGIN_NAME, VERSION)]
	[BepInIncompatibility("SVS_HSceneAddOns")]
	public partial class AhegaoPlugin : BasePlugin
	{
		#region PLUGIN_INFO

		/*PLUGIN INFO*/
		public const string PLUGIN_NAME = "SVS_Ahegao";
		public const string COPYRIGHT = "";
		public const string COMPANY = "https://github.com/TonWonton/SVS_Ahegao";

		public const string PROCESS_NAME = "SamabakeScramble";
		public const string GUID = "SVS_Ahegao";
		public const string VERSION = "1.0.0";

		#endregion



		/*VARIABLES*/
		//Instance
		public static AhegaoPlugin Instance { get; private set; } = null!;
		private static ManualLogSource _log = null!;

		//CustomFacialExpression
		private static CustomFacialExpression _ahegaoOrgasmFacialExpression;
		private static CustomFacialExpression _ahegaoFaintnessFacialExpression;
		private static CustomFacialExpression _ahegaoFaintnessSpeedFacialExpression;

		public static CustomFacialExpression AhegaoOrgasmFacialExpression { get { return _ahegaoOrgasmFacialExpression; } }
		public static CustomFacialExpression AhegaoFaintnessFacialExpression { get { return _ahegaoFaintnessFacialExpression; } }
		public static CustomFacialExpression AhegaoFaintnessSpeedFacialExpression { get { return _ahegaoFaintnessSpeedFacialExpression; } }



		/*METHODS*/
		public static bool TryGetAhegaoComponent([MaybeNullWhen(false)] out AhegaoComponent ahegaoComponent)
		{
			ahegaoComponent = AhegaoComponent.Instance;
			return ahegaoComponent != null;
		}

		public static AhegaoComponent GetOrAddAhegaoComponent()
		{
			AhegaoComponent? ahegaoComponent = AhegaoComponent.Instance;

			if (ahegaoComponent != null)
			{
				return ahegaoComponent;
			}
			else
			{
				return Instance.AddComponent<AhegaoComponent>();
			}
		}

		public static void CreateAhegaoOrgasmFacialExpression()
		{
			_ahegaoOrgasmFacialExpression = new CustomFacialExpression(
				eyebrowPtn: orgasmEyebrowPtn.Value,
				eyesPtn: orgasmEyesPtn.Value,
				eyesOpenMax: orgasmEyesOpenMax.Value,
				mouthPtn: orgasmMouthPtn.Value,
				mouthOpenMin: orgasmMouthOpenMin.Value,
				mouthOpenMax: orgasmMouthOpenMax.Value,
				tearsLv: orgasmTearsLv.Value,
				blushAmount: orgasmBlushAmount.Value,
				eyesHighlight: orgasmEyesHighlight.Value,
				eyesBlink: orgasmEyesBlink.Value,
				eyesShake: orgasmEyesShake.Value,
				eyesRollAmount: orgasmEyesRollAmount.Value,
				eyesCrossAmount: orgasmEyesCrossAmount.Value
			);
		}

		public static void CreateAhegaoFaintnessFacialExpression()
		{
			_ahegaoFaintnessFacialExpression = new CustomFacialExpression(
				eyebrowPtn: faintnessEyebrowPtn.Value,
				eyesPtn: faintnessEyesPtn.Value,
				eyesOpenMax: faintnessEyesOpenMax.Value,
				mouthPtn: faintnessMouthPtn.Value,
				mouthOpenMin: faintnessMouthOpenMin.Value,
				mouthOpenMax: faintnessMouthOpenMax.Value,
				tearsLv: faintnessTearsLv.Value,
				blushAmount: faintnessBlushAmount.Value,
				eyesHighlight: faintnessEyesHighlight.Value,
				eyesBlink: faintnessEyesBlink.Value,
				eyesShake: faintnessEyesShake.Value,
				eyesRollAmount: faintnessEyesRollAmount.Value,
				eyesCrossAmount: faintnessEyesCrossAmount.Value
			);
		}

		public static void CreateAhegaoFaintnessSpeedFacialExpression()
		{
			_ahegaoFaintnessSpeedFacialExpression = new CustomFacialExpression(
				eyebrowPtn: faintnessSpeedEyebrowPtn.Value,
				eyesPtn: faintnessSpeedEyesPtn.Value,
				eyesOpenMax: faintnessSpeedEyesOpenMax.Value,
				mouthPtn: faintnessSpeedMouthPtn.Value,
				mouthOpenMin: faintnessSpeedMouthOpenMin.Value,
				mouthOpenMax: faintnessSpeedMouthOpenMax.Value,
				tearsLv: faintnessSpeedTearsLv.Value,
				blushAmount: faintnessSpeedBlushAmount.Value,
				eyesHighlight: faintnessSpeedEyesHighlight.Value,
				eyesBlink: faintnessSpeedEyesBlink.Value,
				eyesShake: faintnessSpeedEyesShake.Value,
				eyesRollAmount: faintnessSpeedEyesRollAmount.Value,
				eyesCrossAmount: faintnessSpeedEyesCrossAmount.Value
			);
		}



		/*EVENT HANDLING*/
		private static void OnAhegaoSettingChanged(object? sender, EventArgs args)
		{
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}

		private static void OnAhegaoOrgasmSettingChanged(object? sender, EventArgs args)
		{
			CreateAhegaoOrgasmFacialExpression();
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}

		private static void OnAhegaoFaintnessSettingChanged(object? sender, EventArgs args)
		{
			CreateAhegaoFaintnessFacialExpression();
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}

		private static void OnAhegaoFaintnessSpeedSettingChanged(object? sender, EventArgs args)
		{
			CreateAhegaoFaintnessSpeedFacialExpression();
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}



		/*PLUGIN LOAD*/
		public override void Load()
		{
			//Instance
			Instance = this;
			_log = Log;

			//Initialize
			InitializeConfig();
			CreateAhegaoOrgasmFacialExpression();
			CreateAhegaoFaintnessFacialExpression();
			CreateAhegaoFaintnessSpeedFacialExpression();

			//Create hooks
			Harmony.CreateAndPatchAll(typeof(AhegaoComponent.Hooks), GUID);
			Logging.LogInfo("Loaded");
		}



		//Logging
		public static class Logging
		{
			public static void Log(LogLevel level, string message)
			{
				_log.Log(level, message);
			}

			public static void LogFatal(string message)
			{
				_log.LogFatal(message);
			}

			public static void LogError(string message)
			{
				_log.LogError(message);
			}

			public static void LogWarning(string message)
			{
				_log.LogWarning(message);
			}

			public static void LogMessage(string message)
			{
				_log.LogMessage(message);
			}

			public static void LogInfo(string message)
			{
				_log.LogInfo(message);
			}

			public static void LogDebug(string message)
			{
				_log.LogDebug(message);
			}
		}
	}
}
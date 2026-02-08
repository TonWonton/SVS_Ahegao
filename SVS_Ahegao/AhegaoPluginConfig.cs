using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;

using Desc = BepInEx.Configuration.ConfigDescription;
using Order = SVS_Ahegao.ConfigurationManagerAttributes;
using ADV.Commands.Base;


namespace SVS_Ahegao
{
	public partial class AhegaoPlugin : BasePlugin
	{
		/*CONFIG*/
		//Ahegao
		public const string AHEGAO = "Ahegao";
		public static ConfigEntry<bool> ahegao = null!;
		public static ConfigEntry<bool> ahegaoOnOrgasm = null!;
		public static ConfigEntry<bool> ahegaoFemale = null!;
		public static ConfigEntry<bool> ahegaoMale = null!;
		public static ConfigEntry<int> orgasmAmount = null!;
		public static ConfigEntry<float> faintnessSpeedThreshold = null!;

		//Ahegao orgasm
		public const string AHEGAO_ORGASM = "Ahegao Orgasm";
		public static ConfigEntry<int> orgasmEyebrowPtn = null!;
		public static ConfigEntry<int> orgasmEyesPtn = null!;
		public static ConfigEntry<float> orgasmEyesOpenMax = null!;
		public static ConfigEntry<float> orgasmEyesRollAmount = null!;
		public static ConfigEntry<float> orgasmEyesCrossAmount = null!;
		public static ConfigEntry<bool> orgasmEyesHighlight = null!;
		public static ConfigEntry<bool> orgasmEyesBlink = null!;
		public static ConfigEntry<bool> orgasmEyesShake = null!;
		public static ConfigEntry<float> orgasmEyesShakeAmount = null!;
		public static ConfigEntry<float> orgasmEyesShakeFrequency = null!;
		public static ConfigEntry<byte> orgasmTearsLv = null!;
		public static ConfigEntry<int> orgasmMouthPtn = null!;
		public static ConfigEntry<float> orgasmMouthOpenMin = null!;
		public static ConfigEntry<float> orgasmMouthOpenMax = null!;
		public static ConfigEntry<float> orgasmBlushAmount = null!;

		//Ahegao faintness
		public const string AHEGAO_FAINTNESS = "Ahegao Faintness";
		public static ConfigEntry<int> faintnessEyebrowPtn = null!;
		public static ConfigEntry<int> faintnessEyesPtn = null!;
		public static ConfigEntry<float> faintnessEyesOpenMax = null!;
		public static ConfigEntry<float> faintnessEyesRollAmount = null!;
		public static ConfigEntry<float> faintnessEyesCrossAmount = null!;
		public static ConfigEntry<bool> faintnessEyesHighlight = null!;
		public static ConfigEntry<bool> faintnessEyesBlink = null!;
		public static ConfigEntry<bool> faintnessEyesShake = null!;
		public static ConfigEntry<float> faintnessEyesShakeAmount = null!;
		public static ConfigEntry<float> faintnessEyesShakeFrequency = null!;
		public static ConfigEntry<byte> faintnessTearsLv = null!;
		public static ConfigEntry<int> faintnessMouthPtn = null!;
		public static ConfigEntry<float> faintnessMouthOpenMin = null!;
		public static ConfigEntry<float> faintnessMouthOpenMax = null!;
		public static ConfigEntry<float> faintnessBlushAmount = null!;

		//Ahegao faintness speed
		public const string AHEGAO_FAINTNESS_SPEED = "Ahegao Faintness Speed";
		public static ConfigEntry<int> faintnessSpeedEyebrowPtn = null!;
		public static ConfigEntry<int> faintnessSpeedEyesPtn = null!;
		public static ConfigEntry<float> faintnessSpeedEyesOpenMax = null!;
		public static ConfigEntry<float> faintnessSpeedEyesRollAmount = null!;
		public static ConfigEntry<float> faintnessSpeedEyesCrossAmount = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesHighlight = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesBlink = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesShake = null!;
		public static ConfigEntry<float> faintnessSpeedEyesShakeAmount = null!;
		public static ConfigEntry<float> faintnessSpeedEyesShakeFrequency = null!;
		public static ConfigEntry<byte> faintnessSpeedTearsLv = null!;
		public static ConfigEntry<int> faintnessSpeedMouthPtn = null!;
		public static ConfigEntry<float> faintnessSpeedMouthOpenMin = null!;
		public static ConfigEntry<float> faintnessSpeedMouthOpenMax = null!;
		public static ConfigEntry<float> faintnessSpeedBlushAmount = null!;

		//Description
		public const string EYEBROW_PTN = "Eyebrow pattern (まゆげパターン)";
		public const string EYES_PTN = "Eye pattern (目パターン)";
		public const string EYES_MAX_OPEN = "Eyes open amount (目オープン)";
		public const string EYES_ROLL_AMOUNT = "Eyes roll amount offset (白目Y)";
		public const string EYES_CROSS_AMOUNT = "Eyes cross amount offset (白目X)";
		public const string EYE_HIGHLIGHT = "Eye highlight (目ハイライト)";
		public const string EYES_BLINK = "Eyes blink (目まばたき)";
		public const string EYES_SHAKE = "Eyes shake (目ゆれ)";
		public const string EYES_SHAKE_AMOUNT = "Eyes shake amount (目ゆれ量)";
		public const string EYES_SHAKE_FREQUENCY = "Eyes shake frequency (目ゆれ速度)";
		public const string TEARS_LV = "Tears level (なみだレベル)";
		public const string MOUTH_PTN = "Mouth pattern (口パターン)";
		public const string MOUTH_OPEN_MIN = "Mouth min open amount (口オープンMIN)";
		public const string MOUTH_OPEN_MAX = "Mouth max open amount (口オープンMAX)";
		public const string BLUSH_AMOUNT = "Blush amount offset (ほほあか)";

		private static int _currentOrder = 0;
		private static Order CurrentOrder { get { return new Order() { Order = _currentOrder-- }; } }
		private static void ResetOrder() { _currentOrder = 0; }

		//Values
		private Desc EyebrowPtnDescription => new Desc(string.Empty, new AcceptableValueRange<int>(0, 10), CurrentOrder);
		private Desc EyesPtnDescription => new Desc(string.Empty, new AcceptableValueRange<int>(0, 24), CurrentOrder);
		private Desc EyesOpenMaxDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc EyesRollAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(-1.2f, 1.2f), CurrentOrder);
		private Desc EyesCrossAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(-1.2f, 1.2f), CurrentOrder);
		private Desc EyesHighlightDescription => new Desc(string.Empty, null, CurrentOrder);
		private Desc EyesBlinkDescription => new Desc(string.Empty, null, CurrentOrder);
		private Desc EyesShakeDescription => new Desc(string.Empty, null, CurrentOrder);
		private Desc EyesShakeAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 0.8f), CurrentOrder);
		private Desc EyesShakeFrequencyDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1.001f), CurrentOrder);
		private Desc TearsLvDescription => new Desc(string.Empty, new AcceptableValueList<byte>(0, 1, 2, 3), CurrentOrder);
		private Desc MouthPtnDescription => new Desc(string.Empty, new AcceptableValueRange<int>(0, 27), CurrentOrder);
		private Desc MouthOpenMinDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc MouthOpenMaxDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc BlushAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(-1.6f, 3.4f), CurrentOrder);

		private void InitializeConfig()
		{
			//Ahegao
			ahegao = Config.Bind(AHEGAO, "Ahegao (アヘ顔)", true);
			ahegaoOnOrgasm = Config.Bind(AHEGAO, "Ahegao on orgasm (オーガズムアヘ顔)", true);
			ahegaoFemale = Config.Bind(AHEGAO, "Ahegao for female characters (女性アヘ顔)", true, new Desc(string.Empty, null, CurrentOrder));
			ahegaoMale = Config.Bind(AHEGAO, "Ahegao for male characters (男性アヘ顔)", false);
			orgasmAmount = Config.Bind(AHEGAO, "Orgasm amount for faintness", 3, new Desc(string.Empty, new AcceptableValueRange<int>(1, 10)));
			faintnessSpeedThreshold = Config.Bind(AHEGAO, "Faintness speed threshold", 0.8f, new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f)));
			ResetOrder();

			//Ahegao orgasm
			orgasmEyebrowPtn = Config.Bind(AHEGAO_ORGASM, EYEBROW_PTN, 8, EyebrowPtnDescription);
			orgasmEyesPtn = Config.Bind(AHEGAO_ORGASM, EYES_PTN, 3, EyesPtnDescription);
			orgasmEyesOpenMax = Config.Bind(AHEGAO_ORGASM, EYES_MAX_OPEN, 0.72f, EyesOpenMaxDescription);
			orgasmEyesRollAmount = Config.Bind(AHEGAO_ORGASM, EYES_ROLL_AMOUNT, 0.26f, EyesRollAmountDescription);
			orgasmEyesCrossAmount = Config.Bind(AHEGAO_ORGASM, EYES_CROSS_AMOUNT, 0f, EyesCrossAmountDescription);
			orgasmEyesHighlight = Config.Bind(AHEGAO_ORGASM, EYE_HIGHLIGHT, true, EyesHighlightDescription);
			orgasmEyesBlink = Config.Bind(AHEGAO_ORGASM, EYES_BLINK, true, EyesBlinkDescription);
			orgasmEyesShake = Config.Bind(AHEGAO_ORGASM, EYES_SHAKE, true, EyesShakeDescription);
			orgasmEyesShakeAmount = Config.Bind(AHEGAO_ORGASM, EYES_SHAKE_AMOUNT, 0.024f, EyesShakeAmountDescription);
			orgasmEyesShakeFrequency = Config.Bind(AHEGAO_ORGASM, EYES_SHAKE_FREQUENCY, 0.56f, EyesShakeFrequencyDescription);
			orgasmTearsLv = Config.Bind<byte>(AHEGAO_ORGASM, TEARS_LV, 3, TearsLvDescription);
			orgasmMouthPtn = Config.Bind(AHEGAO_ORGASM, MOUTH_PTN, 18, MouthPtnDescription);
			orgasmMouthOpenMin = Config.Bind(AHEGAO_ORGASM, MOUTH_OPEN_MIN, 0.2f, MouthOpenMinDescription);
			orgasmMouthOpenMax = Config.Bind(AHEGAO_ORGASM, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			orgasmBlushAmount = Config.Bind(AHEGAO_ORGASM, BLUSH_AMOUNT, 1.6f, BlushAmountDescription);
			ResetOrder();

			//Ahegao faintness
			faintnessEyebrowPtn = Config.Bind(AHEGAO_FAINTNESS, EYEBROW_PTN, 6, EyebrowPtnDescription);
			faintnessEyesPtn = Config.Bind(AHEGAO_FAINTNESS, EYES_PTN, 2, EyesPtnDescription);
			faintnessEyesOpenMax = Config.Bind(AHEGAO_FAINTNESS, EYES_MAX_OPEN, 0.72f, EyesOpenMaxDescription);
			faintnessEyesRollAmount = Config.Bind(AHEGAO_FAINTNESS, EYES_ROLL_AMOUNT, 0.06f, EyesRollAmountDescription);
			faintnessEyesCrossAmount = Config.Bind(AHEGAO_FAINTNESS, EYES_CROSS_AMOUNT, 0f, EyesCrossAmountDescription);
			faintnessEyesHighlight = Config.Bind(AHEGAO_FAINTNESS, EYE_HIGHLIGHT, true, EyesHighlightDescription);
			faintnessEyesBlink = Config.Bind(AHEGAO_FAINTNESS, EYES_BLINK, true, EyesBlinkDescription);
			faintnessEyesShake = Config.Bind(AHEGAO_FAINTNESS, EYES_SHAKE, true, EyesShakeDescription);
			faintnessEyesShakeAmount = Config.Bind(AHEGAO_FAINTNESS, EYES_SHAKE_AMOUNT, 0.024f, EyesShakeAmountDescription);
			faintnessEyesShakeFrequency = Config.Bind(AHEGAO_FAINTNESS, EYES_SHAKE_FREQUENCY, 0.56f, EyesShakeFrequencyDescription);
			faintnessTearsLv = Config.Bind<byte>(AHEGAO_FAINTNESS, TEARS_LV, 2, TearsLvDescription);
			faintnessMouthPtn = Config.Bind(AHEGAO_FAINTNESS, MOUTH_PTN, 3, MouthPtnDescription);
			faintnessMouthOpenMin = Config.Bind(AHEGAO_FAINTNESS, MOUTH_OPEN_MIN, 0.4f, MouthOpenMinDescription);
			faintnessMouthOpenMax = Config.Bind(AHEGAO_FAINTNESS, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			faintnessBlushAmount = Config.Bind(AHEGAO_FAINTNESS, BLUSH_AMOUNT, 1f, BlushAmountDescription);
			ResetOrder();

			//Ahegao faintness speed
			faintnessSpeedEyebrowPtn = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYEBROW_PTN, 6, EyebrowPtnDescription);
			faintnessSpeedEyesPtn = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_PTN, 2, EyesPtnDescription);
			faintnessSpeedEyesOpenMax = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_MAX_OPEN, 0.72f, EyesOpenMaxDescription);
			faintnessSpeedEyesRollAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_ROLL_AMOUNT, 0.16f, EyesRollAmountDescription);
			faintnessSpeedEyesCrossAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_CROSS_AMOUNT, 0f, EyesCrossAmountDescription);
			faintnessSpeedEyesHighlight = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYE_HIGHLIGHT, true, EyesHighlightDescription);
			faintnessSpeedEyesBlink = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_BLINK, true, EyesBlinkDescription);
			faintnessSpeedEyesShake = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_SHAKE, true, EyesShakeDescription);
			faintnessSpeedEyesShakeAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_SHAKE_AMOUNT, 0.024f, EyesShakeAmountDescription);
			faintnessSpeedEyesShakeFrequency = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_SHAKE_FREQUENCY, 0.56f, EyesShakeFrequencyDescription);
			faintnessSpeedTearsLv = Config.Bind<byte>(AHEGAO_FAINTNESS_SPEED, TEARS_LV, 3, TearsLvDescription);
			faintnessSpeedMouthPtn = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_PTN, 3, MouthPtnDescription);
			faintnessSpeedMouthOpenMin = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_OPEN_MIN, 0.4f, MouthOpenMinDescription);
			faintnessSpeedMouthOpenMax = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			faintnessSpeedBlushAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, BLUSH_AMOUNT, 1.4f, BlushAmountDescription);
			ResetOrder();

			//Register setting changed event
			ahegao.SettingChanged += OnAhegaoSettingChanged;
			ahegaoOnOrgasm.SettingChanged += OnAhegaoSettingChanged;
			ahegaoFemale.SettingChanged += OnAhegaoSettingChanged;
			ahegaoMale.SettingChanged += OnAhegaoSettingChanged;
			orgasmAmount.SettingChanged += OnAhegaoSettingChanged;
			faintnessSpeedThreshold.SettingChanged += OnAhegaoSettingChanged;

			orgasmEyebrowPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesOpenMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesRollAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesCrossAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesHighlight.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesBlink.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShake.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShakeAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShakeFrequency.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmTearsLv.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthOpenMin.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthOpenMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmBlushAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;

			faintnessEyebrowPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesOpenMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesRollAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesCrossAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesHighlight.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesBlink.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShake.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShakeAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShakeFrequency.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessTearsLv.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthOpenMin.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthOpenMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessBlushAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;

			faintnessSpeedEyebrowPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesOpenMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesRollAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesCrossAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesHighlight.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesBlink.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShake.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShakeAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShakeFrequency.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedTearsLv.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthOpenMin.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthOpenMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedBlushAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
		}
	}

#pragma warning disable 0169, 0414, 0649
	internal sealed class ConfigurationManagerAttributes
	{
		/// <summary>
		/// Should the setting be shown as a percentage (only use with value range settings).
		/// </summary>
		public bool? ShowRangeAsPercent;

		/// <summary>
		/// Custom setting editor (OnGUI code that replaces the default editor provided by ConfigurationManager).
		/// See below for a deeper explanation. Using a custom drawer will cause many of the other fields to do nothing.
		/// </summary>
		public System.Action<BepInEx.Configuration.ConfigEntryBase> CustomDrawer;

		/// <summary>
		/// Custom setting editor that allows polling keyboard input with the Input (or UnityInput) class.
		/// Use either CustomDrawer or CustomHotkeyDrawer, using both at the same time leads to undefined behaviour.
		/// </summary>
		public CustomHotkeyDrawerFunc CustomHotkeyDrawer;

		/// <summary>
		/// Custom setting draw action that allows polling keyboard input with the Input class.
		/// Note: Make sure to focus on your UI control when you are accepting input so user doesn't type in the search box or in another setting (best to do this on every frame).
		/// If you don't draw any selectable UI controls You can use `GUIUtility.keyboardControl = -1;` on every frame to make sure that nothing is selected.
		/// </summary>
		/// <example>
		/// CustomHotkeyDrawer = (ConfigEntryBase setting, ref bool isEditing) =>
		/// {
		///     if (isEditing)
		///     {
		///         // Make sure nothing else is selected since we aren't focusing on a text box with GUI.FocusControl.
		///         GUIUtility.keyboardControl = -1;
		///                     
		///         // Use Input.GetKeyDown and others here, remember to set isEditing to false after you're done!
		///         // It's best to check Input.anyKeyDown and set isEditing to false immediately if it's true,
		///         // so that the input doesn't have a chance to propagate to the game itself.
		/// 
		///         if (GUILayout.Button("Stop"))
		///             isEditing = false;
		///     }
		///     else
		///     {
		///         if (GUILayout.Button("Start"))
		///             isEditing = true;
		///     }
		/// 
		///     // This will only be true when isEditing is true and you hold any key
		///     GUILayout.Label("Any key pressed: " + Input.anyKey);
		/// }
		/// </example>
		/// <param name="setting">
		/// Setting currently being set (if available).
		/// </param>
		/// <param name="isCurrentlyAcceptingInput">
		/// Set this ref parameter to true when you want the current setting drawer to receive Input events.
		/// The value will persist after being set, use it to see if the current instance is being edited.
		/// Remember to set it to false after you are done!
		/// </param>
		public delegate void CustomHotkeyDrawerFunc(BepInEx.Configuration.ConfigEntryBase setting, ref bool isCurrentlyAcceptingInput);

		/// <summary>
		/// Show this setting in the settings screen at all? If false, don't show.
		/// </summary>
		public bool? Browsable;

		/// <summary>
		/// Category the setting is under. Null to be directly under the plugin.
		/// </summary>
		public string Category;

		/// <summary>
		/// If set, a "Default" button will be shown next to the setting to allow resetting to default.
		/// </summary>
		public object DefaultValue;

		/// <summary>
		/// Force the "Reset" button to not be displayed, even if a valid DefaultValue is available. 
		/// </summary>
		public bool? HideDefaultButton;

		/// <summary>
		/// Force the setting name to not be displayed. Should only be used with a <see cref="CustomDrawer"/> to get more space.
		/// Can be used together with <see cref="HideDefaultButton"/> to gain even more space.
		/// </summary>
		public bool? HideSettingName;

		/// <summary>
		/// Optional description shown when hovering over the setting.
		/// Not recommended, provide the description when creating the setting instead.
		/// </summary>
		public string Description;

		/// <summary>
		/// Name of the setting.
		/// </summary>
		public string DispName;

		/// <summary>
		/// Order of the setting on the settings list relative to other settings in a category.
		/// 0 by default, higher number is higher on the list.
		/// </summary>
		public int? Order;

		/// <summary>
		/// Only show the value, don't allow editing it.
		/// </summary>
		public bool? ReadOnly;

		/// <summary>
		/// If true, don't show the setting by default. User has to turn on showing advanced settings or search for it.
		/// </summary>
		public bool? IsAdvanced;

		/// <summary>
		/// Custom converter from setting type to string for the built-in editor textboxes.
		/// </summary>
		public System.Func<object, string> ObjToStr;

		/// <summary>
		/// Custom converter from string to setting type for the built-in editor textboxes.
		/// </summary>
		public System.Func<string, object> StrToObj;
	}
}
using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;


namespace SVS_Ahegao
{
	public partial class AhegaoPlugin : BasePlugin
	{
		/*CONFIG*/
		//Ahegao
		public const string CATEGORY_AHEGAO = "Ahegao";
		public static ConfigEntry<bool> ahegao = null!;
		public static ConfigEntry<bool> ahegaoMale = null!;
		public static ConfigEntry<int> orgasmAmount = null!;
		public static ConfigEntry<float> faintnessSpeedThreshold = null!;

		//Ahegao orgasm
		public const string CATEGORY_AHEGAO_ORGASM = "Ahegao Orgasm";
		public static ConfigEntry<bool> ahegaoOnOrgasm = null!;
		public static ConfigEntry<int> orgasmEyebrowPtn = null!;
		public static ConfigEntry<int> orgasmEyesPtn = null!;
		public static ConfigEntry<float> orgasmEyesOpenMax = null!;
		public static ConfigEntry<int> orgasmMouthPtn = null!;
		public static ConfigEntry<float> orgasmMouthOpenMin = null!;
		public static ConfigEntry<float> orgasmMouthOpenMax = null!;
		public static ConfigEntry<byte> orgasmTearsLv = null!;
		public static ConfigEntry<float> orgasmBlushAmount = null!;
		public static ConfigEntry<bool> orgasmEyesHighlight = null!;
		public static ConfigEntry<bool> orgasmEyesBlink = null!;
		public static ConfigEntry<bool> orgasmEyesShake = null!;
		public static ConfigEntry<float> orgasmEyesRollAmount = null!;
		public static ConfigEntry<float> orgasmEyesCrossAmount = null!;

		//Ahegao faintness
		public const string CATEGORY_AHEGAO_FAINTNESS = "Ahegao Faintness";
		public static ConfigEntry<int> faintnessEyebrowPtn = null!;
		public static ConfigEntry<int> faintnessEyesPtn = null!;
		public static ConfigEntry<float> faintnessEyesOpenMax = null!;
		public static ConfigEntry<int> faintnessMouthPtn = null!;
		public static ConfigEntry<float> faintnessMouthOpenMin = null!;
		public static ConfigEntry<float> faintnessMouthOpenMax = null!;
		public static ConfigEntry<byte> faintnessTearsLv = null!;
		public static ConfigEntry<float> faintnessBlushAmount = null!;
		public static ConfigEntry<bool> faintnessEyesHighlight = null!;
		public static ConfigEntry<bool> faintnessEyesBlink = null!;
		public static ConfigEntry<bool> faintnessEyesShake = null!;
		public static ConfigEntry<float> faintnessEyesRollAmount = null!;
		public static ConfigEntry<float> faintnessEyesCrossAmount = null!;

		//Ahegao faintness speed
		public const string CATEGORY_AHEGAO_FAINTNESS_SPEED = "Ahegao Faintness Speed";
		public static ConfigEntry<int> faintnessSpeedEyebrowPtn = null!;
		public static ConfigEntry<int> faintnessSpeedEyesPtn = null!;
		public static ConfigEntry<float> faintnessSpeedEyesOpenMax = null!;
		public static ConfigEntry<int> faintnessSpeedMouthPtn = null!;
		public static ConfigEntry<float> faintnessSpeedMouthOpenMin = null!;
		public static ConfigEntry<float> faintnessSpeedMouthOpenMax = null!;
		public static ConfigEntry<byte> faintnessSpeedTearsLv = null!;
		public static ConfigEntry<float> faintnessSpeedBlushAmount = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesHighlight = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesBlink = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesShake = null!;
		public static ConfigEntry<float> faintnessSpeedEyesRollAmount = null!;
		public static ConfigEntry<float> faintnessSpeedEyesCrossAmount = null!;

		//Description
		public const string EYEBROW_PTN = "Eyebrow pattern (まゆげパターン)";
		public const string EYES_PTN = "Eye pattern (目パターン)";
		public const string EYES_MAX_OPEN = "Eyes open amount (目オープン)";
		public const string MOUTH_PTN = "Mouth pattern (口パターン)";
		public const string MOUTH_OPEN_MIN = "Mouth min open amount (口オープンMIN)";
		public const string MOUTH_OPEN_MAX = "Mouth max open amount (口オープンMAX)";
		public const string TEARS_LV = "Tears level (なみだレベル)";
		public const string BLUSH_AMOUNT = "Blush amount offset (ほほあか)";
		public const string EYE_HIGHLIGHT = "Eye highlight (目ハイライト)";
		public const string EYES_BLINK = "Eyes blink (目まばたき)";
		public const string EYES_SHAKE = "Eyes shake (目ゆれ)";
		public const string EYES_ROLL_AMOUNT = "Eyes roll amount offset (白目Y)";
		public const string EYES_CROSS_AMOUNT = "Eyes cross amount offset (白目X)";

		//Values
		public ConfigDescription EyebrowPtnDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes { Order = 9 });
		public ConfigDescription EyesPtnDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<int>(0, 24), new ConfigurationManagerAttributes { Order = 8 });
		public ConfigDescription EyesOpenMaxDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { Order = 5 });
		public ConfigDescription MouthPtnDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<int>(0, 27), new ConfigurationManagerAttributes { Order = 7 });
		public ConfigDescription MouthOpenMinDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { Order = 2 });
		public ConfigDescription MouthOpenMaxDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<float>(0f, 1f), new ConfigurationManagerAttributes { Order = 1 });
		public ConfigDescription TearsLvDescription => new ConfigDescription(string.Empty, new AcceptableValueList<byte>(0, 1, 2, 3), new ConfigurationManagerAttributes { Order = 6 });
		public ConfigDescription BlushAmountDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<float>(0f, 3.4f), new ConfigurationManagerAttributes { Order = 0 });
		public ConfigDescription EyesHighlightDescription => new ConfigDescription(string.Empty, null, new ConfigurationManagerAttributes { Order = 10 });
		public ConfigDescription EyesBlinkDescription => new ConfigDescription(string.Empty, null, new ConfigurationManagerAttributes { Order = 12 });
		public ConfigDescription EyesShakeDescription => new ConfigDescription(string.Empty, null, new ConfigurationManagerAttributes { Order = 11 });
		public ConfigDescription EyesRollAmountDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<float>(-1, 1f), new ConfigurationManagerAttributes { Order = 4 });
		public ConfigDescription EyesCrossAmountDescription => new ConfigDescription(string.Empty, new AcceptableValueRange<float>(-1, 1f), new ConfigurationManagerAttributes { Order = 3 });

		private void InitializeConfig()
		{
			//Ahegao
			ahegao = Config.Bind(CATEGORY_AHEGAO, "Ahegao (アヘ顔)", true);
			ahegaoOnOrgasm = Config.Bind(CATEGORY_AHEGAO, "Ahegao on orgasm (オーガズムアヘ顔)", true);
			ahegaoMale = Config.Bind(CATEGORY_AHEGAO, "Ahegao for male characters (男性アヘ顔)", false);
			orgasmAmount = Config.Bind(CATEGORY_AHEGAO, "Orgasm amount for faintness", 3, new ConfigDescription(string.Empty, new AcceptableValueRange<int>(1, 10)));
			faintnessSpeedThreshold = Config.Bind(CATEGORY_AHEGAO, "Faintness speed threshold", 0.8f, new ConfigDescription(string.Empty, new AcceptableValueRange<float>(0f, 1f)));

			//Ahegao orgasm
			orgasmEyebrowPtn = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYEBROW_PTN, 8, EyebrowPtnDescription);
			orgasmEyesPtn = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYES_PTN, 3, EyesPtnDescription);
			orgasmEyesOpenMax = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYES_MAX_OPEN, 0.72f, EyesOpenMaxDescription);
			orgasmMouthPtn = Config.Bind(CATEGORY_AHEGAO_ORGASM, MOUTH_PTN, 18, MouthPtnDescription);
			orgasmMouthOpenMin = Config.Bind(CATEGORY_AHEGAO_ORGASM, MOUTH_OPEN_MIN, 0.2f, MouthOpenMinDescription);
			orgasmMouthOpenMax = Config.Bind(CATEGORY_AHEGAO_ORGASM, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			orgasmTearsLv = Config.Bind<byte>(CATEGORY_AHEGAO_ORGASM, TEARS_LV, 3, TearsLvDescription);
			orgasmBlushAmount = Config.Bind(CATEGORY_AHEGAO_ORGASM, BLUSH_AMOUNT, 1.6f, BlushAmountDescription);
			orgasmEyesHighlight = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYE_HIGHLIGHT, true, EyesHighlightDescription);
			orgasmEyesBlink = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYES_BLINK, true, EyesBlinkDescription);
			orgasmEyesShake = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYES_SHAKE, true, EyesShakeDescription);
			orgasmEyesRollAmount = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYES_ROLL_AMOUNT, 0.26f, EyesRollAmountDescription);
			orgasmEyesCrossAmount = Config.Bind(CATEGORY_AHEGAO_ORGASM, EYES_CROSS_AMOUNT, 0f, EyesCrossAmountDescription);

			//Ahegao faintness
			faintnessEyebrowPtn = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYEBROW_PTN, 6, EyebrowPtnDescription);
			faintnessEyesPtn = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYES_PTN, 2, EyesPtnDescription);
			faintnessEyesOpenMax = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYES_MAX_OPEN, 0.72f, EyesOpenMaxDescription);
			faintnessMouthPtn = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, MOUTH_PTN, 3, MouthPtnDescription);
			faintnessMouthOpenMin = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, MOUTH_OPEN_MIN, 0.4f, MouthOpenMinDescription);
			faintnessMouthOpenMax = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			faintnessTearsLv = Config.Bind<byte>(CATEGORY_AHEGAO_FAINTNESS, TEARS_LV, 2, TearsLvDescription);
			faintnessBlushAmount = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, BLUSH_AMOUNT, 1f, BlushAmountDescription);
			faintnessEyesHighlight = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYE_HIGHLIGHT, true, EyesHighlightDescription);
			faintnessEyesBlink = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYES_BLINK, true, EyesBlinkDescription);
			faintnessEyesShake = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYES_SHAKE, true, EyesShakeDescription);
			faintnessEyesRollAmount = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYES_ROLL_AMOUNT, 0.06f, EyesRollAmountDescription);
			faintnessEyesCrossAmount = Config.Bind(CATEGORY_AHEGAO_FAINTNESS, EYES_CROSS_AMOUNT, 0f, EyesCrossAmountDescription);

			//Ahegao faintness speed
			faintnessSpeedEyebrowPtn = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYEBROW_PTN, 6, EyebrowPtnDescription);
			faintnessSpeedEyesPtn = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYES_PTN, 2, EyesPtnDescription);
			faintnessSpeedEyesOpenMax = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYES_MAX_OPEN, 0.72f, EyesOpenMaxDescription);
			faintnessSpeedMouthPtn = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, MOUTH_PTN, 3, MouthPtnDescription);
			faintnessSpeedMouthOpenMin = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, MOUTH_OPEN_MIN, 0.4f, MouthOpenMinDescription);
			faintnessSpeedMouthOpenMax = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			faintnessSpeedTearsLv = Config.Bind<byte>(CATEGORY_AHEGAO_FAINTNESS_SPEED, TEARS_LV, 3, TearsLvDescription);
			faintnessSpeedBlushAmount = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, BLUSH_AMOUNT, 1.4f, BlushAmountDescription);
			faintnessSpeedEyesHighlight = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYE_HIGHLIGHT, true, EyesHighlightDescription);
			faintnessSpeedEyesBlink = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYES_BLINK, true, EyesBlinkDescription);
			faintnessSpeedEyesShake = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYES_SHAKE, true, EyesShakeDescription);
			faintnessSpeedEyesRollAmount = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYES_ROLL_AMOUNT, 0.16f, EyesRollAmountDescription);
			faintnessSpeedEyesCrossAmount = Config.Bind(CATEGORY_AHEGAO_FAINTNESS_SPEED, EYES_CROSS_AMOUNT, 0f, EyesCrossAmountDescription);

			//Register setting changed event
			ahegao.SettingChanged += OnAhegaoSettingChanged;
			ahegaoOnOrgasm.SettingChanged += OnAhegaoSettingChanged;
			orgasmAmount.SettingChanged += OnAhegaoSettingChanged;

			orgasmEyebrowPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesOpenMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthOpenMin.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthOpenMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmTearsLv.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmBlushAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesHighlight.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesBlink.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShake.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesRollAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesCrossAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;

			faintnessEyebrowPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesOpenMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthOpenMin.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthOpenMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessTearsLv.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessBlushAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesHighlight.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesBlink.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShake.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesRollAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesCrossAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;

			faintnessSpeedEyebrowPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesOpenMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthOpenMin.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthOpenMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedTearsLv.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedBlushAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesHighlight.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesBlink.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShake.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesRollAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesCrossAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
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
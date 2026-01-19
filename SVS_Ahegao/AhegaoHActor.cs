#nullable enable
using UnityEngine;
using Character;
using HarmonyLib;
using ILLGames.Unity.Animations;
using SV.H;
using System.Collections.Generic;

using Logging = SVS_Ahegao.AhegaoPlugin.Logging;


namespace SVS_Ahegao
{
	public class AhegaoHActor
	{
		//Harmony
		private static Harmony? _ahegaoHActorHooks;

		//Variables
		//Static
		private static HashSet<HumanFace> _ahegaoFaceHashSet = new HashSet<HumanFace>();
		private static Dictionary<Human, EyeLookMaterialControll[]> _elmcFromHuman = new Dictionary<Human, EyeLookMaterialControll[]>();
		private static Dictionary<Human, AhegaoHActor> _ahegaoHActorFromHuman = new Dictionary<Human, AhegaoHActor>();
		//Instance
		private HActor _hActor;
		private CustomFacialExpression _originalFacialExpression;
		private AhegaoState _ahegaoState = AhegaoState.None;
		private float _previousGaugeValue = 0f;
		private int _finishCount = 0;

		//Read-only properties
		public HActor HActor { get { return _hActor; } }
		public CustomFacialExpression OriginalFacialExpression { get { return _originalFacialExpression; } }
		public AhegaoState AhegaoState { get { return _ahegaoState; } }
		public float PreviousGaugeValue { get { return _previousGaugeValue; } }
		public int FinishCount { get { return _finishCount; } }

		//Properties
		public bool CanFinish { get { return _hActor.GaugeValue >= 80f; } }
		public bool Finished { get { return _previousGaugeValue - _hActor.GaugeValue >= 80f; } }
		public bool IsFaintnessProc { get { return _finishCount >= AhegaoPlugin.orgasmAmount.Value; } }

		//Methods
		public void EnableFacialExpressionChange() { _ahegaoFaceHashSet.Remove(_hActor.Human.face); }
		public void DisableFacialExpressionChange() { _ahegaoFaceHashSet.Add(_hActor.Human.face); }
		public void UpdatePreviousGaugeValue() { _previousGaugeValue = _hActor.GaugeValue; }
		public void IncrementFinishCount() { _finishCount++; }

		public void SetAhegaoState(AhegaoState ahegaoState)
		{
			if (AhegaoPlugin.ahegao.Value == false || (_hActor.IsMan && AhegaoPlugin.ahegaoMale.Value == false))
			{
				_ahegaoState = AhegaoState.None;
				EnableFacialExpressionChange();
				return;
			}

			if (_ahegaoState != ahegaoState)
			{
				_ahegaoState = ahegaoState;

				switch (ahegaoState)
				{
					case AhegaoState.None:
					{
						EnableFacialExpressionChange();
						return;
					}

					case AhegaoState.Orgasm:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoOrgasmFacialExpression);
						return;
					}

					case AhegaoState.Faintness:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoFaintnessFacialExpression);
						return;
					}

					case AhegaoState.FaintnessSpeed:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoFaintnessSpeedFacialExpression);
						return;
					}
				}
			}
		}

		public void UpdateAhegao()
		{
			switch (_ahegaoState)
				{
					case AhegaoState.None:
					{
						EnableFacialExpressionChange();
						return;
					}

					case AhegaoState.Orgasm:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoOrgasmFacialExpression);
						return;
					}

					case AhegaoState.Faintness:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoFaintnessFacialExpression);
						return;
					}

					case AhegaoState.FaintnessSpeed:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoFaintnessSpeedFacialExpression);
						return;
					}
				}
		}

		private void SetFacialExpression(CustomFacialExpression customFacialExpression)
		{
			Human human = _hActor.Human;
			HumanFace face = human.face;

			bool wasDisabled = _ahegaoFaceHashSet.Remove(face);

			face.ChangeEyebrowPtn(customFacialExpression.eyebrowPtn);
			face.ChangeEyesPtn(customFacialExpression.eyesPtn);
			face.ChangeEyesOpenMax(customFacialExpression.eyesOpenMax);
			face.ChangeMouthPtn(customFacialExpression.mouthPtn);
			face.ChangeMouthOpenMin(customFacialExpression.mouthOpenMin);
			face.ChangeMouthOpenMax(customFacialExpression.mouthOpenMax);
			human.fileStatus.tearsLv = customFacialExpression.tearsLv;
			face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(customFacialExpression.blushAmount));
			face.HideEyeHighlight(!customFacialExpression.eyesHighlight);
			face.ChangeEyesBlinkFlag(customFacialExpression.eyesBlink);
			face.ChangeEyesShaking(customFacialExpression.eyesShake);

			if (wasDisabled) _ahegaoFaceHashSet.Add(face);
		}

		public void UpdateBlushAmount()
		{
			HumanFace face = _hActor.Human.face;

			float originalBlushAmount = _originalFacialExpression.blushAmount;
			float newBlushAmount;

			switch (_ahegaoState)
			{
				case AhegaoState.Orgasm:
				{
					newBlushAmount = originalBlushAmount + AhegaoPlugin.orgasmBlushAmount.Value;
					face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(newBlushAmount)); return;
				}
				case AhegaoState.Faintness:
				{
					newBlushAmount = originalBlushAmount + AhegaoPlugin.faintnessBlushAmount.Value;
					face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(newBlushAmount)); return;
				}
				case AhegaoState.FaintnessSpeed:
				{
					newBlushAmount = originalBlushAmount + AhegaoPlugin.faintnessSpeedBlushAmount.Value;
					face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(newBlushAmount)); return;
				}
				case AhegaoState.None:
				{
					face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(originalBlushAmount)); return;
				}
			}
		}

		private static void OnHumanPostLateUpdate(Human human)
		{
			if (_elmcFromHuman.TryGetValue(human, out EyeLookMaterialControll[]? elmcArray) && _ahegaoHActorFromHuman.TryGetValue(human, out AhegaoHActor? ahegaoHActor))
			{
				float originalEyesCrossAmount = ahegaoHActor._originalFacialExpression.eyesCrossAmount;
				float originalEyesRollAmount = ahegaoHActor._originalFacialExpression.eyesRollAmount;

				float newEyesCrossAmount;
				float newEyesRollAmount;

				switch (ahegaoHActor._ahegaoState)
				{
					case AhegaoState.Orgasm:
					{
						newEyesCrossAmount = originalEyesCrossAmount + AhegaoPlugin.orgasmEyesCrossAmount.Value;
						newEyesRollAmount = originalEyesRollAmount + AhegaoPlugin.orgasmEyesRollAmount.Value;

						elmcArray[0].SetPositionX(0, newEyesCrossAmount);
						elmcArray[0].SetPositionY(0, newEyesRollAmount);
						elmcArray[1].SetPositionX(0, newEyesCrossAmount);
						elmcArray[1].SetPositionY(0, newEyesRollAmount);
						return;
					}
					case AhegaoState.Faintness:
					{
						newEyesCrossAmount = originalEyesCrossAmount + AhegaoPlugin.faintnessEyesCrossAmount.Value;
						newEyesRollAmount = originalEyesRollAmount + AhegaoPlugin.faintnessEyesRollAmount.Value;

						elmcArray[0].SetPositionX(0, newEyesCrossAmount);
						elmcArray[0].SetPositionY(0, newEyesRollAmount);
						elmcArray[1].SetPositionX(0, newEyesCrossAmount);
						elmcArray[1].SetPositionY(0, newEyesRollAmount);
						return;
					}
					case AhegaoState.FaintnessSpeed:
					{
						newEyesCrossAmount = originalEyesCrossAmount + AhegaoPlugin.faintnessSpeedEyesCrossAmount.Value;
						newEyesRollAmount = originalEyesRollAmount + AhegaoPlugin.faintnessSpeedEyesRollAmount.Value;

						elmcArray[0].SetPositionX(0, newEyesCrossAmount);
						elmcArray[0].SetPositionY(0, newEyesRollAmount);
						elmcArray[1].SetPositionX(0, newEyesCrossAmount);
						elmcArray[1].SetPositionY(0, newEyesRollAmount);
						return;
					}
				}
			}
		}



		//Event handling
		private static void OnAhegaoComponentStarted()
		{
			_ahegaoHActorHooks = Harmony.CreateAndPatchAll(typeof(Hooks), AhegaoPlugin.GUID + "_AhegaoHActorHooks");
		}

		private static void OnAhegaoComponentDestroyed()
		{
			_ahegaoHActorHooks?.UnpatchSelf();
			_ahegaoFaceHashSet.Clear();
			_elmcFromHuman.Clear();
		}



		//Initialization
		public AhegaoHActor(HActor hActor)
		{
			_hActor = hActor;
			Human human = hActor.Human;

			_originalFacialExpression = new CustomFacialExpression(human);
			_previousGaugeValue = hActor.GaugeValue;

			_elmcFromHuman[human] = human.face.eyeLookMatCtrl;
			_ahegaoHActorFromHuman[human] = this;
		}

		static AhegaoHActor()
		{
			AhegaoComponent.Started += OnAhegaoComponentStarted;
			AhegaoComponent.Destroyed += OnAhegaoComponentDestroyed;
		}



		//Hooks
		public static class Hooks
		{
			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeEyebrowPtn))]
			public static bool HumanFacePreChangeEyebrowPtn(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeEyesPtn))]
			public static bool HumanFacePreChangeEyesPtn(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeEyesOpenMax))]
			public static bool HumanFacePreChangeEyesOpenMax(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeMouthPtn))]
			public static bool HumanFacePreChangeMouthPtn(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeMouthOpenMin))]
			public static bool HumanFacePreChangeMouthOpenMin(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeMouthOpenMax))]
			public static bool HumanFacePreChangeMouthOpenMax(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			//[HarmonyPrefix]
			//[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeHohoAkaRate))]
			//public static bool HumanFacePreChangeHohoAkaRate(HumanFace __instance, IntPtr value)
			//{
			//	if (_ahegaoFaceHashSet.Contains(__instance)) return false;
			//	else return true;
			//}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.HideEyeHighlight))]
			public static bool HumanFacePreHideEyeHighlight(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeEyesBlinkFlag))]
			public static bool HumanFacePreChangeEyesBlinkFlag(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeEyesShaking))]
			public static bool HumanFacePreChangeEyesShaking(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyAfter("FixationalEyeMovement")]
			[HarmonyPatch(typeof(Human), nameof(Human.LateUpdate))]
			public static void HumanPostLateUpdate(Human __instance)
			{
				OnHumanPostLateUpdate(__instance);
			}
		}
	}
}
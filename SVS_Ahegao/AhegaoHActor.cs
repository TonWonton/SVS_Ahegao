#nullable enable
using UnityEngine;
using Character;
using HarmonyLib;
using ILLGames.Unity.Animations;
using SV.H;
using System.Collections.Generic;
using ILLGames.Unity;

using Logging = SVS_Ahegao.AhegaoPlugin.Logging;
using Il2CppArrays = Il2CppInterop.Runtime.InteropTypes.Arrays;


namespace SVS_Ahegao
{
	public class AhegaoHActor
	{
		/*VARIABLES*/
		//Harmony
		private static Harmony? _ahegaoHActorHooks;

		//Variables
		//Static
		private static HashSet<HumanFace> _ahegaoFaceHashSet = new HashSet<HumanFace>();
		private static Dictionary<Human, AhegaoHActor> _ahegaoHActorFromHuman = new Dictionary<Human, AhegaoHActor>();
		private static Dictionary<Human, EyeLookMaterialControll[]> _elmcFromHuman = new Dictionary<Human, EyeLookMaterialControll[]>();
		private static HashSet<EyeLookCalc> _eyeLookCalcHashSet = new HashSet<EyeLookCalc>();

		//HActor
		private HActor _hActor;
		private Human _human;
		private HumanFace _face;
		private EyeLookMaterialControll[] _elmc;
		private CustomFacialExpression _originalFacialExpression;
		private AhegaoState _ahegaoState = AhegaoState.None;

		private float _previousGaugeValue = 0f;
		private int _finishCount = 0;
		private float _eyesShakeTimer = 0f;
		private float _eyesRollTarget;
		private float _eyesCrossTarget;
		private float _eyesRollShakeTarget;
		private float _eyesCrossShakeTargetL;
		private float _eyesCrossShakeTargetR;

		//Read-only properties
		public HActor HActor { get { return _hActor; } }
		public AhegaoState AhegaoState { get { return _ahegaoState; } }

		//Properties
		public bool CanFinish { get { return _hActor.GaugeValue >= 80f; } }
		public bool Finished { get { return _previousGaugeValue - _hActor.GaugeValue >= 80f; } }
		public bool IsFaintnessProc { get { return _finishCount >= AhegaoPlugin.orgasmAmount.Value; } }



		/*METHODS*/
		public bool EnableFacialExpressionChange()
		{
			return _ahegaoFaceHashSet.Remove(_hActor.Human.face);
		}
		public bool DisableFacialExpressionChange()
		{
			return _ahegaoFaceHashSet.Add(_hActor.Human.face);
		}

		public void UpdatePreviousGaugeValue() { _previousGaugeValue = _hActor.GaugeValue; }
		public void IncrementFinishCount() { _finishCount++; }

		public void SetAhegaoState(AhegaoState ahegaoState)
		{
			bool isMan = _hActor.IsMan;
			if (AhegaoPlugin.ahegao.Value == false || (isMan && AhegaoPlugin.ahegaoMale.Value == false) || (isMan == false && AhegaoPlugin.ahegaoFemale.Value == false))
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
			bool isMan = _hActor.IsMan;
			if (AhegaoPlugin.ahegao.Value == false || (isMan && AhegaoPlugin.ahegaoMale.Value == false) || (isMan == false && AhegaoPlugin.ahegaoFemale.Value == false))
			{
				_ahegaoState = AhegaoState.None;
				EnableFacialExpressionChange();
				return;
			}

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
			CustomFacialExpression originalFacialExpression = _originalFacialExpression;
			Human human = _hActor.Human;
			HumanFace face = human.face;

			bool wasDisabled = EnableFacialExpressionChange();

			face.ChangeEyebrowPtn(customFacialExpression.eyebrowPtn);
			face.ChangeEyesPtn(customFacialExpression.eyesPtn);
			face.ChangeMouthPtn(customFacialExpression.mouthPtn);

			face.ChangeEyesOpenMax(customFacialExpression.eyesOpenMax);
			face.ChangeMouthOpenMin(customFacialExpression.mouthOpenMin);
			face.ChangeMouthOpenMax(customFacialExpression.mouthOpenMax);

			float eyesRollTarget = originalFacialExpression.eyesRollAmount + customFacialExpression.eyesRollAmount;
			float eyesCrossTarget = originalFacialExpression.eyesCrossAmount + customFacialExpression.eyesCrossAmount;
			_eyesRollTarget = eyesRollTarget;
			_eyesRollShakeTarget = eyesRollTarget;
			_eyesCrossTarget = eyesCrossTarget;
			_eyesCrossShakeTargetL = eyesCrossTarget;
			_eyesCrossShakeTargetR = eyesCrossTarget;

			human.fileStatus.tearsLv = customFacialExpression.tearsLv;
			face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(customFacialExpression.blushAmount));
			face.HideEyeHighlight(!customFacialExpression.eyesHighlight);
			face.ChangeEyesBlinkFlag(customFacialExpression.eyesBlink);
			face.ChangeEyesShaking(customFacialExpression.eyesShake);

			if (wasDisabled) DisableFacialExpressionChange();
		}

		public void UpdateBlushAmount()
		{
			float newBlushAmount = _originalFacialExpression.blushAmount;

			switch (_ahegaoState)
			{
				case AhegaoState.Orgasm: { newBlushAmount += AhegaoPlugin.orgasmBlushAmount.Value; break; }
				case AhegaoState.Faintness: { newBlushAmount += AhegaoPlugin.faintnessBlushAmount.Value; break; }
				case AhegaoState.FaintnessSpeed: { newBlushAmount += AhegaoPlugin.faintnessSpeedBlushAmount.Value; break; }
				case AhegaoState.None: { return; }
				default: { return; }
			}

			_face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(newBlushAmount));
		}

		private void UpdateEyePosition()
		{
			AhegaoState ahegaoState = _ahegaoState;
			if (ahegaoState != AhegaoState.None && AhegaoPlugin.TryGetAhegaoStateCustomFacialExpression(ahegaoState, out CustomFacialExpression customFacialExpression))
			{
				EyeLookMaterialControll[] elmc = _elmc;

				float eyesRollTarget = _eyesRollTarget;
				float eyesCrossTarget = _eyesCrossTarget;

				float eyesShakeAmount = customFacialExpression.eyesShakeAmount;
				float eyesShakeFrequency = 1.001f - customFacialExpression.eyesShakeFrequency;
				bool eyesShake = customFacialExpression.eyesShake;

				if (eyesShake)
				{
					if (_eyesShakeTimer >= eyesShakeFrequency)
					{
						float shakeOffsetV = Random.Range(-eyesShakeAmount, eyesShakeAmount);
						float shakeOffsetH = Random.Range(-eyesShakeAmount, eyesShakeAmount);

						_eyesRollShakeTarget = Mathf.ClampOffsetRadius(_eyesRollShakeTarget + shakeOffsetV, eyesRollTarget, eyesShakeAmount);
						_eyesCrossShakeTargetL = Mathf.ClampOffsetRadius(_eyesCrossShakeTargetL + shakeOffsetH, eyesCrossTarget, eyesShakeAmount);
						_eyesCrossShakeTargetR = Mathf.ClampOffsetRadius(_eyesCrossShakeTargetR - shakeOffsetH, eyesCrossTarget, eyesShakeAmount);

						_eyesShakeTimer -= eyesShakeFrequency + (Random.Range(-eyesShakeFrequency, eyesShakeFrequency) * 0.5f);
					}
					else
					{
						_eyesShakeTimer += Time.deltaTime;
					}

					elmc[0].SetPositionY(0, _eyesRollShakeTarget);
					elmc[1].SetPositionY(0, _eyesRollShakeTarget);
					elmc[0].SetPositionX(0, _eyesCrossShakeTargetL);
					elmc[1].SetPositionX(0, _eyesCrossShakeTargetR);
				}
				else
				{
					elmc[0].SetPositionY(0, eyesRollTarget);
					elmc[1].SetPositionY(0, eyesRollTarget);
					elmc[0].SetPositionX(0, eyesCrossTarget);
					elmc[1].SetPositionX(0, eyesCrossTarget);
				}
			}
		}

		private static void OnHumanPostLateUpdate(Human human)
		{
			if (_elmcFromHuman.TryGetValue(human, out EyeLookMaterialControll[]? elmcArray) && _ahegaoHActorFromHuman.TryGetValue(human, out AhegaoHActor? ahegaoHActor))
			{
				ahegaoHActor.UpdateEyePosition();
			}
		}



		/*EVENT HANDLING*/
		private static void OnAhegaoComponentStarted()
		{
			Logging.Info("Creating AhegaoHActor hooks");
			_ahegaoHActorHooks = Harmony.CreateAndPatchAll(typeof(Hooks), AhegaoPlugin.GUID + "_AhegaoHActorHooks");
		}

		private static void OnAhegaoComponentDestroyed()
		{
			Logging.Info("Unpatching AhegaoHActor hooks");
			_ahegaoHActorHooks?.UnpatchSelf();
			_ahegaoFaceHashSet.Clear();
			_elmcFromHuman.Clear();
		}



		/*INITIALIZATION*/
		public AhegaoHActor(HActor hActor)
		{
			_hActor = hActor;

			Human human = hActor.Human;
			HumanFace face = human.face;

			_human = human;
			_face = face;

			_originalFacialExpression = new CustomFacialExpression(human);
			_elmc = face.eyeLookMatCtrl;
			_elmcFromHuman[human] = face.eyeLookMatCtrl;
			_ahegaoHActorFromHuman[human] = this;

			_previousGaugeValue = hActor.GaugeValue;
		}

		public static void Initialize()
		{
			AhegaoComponent.Started += OnAhegaoComponentStarted;
			AhegaoComponent.Destroyed += OnAhegaoComponentDestroyed;
		}



		/*HOOKS*/
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
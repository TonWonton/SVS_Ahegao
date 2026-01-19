#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using SV.H;
using SV.H.Words;

using Logging = SVS_Ahegao.AhegaoPlugin.Logging;


namespace SVS_Ahegao
{
	public class AhegaoComponent : MonoBehaviour
	{
		/*INSTANCE*/
		public static AhegaoComponent? Instance { get; private set; }
		private HScene _hScene = null!;
		private GeneralWords.SetCategory _currentCategory;
		private GeneralWords.SetCategory _previousCategory;
		private float _currentSpeed = 0f;

		/*VARIABLES*/
		private List<AhegaoHActor> _ahegaoHActorList = new List<AhegaoHActor>();
		private List<AhegaoHActor> _femaleAhegaoHActorList = new List<AhegaoHActor>();
		private List<AhegaoHActor> _maleAhegaoHActorList = new List<AhegaoHActor>();
		private bool _isFinishProc = false;

		//Events
		public static event Action? Started;
		public static event Action? Destroyed;



		/*METHODS*/
		//Category
		private void SetCategory(GeneralWords.SetCategory category)
		{
			if (_currentCategory != category)
			{
				//Set category
				_previousCategory = _currentCategory;
				_currentCategory = category;

				//Check and set _isFinishProc
				switch (category)
				{
					case GeneralWords.SetCategory.BeforeFinish:
					{
						_isFinishProc = true;
						break;
					}

					case GeneralWords.SetCategory.BeforeFinishSame:
					{
						_isFinishProc = true;
						break;
					}
				}

				//Check finish proc and update ahegaos
				CheckFinishProc();
				UpdateAhegaos();
			}
		}

		private void SetRate(float rate)
		{
			if (_currentSpeed != rate)
			{
				float speedThreshold = AhegaoPlugin.faintnessSpeedThreshold.Value;
				float previousSpeed = _currentSpeed;
				_currentSpeed = rate;

				if (rate >= speedThreshold && previousSpeed < speedThreshold ||
					rate < speedThreshold && previousSpeed >= speedThreshold)
				{
					ProcessSpeedChange();
				}
			}
		}

		//Update all
		public void UpdateAhegaos()
		{
			switch (_currentCategory)
			{
				case GeneralWords.SetCategory.None:
				{
					if (_previousCategory == GeneralWords.SetCategory.BeforeFinish ||
						_previousCategory == GeneralWords.SetCategory.BeforeFinishSame)
					{
						//Do nothing if right after finish
					}
					else
					{
						ProcessFaintnessAhegao();
					}

					break;
				}

				case GeneralWords.SetCategory.BeforeFinish:
				{
					//Process orgasm ahegao if enabled, else process faintness ahegao
					if (AhegaoPlugin.ahegaoOnOrgasm.Value)
					{
						ProcessOrgasmAhegaoSingle();
					}
					else
					{
						ProcessFaintnessAhegao();
					}
					return;
				}
				case GeneralWords.SetCategory.BeforeFinishSame:
				{
					//Process orgasm ahegao if enabled, else process faintness ahegao
					if (AhegaoPlugin.ahegaoOnOrgasm.Value)
					{
						ProcessOrgasmAhegaoTogether();
					}
					else
					{
						ProcessFaintnessAhegao();
					}
					return;
				}
				case GeneralWords.SetCategory.AfterFinish:
				{
					ProcessFaintnessAhegao();
					return;
				}
				case GeneralWords.SetCategory.AfterFinishSame:
				{
					ProcessFaintnessAhegao();
					return;
				}
				default:
				{
					ProcessFaintnessAhegao();
					return;
				}
			}
		}

		private void ProcessOrgasmAhegaoSingle()
		{
			bool maleOrgasmProc = false;
			bool femaleOrgasmProc = false;

			//Check who can finish
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				HActor hActor = ahegaoHActor.HActor;
				GeneralWords generalWords = hActor.WordPlayer.General;
				bool isMan = hActor.IsMan;

				if (ahegaoHActor.CanFinish)
				{
					switch (generalWords._postureType)
					{
						case FlagManager.PostureType.None:
						{
							break;
						}

						case FlagManager.PostureType.Caress:
						{
							if (isMan == false) femaleOrgasmProc = true;
							break;
						}

						case FlagManager.PostureType.Service:
						{
							if (isMan) maleOrgasmProc = true;
							break;
						}

						case FlagManager.PostureType.Insert:
						{
							if (isMan) maleOrgasmProc = true;
							else femaleOrgasmProc = true;
							break;
						}

						case FlagManager.PostureType.Les:
						{
							if (isMan == false) femaleOrgasmProc = true;
							break;
						}
					}
				}
			}

			//Process orgasm procs
			if (maleOrgasmProc)
			{
				foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
				{
					maleAhegaoHActor.SetAhegaoState(AhegaoState.Orgasm);
				}
			}

			if (femaleOrgasmProc)
			{
				foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
				{
					femaleAhegaoHActor.SetAhegaoState(AhegaoState.Orgasm);
				}
			}
		}

		private void ProcessOrgasmAhegaoTogether()
		{
			//Set ahegao state for everyone
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				ahegaoHActor.SetAhegaoState(AhegaoState.Orgasm);
			}
		}

		private void ProcessFaintnessAhegao()
		{
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				//Check faintness proc and _currentSpeed
				if (ahegaoHActor.IsFaintnessProc)
				{
					if (_currentSpeed < AhegaoPlugin.faintnessSpeedThreshold.Value)
					{
						ahegaoHActor.SetAhegaoState(AhegaoState.Faintness);
					}
					else
					{
						ahegaoHActor.SetAhegaoState(AhegaoState.FaintnessSpeed);
					}
				}

				//If no condition met set to none
				else
				{
					ahegaoHActor.SetAhegaoState(AhegaoState.None);
				}
			}
		}

		private void ProcessSpeedChange()
		{
			float speedThreshold = AhegaoPlugin.faintnessSpeedThreshold.Value;

			//Check if over speed threshold
			if (_currentSpeed >= speedThreshold)
			{
				foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
				{
					//Set state to FaintnessSpeed if Faintness
					if (ahegaoHActor.AhegaoState == AhegaoState.Faintness)
					{
						ahegaoHActor.SetAhegaoState(AhegaoState.FaintnessSpeed);
					}
				}
			}
			else
			{
				foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
				{
					//Set state to Faintness if FaintnessSpeed
					if (ahegaoHActor.AhegaoState == AhegaoState.FaintnessSpeed)
					{
						ahegaoHActor.SetAhegaoState(AhegaoState.Faintness);
					}
				}
			}
		}

		public void ForceUpdateAhegaos()
		{
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				ahegaoHActor.UpdateAhegao();
			}
		}

		public void UpdateAllBlush()
		{
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				ahegaoHActor.UpdateBlushAmount();
			}
		}

		private void ResetAllAhegaos()
		{
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				ahegaoHActor.SetAhegaoState(AhegaoState.None);
			}
		}

		//Processing
		private void CheckFinishProc()
		{
			//Process finish and update previous gauge value
			if (_isFinishProc)
			{
				bool ahegaoHActorfinished = false;
				bool maleAhegaoHActorFinished = false;
				bool femaleAhegaoHActorFinished = false;

				//Check if AhegaoHActor finished and set procs
				foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
				{
					if (ahegaoHActor.Finished)
					{
						if (ahegaoHActor.HActor.IsMan)
						{
							maleAhegaoHActorFinished = true;
						}
						else
						{
							femaleAhegaoHActorFinished = true;
						}

						ahegaoHActorfinished = true;
					}

					ahegaoHActor.UpdatePreviousGaugeValue();
				}

				//Process finished procs
				if (ahegaoHActorfinished)
				{
					if (maleAhegaoHActorFinished)
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.IncrementFinishCount();
						}
					}

					if (femaleAhegaoHActorFinished)
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.IncrementFinishCount();
						}
					}

					//If someone finished reset _isFinishProc
					_isFinishProc = false;
				}
			}
		}

		//Add to collections
		private void AddAhegaoHActor(HActor? hActor)
		{
			//Create and add AhegaoHActor
			if (hActor != null)
			{
				AhegaoHActor ahegaoHActor = new AhegaoHActor(hActor);
				_ahegaoHActorList.Add(ahegaoHActor);

				if (hActor.IsMan == false)
				{
					_femaleAhegaoHActorList.Add(ahegaoHActor);
				}
				else
				{
					_maleAhegaoHActorList.Add(ahegaoHActor);
				}
			}
		}

		private void GetAhegaoHActors()
		{
			//Get HActors from HScene
			HActor[] hActors = _hScene.Actors;

			foreach (HActor hActor in hActors)
			{
				AddAhegaoHActor(hActor);
			}
		}



		/*EVENT HANDLING*/
		private void OnWordPlayerPostChangeState(GeneralWords.SetCategory category)
		{
			SetCategory(category);
		}

		private void OnWordPlayerPostPlay()
		{
			UpdateAllBlush();
		}



		//Initialization
		private void Start()
		{
			GetAhegaoHActors();
			Started?.Invoke();
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
				Destroyed?.Invoke();
			}
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(this);
			}
		}



		//Component specific hooks
		public static class Hooks
		{
			[HarmonyPostfix]
			[HarmonyPatch(typeof(HScene), nameof(HScene.Start))]
			public static void HScenePostStart(HScene __instance)
			{
				//Create AhegaoComponent and set _hScene
				AhegaoComponent ahegaoComponent = AhegaoPlugin.GetOrAddAhegaoComponent();
				ahegaoComponent._hScene = __instance;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HScene), nameof(HScene.Dispose))]
			public static void HScenePreDispose()
			{
				//Reset and destroy AhegaoComponent
				if (AhegaoPlugin.TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
				{
					ahegaoComponent.ResetAllAhegaos();
					Destroy(ahegaoComponent);
				}
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(WordPlayer), nameof(WordPlayer.ChangeState))]
			public static void WordPlayerPostChangeState(GeneralWords.SetCategory category)
			{
				if (AhegaoPlugin.TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
				{
					ahegaoComponent.OnWordPlayerPostChangeState(category);
				}
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(BaseWords), nameof(BaseWords.Play))]
			public static void BaseWordsPostPlay()
			{
				if (AhegaoPlugin.TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
				{
					ahegaoComponent.OnWordPlayerPostPlay();
				}
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(HScene.AnimeSpeeder), nameof(HScene.AnimeSpeeder.SetRate))]
			public static void HSceneAnimeSpeederPostSetRate(float rate)
			{
				if (AhegaoPlugin.TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
				{
					ahegaoComponent.SetRate(rate);
				}
			}
		}
	}
}
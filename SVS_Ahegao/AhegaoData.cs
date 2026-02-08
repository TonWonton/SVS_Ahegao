#nullable enable
using Character;


namespace SVS_Ahegao
{
	public enum AhegaoState
	{
		None = 0,
		Faintness,
		FaintnessSpeed,
		Orgasm
	}

	public enum AhegaoSettingType
	{
		None = 0,
		Faintness,
		FaintnessSpeed,
		Orgasm,
		PlusOne
	}

	public readonly struct CustomFacialExpression
	{
		public readonly int eyebrowPtn;
		public readonly int eyesPtn;
		public readonly float eyesOpenMax;
		public readonly float eyesRollAmount;
		public readonly float eyesCrossAmount;
		public readonly bool eyesHighlight;
		public readonly bool eyesBlink;
		public readonly bool eyesShake;
		public readonly float eyesShakeAmount;
		public readonly float eyesShakeFrequency;
		public readonly byte tearsLv;
		public readonly int mouthPtn;
		public readonly float mouthOpenMin;
		public readonly float mouthOpenMax;
		public readonly float blushAmount;

		//public CustomFacialExpression(
		//	int eyebrowPtn,
		//	int eyesPtn,
		//	float eyesOpenMax,
		//	float eyesRollAmount,
		//	float eyesCrossAmount,
		//	bool eyesBlink,
		//	bool eyesShake,
		//	float eyesShakeAmount,
		//	float eyesShakeFrequency,
		//	byte tearsLv,
		//	int mouthPtn,
		//	float mouthOpenMin,
		//	float mouthOpenMax,
		//	float mouthWidthMin,
		//	float mouthWidthMax,
		//	float mouthWidthVariation,
		//	float blushAmount
		//	)
		//{
		//	this.eyebrowPtn = eyebrowPtn;
		//	this.eyesPtn = eyesPtn;
		//	this.eyesOpenMax = eyesOpenMax;
		//	this.eyesRollAmount = eyesRollAmount;
		//	this.eyesCrossAmount = eyesCrossAmount;
		//	this.eyesBlink = eyesBlink;
		//	this.eyesShake = eyesShake;
		//	this.eyesShakeAmount = eyesShakeAmount;
		//	this.eyesShakeFrequency = eyesShakeFrequency;
		//	this.tearsLv = tearsLv;
		//	this.mouthPtn = mouthPtn;
		//	this.mouthOpenMin = mouthOpenMin;
		//	this.mouthOpenMax = mouthOpenMax;
		//	this.mouthWidthMin = mouthWidthMin;
		//	this.mouthWidthMax = mouthWidthMax;
		//	this.mouthWidthVariation = mouthWidthVariation;
		//	this.blushAmount = blushAmount;
		//}

		public CustomFacialExpression(Human human)
		{
			HumanFace face = human.face;

			eyebrowPtn = face.GetEyebrowPtn();
			eyesPtn = face.GetEyesPtn();
			eyesOpenMax = face.GetEyesOpenMax();
			eyesRollAmount = human.fileFace.eyeY;
			eyesCrossAmount = human.fileFace.eyeX;
			eyesHighlight = !human.fileStatus.hideEyesHighlight;
			eyesBlink = face.GetEyesBlinkFlag();
			eyesShake = face.GetEyesShaking();
			eyesShakeAmount = 0f;
			eyesShakeFrequency = 0f;
			tearsLv = human.fileStatus.tearsLv;
			mouthPtn = face.GetMouthPtn();
			mouthOpenMin = face.mouthCtrl.randScaleMin;
			mouthOpenMax = face.mouthCtrl.randScaleMax;
			blushAmount = human.fileStatus.hohoAkaRate;
		}

		public CustomFacialExpression(AhegaoState ahegaoState)
		{
			switch (ahegaoState)
			{
				case AhegaoState.Orgasm:
				{
					eyebrowPtn = AhegaoPlugin.orgasmEyebrowPtn.Value;
					eyesPtn = AhegaoPlugin.orgasmEyesPtn.Value;
					eyesOpenMax = AhegaoPlugin.orgasmEyesOpenMax.Value;
					eyesRollAmount = AhegaoPlugin.orgasmEyesRollAmount.Value;
					eyesCrossAmount = AhegaoPlugin.orgasmEyesCrossAmount.Value;
					eyesHighlight = AhegaoPlugin.orgasmEyesHighlight.Value;
					eyesBlink = AhegaoPlugin.orgasmEyesBlink.Value;
					eyesShake = AhegaoPlugin.orgasmEyesShake.Value;
					eyesShakeAmount = AhegaoPlugin.orgasmEyesShakeAmount.Value;
					eyesShakeFrequency = AhegaoPlugin.orgasmEyesShakeFrequency.Value;
					tearsLv = AhegaoPlugin.orgasmTearsLv.Value;
					mouthPtn = AhegaoPlugin.orgasmMouthPtn.Value;
					mouthOpenMin = AhegaoPlugin.orgasmMouthOpenMin.Value;
					mouthOpenMax = AhegaoPlugin.orgasmMouthOpenMax.Value;
					blushAmount = AhegaoPlugin.orgasmBlushAmount.Value;
					return;
				}

				case AhegaoState.Faintness:
				{
					eyebrowPtn = AhegaoPlugin.faintnessEyebrowPtn.Value;
					eyesPtn = AhegaoPlugin.faintnessEyesPtn.Value;
					eyesOpenMax = AhegaoPlugin.faintnessEyesOpenMax.Value;
					eyesRollAmount = AhegaoPlugin.faintnessEyesRollAmount.Value;
					eyesCrossAmount = AhegaoPlugin.faintnessEyesCrossAmount.Value;
					eyesHighlight = AhegaoPlugin.faintnessEyesHighlight.Value;
					eyesBlink = AhegaoPlugin.faintnessEyesBlink.Value;
					eyesShake = AhegaoPlugin.faintnessEyesShake.Value;
					eyesShakeAmount = AhegaoPlugin.faintnessEyesShakeAmount.Value;
					eyesShakeFrequency = AhegaoPlugin.faintnessEyesShakeFrequency.Value;
					tearsLv = AhegaoPlugin.faintnessTearsLv.Value;
					mouthPtn = AhegaoPlugin.faintnessMouthPtn.Value;
					mouthOpenMin = AhegaoPlugin.faintnessMouthOpenMin.Value;
					mouthOpenMax = AhegaoPlugin.faintnessMouthOpenMax.Value;
					blushAmount = AhegaoPlugin.faintnessBlushAmount.Value;
					return;
				}

				case AhegaoState.FaintnessSpeed:
				{
					eyebrowPtn = AhegaoPlugin.faintnessSpeedEyebrowPtn.Value;
					eyesPtn = AhegaoPlugin.faintnessSpeedEyesPtn.Value;
					eyesOpenMax = AhegaoPlugin.faintnessSpeedEyesOpenMax.Value;
					eyesRollAmount = AhegaoPlugin.faintnessSpeedEyesRollAmount.Value;
					eyesCrossAmount = AhegaoPlugin.faintnessSpeedEyesCrossAmount.Value;
					eyesHighlight = AhegaoPlugin.faintnessSpeedEyesHighlight.Value;
					eyesBlink = AhegaoPlugin.faintnessSpeedEyesBlink.Value;
					eyesShake = AhegaoPlugin.faintnessSpeedEyesShake.Value;
					eyesShakeAmount = AhegaoPlugin.faintnessSpeedEyesShakeAmount.Value;
					eyesShakeFrequency = AhegaoPlugin.faintnessSpeedEyesShakeFrequency.Value;
					tearsLv = AhegaoPlugin.faintnessSpeedTearsLv.Value;
					mouthPtn = AhegaoPlugin.faintnessSpeedMouthPtn.Value;
					mouthOpenMin = AhegaoPlugin.faintnessSpeedMouthOpenMin.Value;
					mouthOpenMax = AhegaoPlugin.faintnessSpeedMouthOpenMax.Value;
					blushAmount = AhegaoPlugin.faintnessSpeedBlushAmount.Value;
					return;
				}

				default:
				{
					eyebrowPtn = 0;
					eyesPtn = 0;
					eyesOpenMax = 0f;
					eyesRollAmount = 0f;
					eyesCrossAmount = 0f;
					eyesHighlight = false;
					eyesBlink = false;
					eyesShake = false;
					eyesShakeAmount = 0f;
					eyesShakeFrequency = 0f;
					tearsLv = 0;
					mouthPtn = 0;
					mouthOpenMin = 0f;
					mouthOpenMax = 0f;
					blushAmount = 0f;
					return;
				}
			}
		}

		public CustomFacialExpression()
		{
			eyebrowPtn = 0;
			eyesPtn = 0;
			eyesOpenMax = 0f;
			eyesRollAmount = 0f;
			eyesCrossAmount = 0f;
			eyesHighlight = false;
			eyesBlink = false;
			eyesShake = false;
			eyesShakeAmount = 0f;
			eyesShakeFrequency = 0f;
			tearsLv = 0;
			mouthPtn = 0;
			mouthOpenMin = 0f;
			mouthOpenMax = 0f;
			blushAmount = 0f;
		}

	}
}
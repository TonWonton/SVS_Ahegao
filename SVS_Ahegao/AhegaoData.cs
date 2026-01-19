#nullable enable


using Character;

namespace SVS_Ahegao
{
	public enum AhegaoState
	{
		None = 0,
		Orgasm,
		Faintness,
		FaintnessSpeed
	}

	public enum AhegaoType
	{
		Orgasm,
		Faintness,
		FaintnessSpeed
	}

	public readonly struct CustomFacialExpression
	{
		public readonly int eyebrowPtn;
		public readonly int eyesPtn;
		public readonly float eyesOpenMax;
		public readonly int mouthPtn;
		public readonly float mouthOpenMin;
		public readonly float mouthOpenMax;
		public readonly byte tearsLv;
		public readonly float blushAmount;
		public readonly bool eyesHighlight;
		public readonly bool eyesBlink;
		public readonly bool eyesShake;
		public readonly float eyesRollAmount;
		public readonly float eyesCrossAmount;

		public CustomFacialExpression(
			int eyebrowPtn,
			int eyesPtn,
			float eyesOpenMax,
			int mouthPtn,
			float mouthOpenMin,
			float mouthOpenMax,
			byte tearsLv,
			float blushAmount,
			bool eyesHighlight,
			bool eyesBlink,
			bool eyesShake,
			float eyesRollAmount,
			float eyesCrossAmount)
		{
			this.eyebrowPtn = eyebrowPtn;
			this.eyesPtn = eyesPtn;
			this.eyesOpenMax = eyesOpenMax;
			this.mouthPtn = mouthPtn;
			this.mouthOpenMin = mouthOpenMin;
			this.mouthOpenMax = mouthOpenMax;
			this.tearsLv = tearsLv;
			this.blushAmount = blushAmount;
			this.eyesHighlight = eyesHighlight;
			this.eyesBlink = eyesBlink;
			this.eyesShake = eyesShake;
			this.eyesRollAmount = eyesRollAmount;
			this.eyesCrossAmount = eyesCrossAmount;
			this.eyesRollAmount = eyesRollAmount;
			this.eyesCrossAmount = eyesCrossAmount;
		}

		public CustomFacialExpression(Human human)
		{
			HumanFace face = human.face;

			eyebrowPtn = face.GetEyebrowPtn();
			eyesPtn = face.GetEyesPtn();
			eyesOpenMax = face.GetEyesOpenMax();
			mouthPtn = face.GetMouthPtn();
			mouthOpenMin = face.GetMouthOpenMin();
			mouthOpenMax = face.GetMouthOpenMax();
			tearsLv = human.fileStatus.tearsLv;
			blushAmount = human.fileStatus.hohoAkaRate;
			eyesHighlight = !face.fileStatus.hideEyesHighlight;
			eyesBlink = face.GetEyesBlinkFlag();
			eyesShake = face.GetEyesShaking();
			eyesRollAmount = human.fileFace.eyeY;
			eyesCrossAmount = human.fileFace.eyeX;
		}

		public CustomFacialExpression(AhegaoType ahegaoType)
		{
			switch (ahegaoType)
			{
				case AhegaoType.Orgasm:
				{
					eyebrowPtn = AhegaoPlugin.orgasmEyebrowPtn.Value;
					eyesPtn = AhegaoPlugin.orgasmEyesPtn.Value;
					eyesOpenMax = AhegaoPlugin.orgasmEyesOpenMax.Value;
					mouthPtn = AhegaoPlugin.orgasmMouthPtn.Value;
					mouthOpenMin = AhegaoPlugin.orgasmMouthOpenMin.Value;
					mouthOpenMax = AhegaoPlugin.orgasmMouthOpenMax.Value;
					tearsLv = AhegaoPlugin.orgasmTearsLv.Value;
					blushAmount = AhegaoPlugin.orgasmBlushAmount.Value;
					eyesHighlight = AhegaoPlugin.orgasmEyesHighlight.Value;
					eyesBlink = AhegaoPlugin.orgasmEyesBlink.Value;
					eyesShake = AhegaoPlugin.orgasmEyesShake.Value;
					eyesRollAmount = AhegaoPlugin.orgasmEyesRollAmount.Value;
					eyesCrossAmount = AhegaoPlugin.orgasmEyesCrossAmount.Value;
					return;
				}

				case AhegaoType.Faintness:
				{
					eyebrowPtn = AhegaoPlugin.faintnessEyebrowPtn.Value;
					eyesPtn = AhegaoPlugin.faintnessEyesPtn.Value;
					eyesOpenMax = AhegaoPlugin.faintnessEyesOpenMax.Value;
					mouthPtn = AhegaoPlugin.faintnessMouthPtn.Value;
					mouthOpenMin = AhegaoPlugin.faintnessMouthOpenMin.Value;
					mouthOpenMax = AhegaoPlugin.faintnessMouthOpenMax.Value;
					tearsLv = AhegaoPlugin.faintnessTearsLv.Value;
					blushAmount = AhegaoPlugin.faintnessBlushAmount.Value;
					eyesHighlight = AhegaoPlugin.faintnessEyesHighlight.Value;
					eyesBlink = AhegaoPlugin.faintnessEyesBlink.Value;
					eyesShake = AhegaoPlugin.faintnessEyesShake.Value;
					eyesRollAmount = AhegaoPlugin.faintnessEyesRollAmount.Value;
					eyesCrossAmount = AhegaoPlugin.faintnessEyesCrossAmount.Value;
					return;
				}

				case AhegaoType.FaintnessSpeed:
				{
					eyebrowPtn = AhegaoPlugin.faintnessSpeedEyebrowPtn.Value;
					eyesPtn = AhegaoPlugin.faintnessSpeedEyesPtn.Value;
					eyesOpenMax = AhegaoPlugin.faintnessSpeedEyesOpenMax.Value;
					mouthPtn = AhegaoPlugin.faintnessSpeedMouthPtn.Value;
					mouthOpenMin = AhegaoPlugin.faintnessSpeedMouthOpenMin.Value;
					mouthOpenMax = AhegaoPlugin.faintnessSpeedMouthOpenMax.Value;
					tearsLv = AhegaoPlugin.faintnessSpeedTearsLv.Value;
					blushAmount = AhegaoPlugin.faintnessSpeedBlushAmount.Value;
					eyesHighlight = AhegaoPlugin.faintnessSpeedEyesHighlight.Value;
					eyesBlink = AhegaoPlugin.faintnessSpeedEyesBlink.Value;
					eyesShake = AhegaoPlugin.faintnessSpeedEyesShake.Value;
					eyesRollAmount = AhegaoPlugin.faintnessSpeedEyesRollAmount.Value;
					eyesCrossAmount = AhegaoPlugin.faintnessSpeedEyesCrossAmount.Value;
					return;
				}

				default:
				{
					eyebrowPtn = 0;
					eyesPtn = 0;
					eyesOpenMax = 1f;
					mouthPtn = 0;
					mouthOpenMin = 0f;
					mouthOpenMax = 0f;
					tearsLv = 0;
					blushAmount = 0f;
					eyesHighlight = true;
					eyesBlink = false;
					eyesShake = false;
					eyesRollAmount = 0f;
					eyesCrossAmount = 0f;
					return;
				}
			}
		}
	}
}
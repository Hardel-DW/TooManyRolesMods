using Hazel;
using System.Collections.Generic;
using UnityEngine;

namespace RolesMods.Utility {
	public static class Utils {
		private static readonly FloatRange XRange = new FloatRange(-40f, 40f);
		private static readonly FloatRange YRange = new FloatRange(-40f, 40f);

		public static void WriteVector2(this MessageWriter writer, Vector2 vec) {
			ushort value = (ushort) (XRange.ReverseLerp(vec.x) * 65535f);
			ushort value2 = (ushort) (YRange.ReverseLerp(vec.y) * 65535f);
			writer.Write(value);
			writer.Write(value2);
		}

		public static Vector2 ReadVector2(this MessageReader reader) {
			float v = (float) reader.ReadUInt16() / 65535f;
			float v2 = (float) reader.ReadUInt16() / 65535f;
			return new Vector2(XRange.Lerp(v), YRange.Lerp(v2));
		}
    }

	public class FloatRange {
		public float Last {
			get; private set;
		}

		public float Width {
			get {
				return this.max - this.min;
			}
		}

		public FloatRange(float min, float max) {
			this.min = min;
			this.max = max;
		}

		public float ChangeRange(float y, float min, float max) {
			return Mathf.Lerp(min, max, (y - this.min) / this.Width);
		}

		public float Clamp(float value) {
			return Mathf.Clamp(value, this.min, this.max);
		}

		public bool Contains(float t) {
			return this.min <= t && this.max >= t;
		}

		public float CubicLerp(float v) {
			if (this.min >= this.max) {
				return this.min;
			}
			v = Mathf.Clamp(0f, 1f, v);
			return v * v * v * (this.max - this.min) + this.min;
		}

		public float EitherOr() {
			if (UnityEngine.Random.value <= 0.5f) {
				return this.max;
			}
			return this.min;
		}

		public float LerpUnclamped(float v) {
			return Mathf.LerpUnclamped(this.min, this.max, v);
		}

		public float Lerp(float v) {
			return Mathf.Lerp(this.min, this.max, v);
		}

		public float ExpOutLerp(float v) {
			return this.Lerp(1f - Mathf.Pow(2f, -10f * v));
		}

		public static float ExpOutLerp(float v, float min, float max) {
			return Mathf.Lerp(min, max, 1f - Mathf.Pow(2f, -10f * v));
		}

		public static float Next(float min, float max) {
			return UnityEngine.Random.Range(min, max);
		}

		public float Next() {
			return this.Last = UnityEngine.Random.Range(this.min, this.max);
		}

		public IEnumerable<float> Range(int numStops) {
			float num;
			for (float i = 0f; i <= (float) numStops; i = num) {
				yield return Mathf.Lerp(this.min, this.max, i / (float) numStops);
				num = i + 1f;
			}
			yield break;
		}

		public IEnumerable<float> RandomRange(int numStops) {
			float num;
			for (float i = 0f; i <= (float) numStops; i = num) {
				yield return this.Next();
				num = i + 1f;
			}
			yield break;
		}

		internal float ReverseLerp(float t) {
			return Mathf.Clamp((t - this.min) / this.Width, 0f, 1f);
		}

		public static float ReverseLerp(float t, float min, float max) {
			float num = max - min;
			return Mathf.Clamp((t - min) / num, 0f, 1f);
		}

		public IEnumerable<float> SpreadToEdges(int stops) {
			return FloatRange.SpreadToEdges(this.min, this.max, stops);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005ACD File Offset: 0x00003CCD
		public IEnumerable<float> SpreadEvenly(int stops) {
			return FloatRange.SpreadEvenly(this.min, this.max, stops);
		}

		public static IEnumerable<float> SpreadToEdges(float min, float max, int stops) {
			if (stops == 1) {
				yield break;
			}
			int num;
			for (int i = 0; i < stops; i = num) {
				yield return Mathf.Lerp(min, max, (float) i / ((float) stops - 1f));
				num = i + 1;
			}
			yield break;
		}

		public static IEnumerable<float> SpreadEvenly(float min, float max, int stops) {
			float step = 1f / ((float) stops + 1f);
			for (float i = step; i < 1f; i += step) {
				yield return Mathf.Lerp(min, max, i);
			}
			yield break;
		}

		public float min;
		public float max;
	}
}

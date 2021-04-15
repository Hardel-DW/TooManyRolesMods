using System.Collections;
using UnityEngine;

namespace RolesMods.Utility {
    class Animation {
		public static IEnumerator FadeOut(float speed, GameObject gameObject, bool destroyAtEnd) {
			float alpha = 1f;
			SpriteRenderer renderer = null;

			if (gameObject.GetComponent<SpriteRenderer>() != null)
				renderer = gameObject.GetComponent<SpriteRenderer>();
			else
				yield return true;

			while (alpha > 0) {
				if (gameObject == null)
					break;

				alpha = Mathf.Max(0, alpha - (speed * Time.deltaTime));
				renderer.color = new Color(1f, 1f, 1f, alpha);
				yield return new WaitForEndOfFrame();
			}

			if (destroyAtEnd)
				Object.Destroy(gameObject);

			yield return true;
		}

		public static IEnumerator FadeIn(float speed, GameObject gameObject, bool destroyAtEnd) {
			float alpha = 0f;
			SpriteRenderer renderer = null;

			if (gameObject.GetComponent<SpriteRenderer>() != null)
				renderer = gameObject.GetComponent<SpriteRenderer>();
			else
				yield return true;

			while (alpha < 1) {
				if (gameObject == null)
					break;

				alpha = Mathf.Max(0, alpha + (speed * Time.deltaTime));
				renderer.color = new Color(1f, 1f, 1f, alpha);
				yield return new WaitForEndOfFrame();
			}

			if (destroyAtEnd)
				Object.Destroy(gameObject);

			yield return true;
		}

	}
}

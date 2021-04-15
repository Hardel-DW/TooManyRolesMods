using System;
using System.Collections;
using UnityEngine;

namespace RolesMods.Utility {
    public static class GameObjectUtils {

        /// <summary>
        /// Change the player size smoothly.
        /// </summary>
        /// <param name="Player">Player who change scale</param>
        /// <param name="Duration">The duration in float (Seconds)</param>
        /// <param name="Size">Size, the new size of player after ended effect</param>
        public static IEnumerator ChangeSize(GameObject gameObject, float Duration, float Size) {
            float elapsedTime = 0;

            while (elapsedTime < Duration) {
                gameObject.transform.localScale = Vector2.Lerp(gameObject.transform.localScale, new Vector2(Size, Size), (elapsedTime / Duration));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            yield return true;
        }

        /// <summary>
        /// Change the player size smoothly.
        /// </summary>
        /// <param name="Player">Player who change scale</param>
        /// <param name="Duration">The duration in float (Seconds)</param>
        /// <param name="Size">Size, the new size of player after ended effect</param>
        /// <param name="EndedAction">Do Something when it's ended of function</param>
        public static IEnumerator ChangeSize(GameObject gameObject, float Duration, float Size, Action EndedAction) {
            float elapsedTime = 0;

            while (elapsedTime < Duration) {
                gameObject.transform.localScale = Vector2.Lerp(gameObject.transform.localScale, new Vector2(Size, Size), (elapsedTime / Duration));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            EndedAction();
            yield return true;
        }

    }
}

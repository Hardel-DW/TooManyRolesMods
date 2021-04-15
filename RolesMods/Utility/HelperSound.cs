using System.Collections;
using UnityEngine;

namespace RolesMods.Utility {
    public class HelperSound {

        public static IEnumerator PlaySoundOverride(AudioClip clip, bool isRepeated, float volume, Vector2 position) {
            GameObject soundManager = new GameObject { name = "Sound Manager" };
            AudioSource audio = soundManager.AddComponent<AudioSource>();
            if (SoundManager.Instance != null) audio.outputAudioMixerGroup = (isRepeated ? SoundManager.Instance.musicMixer : SoundManager.Instance.sfxMixer);
            audio.volume = volume;
            audio.loop = isRepeated;
            audio.clip = clip;
            audio.minDistance = 0f;
            audio.maxDistance = 10f;
            audio.Play();
            soundManager.transform.position = position;
            soundManager.SetActive(true);

            while (audio.isPlaying) 
                yield return null;

            Object.Destroy(soundManager);

            yield return true;
        }
    }
}

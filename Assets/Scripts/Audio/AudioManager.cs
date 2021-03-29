using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Sound
    {
        playerShoot,
        explosion,
        playerWin,
        playerLose
    }

    [System.Serializable]
    public struct SoundAudioClip
    {
        [SerializeField] private Sound _sound;
        public Sound _Sound => _sound;

        [SerializeField] private AudioClip _audioClip;
        public AudioClip _AudioClip => _audioClip;
    }

    [SerializeField] private GameObject _audioSourcePrefab;
    public GameObject _AudioSourcePrefab => _audioSourcePrefab;

    [SerializeField] private SoundAudioClip[] _soundAudioClips;
    public SoundAudioClip[] _SoundAudioClips => _soundAudioClips;

    private ObjectPool.Pool poolTransferer = new ObjectPool.Pool();

    private AudioClip GetAudioClip(Sound sound)
    {
        for (int a = 0; a < _soundAudioClips.Length; a++)
        {
            if(sound == _soundAudioClips[a]._Sound)
            {
                return _soundAudioClips[a]._AudioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    public void PlaySound(Sound sound)
    {
        AudioPlayer audioPlayer = poolTransferer.Aquire(_audioSourcePrefab).GetComponent<AudioPlayer>();
        audioPlayer.PlaySound(GetAudioClip(sound));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, IPoolable
{
    #region ObjectPool

    public GameObject GameObject { get; set; }

    public ObjectPool.Pool Pool { get; set; }

    public void OnAquire()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    #endregion

    [SerializeField] private AudioSource _audioSource;
    public AudioSource _AudioSource => _audioSource;

    private static float RELEASE_DELAY = 1f;

    private void Awake()
    {
        GameObject = this.gameObject;
        Pool = new ObjectPool.Pool();
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);

        StartCoroutine("ReleaseWithDelay");
    }

    IEnumerator ReleaseWithDelay()
    {
        yield return new WaitForSeconds(RELEASE_DELAY);
        Pool.Release(this);
    }
}

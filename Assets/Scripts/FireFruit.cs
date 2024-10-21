using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFireEffect { HandEffect, FirePist}

public class FireFruit : MonoBehaviour
{
    [SerializeField] private GameObject[] _fireEffects;

    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip[] _audioClips;

    [SerializeField] private float _firePistdis;

    private GameObject _handEffect;

    private GameObject _firePist; 

    
    public void ComeOutHandFireEffect()
    {
        _audio.clip = _audioClips[(int)EFireEffect.HandEffect];
        _audio.loop = true;
        _audio.Play();
        _handEffect = Instantiate(_fireEffects[(int)EFireEffect.HandEffect], transform);
        _handEffect.transform.localPosition = Vector3.zero;
    }

    public void ComeInHandFireEffect()
    {
        if (_handEffect != null)
        {
            Destroy(_handEffect);
        }
    }

    public void FirePist()
    {
        _audio.Stop();
        _audio.PlayOneShot(_audioClips[(int)EFireEffect.FirePist]);
        _firePist = Instantiate(_fireEffects[(int)EFireEffect.FirePist], transform);
        _firePist.transform.localPosition = new Vector3(0, 0, _firePistdis);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public enum EEarthQuakeEffect { HandEffect, SkillEffect  }

public class EarthQuakeFruit : MonoBehaviour
{

    [SerializeField] private GameObject[] _earthQuakeEffects;

    private GameObject _earthQuakeEnerge;

    private GameObject _earthQuakeEffect;

    [SerializeField] private GameObject _earthQuakeRange;

    private bool _isUseSkill;

    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip[] _audioClips;

    private XRDirectInteractor _interactor;

    [SerializeField] private float _vibration;

    [SerializeField] private float _vibrateDurateTime; 

    public void GetController(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject as XRDirectInteractor;
        Debug.Log(_interactor.gameObject.name);
    }

    public void GetEarthQuakeEnerge()
    {
        _audio.clip = _audioClips[(int)EEarthQuakeEffect.HandEffect];
        _audio.Play();
        _isUseSkill = true;
        _earthQuakeEnerge = Instantiate(_earthQuakeEffects[(int)EEarthQuakeEffect.HandEffect], transform);
        _earthQuakeEnerge.transform.localPosition = Vector3.zero;
        _earthQuakeRange.SetActive(true);
    }

    public void ReleaseEarthQuakeEnerge()
    {
        if (_earthQuakeEnerge != null)
        {
            Destroy(_earthQuakeEnerge);
        }
        _isUseSkill = false;
        _earthQuakeRange.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ÁöÁøÀÏ¾î³¿!");

        if(other.gameObject.tag == "EarthQuakeRange" && _isUseSkill == true)
        {
            _audio.PlayOneShot(_audioClips[(int)EEarthQuakeEffect.SkillEffect]);

            _interactor.SendHapticImpulse(_vibration, _vibrateDurateTime);

            _earthQuakeEffect = Instantiate(_earthQuakeEffects[(int)EEarthQuakeEffect.SkillEffect]);
            _earthQuakeEffect.transform.position = other.ClosestPoint(transform.position);
            Vector3 effectDir = (other.ClosestPoint(transform.position) - transform.position).normalized;
            _earthQuakeEffect.transform.rotation = Quaternion.LookRotation(effectDir);

        }
 
    }

   


}

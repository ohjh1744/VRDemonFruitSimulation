using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public enum EHakiSound { Select, ChargingEnergy, ReleaseEnergy }

public enum EHakiEffect { Light, Bolt }

public class Haki : MonoBehaviour
{
    [SerializeField] private GameObject[] _HakiEffect;

    [SerializeField] private GameObject _skillEffect;

    [SerializeField] private int _canUseSkillGage; // 3���� �� ����

    [SerializeField] private int _gatherGageAmount; // 1�� �� ����

    [SerializeField] private float _gatherDurateTime; //1�ʷ� �� ����

    [SerializeField] private float _skillFinishTime; //7�ʷ� �� ����

    [SerializeField] private float _spritEffectEuler; //

    [SerializeField] private float _lightIntensity; //빛세기

    [SerializeField] private float _vibration; // ��������

    [SerializeField] private float _maxVibration; // ��������

    [SerializeField] private float _vibrateDurateTime; //�����ð�

    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip[] _audioClips;

    private Coroutine _gatherEnergyRoutine;

    private Coroutine _releaseEnergyRoutine;

    private WaitForSeconds _gatherSeconds;

    private WaitForSeconds _releaseSeconds;

    private float _gage;

    private bool _isUseSkill;

    private GameObject _galaxyDevide;

    private GameObject _lightEffect;

    private Vector3 _spritEffectdir;

    private List<GameObject> _boltEffects;

    private InputDevice _controller;

    private void Start()
    {
        _boltEffects = new List<GameObject>();
        _gatherSeconds = new WaitForSeconds(_gatherDurateTime);
        _releaseSeconds = new WaitForSeconds(_skillFinishTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "RightHand")
        {
            Debug.Log("hi");
            _controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
    }
    public void SelectHaki()
    {
        _audio.PlayOneShot(_audioClips[(int)EHakiSound.Select]);
    }
    public void GatherEnergy()
    {
        _gatherEnergyRoutine = StartCoroutine(UpEnergy());
    }

    public void ReleaseEnergy()
    {
        if (_gatherEnergyRoutine != null)
        {
            StopCoroutine(_gatherEnergyRoutine);
        }
        _gatherEnergyRoutine = null;
        _releaseEnergyRoutine = StartCoroutine(UseGalaxyDevide());
    }

    public void Reset()
    {
        if (_gatherEnergyRoutine != null)
        {
            for (int i = 0; i < _boltEffects.Count; i++)
            {
                if ((_boltEffects[i].activeSelf == true))
                {
                    Destroy(_boltEffects[i]);
                }
            }
            if (_lightEffect.activeSelf == true)
            {
                Destroy(_lightEffect);
            }
            _boltEffects.Clear();
            StopCoroutine(_gatherEnergyRoutine);
            _gatherEnergyRoutine = null;
        }
        if (_releaseEnergyRoutine != null)
        {
            if (_galaxyDevide.activeSelf == true)
            {
                Destroy(_galaxyDevide);
            }
            StopCoroutine(_releaseEnergyRoutine);
            _releaseEnergyRoutine = null;
        }

        _isUseSkill = false;
        _audio.loop = false;
        _audio.Stop();
    }

    private IEnumerator UpEnergy()
    {
        _gage = 0;
        if (_isUseSkill == false)
        {
            _audio.clip = _audioClips[(int)EHakiSound.ChargingEnergy];
            _audio.loop = true;
            _audio.Play();

            _lightEffect = Instantiate(_HakiEffect[(int)EHakiEffect.Light], transform);
            _lightEffect.transform.localPosition = Vector3.zero;
            Light light = _lightEffect.GetComponentInChildren<Light>();

            float vibration = 0;
            while (_gage < _canUseSkillGage)
            {
                vibration += _vibration;
                vibration = Mathf.Clamp(vibration, 0f, _maxVibration);
                _controller.SendHapticImpulse(0, vibration, _vibrateDurateTime);

                light.intensity += _lightIntensity;
                _spritEffectdir = Vector3.zero;

                _gage += _gatherGageAmount;
                GameObject boltEffect = Instantiate(_HakiEffect[(int)EHakiEffect.Bolt], transform);
                _boltEffects.Add(boltEffect);

                boltEffect.transform.localPosition = Vector3.zero;
                _spritEffectdir.y = _spritEffectEuler * (_boltEffects.Count - 1);
                boltEffect.transform.localRotation = Quaternion.Euler(_spritEffectdir);

                yield return _gatherSeconds;
            }
        }
    }

    private IEnumerator UseGalaxyDevide()
    {
        for (int i = 0; i < _boltEffects.Count; i++)
        {
            Destroy(_boltEffects[i]);
        }
        _boltEffects.Clear();
        Destroy(_lightEffect);

        if (_gage < _canUseSkillGage)
        {
            yield break;
        }

        _isUseSkill = true;

        _audio.clip = _audioClips[(int)EHakiSound.ReleaseEnergy];
        _audio.loop = true;
        _audio.Play();

        if (_gage >= _canUseSkillGage)
        {
            _galaxyDevide = Instantiate(_skillEffect, transform);
            _galaxyDevide.transform.localPosition = Vector3.zero;
        }

        yield return _releaseSeconds;

        Destroy(_galaxyDevide);
        Reset();
    }

}
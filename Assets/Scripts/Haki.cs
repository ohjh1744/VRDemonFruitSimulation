using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum EHakiSound { Select, ChargingEnergy, ReleaseEnergy }
public enum EHakiEffect { Light, Bolt, Energy }

public class Haki : MonoBehaviour
{
    [SerializeField] private GameObject[] _hakiEffect;

    [SerializeField] private int _canUseSkillGage; // 3���� �� ����

    [SerializeField] private int _gatherGageAmount; // 1�� �� ����

    [SerializeField] private float _gatherDurateTime; //1�ʷ� �� ����

    [SerializeField] private float _skillFinishTime; //7�ʷ� �� ����

    [SerializeField] private float _spritEffectEuler; 

    [SerializeField] private float _lightIntensity; //빛세기

    [SerializeField] private float _MaxlightIntensity; //빛세기

    [SerializeField] private int _boltEffectNum; // 1�� �� ����

    [SerializeField] private float _vibration; // ��������

    [SerializeField] private float _maxVibration; // ��������

    [SerializeField] private float _vibrateDurateTime; //�����ð�

    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip[] _audioClips;

    private XRDirectInteractor _interactor;

    private Coroutine _gatherEnergyRoutine;

    private Coroutine _releaseEnergyRoutine;

    private WaitForSeconds _gatherSeconds;

    private WaitForSeconds _releaseSeconds;

    private float _gage;

    private bool _isUseSkill;

    private GameObject _galaxyDevide;

    private GameObject _lightEffect;

    private Vector3 _boltEffectdir;

    private List<GameObject> _boltEffects;


    private void Start()
    {
        _boltEffects = new List<GameObject>();
        _gatherSeconds = new WaitForSeconds(_gatherDurateTime);
        _releaseSeconds = new WaitForSeconds(_skillFinishTime);
    }

    public void GetController(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject as XRDirectInteractor;
        Debug.Log(_interactor.gameObject.name);
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
        _releaseEnergyRoutine = StartCoroutine(UseGalaxyDevide());
    }

    public void Reset()
    {
        ResetGatherEnergy();
        ResetReleaseEnergy();
        _isUseSkill = false;
        _audio.loop = false;
        _audio.Stop();
    }

    private void ResetGatherEnergy()
    {
        if (_gatherEnergyRoutine != null)
        {
            foreach (GameObject bolt in _boltEffects)
            {
                if (bolt.activeSelf == true)
                {
                    Destroy(bolt);
                }
            }
            _boltEffects.Clear();

            if (_lightEffect.activeSelf == true)
            {
                Destroy(_lightEffect);
            }

            StopCoroutine(_gatherEnergyRoutine);
            _gatherEnergyRoutine = null;
        }
    }

    private void ResetReleaseEnergy()
    {
        if (_releaseEnergyRoutine != null)
        {
            if (_galaxyDevide.activeSelf == true)
            {
                Destroy(_galaxyDevide);
            }

            StopCoroutine(_releaseEnergyRoutine);
            _releaseEnergyRoutine = null;
        }
    }
    private IEnumerator UpEnergy()
    {
        _gage = 0;
        if (_isUseSkill == false)
        {
            _audio.clip = _audioClips[(int)EHakiSound.ChargingEnergy];
            _audio.loop = true;
            _audio.Play();

            _lightEffect = Instantiate(_hakiEffect[(int)EHakiEffect.Light], transform);
            _lightEffect.transform.localPosition = Vector3.zero;
            Light light = _lightEffect.GetComponentInChildren<Light>();

            float vibration = 0;
            while (true)
            {
                light.intensity += _lightIntensity;
                light.intensity = Mathf.Clamp(light.intensity, 0f, _MaxlightIntensity);

                vibration += _vibration;
                vibration = Mathf.Clamp(vibration, 0f, _maxVibration);
                _interactor.SendHapticImpulse(vibration, _vibrateDurateTime);

                _gage += _gatherGageAmount;
                _gage = Mathf.Clamp(_gage, 0f, _canUseSkillGage);

                if (_boltEffects.Count < _boltEffectNum)
                {
                    GameObject boltEffect = Instantiate(_hakiEffect[(int)EHakiEffect.Bolt], transform);
                    _boltEffects.Add(boltEffect);
                    boltEffect.transform.localPosition = Vector3.zero;
                    _boltEffectdir = Vector3.zero;
                    _boltEffectdir.y = _spritEffectEuler * (_boltEffects.Count - 1);
                    boltEffect.transform.localRotation = Quaternion.Euler(_boltEffectdir);
                }

                yield return _gatherSeconds;
            }
        }
    }

    private IEnumerator UseGalaxyDevide()
    {
        ResetGatherEnergy();

        if (_gage < _canUseSkillGage)
        {
            yield break;
        }

        _isUseSkill = true;

        _audio.clip = _audioClips[(int)EHakiSound.ReleaseEnergy];
        _audio.loop = false;
        _audio.Play();

        _galaxyDevide = Instantiate(_hakiEffect[(int)EHakiEffect.Energy], transform);
        _galaxyDevide.transform.localPosition = Vector3.zero;

        yield return _releaseSeconds;

        Reset();
    }

}
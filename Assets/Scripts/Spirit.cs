using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    [SerializeField] private GameObject _spritEffect;

    [SerializeField] private GameObject _skillEffect;

    [SerializeField] private int _canUseSkillGage; // 3으로 둘 예정

    [SerializeField] private int _gatherGageAmount; // 1로 둘 예정

    [SerializeField] private float _gatherDurateTime; //1초로 둘 예정

    [SerializeField] private float _skillFinishTime; //7초로 둘 예정

    [SerializeField] private float _spritEffectEuler; //45도씩 돌려서 생성

    private Coroutine _gatherEnergyRoutine;

    private Coroutine _releaseEnergyRoutine;

    private WaitForSeconds _gatherSeconds;

    private WaitForSeconds _releaseSeconds;

    private float _gage;

    private bool _isUseSkill;

    private GameObject _galaxyDevide;

    private Vector3 _spritEffectdir;

    private List<GameObject> _skillEffects;

    private void Start()
    {
        _skillEffects = new List<GameObject>();
        _gatherSeconds = new WaitForSeconds(_gatherDurateTime);
        _releaseSeconds = new WaitForSeconds(_skillFinishTime);
    }

    public void GatherEnergy()
    {
        _gatherEnergyRoutine = StartCoroutine(UpEnergy());
    }

    public void ReleaseEnergy()
    {
        if(_gatherEnergyRoutine != null)
        {     
            StopCoroutine(_gatherEnergyRoutine);
        }
        _gatherEnergyRoutine = null;
        _releaseEnergyRoutine = StartCoroutine(UseGalaxyDevide());
    }

    private IEnumerator UseGalaxyDevide()
    {
        for (int i = 0; i < _skillEffects.Count; i++)
        {
            Destroy(_skillEffects[i]);
        }
        _skillEffects.Clear();

        if (_gage < _canUseSkillGage)
        {
            yield break;
        }

        _isUseSkill = true;

        if(_gage >= _canUseSkillGage)
        {
            _galaxyDevide = Instantiate(_skillEffect, transform);
            _galaxyDevide.transform.localPosition = Vector3.zero;
        }

        yield return _releaseSeconds;

        Destroy(_galaxyDevide);
        _isUseSkill = false;
        _gage = 0;
        StopCoroutine(_releaseEnergyRoutine);
        _releaseEnergyRoutine = null;
    }

    private IEnumerator UpEnergy()
    {
        while (_isUseSkill == false && _gage < _canUseSkillGage)
        {
            _spritEffectdir = Vector3.zero;
            _gage += _gatherGageAmount;

            GameObject spiritEffect = Instantiate(_spritEffect, transform);
            _skillEffects.Add(spiritEffect);

            spiritEffect.transform.localPosition = Vector3.zero;
            _spritEffectdir.y = _spritEffectEuler * (_skillEffects.Count - 1);
            spiritEffect.transform.localRotation = Quaternion.Euler(_spritEffectdir);

    
            yield return _gatherSeconds;
        }
    }

  

}

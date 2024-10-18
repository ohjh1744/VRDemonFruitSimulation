using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EIceEffect {HandIceEffect, IceEffect }
public class IceFruit : MonoBehaviour
{
    [SerializeField] private GameObject[] _skillEffects;

    [SerializeField] private float _iceAgeWideTime;

    [SerializeField] private float _iceAgeWideRange;

    [SerializeField] private float _iceAgeDestroyTime;

    private GameObject _handEffect;

    private GameObject _iceAge;

    private bool _isUseSkill;

    private Coroutine _makeRoutine;

    private WaitForSeconds _makeRoutineSeconds;

    private Vector3 _iceAgeRange;

    private Coroutine _destroyRoutine;

    private WaitForSeconds _destroyRoutineSeconds;

    private void Start()
    {
        _makeRoutineSeconds = new WaitForSeconds(_iceAgeWideTime);
        _iceAgeRange = Vector3.zero;
        _destroyRoutineSeconds = new WaitForSeconds(_iceAgeDestroyTime);
    }

    private void ResetSkill()
    {
        if(_handEffect.activeSelf == true)
        {
            Destroy(_handEffect);
        }
        _isUseSkill = false;
    }

    public void ComeOutHandIceEffect()
    {
        _handEffect = Instantiate(_skillEffects[(int)EIceEffect.HandIceEffect], transform);
        _handEffect.transform.position = Vector3.zero;
        _isUseSkill = true;
    }

    public void ComeInHandIceEffect()
    {
        ResetSkill();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != null && _isUseSkill)
        {
            _iceAge = Instantiate(_skillEffects[(int)EIceEffect.IceEffect]);
            _iceAge.transform.position = collision.contacts[0].point;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _makeRoutine = StartCoroutine(MakeWideIceAge());
    }

    private void OnCollisionExit(Collision collision)
    {
        StopCoroutine(_makeRoutine);
        _destroyRoutine = StartCoroutine(DestroyIceAge());

    }

    private IEnumerator MakeWideIceAge()
    {
        while (true)
        {
            _iceAgeRange.x += _iceAgeWideRange;
            _iceAgeRange.z += _iceAgeWideRange;
            _iceAge.transform.localScale = _iceAgeRange;
            yield return _makeRoutineSeconds;
        }
    }

    private IEnumerator DestroyIceAge()
    {
        ParticleSystem[] particles = _iceAge.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            particle.loop = false;
        }

        yield return _destroyRoutineSeconds;

        Destroy(_iceAge);
        _destroyRoutine = null;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum EIceEffect {HandIceEffect, IceEffect }

public class IceFruit : MonoBehaviour
{
    [SerializeField] private GameObject[] _skillEffects;

    [SerializeField] private float _iceAgeMakeDurateTime;

    [SerializeField] private float _fruitScale;

    [SerializeField] private int _devilFruitLayer;

    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip _audioClip;

    private GameObject _handEffect;

    private GameObject _iceAge;

    private bool _isUseSkill;

    private Coroutine _makeRoutine;

    private WaitForSeconds _makeRoutineSeconds;

    private XRDirectInteractor _interactor;

    private void Start()
    {
        _makeRoutineSeconds = new WaitForSeconds(_iceAgeMakeDurateTime);
    }

    private void Update()
    {
        // ��ų ����ϰ� ���������� üũ
        if (_isUseSkill == true)
        {
            CheckContact();
        }
    }

    private void CheckContact()
    {

        Collider[] contacts = Physics.OverlapSphere(transform.position, _fruitScale);
        Debug.Log(contacts.Length);
        // ���� ������Ʈ�� ���ٸ�  ���̽������� ���� ����. 1�� �� �ϳ��� ���� ������Ʈ�� ����.
        if (contacts.Length == 1)
        {
            if (_makeRoutine != null)
            {
                Debug.Log("hhh");
                StopCoroutine(_makeRoutine);
                _makeRoutine = null;
            }
        }
        else
        {
            // ���� ������Ʈ�� �ִٸ� ���̽�����������
            foreach (Collider contact in contacts)
            {
                if (contact.gameObject.layer != _devilFruitLayer && _makeRoutine == null)
                {
                    _makeRoutine = StartCoroutine(MakeWideIceAge(contact.ClosestPoint(transform.position)));
                    break;
                }
            }
        }
    }

    public void ComeOutHandIceEffect()
    {
        _audio.loop = true;
        _audio.clip = _audioClip;
        _audio.Play();
        _handEffect = Instantiate(_skillEffects[(int)EIceEffect.HandIceEffect], transform);
        _handEffect.transform.localPosition = Vector3.zero;
        _isUseSkill = true;
    }

    public void ComeInHandIceEffect()
    {
        _audio.loop = false;
        _audio.Stop();
        if (_handEffect != null)
        {
            Destroy(_handEffect);
        }
        if(_makeRoutine != null)
        {
            StopCoroutine(_makeRoutine);
            _makeRoutine = null;
        }
        _isUseSkill = false;
    }

    private IEnumerator MakeWideIceAge(Vector3 iceAgePos)
    {
        while (true)
        {
            _iceAge = Instantiate(_skillEffects[(int)EIceEffect.IceEffect]);
            _iceAge.transform.position = iceAgePos;

            yield return _makeRoutineSeconds;

            _audio.Stop();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _fruitScale);
    }

}

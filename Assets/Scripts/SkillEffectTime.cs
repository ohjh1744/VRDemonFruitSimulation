using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectTime : MonoBehaviour
{
    [SerializeField] private float _time;
    
    private float _currentTIme;

    // Update is called once per frame
    void Update()
    {
        _currentTIme += Time.deltaTime;
        if(_currentTIme > _time)
        {
            Destroy(gameObject);
        }
    }
}

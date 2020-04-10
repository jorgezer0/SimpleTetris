using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemBurstController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        EventBus.OnAddPoints += OnAddPoints;
    }

    private void OnDestroy()
    {
        EventBus.OnAddPoints -= OnAddPoints;
    }

    private void OnAddPoints(object sender, EventArgs e)
    {
        var arg = (EventBus.ScoreArgs) e;

        _particleSystem.Emit(100 * arg.combo * arg.combo);
    }

}

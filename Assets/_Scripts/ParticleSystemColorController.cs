using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleSystemColorController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Renderer _particleRenderer;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleRenderer = _particleSystem.GetComponent<Renderer>();

        EventBus.OnLevelUp += OnLevelUp;
    }

    private void OnDestroy()
    {
        EventBus.OnLevelUp -= OnLevelUp;
    }

    private void OnLevelUp(object sender, EventArgs e)
    {
        StartCoroutine(ColorTransition());
    }

    private IEnumerator ColorTransition()
    {
        var intensity = Mathf.Pow(2, 5.5f);
        var newColor = RandomColor() * intensity;
        var oldColor = _particleRenderer.material.GetColor(EmissionColor);

        var noise = _particleSystem.noise;
        
        var i = 0f;
        while (i<1)
        {
            var lerpedColor = Color.Lerp(oldColor, newColor, i);
            _particleRenderer.material.SetColor(EmissionColor, lerpedColor);
            i += Time.deltaTime;

            noise.strength = 10 * (1 - i);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private Color RandomColor()
    {
        var newColor = new Color(
            Random.Range(0.15f, 0.45f),
            Random.Range(0.15f, 0.45f),
            Random.Range(0.15f, 0.45f));

        return newColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    private TextMesh _textMesh;
    private Renderer _textRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _textMesh = GetComponent<TextMesh>();
        _textRenderer = _textMesh.GetComponent<Renderer>();

        StartCoroutine(ColorTransition());
    }

    private IEnumerator ColorTransition()
    {
        while (true)
        {
            var newColor = Random.ColorHSV();
            newColor.a = 1f;
            var oldColor = _textRenderer.material.color;

            var i = 0f;
            while (i < 1)
            {
                _textRenderer.material.color = Color.Lerp(oldColor, newColor, i);
                i += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}

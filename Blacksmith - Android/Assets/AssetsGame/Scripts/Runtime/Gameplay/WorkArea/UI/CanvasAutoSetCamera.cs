using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAutoSetCamera : MonoBehaviour
{
    private void OnValidate()
    {
        var _canvas = GetComponent<Canvas>();
        if(_canvas == null)
        {
            Debug.LogError($"This Script Component only use for GameObject Canvas");
            return;
        }

        if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) return;
        _canvas.worldCamera = Camera.main;
    }
}

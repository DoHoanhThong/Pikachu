using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CANVASScaler : MonoBehaviour
{
    //Landscape Screen
    private const float WIDTH_DEFAULT = 1920f;
    private const float HEIGHT_DEFAULT = 1080f;

    private float _currentWidth;
    private float _currentHeight;
    private CanvasScaler _canvasScaler;

    private float _originScale = 0.5f;

    private void Start()
    {
        _currentWidth = GetComponent<RectTransform>().rect.width;
        _currentHeight = GetComponent<RectTransform>().rect.height;
        _canvasScaler = GetComponent<CanvasScaler>();

        _originScale = _canvasScaler.matchWidthOrHeight;
        ScaleScreen();
    }
    //private void Update()
    //{
    //    ScaleScreen();
    //}
    private void ScaleScreen()
    {
        float currentWidth = GetComponent<RectTransform>().rect.width;
        float currentHeight = GetComponent<RectTransform>().rect.height;
        float ratioCurrent = currentHeight / currentWidth;
        float ratioDefault = HEIGHT_DEFAULT / WIDTH_DEFAULT;

        //CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        _canvasScaler.matchWidthOrHeight = (ratioCurrent / ratioDefault) * _originScale;

        //if (ratioCurrent > ratioDefault) canvasScaler.matchWidthOrHeight = 0f;
        //if (ratioCurrent < ratioDefault) canvasScaler.matchWidthOrHeight = 1f;
        //if (ratioCurrent == ratioDefault) canvasScaler.matchWidthOrHeight = 0.5f;

    }
}

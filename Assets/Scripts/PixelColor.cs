using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelColor : MonoBehaviour
{
    public PhoneCameraProjection cameraController;

    Color pixelColor;

    public SpriteRenderer? sprite;
    public Image? image;

    void Update()
    {
        pixelColor = cameraController.pixelColor;

        sprite.color = pixelColor;
        image.color = pixelColor;

    }
}

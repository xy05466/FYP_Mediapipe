using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scaler : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        var camera = Camera.main;

        // Line up sprite with camera
        transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z);

        // Get viewport sizes
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Scale sprite
        var spriteSize = sr.sprite.bounds.size;
        Vector3 scale = Vector3.one;
        scale.x = worldScreenWidth / spriteSize.x;
        scale.y = worldScreenHeight / spriteSize.y;

        transform.localScale = scale;
    }
}

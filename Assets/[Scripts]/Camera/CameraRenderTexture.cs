using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{
    public RenderTexture renderTexture;

    void OnPreRender()
    {
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = activeRT;
    }
    Camera camera;
    private void Start()
    {
        //camera.clearFlags = CameraClearFlags.SolidColor;
        //camera.backgroundColor = Color.black; // or any color you need
        camera = GetComponent<Camera>();
    }
    //private void Start()
    //{
    //    //renderTexture = new RenderTexture(camer.x, screen.y, screen.z);

    //    //renderTexture.enableRandomWrite = true;

    //    //renderTexture.Create();
    //}
    private void Update()
    {
        camera.Render();
    }
}

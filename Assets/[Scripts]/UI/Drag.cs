using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RawImage image;

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)image.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);
        RectTransform imagePosRectTransfirm = (RectTransform)image.transform;
        Rect imagePosRect = imagePosRectTransfirm.rect;
        if (!imagePosRect.Contains(position))
            return;
        transform.position=image.transform.TransformPoint(position);
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

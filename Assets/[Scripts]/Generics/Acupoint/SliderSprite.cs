using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSprite : MonoBehaviour
{
    SpriteRenderer sr;
    public List<Sprite> list_sprites = new List<Sprite>();
    private int index = 0;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        
    }
    public void Fill(float value)
    {
        int totalValue =(int)((value/1) * list_sprites.Count);
        Debug.Log(totalValue);
        index = totalValue;
        sr.sprite = list_sprites[index];
    }
    public void ResetSlider()
    {
        index = 0;
        sr.sprite = list_sprites[index];
    }
}

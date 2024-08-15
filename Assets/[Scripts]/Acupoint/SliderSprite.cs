using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class SliderSprite : MonoBehaviour
    {
        private SpriteRenderer sr;
        public List<Sprite> list_sprites = new List<Sprite>();
        private int index = 0;
        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }
        public void Fill(float value)
        {
            int totalValue = (int)((value / 1) * list_sprites.Count);
            index = totalValue;
            sr.sprite = list_sprites[index];
        }
        public void ResetSlider()
        {
            index = 0;
            sr.sprite = list_sprites[index];
        }
    }
}
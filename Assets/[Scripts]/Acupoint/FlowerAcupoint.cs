using UnityEngine;

namespace SDM_FythmPipe
{
    public class FlowerAcupoint : MonoBehaviour
    {
        public float fadeSpeed = 10;
        [SerializeField] GameObject slicedPetal;
        [SerializeField] GameObject pps;
        private SpriteRenderer petalSR;
        //public bool collided = false;
        private bool done = false;
        private float alpha;
        public int pointId;
        private System.Action<FlowerAcupoint> OnPetalDone;
        private void Start()
        {
            petalSR = GetComponent<SpriteRenderer>();
            //slicedPetal.SetActive(true);
            GameObject newParticleSystem = Instantiate(pps, transform);
            Destroy(newParticleSystem);
            alpha = this.petalSR.color.a;
        }
        public void Progress()
        {
            if (done) return;
            alpha -= Time.deltaTime * fadeSpeed;
            alpha = Mathf.Clamp(alpha, 0, 1);
            petalSR.color = new Color(petalSR.color.r, petalSR.color.g, petalSR.color.b, alpha);

            if (alpha <= 0)
            {
                done = true;
                OnPetalDone?.Invoke(this);
            }
        }

        public void ResetPetal()
        {
            done = false;
            alpha = 1;
        }

        public void SubscribePetalsDone(System.Action<FlowerAcupoint> action) => OnPetalDone += action;
        public void UnSubscribePetalsDone(System.Action<FlowerAcupoint> action) => OnPetalDone -= action;
    }
}
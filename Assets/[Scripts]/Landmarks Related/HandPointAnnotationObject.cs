using UnityEngine;

namespace SDM_FythmPipe
{
    public class HandPointAnnotationObject : MonoBehaviour
    {
        public string tagCollider = "Facepoint";
        public Color colorColliding = Color.green;
        public Color colorDefault = Color.white;
        public System.Action<int, GameObject, int, GameObject> OnCollidingFacePoint; // hand point collide with face point
    }
}
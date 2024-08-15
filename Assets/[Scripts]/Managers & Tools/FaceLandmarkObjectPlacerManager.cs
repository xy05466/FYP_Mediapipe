using System.Collections.Generic;
using UnityEngine;


namespace SDM_FythmPipe
{
    public class FaceLandmarkObjectPlacerManager : MonoBehaviour
    {
        [SerializeField] private LandmarkManager landmarkManager;
        [SerializeField] List<GameObject> prefebs;

        private List<FacePointIndexObject> facePointObjects = new List<FacePointIndexObject>();
        [SerializeField] GameObject objectsParent;
        public List<int> listFaceLandmarks = new List<int>();

        private void Start()
        {
            landmarkManager.SubscribeFacePointsActive(EnableObjects);
        }
        private void Update()
        {
            if (!landmarkManager.IsFaceLoaded) return;
            //    petal.transform.position = facepointsObj[petal.pointId].transform.position;
            foreach (var facePoint in facePointObjects)
            {
                facePoint.transform.position = landmarkManager.GetFacePoints()[facePoint.GetIndex()].transform.position;
            }

        }
        // Call at initialize
        public void PlaceObjects()
        {
            for (int i = 0; i < listFaceLandmarks.Count; ++i)
            {
                GameObject newObject = Instantiate(prefebs[Random.Range(0, prefebs.Count)], objectsParent.transform);
                FacePointIndexObject facePointIndex = newObject.GetComponent<FacePointIndexObject>();
                if (facePointIndex == null)
                    facePointIndex = newObject.AddComponent<FacePointIndexObject>();

                facePointIndex.SetIndex(listFaceLandmarks[i]);
                newObject.transform.localScale *= 10;
                newObject.transform.localPosition = new Vector3(0, 0, -10);
                facePointObjects.Add(facePointIndex);
            }
        }
        public void EnableObjects(bool enable)
        {
            if (objectsParent.activeInHierarchy != enable)
                objectsParent.gameObject.SetActive(enable);
        }

        public List<FacePointIndexObject> GetFacePointOjects() => facePointObjects;
        public FacePointIndexObject GetFacePointObject(int n) => facePointObjects[n];
    }
}
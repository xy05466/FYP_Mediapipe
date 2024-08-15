using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Will handle the pages and splash screen pop up etc.
// Basically all the common things in a scene
namespace SDM_FythmPipe
{
    public class PageSystem : MonoBehaviour
    {
        [Header("Managers")]
        public LandmarkManager landmarkManager;
        public List<PageLogic> pageLogics;
        private PageLogic currentPageLogic;
        int currentPageIndex = 0;


        [Header("Splash Page")]
        public SplashScreen splashScreen;
        public UISpriteAnimation sceneAnimationStart;
        public UISpriteAnimation sceneAnimationEnd;
        public Transform headertransform;
        public TMP_Text tmp_title;
        public TMP_Text tmp_titleSub;


        [Header("Hand Point Collision")]
        public float distanceCollision = 0.2f;
        [Header("Pages")]
        private bool autoNavigate = false;
        private bool pageEnded = false;

        [Header("Annotation screens")]
        public GameObject annotationFace;
        public GameObject annotationHand;

        [Header("Destection Solution")]

        public System.Action<int, int, GameObject, int, GameObject> OnCollidingFacePoint; // hand point collide with face point

        //bool handPointsStored = false;
        private void Start()
        {

            annotationFace.gameObject.SetActive(false);
            annotationHand.gameObject.SetActive(false);



            sceneAnimationEnd.OnAnimationFinished += NextPage;


            landmarkManager.SubscribeFacePointsActive(OnFaceSpotted);
            landmarkManager.SubscribeHandPointsActive(OnHandSpotted);

            landmarkManager.SubscribeFacePointsLoaded(StoreFacePoints);
            landmarkManager.SubscribeHandPointsLoaded_1(StoreHandPoints);
            landmarkManager.SubscribeHandPointsLoaded_2(StoreHandPoints);
            for (int i = 0; i < pageLogics.Count; i++)
            {
                landmarkManager.SubscribeFacePointsActive(pageLogics[i].OnFaceAnnotationActive);
                landmarkManager.SubscribeHandPointsActive(pageLogics[i].OnHandAnnotationActive);
            }


            // Disable all pages
            for (int i = 0; i < pageLogics.Count; i++)
                pageLogics[i].gameObject.SetActive(false);

            for (int i = 0; i < pageLogics.Count; i++)
                pageLogics[i].Initialize();

            StartPage();
        }
        private void Update()
        {
            if (!sceneAnimationEnd.IsDone()) return;
            // update hand face point collision
            if (landmarkManager.IsFaceloadedAndActive)
            {
                // for loop all face points
                for (int j = 0; j < landmarkManager.GetFacePoints().Count; j++)
                {
                    // check if hand 1 is active
                    if (landmarkManager.IsHandActive(0))
                    {
                        // loop all hand points 1
                        for (int i = 0; i < landmarkManager.GetHandPoints(0).Count; i++)
                        {
                            HandPointAnnotationObject handPoint = landmarkManager.GetHandPoints(0)[i];
                            GameObject facePoint = landmarkManager.GetFacePoints()[j];
                            if (Vector2.Distance(handPoint.transform.position, facePoint.transform.position) <= distanceCollision)
                            {
                                OnCollidingFacePoint?.Invoke(0, i, handPoint.gameObject, j, facePoint);
                            }
                        }
                    }
                    // check if hand 2 is active
                    if (landmarkManager.IsHandActive(1))
                    {
                        // loop all hand points 2
                        for (int i = 0; i < landmarkManager.GetHandPoints(1).Count; i++)
                        {
                            HandPointAnnotationObject handPoint = landmarkManager.GetHandPoints(1)[i];
                            GameObject facePoint = landmarkManager.GetFacePoints()[j];
                            if (Vector2.Distance(handPoint.transform.position, facePoint.transform.position) <= distanceCollision)
                            {
                                OnCollidingFacePoint?.Invoke(1, i, handPoint.gameObject, j, facePoint);
                            }
                        }
                    }
                }
            }


            // check current page logic
            if (currentPageLogic != null)
            {
                // run curring page logic
                if (currentPageLogic.CompareState(PageLogic.PageState.START))
                {
                    currentPageLogic.RunLogic(landmarkManager.GetHandPoints(0), landmarkManager.GetHandPoints(1), landmarkManager.GetFacePoints());
                    if (currentPageLogic.progressionComponent != null)
                    {
                        currentPageLogic.progressionComponent.UpdateProgression(currentPageLogic.GetSubProgression(),
                            currentPageLogic.GetPageProgress(),
                            currentPageLogic.GetMaxPageProgress());
                    }
                }

                if (currentPageLogic.IsPageDone() && !pageEnded)
                {
                    pageEnded = true;
                    if (autoNavigate)
                        sceneAnimationEnd.PlayUIAnim();
                }
            }
        }
        private void OnFaceSpotted(bool active)
        {
            //img_face.gameObject.SetActive(active);
        }
        private void OnHandSpotted(int actives)
        {
            //img_hand.gameObject.SetActive(active);
        }
        public void StoreFacePoints(List<GameObject> facePoints)
        {
            //this.facePoints = facePoints;
            foreach (var pageL in pageLogics)
                pageL.LoadFacePoints(true);

        }
        public void StoreHandPoints(List<HandPointAnnotationObject> handPoints)
        {

            foreach (var pageL in pageLogics)
                pageL.LoadHandPoints(true);

            ////this.handPoints = handPoints;
            //foreach (var item in handPoints)
            //    item.OnCollidingFacePoint += OnHandPointCollide;

            //handPointsStored = true;

            //foreach (var item in handPoints)
            //    item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;

            OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;


            //foreach (var pageL in pageLogics)
            //{
            //    //foreach (var item in handPoints)
            //    //    item.OnCollidingFacePoint += pageL.OnHandPointCollideFacePoint;

            //    OnCollidingFacePoint += pageL.OnHandPointCollideFacePoint;
            //}
        }
        public void OnHandPointCollide(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
        {
            //Debug.Log(handPointIndex + " face: " + facePointIndex);
        }

        // Start page
        private void StartPage()
        {
            sceneAnimationStart.PlayUIAnim();
            currentPageLogic = pageLogics[currentPageIndex];
            currentPageLogic.gameObject.SetActive(true);

            splashScreen.img_splashIcon.sprite = currentPageLogic.pageData.spr_splashIcon;
            splashScreen.tmp_description.text = currentPageLogic.pageData.splashDescription;

            // enable face / hand traking
            //if(isFaceDetectionTask)
            //    annotationFace_task.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
            //else
            annotationFace.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);

            //if(isHandDetectionTask)
            //    annotationHand_task.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
            //else
            annotationHand.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
            //// enable header icons
            //img_face.transform.parent.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
            //img_hand.transform.parent.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);

            // set up title
            tmp_title.text = currentPageLogic.pageData.title;
            tmp_titleSub.text = currentPageLogic.pageData.titleSub;
            autoNavigate = currentPageLogic.pageData.autoNavigate;

            if (currentPageLogic.pageData.enableSplashPage)
                splashScreen.ShowSplash();
            else
                splashScreen.gameObject.SetActive(false);

            if (headertransform)
                headertransform.gameObject.SetActive(currentPageLogic.pageData.enableHeader);

            // start page for current page
            currentPageLogic.StartPage();
            currentPageLogic.ChangeState(PageLogic.PageState.START);

            //// subscribe hand points events to page
            //if (handPointsStored && !currentPageLogic.HandpointsLoaded)
            //{
            //    foreach (var item in landmarkManager.GetHandPoints())
            //        item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
            //}

            OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
        }

        public void PreNextPage()
        {
            if (currentPageIndex < pageLogics.Count - 1)
                sceneAnimationEnd.PlayUIAnim();

        }

        // Next page
        private void NextPage()
        {
            currentPageLogic.EndPage();
            currentPageLogic.gameObject.SetActive(false);
            currentPageIndex++; // increase page index
            sceneAnimationStart.OnAnimationFinished = null;
            currentPageLogic.ChangeState(PageLogic.PageState.ENDED);
            pageEnded = false;

            //if(landmarkManager.IsHandLoaded)
            //{
            //    foreach (var item in landmarkManager.GetHandPoints())
            //        item.OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;

            //    OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;
            //}
            if (landmarkManager.NumHandsLoaded > 0)
                OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;
            StartPage();
        }
    }
}
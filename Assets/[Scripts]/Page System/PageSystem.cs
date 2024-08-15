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

        public Transform headertransform;
        public TMP_Text tmp_title;
        public TMP_Text tmp_titleSub;
        [Header("Splash Page")]
        public SplashScreen splashScreen;
        [Header("Transition Page")]
        public TransitionPage_TextureAnimation transitionStart;
        public TransitionPage_TextureAnimation transitionEnd;


        [Header("Hand Point Collision")]
        public float distanceCollision = 0.2f;
        [Header("Pages")]
        private bool autoNavigate = true;
        private bool pageEnded = false;

        [Header("Annotation Layers")]
        public GameObject annotationFace;
        public GameObject annotationHand;

        [Header("Destection Solution")]
        public System.Action<int, int, GameObject, int, GameObject> OnCollidingFacePoint; // hand point collide with face point

        //bool handPointsStored = false;
        private void Start()
        {
            transitionStart?.PlayUIAnim();
            annotationFace.gameObject.SetActive(true);
            annotationHand.gameObject.SetActive(true);


            transitionEnd?.SubscribeTransitionEnded(NextPage);


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
            if (transitionEnd && !transitionEnd.IsDone()) return;
            // update hand face point collision
            if (landmarkManager.IsFaceloadedAndActive)
            {
                if (splashScreen != null && !splashScreen.gameObject.activeInHierarchy || splashScreen == null)
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
            }


            // check current page logic
            if (currentPageLogic != null)
            {
                // run curring page logic
                if (currentPageLogic.CompareState(PageLogic.PageState.START))
                {
                    if (splashScreen==null ||!splashScreen.isActiveAndEnabled)
                    {
                        currentPageLogic.RunLogic(landmarkManager.GetHandPoints(0), landmarkManager.GetHandPoints(1), landmarkManager.GetFacePoints());
                        if (currentPageLogic.progressionComponent != null)
                        {
                            currentPageLogic.progressionComponent.UpdateProgression(currentPageLogic.GetSubProgression(),
                                currentPageLogic.GetPageProgress(),
                                currentPageLogic.GetMaxPageProgress());
                        }
                    }
                }

                if (currentPageLogic.IsPageDone() && !pageEnded)
                {
                    pageEnded = true;
                    if (autoNavigate)
                    {
                        if (transitionEnd)
                            transitionEnd.PlayUIAnim();
                        else
                            NextPage();
                    }
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
        private void StoreFacePoints(List<GameObject> facePoints)
        {
            //this.facePoints = facePoints;
            foreach (var pageL in pageLogics)
                pageL.LoadFacePoints(true);

        }
        private void StoreHandPoints(List<HandPointAnnotationObject> handPoints)
        {

            foreach (var pageL in pageLogics)
                pageL.LoadHandPoints(true);


            OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
        }

        // Start page
        private void StartPage()
        {
            transitionStart?.PlayUIAnim();
            currentPageLogic = pageLogics[currentPageIndex];
            currentPageLogic.gameObject.SetActive(true);

            if (splashScreen)
            {
                splashScreen.img_splashIcon.sprite = currentPageLogic.pageData.spr_splashIcon;
                splashScreen.tmp_description.text = currentPageLogic.pageData.splashDescription;
            }
            if (currentPageLogic.pageData != null)
            {
                annotationFace.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
                annotationHand.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
                // set up title
                if (tmp_title)
                    tmp_title.text = currentPageLogic.pageData.title;
                if (tmp_titleSub)
                    tmp_titleSub.text = currentPageLogic.pageData.titleSub;
                // set up autonavgate
                autoNavigate = currentPageLogic.pageData.autoNavigate;
            }
            else
            {
                autoNavigate = true;
            }

         

            if (splashScreen)
            {
                if (currentPageLogic.pageData ==null || currentPageLogic.pageData.enableSplashPage)
                    splashScreen.ShowSplash();
                else
                    splashScreen.gameObject.SetActive(false);
            }
   

            if (headertransform && currentPageLogic.pageData!=null)
                headertransform.gameObject.SetActive(currentPageLogic.pageData.enableHeader);

            // start page for current page
            currentPageLogic.StartPage();
            currentPageLogic.ChangeState(PageLogic.PageState.START);


            OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
        }

        // Call for buttons
        private void CallNextPage()
        {
            if (currentPageIndex < pageLogics.Count - 1)
                transitionEnd.PlayUIAnim();

        }

        // Next page
        private void NextPage()
        {
            currentPageLogic.EndPage();
            currentPageLogic.gameObject.SetActive(false);
            currentPageIndex++; // increase page index
            transitionStart?.ClearEvent();
            currentPageLogic.ChangeState(PageLogic.PageState.ENDED);
            pageEnded = false;

            if (landmarkManager.NumHandsLoaded > 0)
                OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;
            StartPage();
        }
    }
}
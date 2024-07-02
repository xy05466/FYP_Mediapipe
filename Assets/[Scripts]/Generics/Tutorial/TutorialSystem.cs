using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private HandGestureManager handGestureManager;
    [Space]
    [SerializeField] private RectTransform transformFocusParent;
    [SerializeField] private RectTransform transformFocus;
    [SerializeField] private Button bu_next;
    //[Tooltip("Will only find active objects")]
    //public bool autoFind = false;
    [SerializeField] private List<TutorialComponent> tutorialComponents = new List<TutorialComponent>();
    private Queue<TutorialComponent> tutorialComponentsQueue = new Queue<TutorialComponent>();
    [SerializeField] private TMP_Text tmp_tutorialText;
    bool isPlaying = false;
    float timeTutorial = 2f;

    private void Awake()
    {
        //if (autoFind)
        //    tutorialComponents = GameObject.FindObjectsByType<TutorialComponent>(FindObjectsSortMode.None).ToList();

        // subscribe all focus components
        foreach (TutorialComponent component in tutorialComponents)
            component.SubscribeFocus(OnTutorialComponentFocus);

        // Start all tutorial componenents
        foreach (TutorialComponent component in tutorialComponents)
            component.StartComponent();
    }
    private void Start()
    {
        bu_next.onClick.AddListener(NextTutorial);
    }
    // when each tutorial components is focus
    void OnTutorialComponentFocus(TutorialComponent tutorialComponent)
    {
        Debug.Log("dsds");
        // add to queue
        tutorialComponentsQueue.Enqueue(tutorialComponent);
        if(!isPlaying)
        {
            Debug.Log("focusig");
            NextTutorial();
        }
        isPlaying = true;
    }
    // go to next tutorial
    public void NextTutorial()
    {
        Invoke("DequeueTutorial", timeTutorial);
    }
    public void DequeueTutorial()
    {
        if (tutorialComponentsQueue.Count > 0)
        {
            transformFocusParent.gameObject.SetActive(true);
            Focus(tutorialComponentsQueue.Dequeue());
            tmp_tutorialText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            transformFocusParent.gameObject.SetActive(false);
            transformFocus.SetParent(transformFocusParent);
            tmp_tutorialText.transform.parent.gameObject.SetActive(false);
            isPlaying = false;
        }
        timeTutorial = 0f;
    }
    // current focus on tutorial
    public void Focus(TutorialComponent tutorialComponent)
    {
        transformFocus.SetParent(tutorialComponent.GetTargetRect(),true);
        transformFocus.anchoredPosition = Vector2.zero;
        //transformFocus.localScale = Vector3.one;
        tmp_tutorialText.text = tutorialComponent.GetInstructionText();
    }

    // Check if tutorial is in progress
    public bool InProgress()
    {
        return isPlaying;
    }

    //public void AddToQueue(TutorialComponent tutorialComponent)
    //{
    //    tutorialComponent.SubscribeFocus(OnTutorialComponentFocus);
    //    tutorialComponent.StartComponent();
    //}

#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialSystem))]
    public class TutorialManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TutorialSystem tutorialManager = (TutorialSystem)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Get All Components"))
            {
                tutorialManager.tutorialComponents.Clear();
                List<GameObject> allObjects = new List<GameObject>();
                Scene activeScene = SceneManager.GetActiveScene();
                GameObject[] rootObjects = activeScene.GetRootGameObjects();

                foreach (GameObject rootObject in rootObjects)
                {
                    GetAllGameObjects(rootObject, allObjects);
                }

                foreach (GameObject obj in allObjects)
                {
                    TutorialComponent component = obj.GetComponent<TutorialComponent>();
                    if (component != null)
                    {
                        bool isActive = obj.activeInHierarchy;
                        tutorialManager.tutorialComponents.Add(component);
                    }
                }

            }
        }

        void GetAllGameObjects(GameObject root, List<GameObject> allObjects)
        {
            allObjects.Add(root);
            foreach (Transform child in root.transform)
            {
                GetAllGameObjects(child.gameObject, allObjects);
            }
        }
    }
    #endif
}

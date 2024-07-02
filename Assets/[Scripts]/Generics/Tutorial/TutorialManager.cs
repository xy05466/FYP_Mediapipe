using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private RectTransform transformFocusParent;
    [SerializeField] private RectTransform transformFocus;
    [SerializeField] private Button bu_next;
    public bool autoFind = false;
    [SerializeField] private List<TutorialComponent> tutorialComponents = new List<TutorialComponent>();
    private Queue<TutorialComponent> tutorialComponentsQueue = new Queue<TutorialComponent>();

    bool focusing = false;

    private void Awake()
    {
        if (autoFind)
            tutorialComponents = GameObject.FindObjectsByType<TutorialComponent>(FindObjectsSortMode.None).ToList();

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
        // add to queue
        tutorialComponentsQueue.Enqueue(tutorialComponent);
        if(!focusing)
        {
            NextTutorial();
        }
        focusing = true;
    }
    float timeTutorial = 2f;
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
        }
        else
        {
            transformFocusParent.gameObject.SetActive(false);
            transformFocus.SetParent(transformFocusParent);
        }
        timeTutorial = 0f;
    }
    // current focus on tutorial
    public void Focus(TutorialComponent tutorialComponent)
    {
        transformFocus.SetParent(tutorialComponent.GetTargetRect(),true);
        transformFocus.anchoredPosition = Vector2.zero;
        //transformFocus.localScale = Vector3.one;
    }
}

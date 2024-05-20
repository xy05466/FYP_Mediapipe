using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilderSet : MonoBehaviour
{
    //all anchorpoints and positions sotred here
    public ListOfLists<Vector3> anchorPostions;
    List<JadenCurveManager> path;
    [SerializeField] GameObject prefab;
    public bool completed = false;
    public int numOfSetsToComplete = 3;
    int numOfSets = 0;
    int setsCompleted = 0;
    public bool lockPositions = false;
    //time pressed
    public delegate void ResetBallPos();
    // Declare a delegate variable
    public static event ResetBallPos resetBallPos;
    //time pressed
    public delegate void OnSetCompleted();
    // Declare a delegate variable
    public static event OnSetCompleted onSetCompleted;
    //time pressed
    public delegate void UpdateMassageUI(string value);
    // Declare a delegate variable
    public static event UpdateMassageUI onUpdateMassageUI;
    private void Start()
    {

        path = new List<JadenCurveManager>();

        //init set prefabs (always spawn with 3)
        for (var i = 0; i < anchorPostions.CountOfLists(); i++)
        {
            GameObject newSet = Instantiate(prefab, this.gameObject.transform);
            gameObject.SetActive(true);

            //path[i].Add(newSet.GetComponent<JadenCurveManager>());
            path.Add(newSet.GetComponent<JadenCurveManager>());
            //set anchorPoints

            path[i].points[0].transform.position = new(anchorPostions.GetItemInList(i, 0).x, anchorPostions.GetItemInList(i, 0).y, 0);
            path[i].points[3].transform.position = new(anchorPostions.GetItemInList(i, 1).x, anchorPostions.GetItemInList(i, 1).y, 0);
            path[i].points[6].transform.position = new(anchorPostions.GetItemInList(i, 2).x, anchorPostions.GetItemInList(i, 2).y, 0);
        }
        //set num of sets and 
        numOfSets = 0;
        //get reference to paths
        //then set anchors to 
        //init event
        BallMovement.onComplete += SetCompleted;
        onUpdateMassageUI((numOfSetsToComplete - numOfSets).ToString());

    }
    private void OnDestroy()
    {
        BallMovement.onComplete -= SetCompleted;
    }
    //hold points 
    //update points
    //spawn all points in 
    private void Update()
    {

        if (lockPositions)
            return;
        if(Input.GetKeyDown(KeyCode.K))
        {
            lockPositions = true;
        }
        //Debug.Log("anchorPostion : " + " " + path[0].points[0].transform.position);
        //update all the individual points
        for (var i = 0; i < anchorPostions.CountOfLists(); i++)
        {
            path[i].points[0].transform.position = anchorPostions.GetItemInList(i, 0);
            path[i].points[3].transform.position = anchorPostions.GetItemInList(i, 1);
            path[i].points[6].transform.position = anchorPostions.GetItemInList(i, 2);

            Vector3 currentLocalPosition;

            currentLocalPosition = path[i].points[0].transform.localPosition;
            path[i].points[0].transform.localPosition = currentLocalPosition.WithZ(0f);

            currentLocalPosition = path[i].points[3].transform.localPosition;
            path[i].points[3].transform.localPosition = currentLocalPosition.WithZ(0f);
            currentLocalPosition = path[i].points[6].transform.localPosition;
            path[i].points[6].transform.localPosition = currentLocalPosition.WithZ(0f);

        }
        //check if sets have been completed enough times
        CheckCompleted();
    }
    public void Init()
    {
        anchorPostions = new ListOfLists<Vector3>();
    }
    public void AddList(int i)
    {
        anchorPostions.AddList(new List<Vector3>());
        anchorPostions.AddItemToList(i, Vector3.zero);
        anchorPostions.AddItemToList(i, Vector3.zero);
        anchorPostions.AddItemToList(i, Vector3.zero);
    }
    private void SetCompleted()
    {
        setsCompleted++;
        Debug.Log("Sets completed: " + setsCompleted);
        if (setsCompleted == anchorPostions.CountOfLists())
        {
            if (numOfSets == numOfSetsToComplete)
            {
                completed = true;
                Debug.Log("All sets completed");
                // Move on to next exercise here
            }
            else
            {
                Debug.Log("One set completed");
                numOfSets++;
                Debug.Log(numOfSets + " set completed");
                onUpdateMassageUI((numOfSetsToComplete - numOfSets).ToString());
                resetSet();
            }
        }
    }
    public void CheckCompleted()
    {
        if (setsCompleted == anchorPostions.CountOfLists())
        {
            completed = true;
        }
    }
    public void resetSet()
    {
        resetBallPos?.Invoke();
        setsCompleted = 0;
        //numOfSets = 0;
        onSetCompleted?.Invoke();
    }
    
}

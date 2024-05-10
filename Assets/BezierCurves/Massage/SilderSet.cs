using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilderSet : MonoBehaviour
{
    //all anchorpoints and positions sotred here
    public ListOfLists<Vector2> anchorPostions;
    List<Path> path;
    [SerializeField] GameObject prefab;
    public bool completed = false;
    int setsCompleted = 0;
    private void Awake()
    {
        anchorPostions = new ListOfLists<Vector2>();
        path = new List<Path>();
        //init set prefabs (always spawn with 3)
        for (var i = 0; i  < anchorPostions.CountOfLists() + 1; i++ )
        {
            anchorPostions.AddList(new List<Vector2>());
            GameObject newSet = Instantiate(prefab, this.gameObject.transform);
            gameObject.SetActive(true);

            path[i] = newSet.GetComponent<PathCreator>().path;
            //set anchorPoints
            anchorPostions.AddItemToList(i, Vector2.zero);
            anchorPostions.AddItemToList(i, Vector2.zero);
            anchorPostions.AddItemToList(i, Vector2.zero);
            Debug.Log("balls");
            //path[i].SetAnchorPointPosition(1, anchorPostions.GetItemInList(i + 1, 0).x, anchorPostions.GetItemInList(i + 1, 0).y);
            //path[i].SetAnchorPointPosition(2, anchorPostions.GetItemInList(i + 1, 1).x, anchorPostions.GetItemInList(i + 1, 1).y);
            //path[i].SetAnchorPointPosition(3, anchorPostions.GetItemInList(i + 1, 2).x, anchorPostions.GetItemInList(i + 1, 2).y);
        }
        //get reference to paths
        //then set anchors to 
    }
    //hold points 
    //update points
    //spawn all points in 
    private void Update()
    {
        //update all the individual points
        for (var i = 0; i < anchorPostions.CountOfLists() - 1; i++)
        {
            //update anchorPoints
            path[i].SetAnchorPointPosition(1, anchorPostions.GetItemInList(i + 1, 0).x, anchorPostions.GetItemInList(i + 1, 0).y);
            path[i].SetAnchorPointPosition(2, anchorPostions.GetItemInList(i + 1, 1).x, anchorPostions.GetItemInList(i + 1, 1).y);
            path[i].SetAnchorPointPosition(3, anchorPostions.GetItemInList(i + 1, 2).x, anchorPostions.GetItemInList(i + 1, 2).y);
        }
    }
    public void CheckCompleted()
    {
        if(setsCompleted == anchorPostions.CountOfLists())
        {
            completed = true;
        }
    }
}

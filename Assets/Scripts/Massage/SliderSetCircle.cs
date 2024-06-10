using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliderSetCircle : SilderSet
{
    new private void SetUpPath()
    {
        setUp = true;
        //init set prefabs (always spawn with 3)
        for (var i = 0; i < anchorPostions.CountOfLists(); i++)
        {
            //JadenCurveManager newSet = Instantiate(prefabJadenCurveManager, this.gameObject.transform);
            //gameObject.SetActive(true);

            //path[i].Add(newSet.GetComponent<JadenCurveManager>());
            //path.Add(newSet);
            //set anchorPoints
            Debug.Log("asdasd");
            //path[i].points[0].transform.position = new(anchorPostions.GetItemInList(i, 0).x, anchorPostions.GetItemInList(i, 0).y, 0);
            //path[i].points[3].transform.position = new(anchorPostions.GetItemInList(i, 1).x, anchorPostions.GetItemInList(i, 1).y, 0);
            //path[i].points[6].transform.position = new(anchorPostions.GetItemInList(i, 2).x, anchorPostions.GetItemInList(i, 2).y, 0);
            //path[i].points[9].transform.position = new(anchorPostions.GetItemInList(i, 3).x, anchorPostions.GetItemInList(i, 3).y, 0);
            //path[i].points[12].transform.position = new(anchorPostions.GetItemInList(i, 4).x, anchorPostions.GetItemInList(i, 4).y, 0);
            //path[i].points[15].transform.position = new(anchorPostions.GetItemInList(i, 5).x, anchorPostions.GetItemInList(i, 5).y, 0);
            //path[i].points[18].transform.position = new(anchorPostions.GetItemInList(i, 6).x, anchorPostions.GetItemInList(i, 6).y, 0);
            //path[i].points[21].transform.position = new(anchorPostions.GetItemInList(i, 7).x, anchorPostions.GetItemInList(i, 7).y, 0);
            //path[i].points[24].transform.position = new(anchorPostions.GetItemInList(i, 8).x, anchorPostions.GetItemInList(i, 8).y, 0);
        }
    }
}

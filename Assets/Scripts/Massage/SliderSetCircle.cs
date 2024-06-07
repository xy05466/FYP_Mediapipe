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
            JadenCurveManager newSet = Instantiate(prefabJadenCurveManager, this.gameObject.transform);
            gameObject.SetActive(true);

            //path[i].Add(newSet.GetComponent<JadenCurveManager>());
            path.Add(newSet);
            //set anchorPoints

            path[i].points[0].transform.position = new(anchorPostions.GetItemInList(i, 0).x, anchorPostions.GetItemInList(i, 0).y, 0);
            path[i].points[3].transform.position = new(anchorPostions.GetItemInList(i, 1).x, anchorPostions.GetItemInList(i, 1).y, 0);
            path[i].points[6].transform.position = new(anchorPostions.GetItemInList(i, 2).x, anchorPostions.GetItemInList(i, 2).y, 0);
            path[i].points[9].transform.position = new(anchorPostions.GetItemInList(i, 3).x, anchorPostions.GetItemInList(i, 3).y, 0);
            path[i].points[12].transform.position = new(anchorPostions.GetItemInList(i, 4).x, anchorPostions.GetItemInList(i, 4).y, 0);
        }
    }
}

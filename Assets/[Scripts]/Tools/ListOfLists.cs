using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    [System.Serializable]
    public class ListOfLists<T>
    {
        [SerializeField] private List<List<T>> listOfLists;

        //init
        public ListOfLists()
        {
            listOfLists = new List<List<T>>();
        }

        //add another list
        public void AddList(List<T> list)
        {
            listOfLists.Add(list);
        }
        //add item to list 
        public void AddItemToList(int listIndex, T item)
        {
            if (listIndex >= 0 && listIndex < listOfLists.Count)
            {
                listOfLists[listIndex].Add(item);
            }
            else
            {
                throw new IndexOutOfRangeException("List index out of range");
            }
        }
        public T GetItemInList(int listIndex, int itemIndex)
        {
            List<T> list = listOfLists[listIndex];
            if (listIndex >= 0 && listIndex < listOfLists.Count)
            {
                return list[itemIndex];
            }
            else
            {
                throw new IndexOutOfRangeException("List index out of range");
            }
        }
        // Get a reference to a list
        public List<T> GetList(int listIndex)
        {
            if (listIndex >= 0 && listIndex < listOfLists.Count)
            {
                return listOfLists[listIndex];
            }
            else
            {
                throw new IndexOutOfRangeException("List index out of range");
            }
        }
        // Method to get the count of lists
        public int CountOfLists()
        {
            return listOfLists.Count;
        }

        // Method to get the count of items in a specific list
        public int CountOfItemsInList(int listIndex)
        {
            if (listIndex >= 0 && listIndex < listOfLists.Count)
            {
                return listOfLists[listIndex].Count;
            }
            else
            {
                throw new IndexOutOfRangeException("List index out of range");
            }
        }
        public void SetItemInList(int listIndex, int itemIndex, T item)
        {
            if (listIndex >= 0 && listIndex < listOfLists.Count)
            {
                List<T> list = listOfLists[listIndex];
                if (itemIndex >= 0 && itemIndex < list.Count)
                {
                    list[itemIndex] = item;
                }
                else
                {
                    throw new IndexOutOfRangeException("Item index out of range");
                }
            }
            else
            {
                throw new IndexOutOfRangeException("List index out of range");
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{ 
    public T[] items;
    private int currentItemCount;
    private IComparer<T> comparer;


    public Heap(int maxHeapSize, IComparer<T> comparer)
    {
        items = new T[maxHeapSize];
        this.comparer = comparer;
    }

    public void Add(T item)
    {
        //adds an item to the heap of the heap and calls for it to be sorted 
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        //removes the first node from the heap and calls for the items to be sorted 
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count {
		get {
			return currentItemCount;
		}
	}
    
    public bool Contains(T item) {
        //checks to see if the heap contains a certain node
        if (item.HeapIndex < currentItemCount)
        {
            return Equals(items[item.HeapIndex], item);
        } else
        {
            return false;
        }
    }

    void SortDown(T item)
    {
        while (true) {
            //gets the indexes for the children of the current node
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            //checks the current left index to make sure its still with in the range of the heap
            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                //checks the current left index to make sure its still with in the range of the heap
                if (childIndexRight < currentItemCount) {
                    //compares the left and right nodes
                    if (comparer.Compare(items[childIndexLeft],items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }
                //compares the decided child node against its parent
                if (comparer.Compare(item,items[swapIndex]) < 0) {
                    Swap (item,items[swapIndex]);
                }
                else {
                    return;
                }

            }
            else {
                return;
            }

        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex-1)/2;
		
        while (true) {
            T parentItem = items[parentIndex];
            //compares node against its parent and swaps if the comparison shows they are in the wrong positiosn
            if (comparer.Compare(item,parentItem) > 0) {
                Swap (item,parentItem);
            }
            else {
                break;
            }

            parentIndex = (item.HeapIndex-1)/2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        //this swaps the position of two items in the heap
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
    
    public void Clear()
    { 
        currentItemCount = 0;
    }
}

public interface IHeapItem<T>
{
    int HeapIndex {
        get;
        set;
    }
}
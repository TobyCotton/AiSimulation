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
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
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
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (comparer.Compare(items[childIndexLeft],items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

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
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T>
{
    int HeapIndex {
        get;
        set;
    }
}
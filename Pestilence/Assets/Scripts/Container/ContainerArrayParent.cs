using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerArrayParent : MonoBehaviour
{
    public ContainerArray containerArray;
    public GridLayoutGroup grid;
    public int containerSize;
    public Container currentContainer;

    public void UpdateSlots()
    {
        int i = 0;
        if (containerArray != null)
        {
            foreach (Transform child in transform)
            {
                if (i < containerSize)
                    child.gameObject.SetActive(true);
                i++;
            }

            int t = 0;
            int factor = 0;
            for (t = 2; t < containerSize; t++)     
            {
                if (15 % t == 0)
                {
                    factor = t;
                    break;
                }
            }
            grid.constraintCount = factor != 0 ? factor : grid.constraintCount;
        }
        else
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
            grid.constraintCount = 9;
        }
    }
}

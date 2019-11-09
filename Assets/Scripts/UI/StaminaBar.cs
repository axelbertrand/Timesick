using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private int valueMax;
    public int value;

    public GameObject pointExemple;

    public List<GameObject> points;

    public Color fullColor;
    public Color emptyColor;

    public int currentValue;

    public void Initialize()//int value)
    {
        points = new List<GameObject>();

        valueMax = value;
        currentValue = value;

        for(int i = 0;i<value;i++)
        {
            float leftAnchor = ((float) i / value) + 0.005f;
            float rightAnchor = ((float) (i+1) / value) - 0.005f;

            GameObject newPoint = Instantiate(pointExemple);
            newPoint.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>());
            newPoint.GetComponent<RectTransform>().anchorMin = new Vector2(leftAnchor, 0.1f);
            newPoint.GetComponent<RectTransform>().anchorMax = new Vector2(rightAnchor, 0.9f);
        
            newPoint.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            newPoint.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

            newPoint.SetActive(true);

            points.Add(newPoint);
        }

        FillBar();
    }

    private void ChangeAll(Color newColor)
    {
        foreach (GameObject point in points)
        {
            point.GetComponent<Image>().color = newColor;
        }
    }

    public void FillBar()
    {
        ChangeAll(fullColor);
    }

    public void EmptyBar()
    {
        ChangeAll(emptyColor);
    }

    public void UpdateValue(int newValue)
    {
        if(newValue > currentValue)
        {
            Refill(currentValue - value); 
        }
        else if (currentValue < newValue)
        {
            Consume(newValue - currentValue);
        }
    }

    public void Refill(int howMuch)
    {
        if(howMuch >= valueMax)
        {
            FillBar();
        }
        else
        {
            for(int i = currentValue-1; i < currentValue + howMuch; i++)
            {
                points[i].GetComponent<Image>().color = fullColor;
            }
        }
    }

    public void Consume(int howMuch)
    {
        if(howMuch >= currentValue)
        {
            EmptyBar();
        }
        else
        {
            for (int i = currentValue - 1; i > currentValue - howMuch - 1; i--)
            {
                points[i].GetComponent<Image>().color = emptyColor;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public int valueMax;
    public int currentValue;

    public GameObject pointExemple;

    private List<GameObject> points;

    [SerializeField]
    protected Color fullColor;
    [SerializeField]
    protected Color emptyColor;

    [SerializeField]
    protected float pointsOffset = 0.005f;
    [SerializeField]
    protected float yAnchorMin = 0.1f;
    [SerializeField]
    protected float yAnchorMax = 0.9f;

    #region Properties
    public Color FullColor
    {
        get => fullColor;
        protected set
        {
            fullColor = value;
        }
    }

    public Color EmptyColor
    {
        get => emptyColor;
        protected set
        {
            emptyColor = value;
        }
    }

    public float PointsOffset
    {
        get => pointsOffset;
        protected set
        {
            pointsOffset = value;
        }
    }

    public float YAnchorMin
    {
        get => yAnchorMin;
        protected set
        {
            yAnchorMin = value;
        }
    }

    public float YAnchorMax
    {
        get => yAnchorMax;
        protected set
        {
            yAnchorMax = value;
        }
    }
    #endregion


    public void Initialize(int max,int current)
    {
        points = new List<GameObject>();

        valueMax = max;
        currentValue = current;

        for(int i = 0;i< valueMax; i++)
        {
            float leftAnchor = ((float) i / valueMax) + pointsOffset;
            float rightAnchor = ((float) (i+1) / valueMax) - pointsOffset;

            GameObject newPoint = Instantiate(pointExemple);
            newPoint.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform>());
            newPoint.GetComponent<RectTransform>().anchorMin = new Vector2(leftAnchor, yAnchorMin);
            newPoint.GetComponent<RectTransform>().anchorMax = new Vector2(rightAnchor, yAnchorMax);
        
            newPoint.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            newPoint.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

            newPoint.SetActive(true);

            if(currentValue > i)
            {
                newPoint.GetComponent<Image>().color = fullColor;
            }
            else
            {
                newPoint.GetComponent<Image>().color = emptyColor;
            }

            points.Add(newPoint);
        }

        //FillBar();
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
        
        if (newValue > currentValue)
        {
            Refill(newValue - currentValue); 
        }
        else if (currentValue > newValue)
        {
            Consume(currentValue - newValue);
        }

        currentValue = newValue;
    }

    public void Refill(int howMuch)
    {
        if(howMuch >= valueMax)
        {
            FillBar();
        }
        else
        {
            for (int i = currentValue; i < currentValue + howMuch; i++)
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

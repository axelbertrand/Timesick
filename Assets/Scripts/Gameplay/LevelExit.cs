using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool isEnabled;
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            spriteRenderer.enabled = isEnabled;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

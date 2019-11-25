using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField]
    //Time necessary to escape after stealing the medicine
    private float timeRequiredToDigTunnel;

    private float timeSpentDigging;

    private bool tunnelDug = false;
    void Start()
    {
        timeSpentDigging = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (tunnelDug){
            WaitHuman();
        }else
        {
            DigTunnel();
        }
    }

    private void DigTunnel()
    {
        timeSpentDigging += Time.deltaTime;
        if(timeSpentDigging > timeRequiredToDigTunnel)
        {
            tunnelDug = true;
            PrepareForEscape();
        }
    }

    private void PrepareForEscape()
    {
        GameObject escapeRoutes = this.transform.GetChild(0).gameObject;
        int numberOfEscapeRoutes = escapeRoutes.transform.childCount;
        GameObject escapeRoute = escapeRoutes.transform.GetChild(Random.Range(0, numberOfEscapeRoutes - 1)).gameObject;
        escapeRoute.SetActive(true);
    }

    private void WaitHuman()
    {

    }
}

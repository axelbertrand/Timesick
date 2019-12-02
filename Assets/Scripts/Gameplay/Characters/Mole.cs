using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox.Audio;

public class Mole : MonoBehaviour
{
    [SerializeField]
    //Time necessary to escape after stealing the medicine
    private float timeRequiredToDigTunnel;

    private float timeSpentDigging;

    private bool tunnelDug = false;

    [SerializeField]
    [Range(0, 50)]
    private int segmentsCount = 30;

    [SerializeField]
    [Range(0, 50)]
    private float maxRadius = 20f;

    private float radius = 0f;
    public Vector2 origin = Vector2.zero;
    private float timeSincefeedbackActivation = 0f;
    private float feedbackCircleSpeed = 15f;

    private LineRenderer lineRenderer;

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < segmentsCount + 1; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(origin.x + x, origin.y + y, -1));

            angle += (360f / segmentsCount);
        }
    }

    void Start()
    {
        timeSpentDigging = 0f;

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        lineRenderer.positionCount = segmentsCount + 1;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(0.78f, 0.19f, 0.19f);
        lineRenderer.endColor = new Color(0.78f, 0.19f, 0.19f);
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.enabled)
        {
            UpdateFeedbackCircle();
        }
        
        if (tunnelDug)
        {
            WaitHuman();
        }
        else
        {
            DigTunnel();
        }
    }

    private void DigTunnel()
    {
        timeSpentDigging += Time.deltaTime;

        if(!AudioManager.Instance.Player.IsCurrentlyPlayed("MoleDigging"))
        {
            AudioManager.Instance.PlaySound("MoleDigging");
        }

        if (timeSpentDigging > timeRequiredToDigTunnel)
        {
            AudioManager.Instance.Player.InterruptSound("MoleDigging");
            tunnelDug = true;
            PrepareForEscape();
        }
    }

    private void PrepareForEscape()
    {
        AudioManager.Instance.PlaySound("MoleArrived");
        GameObject escapeRoutes = this.transform.GetChild(0).gameObject;
        int numberOfEscapeRoutes = escapeRoutes.transform.childCount;
        GameObject escapeRoute = escapeRoutes.transform.GetChild(Random.Range(0, numberOfEscapeRoutes - 1)).gameObject;
        escapeRoute.SetActive(true);

        origin = new Vector2(escapeRoute.transform.position.x, escapeRoute.transform.position.y);
        lineRenderer.enabled = true;
    }

    private void WaitHuman()
    {

    }

    private void UpdateFeedbackCircle()
    {
        timeSincefeedbackActivation += Time.deltaTime;
        radius = feedbackCircleSpeed * timeSincefeedbackActivation;
        if (radius > maxRadius)
        {
            radius = 0f;
            lineRenderer.enabled = false;
        }
        CreatePoints();
    }
}

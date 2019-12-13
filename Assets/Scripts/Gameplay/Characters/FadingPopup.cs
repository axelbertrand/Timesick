using Cawotte.Toolbox.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPopup : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float durationFading = 0.5f;

    [SerializeField]
    private float durationVisible = 0.5f;

    [SerializeField]
    private string noiseOnShow = "";

    private StatePopup state = StatePopup.INVISIBLE;

    private float timer = 0f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == StatePopup.FADING)
        {
            timer -= Time.deltaTime;

            float newAlpha = Mathf.Lerp(0, 1, timer / durationFading);

            SetAlpha(newAlpha);

            if (timer < 0)
            {
                state = StatePopup.INVISIBLE;
            }
        }
        if (state == StatePopup.VISIBLE)
        {

            timer -= Time.deltaTime;

            if (timer < 0)
            {
                state = StatePopup.FADING;
                timer = durationFading;
            }

        }

    }

    public void Init()
    {
        gameObject.SetActive(true);
        SetAlpha(0f);
    }
    public void Show()
    {
        timer = durationVisible;
        state = StatePopup.VISIBLE;

        //Alpha is opaque
        SetAlpha(1f);


        AudioManager.Instance.PlaySound(noiseOnShow);
    }

    public void Hide()
    {
        timer = 0f;
        state = StatePopup.INVISIBLE;
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color temp = sr.color;
        temp.a = alpha;
        sr.color = temp;

    }


    private enum StatePopup
    {
        INVISIBLE, VISIBLE, FADING
    }
}

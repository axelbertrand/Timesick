using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DamagePostProcess : MonoBehaviour
{
    public Material damageMaterial;

    public Material baseMaterial;

    [SerializeField]
    private bool isActive;

    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(isActive)
        {
            Graphics.Blit(source, destination, damageMaterial);
        }
        else
        {
            Graphics.Blit(source, destination, baseMaterial);
        }
    }
}

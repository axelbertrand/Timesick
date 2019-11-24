using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;

public class CameraEffectsManager : Singleton<CameraEffectsManager>
{
    public DamagePostProcess damagePostProcess;

    public void Start()
    {
        damagePostProcess.IsActive = false;
    }

    public void ResetDamageEffect()
    {
        //This function is equivalent to call this one with a parameter of 1
        UpdateDamageEffect(1);
    }

    /**
     * Updates the dammage effect based on the CurrentHealth / Max Health ratio
     * 
     * Percent  0                   ->          1
     * Radius   0.6                 ->          0.75
     * Softness 0.25                ->          0.35
     * Color    (1,0.5,0.5,1)       ->          (1,0.5,0.5,1)
     */
    public void UpdateDamageEffect(float healthPercentage)
    {
        if(healthPercentage == 1)
        {
            damagePostProcess.IsActive = false;
            damagePostProcess.damageMaterial.SetFloat("_VRadius", 1);
            damagePostProcess.damageMaterial.SetColor("_Color", new Color(1,1,1,1));
        }
        else
        {
            float radius = 0.6f + 0.15f * healthPercentage;
            float softness = 0.25f + 0.1f * healthPercentage;

            //We map [0.1[ to [0.5,1[ for the red value and then use the equation of the line between our two colour points 
            float redValue = 0.5f + 0.5f * healthPercentage;
            float greenAndBlueValue = 0.8f * redValue - 0.3f;

            damagePostProcess.damageMaterial.SetFloat("_VRadius", radius);
            damagePostProcess.damageMaterial.SetFloat("_VSoft", softness);
            damagePostProcess.damageMaterial.SetColor("_Color", new Color(redValue, greenAndBlueValue, greenAndBlueValue, 1));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uqac.timesick.gameplay
{
    public interface IDetectable
    {
        //Marker interface for Sightables

        GameObject gameObject
        {
            get;
        }

        Vector2 Position
        {
            get;
        }

    }
}

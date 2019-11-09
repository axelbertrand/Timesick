using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uqac.timesick.gameplay
{
    public interface IDetectable
    {
        void OnSight();
        void OnLoseOfSight();


    }
}

namespace uqac.timesick.gameplay
{

    using Cawotte.Toolbox.AI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateIdle : State<Guard>
    {

        public StateIdle()
        {
            this.stateColor = Color.grey;
        }
        public override void Update()
        {
            //Do nothing until the guard see the player.
        }

        public override void StartState()
        {
            stateMachine.Subject.Sensor.OnSight += PursuitDetectableIfCharacter;
        }

        public override void EndState()
        {
            stateMachine.Subject.Sensor.OnSight -= PursuitDetectableIfCharacter;
        }


        private void PursuitDetectableIfCharacter(IDetectable detectable)
        {
            if (detectable is MainCharacter)
            {
                stateMachine.CurrentState = new StateChase();
            }
        }
    }
}

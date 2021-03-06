﻿namespace uqac.timesick.gameplay
{

    using Cawotte.Toolbox.AI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Cawotte.Toolbox.Audio;

    public class StateChase : State<Guard>
    {

        public StateChase()
        {
            this.stateColor = Color.red;
        }

        public override void Update()
        {
            if (!StateMachine.Subject.IsMoving)
            {
                StateMachine.Subject.GoToPlayerPosition();
            }

        }

        public override void StartState()
        {
            stateMachine.Subject.SightSensor.OnLoseOfSight += IdleIfCharacter;

            if(!StateMachine.Subject.ChaseSoundName.Equals(""))
            {
                AudioManager.Instance.PlaySound(StateMachine.Subject.ChaseSoundName);
            }
        }

        public override void EndState()
        {
            stateMachine.Subject.SightSensor.OnLoseOfSight -= IdleIfCharacter;
            StateMachine.Subject.StopMovement();
        }


        private void IdleIfCharacter(IDetectable detectable)
        {
            if (detectable is MainCharacter)
            {
                stateMachine.CurrentState = new StateIdle();
            }
        }
    }
}

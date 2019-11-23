namespace uqac.timesick.gameplay
{

    using Cawotte.Toolbox.AI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateSearch : State<Guard>
    {

        private Vector2 searchPosition;

        private bool investigateAroundNoise = false;
        private float timeInvestigateAroundNoise = 3f;

        private float timeInvestigated = 0f;

        public StateSearch(Vector2 searchPosition)
        {
            this.stateColor = Color.yellow;
            this.searchPosition = searchPosition;
        }

        public override void Update()
        {
            StateMachine.Subject.RotateInStepDirection();

            //If not close enough of the sound, go investigate it
            if (!stateMachine.Subject.IsMoving && Vector2.Distance(StateMachine.Subject.Position, searchPosition) > 1f)
            {
                StateMachine.Subject.GoTo(searchPosition);
            }
            else if (Vector2.Distance(StateMachine.Subject.Position, searchPosition) < 1f)
            {
                StateMachine.CurrentState = new StatePatrol(
                    MapManager.Instance.PickPointToPatrol(StateMachine.Subject)
                );
            }
        }

        public override void StartState()
        {
                //attack character if seen
            stateMachine.Subject.SightSensor.OnPlayerSight += StartShooting;

                //search new position if new noise
            stateMachine.Subject.HearingSensor.OnNoiseHeard += searchPos;

            stateMachine.Subject.GoTo(searchPosition);
        }

        public override void EndState()
        {
            stateMachine.Subject.SightSensor.OnPlayerSight -= StartShooting;
            stateMachine.Subject.HearingSensor.OnNoiseHeard -= searchPos;

        }



        private void StartShooting(MainCharacter player)
        {
            StateMachine.CurrentState = new StateShoot(player);
        }

        private void searchPos(Vector2 pos)
        {
            stateMachine.CurrentState = new StateSearch(pos);
        }
    }
}

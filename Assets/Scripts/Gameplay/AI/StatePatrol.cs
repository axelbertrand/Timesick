namespace uqac.timesick.gameplay
{

    using Cawotte.Toolbox.AI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Cawotte.Toolbox.Audio;

    public class StatePatrol : State<Guard>
    {

        private PatrolPoint patrolPoint;

        public PatrolPoint PatrolPoint { get => patrolPoint; }

        public StatePatrol(PatrolPoint patrolPoint)
        {
            this.stateColor = Color.green;
            this.patrolPoint = patrolPoint;
        }

        public override void Update()
        {

            if (!StateMachine.Subject.IsMoving)
            {
                StateMachine.Subject.GoTo(patrolPoint.transform.position);
            }


        }

        public override void StartState()
        {
            stateMachine.Subject.OnPositionChange +=
                (oldP, newP) => stateMachine.Subject.RotateToward(newP); //rotate on movement

            stateMachine.Subject.OnPatrolVisit += SetNewPatrolPoint;
            stateMachine.Subject.SightSensor.OnPlayerSight += StartShooting;
            stateMachine.Subject.HearingSensor.OnNoiseHeard += StartSearching;

            StateMachine.Subject.StopMovement();
            StateMachine.Subject.GoTo(patrolPoint.transform.position);
        }

        public override void EndState()
        {
            stateMachine.Subject.HearingSensor.OnNoiseHeard -= StartSearching;
            stateMachine.Subject.SightSensor.OnPlayerSight -= StartShooting;
            stateMachine.Subject.OnPatrolVisit -= SetNewPatrolPoint;


            stateMachine.Subject.OnPositionChange -=
                (oldP, newP) => stateMachine.Subject.RotateToward(newP); //rotate on movement

        }

        private void SetNewPatrolPoint(PatrolPoint pp)
        {
            if (pp.Equals(patrolPoint))
            {
                patrolPoint = MapManager.Instance.PickPointToPatrol(StateMachine.Subject);
            }
        }

        private void StartShooting(MainCharacter player)
        {
            StateMachine.CurrentState = new StateShoot(player);
        }

        private void StartSearching(Vector2 noiseSource)
        {
            StateMachine.CurrentState = new StateSearch(noiseSource);
        }

    }
}

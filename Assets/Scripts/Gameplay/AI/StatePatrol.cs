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
            //Make the guard rotate toward walking direction
            stateMachine.Subject.OnPositionChange +=
                (oldP, newP) => stateMachine.Subject.RotateToward(newP); //rotate on movement

            //On point patrolled, patrol a new point
            stateMachine.Subject.OnPatrolVisit += SetNewPatrolPoint;
            //On player almost  spotted, looks for him.
            stateMachine.Subject.SightSensor.OnPlayerSight += StartSearchingPlayer;
            //On noise heard, looks for it.
            stateMachine.Subject.HearingSensor.OnNoiseHeard += StartSearching;

            StateMachine.Subject.StopMovement();
            StateMachine.Subject.GoTo(patrolPoint.transform.position);
        }

        public override void EndState()
        {
            stateMachine.Subject.HearingSensor.OnNoiseHeard -= StartSearching;
            stateMachine.Subject.SightSensor.OnPlayerSight -= StartSearchingPlayer;
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

        private void StartSearchingPlayer(MainCharacter player)
        {
            StateMachine.CurrentState = new StateSearch(player.Position);
        }

        private void StartSearching(Vector2 noiseSource)
        {
            StateMachine.CurrentState = new StateSearch(noiseSource);
        }

    }
}

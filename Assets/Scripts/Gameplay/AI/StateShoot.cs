namespace uqac.timesick.gameplay
{

    using Cawotte.Toolbox.AI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Cawotte.Toolbox.Audio;

    public class StateShoot : State<Guard>
    {

        private MainCharacter player;
        public StateShoot(MainCharacter player)
        {
            this.stateColor = Color.black;
            this.player = player;
        }

        public override void Update()
        {

            StateMachine.Subject.RotateToward(player.Position);

        }

        public override void StartState()
        {
            stateMachine.Subject.SightSensor.OnPlayerLostOfSight += SearchForPlayer;


            StateMachine.Subject.StopMovement();


            if (!StateMachine.Subject.ChaseSoundName.Equals(""))
            {
                AudioManager.Instance.PlaySound(StateMachine.Subject.ChaseSoundName);
            }
        }

        public override void EndState()
        {
            stateMachine.Subject.SightSensor.OnPlayerLostOfSight -= SearchForPlayer;
            StateMachine.Subject.StopMovement();
        }

        private void SearchForPlayer(MainCharacter player)
        {
            StateMachine.CurrentState = new StateSearch(player.Position);
        }


    }
}

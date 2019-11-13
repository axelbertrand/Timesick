﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uqac.timesick.gameplay
{
    public class MainCharacter : Character
    {


        //endregion


        //region MonoBehaviour Loop

        // Update is called once per frame
        void Update()
        {
            HandleMovements();

            HandleSkills();

        }

        //endregion

        private void HandleMovements()
        {

            //Read the direction of the current inputs
            Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (inputDirection.magnitude < Mathf.Epsilon)
            {
                return;
            }

            //They are normalized for constant speed in all directions.
            inputDirection = inputDirection.normalized;

            MoveToward(Position + inputDirection);

        }

        private void HandleSkills()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (IsInvisible)
                {
                    return;
                }

                IsInvisible = true;
                Debug.Log("Start of invisibility");

                StartCoroutine(WaitAndSetVisible(2f));
            }
        }

        private IEnumerator WaitAndSetVisible(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            IsInvisible = false;
            Debug.Log("End of invisibility");
        }
    }
}

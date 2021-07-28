using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class NPC : MonoBehaviour
    {
        public DialogConfig dialogConfig; //config containing the text of the dialogue

        InteractionTrigger interactionTrigger; // interaction trigger

        private void Start()
        {
            interactionTrigger = GetComponent<InteractionTrigger>();
            interactionTrigger.Init();
        }

        private void Update()
        {
            if (interactionTrigger.inTrigger)//if player in trigger
            {
                if (InputManager.Interaction) // if player press Interaction button
                {
                    InputManager.Interaction = false;
                    Interaction(); //Interaction
                }
            }
        }

        //Interaction method
        void Interaction()
        {
            MessageServer.Broadcast<DialogConfig>(EventType.ShowDialog,dialogConfig); //Show dialog UI
        }

    }


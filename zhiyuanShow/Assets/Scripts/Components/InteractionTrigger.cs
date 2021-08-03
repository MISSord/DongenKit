using UnityEngine;
using UnityEngine.UI;


public class InteractionTrigger : MonoBehaviour
{

    [HideInInspector] public bool inTrigger; //Tracking trigger status

    public void Init()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) //if player ENTER in trigger
    {
        if (collision.gameObject.tag == "Player") //if its player
        {
            inTrigger = true;
            MessageServer.Broadcast<Vector3>(EventType.ShowInteractionKey,transform.position); //UI enable
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//if player Exit in trigger
    {
        if (collision.gameObject.tag == "Player")
        {
            inTrigger = false;
            MessageServer.Broadcast(EventType.CloseInteractionKey); ; //UI disable
        }
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RummyInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    
    public void OnDrag(PointerEventData eventData)
    {  
        RummyManager.instance.GetActivePlayer.moveCard(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>() != null)
            {
                RummyManager.instance.GetActivePlayer.SetSelectedCard(eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RummyManager.instance.GetActivePlayer.releasCard();

        for (int i = 0; i < RummyManager.instance.GetActivePlayer.transform.childCount -2  ; i++)
        {
            
            RummyManager.instance.GetActivePlayer.transform.GetChild(i).gameObject.GetComponent<GroupCard>().checkSequence();
        }

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RummyBot : MonoBehaviour
{
    public List<Card> handHeldCards;


    private void Awake()
    {
        handHeldCards = new List<Card>();
    }

    public void setUpBot()
    {
        Destroy(GetComponent<HorizontalLayoutGroup>());
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
    }

    public void AnimateBotCardDistribution()
    {

    }
    public void RecieveOneCard(Card newCard)
    {
        handHeldCards.Add(newCard);
    }


}

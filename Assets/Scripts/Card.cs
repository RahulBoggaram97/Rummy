using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Basic card details")]
    public Image visual;
    public CardNum _cardNum;
    public CardSuit _cardSuit;
    public bool isWildCard;
    public int childIndex;

    
    public void GenrateTheCard(CardNum newCardNum, CardSuit newCardSuit, Sprite cardImage, bool newisWildCard)
    {
        _cardNum = newCardNum;
        _cardSuit = newCardSuit;
        visual.sprite = cardImage;
        isWildCard = newisWildCard;

        

        gameObject.name = newCardNum.ToString() + " " + newCardSuit.ToString();
    }
   
    


}

public enum CardNum
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace,
    joker
}

public enum CardSuit
{
    Spade,
    Club,
    Diamond,
    Heart, 
    joker
}

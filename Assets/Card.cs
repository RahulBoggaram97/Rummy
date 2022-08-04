using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject visual;
    public CardNum _cardNum;
    public CardSuit _cardSuit;
    public bool isWildCard;

    public void GenrateTheCard(CardNum newCardNum, CardSuit newCardSuit, bool newisWildCard)
    {
        _cardNum = newCardNum;
        _cardSuit = newCardSuit;
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

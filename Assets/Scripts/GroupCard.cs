using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupCard : MonoBehaviour
{
    [SerializeField] Text debugText;

  

    public void checkSequence()
    {
        if (fetchCardListFromGroup() == null)
            return;

        if (isThisPureSequence(fetchCardListFromGroup()))
        {
            debugText.text = "Pure sequence";
        }
        else if (isThisImpurePureSequence(fetchCardListFromGroup()))
        {
            debugText.text = "Pure sequence";
        }
        else if (isThisSet(fetchCardListFromGroup()))
        {
            debugText.text = "Pure sequence";
        }
        else
        {
            debugText.text = "Invalid sequence";
        }
    }

    List<Card> fetchCardListFromGroup()
    {
        List<Card> cardList = new List<Card> ();

        for (int i = 0; i < transform.childCount; i++)
        {
            cardList.Add(transform.GetChild(i).GetComponent<Card>());
        }

        return cardList;
    }

    //SEQUENCE CHECKERS
    bool isThisPureSequence(List<Card> cardTobeChecked)
    {
        //if the group dosent have enough card it is not a sequence
        if (cardTobeChecked.Count < 3)
            return false;

        //checking the suit
        for (int i = 0; i < cardTobeChecked.Count; i++)
            if (cardTobeChecked[0]._cardSuit != cardTobeChecked[i]._cardSuit)
                return false;

        //sort the list


        //checking for sequence
        for (int i = 0; i < cardTobeChecked.Count; i++)
        {
            if (cardTobeChecked[i]._cardNum != CardNum.Ace)
            {
                if (i + 1 < cardTobeChecked.Count)
                {
                    if (cardTobeChecked[i]._cardNum != cardTobeChecked[i + 1]._cardNum - 1)
                    return false;
                }
            }
            else
            {
                if (i + 1 < cardTobeChecked.Count)
                {
                    if (cardTobeChecked[i + 1]._cardNum != CardNum.Two)
                        return false;
                }
            }
        }


        return true;
    }

    bool isThisImpurePureSequence(List<Card> cardTobeChecked)
    {
        //if the group dosent have enough card it is not a sequence
        if (cardTobeChecked.Count < 3)
            return false;



        //sort the list


        //checking for sequence
        for (int i = 0; i < cardTobeChecked.Count; i++)
        {
            //if one of the two card we are checking righ now is a wild card then ignore
            if (!cardTobeChecked[i].isWildCard || !cardTobeChecked[i + 1].isWildCard)
            {
                if (cardTobeChecked[i]._cardNum != CardNum.Ace)
                {
                    if (i + 1 < cardTobeChecked.Count)
                    {
                        if (cardTobeChecked[i]._cardNum != cardTobeChecked[i + 1]._cardNum - 1)
                            return false;
                    }
                }
                else
                {
                    if (i + 1 < cardTobeChecked.Count)
                    {
                        if (cardTobeChecked[i + 1]._cardNum != CardNum.Two)
                            return false;
                    }
                }
            }
        }


        return true;
    }

    bool isThisSet(List<Card> cardTobeChecked)
    {
        //if the group dosent have enough card it is not a sequence
        if (cardTobeChecked.Count < 3)
            return false;

        //check everone have the same value
        for (int i = 0; i < cardTobeChecked.Count; i++)
            if (cardTobeChecked[0]._cardNum != cardTobeChecked[i]._cardNum)
                return false;

        //check if we have duplicate suit
        for (int i = 0; i < cardTobeChecked.Count; i++)
        {
            for (int j = 0; j < cardTobeChecked.Count; j++)
            {
                if (i < j)
                {
                    if (cardTobeChecked[i]._cardSuit == cardTobeChecked[j]._cardSuit)
                    {
                        return false;
                    }
                }
            }
        }



        return true;
    }
}

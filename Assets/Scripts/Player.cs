using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Player : MonoBehaviour
{
    public List<Card> handHeldCards;
    public List<GameObject> Groups;


    private Card selectedCard, nextCard, previousCard;
    public Card SelectedCard { get => selectedCard; }

    public GameObject dummyCardObject, newGoupImgprefab, GroupPrefab;

    Transform currentGroup, nextGroup, previousGroup;

    bool shouldCreatNewGroup;

    int childCount;

    private void Awake()
    {
        handHeldCards = new List<Card>();
        Groups = new List<GameObject>();
       
    }



    //GAEMPLAY UI HANDLERS
    public void RecieveOneCard(GameObject newCard)
    {
        if(Groups.Count == 0)
        {
            CreateNewGroup();
        }
        else
        {
            newCard.transform.SetParent(Groups[Groups.Count - 1].transform);
        }

        handHeldCards.Add(newCard.GetComponent<Card>()); 
    }

    void CreateNewGroup()
    {
        GameObject newGroup = Instantiate(GroupPrefab);
        newGroup.transform.SetParent(transform);
        
        Groups.Add(newGroup);
        newGroup.name = "CardGroup" + Groups.Count;

        newGoupImgprefab.transform.SetSiblingIndex(transform.childCount - 1);
    }

    public void SetSelectedCard(Card newCard)
    {
        int selectedCardIndex = newCard.transform.GetSiblingIndex();


        selectedCard = newCard;
        currentGroup = selectedCard.transform.parent;
        selectedCard.childIndex = selectedCardIndex;

        GetDummyCard().SetActive(true);
        GetDummyCard().transform.SetSiblingIndex(selectedCardIndex);

        newGoupImgprefab.SetActive(true);

        
        selectedCard.transform.SetParent(transform.parent);

        //setting up the next and previous group
        int groupIndex = -1;
        for (int i = 0; i < Groups.Count ; i++)
        {
            if (Groups[i].transform == currentGroup)
            {
                groupIndex = i;
            }
        }
        if (groupIndex != -1)
        {
            if (groupIndex < Groups.Count - 1)
            {
                nextGroup = Groups[groupIndex + 1].transform;
            }
            if(groupIndex != 0)
            {
                previousGroup = Groups[groupIndex - 1].transform;
            }
        }

        //setting up the next card and previous card
        childCount = currentGroup.childCount;
        if (selectedCardIndex + 1 < childCount)
        {
            nextCard = currentGroup.GetChild(selectedCardIndex + 1).GetComponent<Card>();
        }
        if (selectedCardIndex - 1 >= 0)
        {
            previousCard = currentGroup.GetChild(selectedCardIndex - 1).GetComponent<Card>();
        }
    }

    public void moveCard(Vector2 postion)
    {

        if (selectedCard != null)
        {
            selectedCard.transform.position = postion;
            checkWithNextCard();
            checkWithPreviousCard();
            checkForTheNewGroup();
            
        }

    }

    void checkWithNextCard()
    {
        if (nextCard != null)
        {
            //Debug.Log(nextCard._cardNum + " " + nextCard._cardSuit);
            //if(nextGroup != null)
            //Debug.Log(nextGroup.name);
            if (selectedCard.transform.position.x > nextCard.transform.position.x)
            {
                int index = nextCard.transform.GetSiblingIndex();
                nextCard.transform.SetSiblingIndex(dummyCardObject.transform.GetSiblingIndex());
                dummyCardObject.transform.SetSiblingIndex(index);

                previousCard = nextCard;
                if (index + 1 < childCount)
                {
                    nextCard = currentGroup.GetChild(index + 1).GetComponent<Card>();

                }
                else
                {
                   
                    //check for the nextGroup
                    if(nextGroup != null)
                    {
                        if(selectedCard.transform.position.x > nextGroup.GetChild(0).position.x)
                        {
                            previousGroup = currentGroup;
                            currentGroup = nextGroup;

                            dummyCardObject.transform.SetParent(currentGroup);

                            childCount = currentGroup.childCount;
                            nextCard = currentGroup.GetChild(0).GetComponent<Card>();

                        }
                    }
                    else
                    {
                        nextCard = null;
                    }

                }
            }
            
        }
    }

    void checkWithPreviousCard()
    {
        if (previousCard != null)
        {
            if (selectedCard.transform.position.x < previousCard.transform.position.x)
            {
                int index = previousCard.transform.GetSiblingIndex();
                previousCard.transform.SetSiblingIndex(dummyCardObject.transform.GetSiblingIndex());
                dummyCardObject.transform.SetSiblingIndex(index);

                nextCard = previousCard;
                if (index - 1 >= 0)
                {
                    previousCard = currentGroup.GetChild(index - 1).GetComponent<Card>();

                }
                else
                {

                    //check for the previous group
                    //check for the nextGroup
                    if (previousGroup != null)
                    {
                        if (selectedCard.transform.position.x > previousGroup.GetChild(previousGroup.childCount - 1).position.x)
                        {
                            nextGroup = currentGroup;
                            currentGroup = previousGroup;

                            dummyCardObject.transform.SetParent(currentGroup);

                            childCount = currentGroup.childCount;
                            nextCard = currentGroup.GetChild(0).GetComponent<Card>();
                        }
                    }
                    else
                    {
                        previousCard = null;
                    }

                }
            }
        }

    }

    void checkForTheNewGroup()
    {
        if(selectedCard.transform.position.x > newGoupImgprefab.transform.position.x)
        {
            shouldCreatNewGroup = true;
        }
        else
        {
            shouldCreatNewGroup = false;
        }
    }



    GameObject GetDummyCard()
    {
        if (dummyCardObject != null)
        {
            if (dummyCardObject.transform.parent != currentGroup)
            {
                dummyCardObject.transform.SetParent(currentGroup);

            }
            return dummyCardObject;
        }
        else
        {
            dummyCardObject = RummyManager.instance.createDummyCard();
            dummyCardObject.name = "Dummy card";
            dummyCardObject.transform.SetParent(currentGroup);


        }

        return dummyCardObject;
    }

    public void releasCard()
    {
        

        if (selectedCard != null)
        {
            if (shouldCreatNewGroup)
            {
                CreateNewGroup();
                GetDummyCard().SetActive(false);
                selectedCard.transform.SetParent(Groups[Groups.Count - 1].transform);
                GetDummyCard().transform.SetParent(transform);
                newGoupImgprefab.SetActive(false);
                return;

            }

            GetDummyCard().SetActive(false);
            selectedCard.transform.SetParent(currentGroup);
            //Debug.Log(Mathf.Abs(selectedCard.transform.position.y - dummyCardObject.transform.position.y));

            if (Mathf.Abs(selectedCard.transform.position.y - dummyCardObject.transform.position.y) > 80)
            {
                GetDummyCard().transform.SetParent(transform);
                selectedCard.transform.SetSiblingIndex(selectedCard.childIndex);
            }
            else
            {
                
                selectedCard.transform.SetSiblingIndex(GetDummyCard().transform.GetSiblingIndex());
                GetDummyCard().transform.SetParent(transform);
            }




            selectedCard = null;
            newGoupImgprefab.SetActive(false);


        }
    }

    



   
 
}


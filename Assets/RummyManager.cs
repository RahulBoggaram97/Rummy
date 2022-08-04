using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RummyManager : MonoBehaviour
{
    [Header("transform for spawn")]
    public Transform DeckTransform;
    public Transform playerSpawner;

    [Header("Card spirtes")]
    public List<Sprite> allTheCardSprite = new List<Sprite>();

    [Header("Prefabs")]
    public GameObject cardPrefab;
    public GameObject playerPrefab;

    public List<Card> Deck;


    [Header("Player Deck Lists")]
    List<Player> Players;


    private void Start()
    {
        Deck = new List<Card>();
        Players = new List<Player>();

        populateDeck();
        distributeCards(2);





    }



    //Intiating the deck
    void populateDeck()
    {
        Add52Cards();
        Add52Cards();

        //Adding jokers        
        Deck.Add(genrateOneCard(CardNum.joker, CardSuit.joker, false));
        Deck.Add(genrateOneCard(CardNum.joker, CardSuit.joker, false));
    }
    void Add52Cards()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                Deck.Add(genrateOneCard((CardNum)j, (CardSuit)i, false));
            }
        }
    }
    Card genrateOneCard(CardNum newNum, CardSuit newSuit, bool isWildCard)
    {
        GameObject newCardObject = Instantiate(cardPrefab, DeckTransform);
        Card newCard = newCardObject.GetComponent<Card>();
        newCard.GenrateTheCard(newNum, newSuit, false);

        return newCard;

    }
    void shuffleDeck()
    {



    }



    void distributeCards(int numOfPlayers)
    {
        //make players and the distribute the cards
        for (int i = 0; i < numOfPlayers; i++)
        {
            GenrateNewPlayer();
            distributeForOnePlayer(i);
        }


    }
    void distributeForOnePlayer(int playerInde)
    {
        for (int j = 0; j < 13; j++)
        {
            Players[playerInde].handHeldCards.Add(drawOneCardFromDeck());

        }
    }
    void GenrateNewPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, playerSpawner);
        Players.Add(newPlayer.GetComponent<Player>());
    }
    Card drawOneCardFromDeck()
    {
        int randomDeckIndex = Random.Range(0, Deck.Count);
        Card DrawnCard = Deck[randomDeckIndex];
        Deck.Remove(DrawnCard);

        return DrawnCard;
    }

   
}

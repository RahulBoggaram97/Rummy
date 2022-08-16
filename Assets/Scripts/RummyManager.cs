using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RummyManager : MonoBehaviour
{
    public static RummyManager instance;
    private Player activePlayer;
    public Player GetActivePlayer
    {
        get { return activePlayer; }
    }

    [Header("transform for spawn")]
    public Transform DeckTransform;
    public Transform playerSpawner;

    [Header("Card spirtes")]
    [Tooltip("start with two of spade, the j, q, k and ace, and then club, diomand and heart")]
    public List<Sprite> allTheCardSprite = new List<Sprite>();

    [Header("Prefabs")]
    public GameObject cardPrefab;
    public GameObject playerPrefab;

    [Header("The deck")]
    public List<Card> Deck;
    public List<Card> discardPile;
    public GameObject dummyCardPrefab;

    List<Player> playerList;





    System.Random rand = new System.Random();
    
    private void Awake()
    {
        if(instance == null)
        instance = this;
 
    }

    private void Start()
    {
        Deck = new List<Card>();
        playerList = new List<Player>();
        discardPile = new List<Card>();

        populateDeck();
        shuffleDeck();
        distributeCards(1);
        
    }

    //Intiating the deck
    void populateDeck()
    {
        Add52Cards();
        Add52Cards();

        //Adding jokers        
        Deck.Add(genrateOneCard(CardNum.joker, CardSuit.joker, allTheCardSprite[52], false));
        Deck.Add(genrateOneCard(CardNum.joker, CardSuit.joker, allTheCardSprite[52], false));

        //adding the last back card;
        GameObject backCard = Instantiate(cardPrefab, DeckTransform);
        Destroy(backCard.GetComponent<Card>());
        backCard.GetComponent<Image>().sprite = allTheCardSprite[53];
    }
    void Add52Cards()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                Deck.Add(genrateOneCard((CardNum)j, (CardSuit)i, allTheCardSprite[(i*13) + j], false));
            }
        }
    }
    Card genrateOneCard(CardNum newNum, CardSuit newSuit, Sprite cardImage, bool isWildCard)
    {
        //instantiating
        GameObject newCardObject = Instantiate(cardPrefab, DeckTransform);
        Card newCard = newCardObject.GetComponent<Card>();

       

        newCard.GenrateTheCard(newNum, newSuit, cardImage , false);

        return newCard;

    }
    void shuffleDeck()
    {
        for (int i = Deck.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            Card value = Deck[k];
            Deck[k] = Deck[i];
            Deck[i] = value;
        }
    }




    //Distributing the cards
    void distributeCards(int numOfPlayers)
    {
        //make players and the distribute the cards
        for (int i = 0; i < numOfPlayers; i++)
        {
            GenrateNewPlayer();
            distributeForOnePlayer(i);
        }

        activePlayer = playerList[0];

    }
    void distributeForOnePlayer(int playerIndex)
    {
        for (int j = 0; j < 13; j++)
        {
            playerList[playerIndex].RecieveOneCard(drawSequentlyFromDeck());

        }
       ;
    }

    int playergernrated = 0;
    void GenrateNewPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, playerSpawner);
        newPlayer.name = "Player " +  playergernrated.ToString();
        playerList.Add(newPlayer.GetComponent<Player>());

        playergernrated++;
    }


    //Drawing cards from Deck

    void drawWildCardFromDeck()
    {

    }
    void AddToDiscardPile()
    {

    }
    GameObject drawRandomCardFromDeck()
    {
        int randomDeckIndex = Random.Range(0, Deck.Count);
        GameObject DrawnCard = Deck[randomDeckIndex].gameObject;
        Deck.Remove(DrawnCard.GetComponent<Card>());

        return DrawnCard;
    }
    GameObject drawSequentlyFromDeck()
    {
        
        GameObject DrawnCard = Deck[0].gameObject;
        Deck.Remove(DrawnCard.GetComponent<Card>());
        

        return DrawnCard;
    }
    


    //Card movement helpers
    public GameObject createDummyCard()
    {
        return Instantiate(dummyCardPrefab);
    }

}

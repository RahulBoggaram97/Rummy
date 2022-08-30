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
    public Transform discardPileTransform;

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
        if (instance == null)
            instance = this;

    }

    private void Start()
    {
        Deck = new List<Card>();
        playerList = new List<Player>();
        discardPile = new List<Card>();

        populateDeck();
        shuffleDeck();
        distributeCards(4);
        setUpRummyBot();
        drawWildCardFromDeck();

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
                Deck.Add(genrateOneCard((CardNum)j, (CardSuit)i, allTheCardSprite[(i * 13) + j], false));
            }
        }
    }
    Card genrateOneCard(CardNum newNum, CardSuit newSuit, Sprite cardImage, bool isWildCard)
    {
        //instantiating
        GameObject newCardObject = Instantiate(cardPrefab, DeckTransform);
        Card newCard = newCardObject.GetComponent<Card>();



        newCard.GenrateTheCard(newNum, newSuit, cardImage, false);

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

    int playergernrated = 0;
    void GenrateNewPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, playerSpawner);
        newPlayer.name = "Player " + playergernrated.ToString();
        playerList.Add(newPlayer.GetComponent<Player>());

        playergernrated++;
    }

    void setUpRummyBot()
    {
        for (int i = 1; i < playerList.Count; i++)
        {
            MakeOnePlayerBot(playerList[i].gameObject);
        }
    }

    int botGenrated = 0;
    void MakeOnePlayerBot(GameObject _player)
    {
        RummyBot bot = _player.AddComponent<RummyBot>();

        Player newPlayer = _player.GetComponent<Player>();

        //transfering the handheld cards
        for (int i = 0; i < newPlayer.handHeldCards.Count ; i++)
        {
            newPlayer.handHeldCards[i].transform.SetParent(bot.transform);
            bot.handHeldCards.Add(newPlayer.handHeldCards[i]);
          
        }

        Destroy(_player.GetComponent<Player>());
        bot.setUpBot();
        
        

        _player.name = "Bot#" + botGenrated.ToString();
        botGenrated++;
    }
    void distributeForOnePlayer(int playerIndex)
    {
        for (int j = 0; j < 13; j++)
        {
            playerList[playerIndex].RecieveOneCard(drawTheTopCardFromDeck());

        }
       ;
    }

    


    //Drawing cards from Deck

    void drawWildCardFromDeck()
    {
        GameObject wildCard = drawTheTopCardFromDeck();
        WildCardPopUpAnime(wildCard);

    }

    void WildCardPopUpAnime(GameObject wildCard)
    {
        wildCard.transform.LeanSetLocalPosX(40);
        wildCard.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    GameObject drawRandomCardFromDeck()
    {
        int randomDeckIndex = Random.Range(0, Deck.Count);
        GameObject DrawnCard = Deck[randomDeckIndex].gameObject;
        Deck.Remove(DrawnCard.GetComponent<Card>());

        return DrawnCard;
    }
    GameObject drawTheTopCardFromDeck()
    {
        
        GameObject DrawnCard = Deck[0].gameObject;
        Deck.Remove(DrawnCard.GetComponent<Card>());
        

        return DrawnCard;
    }

    public void PackYourTurn()
    {
        //fetch the hand held cards


        //shuffle them 
        for (int i = activePlayer.handHeldCards.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            Card value = activePlayer.handHeldCards[k];
            activePlayer.handHeldCards[k] = activePlayer.handHeldCards[i];
            activePlayer.handHeldCards[i] = value;
        }

        Destroy(DeckTransform.GetChild(DeckTransform.childCount - 1).gameObject);


        //place at the bottom of the deck
        for (int i = 0; i < activePlayer.handHeldCards.Count; i++)
        {
            Deck.Add(activePlayer.handHeldCards[i]);

            activePlayer.handHeldCards[i].gameObject.transform.SetParent(DeckTransform);
            activePlayer.handHeldCards[i].gameObject.transform.localPosition = Vector3.zero;
        }

        //adding the last back card;
        GameObject backCard = Instantiate(cardPrefab, DeckTransform);
        Destroy(backCard.GetComponent<Card>());
        backCard.GetComponent<Image>().sprite = allTheCardSprite[53];

        activePlayer.handHeldCards.Clear();


    }

    //Adding to the discardPile
    void AddToDiscardPile(List<Card> newCardList)
    {
        for (int i = 0; i < newCardList.Count; i++)
        {
            newCardList[i].transform.SetParent(discardPileTransform);
            discardPile.Add(newCardList[i]);
            
        }
    }
    void AddToDiscardPile(Card newCard)
    {
        newCard.transform.SetParent(discardPileTransform);
        discardPile.Add(newCard);

    }

    

    //Card movement helpers
    public GameObject createDummyCard()
    {
        return Instantiate(dummyCardPrefab);
    }


    public void PlayTurn(Player _player)
    {
        
        _player.RecieveOneCard(drawTheTopCardFromDeck());
    }
}

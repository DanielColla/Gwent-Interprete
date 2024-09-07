using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deck : MonoBehaviour
{    
    
    public GameObject cardPrefab;
    public List<Card> Deck = new List<Card>();
    public Transform deckPosition;
    public int x;
    public int deckZise;
    // Start is called before the first frame update
 void Start()
{
    x = 0;
    deckZise = 40;
    HashSet<int> usedIndices = new HashSet<int>();
    
    for (int i = 0; i < 40; i++)
    {
        do
        {
            x = Random.Range(0, CardDataBase.cardList.Count);
        } while (usedIndices.Contains(x));
        
        usedIndices.Add(x);
        Deck.Add(CardDataBase.cardList[x]);
    }
    ShuffleDeck();
}

   
    // Update is called once per frame
    void Update()
    {
        
    }
    void ShuffleDeck()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            int randomIndex = Random.Range(i, Deck.Count);
            Card temp = Deck[i];
            Deck[i] = Deck[randomIndex];
            Deck[randomIndex] = temp;
        }
    }
     void DisplayDeck()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            GameObject cardInstance = Instantiate(cardPrefab, deckPosition.position + new Vector3(i * 2, 0, 0), Quaternion.identity);
            
        }
    }
}
 
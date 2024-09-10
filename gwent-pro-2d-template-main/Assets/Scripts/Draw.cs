using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour
{
    public GameObject meleeZone;
    public GameObject DistanceZone;
    public GameObject AsaltZone;
    public GameObject meleeOponentZone;
    public GameObject DistanceOponentZone;
    public GameObject AsaltOponentZone;
    public GameObject CardPrefab;
    public GameObject Hand;
    public GameObject HandOponent;
    public List<GameObject> CardsInDeck = new List<GameObject>();
    private int randomIndex;
    public GameObject passTurn;
    public GameObject turnsystem; // Referencia al sistema de turnos (GameController)
    public int cardsDrawnThisTurn = 0;
    public int maxCardsPerTurn = 10;
    public deck deckScript;

    void Start()
    {
        for (int i = 0; i < CardsInDeck.Count; i++)
        {
            CardsInDeck[i] = Instantiate(Resources.Load("Card") as GameObject);
            CardsInDeck[i].GetComponent<ThisCard>().thisId = i;
        }
    }

    public void OnClick()
    {
        // Obtener la referencia al GameController
        GameController gameController = turnsystem.GetComponent<GameController>();

        // Verificar si es el primer turno
        if (gameController.IsFirstTurn)
        {
            maxCardsPerTurn = 10; // Primer turno: robar 10 cartas
        }
        else
        {
            maxCardsPerTurn = 2; // Turnos posteriores: robar 2 cartas
        }

        if (cardsDrawnThisTurn < maxCardsPerTurn)
        {
            randomIndex = Random.Range(0, CardsInDeck.Count);
            GameObject drawnCard = Instantiate(CardsInDeck[randomIndex]);
            AssignCardZone(drawnCard);  // Asignar la zona correspondiente
            drawnCard.transform.SetParent(Hand.transform);
            CardsInDeck.RemoveAt(randomIndex);
            cardsDrawnThisTurn++;

            if (cardsDrawnThisTurn >= maxCardsPerTurn)
            {
                this.GetComponent<Button>().interactable = false; // Deshabilitar el botón cuando se alcanzan las cartas permitidas
            }
        }
    }

    public void EndTurn()
    {
        // Reiniciar el contador de cartas robadas al comienzo del nuevo turno
        cardsDrawnThisTurn = 0;
        this.GetComponent<Button>().interactable = true;
    }

    void AssignCardZone(GameObject drawnCard)
    {
        var cardType = drawnCard.GetComponent<ThisCard>().typeCardText.text;
        var attackType = drawnCard.GetComponent<ThisCard>().typeAttackText.text;

        switch (cardType)
        {
            case "clima":
                drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.clima;
                break;
            case "despeje":
                drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.despeje;
                break;
            case "aumento":
                drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.aumento;
                break;
            case "lider":
                drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.lider;
                break;
            default:
                switch (attackType)
                {
                    case "M":
                        drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.M;
                        break;
                    case "S":
                        drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.S;
                        break;
                    case "R":
                        drawnCard.GetComponent<Draggable>().tipozona = Draggable.Mycardenum.R;
                        break;
                }
                break;
        }
    }

    public void DrawCard()
    {
        if (cardsDrawnThisTurn < maxCardsPerTurn)
        {
            if (deckScript.Deck.Count > 0)
            {
                Card drawnCard = deckScript.Deck[0];
                deckScript.Deck.RemoveAt(0);
                cardsDrawnThisTurn++;
                GameObject cardInstance = Instantiate(CardPrefab, Hand.transform);
            }
            else
            {
                Debug.Log("El mazo está vacío.");
            }
        }
        else
        {
            Debug.Log("No puedes robar más cartas en este turno.");
        }
    }
}

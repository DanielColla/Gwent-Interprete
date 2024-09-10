using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Button endYourRoundButton;
    public Button endOpponentRoundButton;
    public GameObject opponentHand;
    public GameObject playerHand;
    public GameObject blockhandoponent;
    public GameObject blockHand;
    public GameObject MYGraveyard;
    public GameObject OponentGraveyard;
    public List<GameObject> Graveyards = new List<GameObject>();
    public List<GameObject> MyZone = new List<GameObject>();  // Zonas del jugador
    public List<GameObject> OponentZone = new List<GameObject>();  // Zonas del oponente
    public int yourRoundWins = 0;
    public int opponentRoundWins = 0;
    public int roundsRequiredToWin = 2;
    public TMP_Text TurnText;
    public TMP_Text VictoryText;
    public GameObject deck1;
    public GameObject deck2;

    public bool isYourTurn = true;  // Indica si es el turno del jugador
    public bool roundEnded = false;
    public bool IsFirstTurn = true;

    void Start()
    {
        // Inicialización
        ResetGame();
        UpdateZoneInteractivity();  // Bloquea/desbloquea las zonas al inicio
    }

    public void EndRound(bool isPlayerEnding)
    {
        // Evita que se ejecute si la ronda ya terminó
        if (roundEnded) return;

        roundEnded = true;

        if (isPlayerEnding)
        {
            // Si el jugador presionó "Finalizar Ronda", el oponente puede jugar
            isYourTurn = false;
            StartCoroutine(WaitForOpponentCard());
            endYourRoundButton.interactable = false;  // Desactivar botón del jugador
        }
        else
        {
            // Si el oponente presionó "Finalizar Ronda", el jugador puede jugar
            isYourTurn = true;
            StartCoroutine(WaitForPlayerCard());
            endOpponentRoundButton.interactable = false;  // Desactivar botón del oponente
        }

        UpdateZoneInteractivity();  // Actualizar la interactividad de las zonas
    }
    public void ChangeTurn()
    {
        isYourTurn = !isYourTurn;  // Cambia el turno
        UpdateZoneInteractivity(); // Bloquea/desbloquea las zonas y manos
        UpdateTurnText();          // Actualiza el mensaje del turno
    }
    IEnumerator WaitForOpponentCard()
    {
        TurnText.text = "Opponent Turn: Play one more card!";
        yield return new WaitForSeconds(2f);  // Espera 2 segundos para que el oponente juegue
        EndRoundAndCalculateResults();
    }
    void UpdateTurnText()
    {
        if (isYourTurn)
        {
            TurnText.text = "Your Turn";
        }
        else
        {
            TurnText.text = "Opponent Turn";
        }
    }
    IEnumerator WaitForPlayerCard()
    {
        TurnText.text = "Your Turn: Play one more card!";
        yield return new WaitForSeconds(2f);  // Espera 2 segundos para que el jugador juegue
        EndRoundAndCalculateResults();
    }

    void EndRoundAndCalculateResults()
    {
        int playerPower = CalculateTotalPower(MyZone);
        int opponentPower = CalculateTotalPower(OponentZone);

        if (playerPower > opponentPower)
        {
            VictoryText.text = "You Won This Round!";
            yourRoundWins++;
        }
        else if (playerPower < opponentPower)
        {
            VictoryText.text = "Opponent Won This Round!";
            opponentRoundWins++;
        }
        else
        {
            VictoryText.text = "It's a Draw!";
        }

        if (yourRoundWins >= roundsRequiredToWin)
        {
            VictoryText.text = "Congratulations! You are the Final Winner!";
            RestartGame();
        }
        else if (opponentRoundWins >= roundsRequiredToWin)
        {
            VictoryText.text = "Sorry, Opponent is the Final Winner!";
            RestartGame();
        }
        else
        {
            RestartGame();
        }
    }

    int CalculateTotalPower(List<GameObject> zone)
    {
        int totalPower = 0;
        foreach (GameObject z in zone)
        {
            for (int i = 0; i < z.transform.childCount; i++)
            {
                totalPower += z.transform.GetChild(i).GetComponent<ThisCard>().power;
            }
        }
        return totalPower;
    }

    void RestartGame()
    {
        isYourTurn = true;
        roundEnded = false;
        TurnText.text = "Your Turn";
        endYourRoundButton.interactable = true;
        endOpponentRoundButton.interactable = true;

        ReturnGraveyardToDeck(MYGraveyard, deck1);
        ReturnGraveyardToDeck(OponentGraveyard, deck2);

        ResetZone(MyZone);
        ResetZone(OponentZone);
        IsFirstTurn = true;
        UpdateZoneInteractivity();  // Actualizar zonas al reiniciar
    }

    void ReturnGraveyardToDeck(GameObject graveyard, GameObject deck)
    {
        while (graveyard.transform.childCount > 0)
        {
            Transform card = graveyard.transform.GetChild(0);
            card.SetParent(deck.transform);
        }
    }

    void ResetZone(List<GameObject> zone)
    {
        foreach (GameObject z in zone)
        {
            for (int i = z.transform.childCount - 1; i >= 0; i--)
            {
                Transform card = z.transform.GetChild(i);
                card.SetParent(MYGraveyard.transform);
            }
        }
    }

    void ResetGame()
    {
        TurnText.text = "Your Turn";
        roundEnded = false;
        isYourTurn = true;
        UpdateZoneInteractivity();  // Actualizar interactividad al reiniciar
    }

    void UpdateZoneInteractivity()
    {
        // Bloquear/desbloquear zonas individualmente
        if (isYourTurn)
        {
            SetZonesInteractivity(MyZone, true);  // Desbloquear zonas del jugador
            SetZonesInteractivity(OponentZone, false);  // Bloquear zonas del oponente
          //  blockHand.GetComponent<CanvasGroup>().blocksRaycasts = false ;
          //  blockhandoponent.GetComponent<CanvasGroup>().blocksRaycasts = true;
            TurnText.text = "Your Turn";
        }
        else
        {
            SetZonesInteractivity(MyZone, false);  // Bloquear zonas del jugador
            SetZonesInteractivity(OponentZone, true);  // Desbloquear zonas del oponente
        //    blockHand.GetComponent<CanvasGroup>().blocksRaycasts = true ;
        //    blockhandoponent.GetComponent<CanvasGroup>().blocksRaycasts = false;
            TurnText.text = "Opponent Turn";
        }
    }

    void SetZonesInteractivity(List<GameObject> zoneList, bool isInteractive)
    {
        foreach (GameObject zone in zoneList)
        {
            CanvasGroup canvasGroup = zone.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = isInteractive;
            }
        }
    }

    
}

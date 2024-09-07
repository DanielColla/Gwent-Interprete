using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject MYGraveyard;
    public GameObject OponentGraveyard;
    public List<GameObject> MyZone = new List<GameObject>();
    public List<GameObject> OponentZone = new List<GameObject>();
    private int[] MyZoneCount = new int[9];
    private int[] OponentZoneCount = new int[9];
    public GameObject DiffenceZone;
    public GameObject DiffenceOponentZone;
    public bool IsFirstTurn { get; private set; } = true;
    public bool isYourTurn;
    public bool EndRound;
    public int YourGainedRound;
    public int OponentGainedRound;
    public int YourTotalPower;
    public int OponentTotalPower;
    public int PassCounter = 0;
    public int pointfinal = 0;
    public int pointfinal2 = 0;
    public TMP_Text TurnText;
    public TMP_Text VictoryText;
    public GameObject deck1;
    public GameObject deck2;
    public bool opponentTurnEnded;
    public bool playerTurnEnded;

    // Variables para controlar el estado del juego
    private int yourRoundWins = 0;
    private int opponentRoundWins = 0;
    private int roundsRequiredToWin = 2; // Número de rondas necesarias para ganar el juego

    void Start()
    {
        IsFirstTurn = true;
        EndRound = false;
        isYourTurn = true;
        YourGainedRound = 0;
        OponentGainedRound = 0;
        YourTotalPower = 0;
        OponentTotalPower = 0;
        yourRoundWins = 0;
        opponentRoundWins = 0;
    }

    void Update()
    {
        if (isYourTurn)
        {
            TurnText.text = "Your Turn";
            ProcessPlayerTurn();
        }
        else
        {
            TurnText.text = "Opponent Turn";
            ProcessOpponentTurn();
        }
    }

    void ProcessPlayerTurn()
    {
        for (int i = 0; i < MyZone.Count; i++)
        {
            if (MyZone[i].transform.childCount == MyZoneCount[i] + 1)
            {
                if (PassCounter == 1)
                {
                    Victory();
                }
                YourTotalPower += MyZone[i].transform.GetChild(MyZoneCount[i]).GetComponent<ThisCard>().power;
                MyZoneCount[i] += 1;

                ToggleZoneInteractivity(true); // Cambia la interactividad de las zonas

                if (EndRound)
                {
                    EndYourTurn();
                }

                UpdateVictoryText();
            }
        }
    }

    void ProcessOpponentTurn()
    {
        for (int k = 0; k < OponentZone.Count; k++)
        {
            if (OponentZone[k].transform.childCount == OponentZoneCount[k] + 1)
            {
                if (PassCounter == 1)
                {
                    Victory();
                }
                OponentTotalPower += OponentZone[k].transform.GetChild(OponentZoneCount[k]).GetComponent<ThisCard>().power;
                OponentZoneCount[k] += 1;

                ToggleZoneInteractivity(false); // Cambia la interactividad de las zonas

                if (EndRound)
                {
                    EndOponentTurn();
                }

                UpdateVictoryText();
            }
        }
    }

    void UpdateVictoryText()
    {
        if (YourTotalPower > OponentTotalPower && PassCounter % 2 == 0)
        {
            VictoryText.text = "You Gained This Turn";
            YourGainedRound++;
        }
        else if (YourTotalPower < OponentTotalPower && PassCounter % 2 == 0)
        {
            VictoryText.text = "Opponent Gained This Turn";
            OponentGainedRound++;
        }
        else if (YourTotalPower == OponentTotalPower && PassCounter % 2 == 0)
        {
            VictoryText.text = "Everybody Gained This Turn";
            YourGainedRound++;
            OponentGainedRound++;
        }
    }

    void ToggleZoneInteractivity(bool isPlayerTurn)
    {
        for (int i = 0; i < MyZone.Count; i++)
        {
            OponentZone[i].GetComponent<CanvasGroup>().blocksRaycasts = isPlayerTurn;
            MyZone[i].GetComponent<CanvasGroup>().blocksRaycasts = !isPlayerTurn;
        }

        DiffenceZone.GetComponent<CanvasGroup>().blocksRaycasts = !isPlayerTurn;
        DiffenceOponentZone.GetComponent<CanvasGroup>().blocksRaycasts = isPlayerTurn;
    }

    IEnumerator WaitBeforeRestart(float waitTime)
    {
        Debug.Log("WaitBeforeRestart() started");

        if (pointfinal > pointfinal2)
        {
            VictoryText.text = "You Won This Round!";
            yourRoundWins++;
        }
        else if (pointfinal < pointfinal2)
        {
            VictoryText.text = "Opponent Won This Round!";
            opponentRoundWins++;
        }
        else
        {
            VictoryText.text = "Both Won This Round!";
        }

        yield return new WaitForSeconds(waitTime);

        if (yourRoundWins >= roundsRequiredToWin)
        {
            VictoryText.text = "Congratulations! You are the Final Winner!";
            Restart();
        }
        else if (opponentRoundWins >= roundsRequiredToWin)
        {
            VictoryText.text = "Sorry, Opponent is the Final Winner!";
            Restart();
        }
        else
        {
            Restart();
        }
    }

    void EndYourTurn()
    {
        playerTurnEnded = true;
        deck1.GetComponent<Button>().interactable = false;
        deck2.GetComponent<Button>().interactable = true;

        ToggleZoneInteractivity(false);
        CheckEndOfRound();
    }

    void EndOponentTurn()
    {
        opponentTurnEnded = true;
        deck1.GetComponent<Button>().interactable = true;
        deck2.GetComponent<Button>().interactable = false;

        ToggleZoneInteractivity(true);
        CheckEndOfRound();
    }

    void CheckEndOfRound()
    {
        if (playerTurnEnded && opponentTurnEnded)
        {
            EndRoundd();
        }
    }

    void EndRoundd()
    {
        Debug.Log("EndRoundd() called");
        pointfinal = 0;
        pointfinal2 = 0;

        CalculatePoints(MyZone, ref pointfinal, MYGraveyard);
        CalculatePoints(OponentZone, ref pointfinal2, OponentGraveyard);

        Debug.Log("Points calculated: Your points = " + pointfinal + ", Opponent points = " + pointfinal2);

        if (pointfinal > pointfinal2)
        {
            VictoryText.text = "You Gained This Round!";
            yourRoundWins++;
        }
        else if (pointfinal < pointfinal2)
        {
            VictoryText.text = "Opponent Gained This Round!";
            opponentRoundWins++;
        }
        else
        {
            VictoryText.text = "Both Gained This Round!";
        }

        if (yourRoundWins >= roundsRequiredToWin || opponentRoundWins >= roundsRequiredToWin)
        {
            StartCoroutine(WaitBeforeRestart(3f));
        }
        else
        {
            Restart();
        }

        playerTurnEnded = false;
        opponentTurnEnded = false;
    }

    void CalculatePoints(List<GameObject> zone, ref int totalPoints, GameObject graveyard)
    {
        foreach (GameObject z in zone)
        {
            for (int i = z.transform.childCount - 1; i >= 0; i--)
            {
                totalPoints += z.transform.GetChild(i).GetComponent<ThisCard>().power;
                z.transform.GetChild(i).SetParent(graveyard.transform);
            }
        }
    }

    void Restart()
    {
        isYourTurn = true;
        EndRound = false;
        YourGainedRound = 0;
        OponentGainedRound = 0;
        YourTotalPower = 0;
        OponentTotalPower = 0;
        PassCounter = 0;
        pointfinal = 0;
        pointfinal2 = 0;
        TurnText.text = "Your Turn";

        ReturnGraveyardToDeck(MYGraveyard, deck1);
        ReturnGraveyardToDeck(OponentGraveyard, deck2);

        ResetZoneCounters(MyZoneCount);
        ResetZoneCounters(OponentZoneCount);
    }

    void ReturnGraveyardToDeck(GameObject graveyard, GameObject deck)
    {
        while (graveyard.transform.childCount > 0)
        {
            Transform card = graveyard.transform.GetChild(0);
            card.SetParent(deck.transform);
        }
    }

    void ResetZoneCounters(int[] zoneCount)
    {
        for (int i = 0; i < zoneCount.Length; i++)
        {
            zoneCount[i] = 0;
        }
    }

    void Victory()
    {
        if (YourTotalPower > OponentTotalPower && PassCounter % 2 == 0)
        {
            VictoryText.text = "You Gained This Turn";
            YourGainedRound++;
        }
        else if (YourTotalPower < OponentTotalPower && PassCounter % 2 == 0)
        {
            VictoryText.text = "Opponent Gained This Turn";
            OponentGainedRound++;
        }
        else if (YourTotalPower == OponentTotalPower && PassCounter % 2 == 0)
        {
            VictoryText.text = "Both Gained This Turn";
            OponentGainedRound++;
            YourGainedRound++;
        }
        ResetTurn();
    }

    void ResetTurn()
    {
        deck1.GetComponent<Draw>().cardsDrawnThisTurn = 0;
        deck1.GetComponent<Button>().interactable = true;
        deck2.GetComponent<Draw>().cardsDrawnThisTurn = 0;
        deck2.GetComponent<Button>().interactable = true;
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController: MonoBehaviour 
{
   public GameObject MYGraveyard;
   public GameObject OponentGraveyard;
   public List<GameObject> MyZone = new List<GameObject>();
   public List<GameObject> OponentZone = new List<GameObject>();
   private int[] MyZoneCount = new int[9];
   private int[] OponentZoneCount = new int[9];
   public GameObject DiffenceZone;
   public GameObject DiffenceOponentZone;
   public bool IsFirstTurn { get; private set; } = true; // Inicialmente es el primer turno
   public bool isYourTurn;
   public int isOponentTurn;
   public bool EndRound;
   public int YourGainedRound;
   public int OponentGainedRound;
   public int YourTotalPower;
   public int OponentTotalPower;
   public int PassCounter=0;
   public int pointfinal=0;
   public int pointfinal2=0;
   public TMP_Text TurnText;
   public TMP_Text VictoryText;
   public GameObject deck1;
   public GameObject deck2;
   public bool opponentTurnEnded;
   public bool playerTurnEnded;
   // Variables para controlar el estado del juego
    private int yourRoundWins = 0;
    private int opponentRoundWins = 0;
    private int roundsRequiredToWin = 2; // Número de rondas necesarias para ganar el juego

   void Start()
   {
        IsFirstTurn = true; // Asignar valor inicial
       EndRound = false;
        isYourTurn = true;
        isOponentTurn = 0;
        YourGainedRound = 0;
        OponentGainedRound = 0;
        YourTotalPower = 0;
        OponentTotalPower = 0;
        yourRoundWins = 0;
        opponentRoundWins = 0;
   }
   void Update()
   {
     if (isYourTurn)
        {
            TurnText.text = "Your Turn";

            for (int i = 0; i < MyZone.Count; i++)
            {
                if (MyZone[i].transform.childCount == MyZoneCount[i] + 1)
                {
                    if (PassCounter == 1)
                    {
                        Victory();
                    }
                    YourTotalPower = YourTotalPower + MyZone[i].transform.GetChild(MyZoneCount[i]).GetComponent<ThisCard>().power;
                    MyZoneCount[i] += 1;
                    for (int j = 0; j < MyZone.Count; j++)
                    {
                        OponentZone[j].GetComponent<CanvasGroup>().blocksRaycasts = true;
                        MyZone[j].GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                    DiffenceZone.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    DiffenceOponentZone.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    if (EndRound)
                    {
                        EndYourTurn();
                    }
                    if (YourTotalPower > OponentTotalPower && PassCounter % 2 == 0)
                    {
                        VictoryText.text = "You Gained This Turn";
                        YourGainedRound++;
                    }
                    else if (YourTotalPower < OponentTotalPower && PassCounter % 2 == 0)
                    {
                        VictoryText.text = "Oponent Gained This Turn";
                        OponentGainedRound++;
                    }
                    else if (YourTotalPower == OponentTotalPower && PassCounter % 2 == 0)
                    {
                        VictoryText.text = "Everybody Gained This Turn";
                        YourGainedRound++;
                        OponentGainedRound++;
                    }
                }
                if (pointfinal > pointfinal2)
                {
                    VictoryText.text = "You Gained This Round";
                }
                else if (pointfinal < pointfinal2)
                {
                    VictoryText.text = "Opponent Gained This Round";
                }
                else if (pointfinal == pointfinal2)
                {
                    VictoryText.text = "Everybody Gained This Round";
                }
                isYourTurn = false;
            }
        }
        else
        {
            TurnText.text = "Opponent Turn";
            for (int k = 0; k < OponentZone.Count; k++)
            {
                if (OponentZone[k].transform.childCount == OponentZoneCount[k] + 1)
                {
                    if (PassCounter == 1)
                    {
                        Victory();
                    }
                    OponentTotalPower = OponentTotalPower + OponentZone[k].transform.GetChild(OponentZoneCount[k]).GetComponent<ThisCard>().power;
                    OponentZoneCount[k] += 1;
                    for (int p = 0; p < OponentZone.Count; p++)
                    {
                        OponentZone[p].GetComponent<CanvasGroup>().blocksRaycasts = false;
                        MyZone[p].GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    DiffenceOponentZone.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    DiffenceZone.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    if (EndRound)
                    {
                        EndOponentTurn();
                    }
                    if (YourTotalPower > OponentTotalPower && PassCounter % 2 == 0)
                    {
                        VictoryText.text = "You Gained This Turn";
                        YourGainedRound++;
                    }
                    else if (YourTotalPower < OponentTotalPower && PassCounter % 2 == 0)
                    {
                        VictoryText.text = "Opponent Gained This Turn";
                        OponentGainedRound++;
                    }
                    else if (YourTotalPower == OponentTotalPower && PassCounter % 2 == 0)
                    {
                        VictoryText.text = "Everybody Gained This Turn";
                        YourGainedRound++;
                        OponentGainedRound++;
                    }
                }
            }
            if (pointfinal > pointfinal2)
            {
                VictoryText.text = "You Gained This Round";
            }
            else if (pointfinal < pointfinal2)
            {
                VictoryText.text = "Opponent Gained This Round";
            }
            else if (pointfinal == pointfinal2)
            {
                VictoryText.text = "Everybody Gained This Round";
            }
            isYourTurn = true;
        }
    }
    IEnumerator WaitBeforeRestart(float waitTime)
{
    Debug.Log("WaitBeforeRestart() started");

    // Mostrar el texto del ganador de la ronda
    if (pointfinal > pointfinal2)
    {
        VictoryText.text = "You Won This Round!";
        yourRoundWins++;
    }
    else if (pointfinal < pointfinal2)
    {
        VictoryText.text = "Opponent Won This Round!";
        opponentRoundWins++;
    }
    else
    {
        VictoryText.text = "Both Won This Round!";
    }

    Debug.Log("Waiting for " + waitTime + " seconds before restarting");

    // Esperar el tiempo especificado
    yield return new WaitForSeconds(waitTime);

    // Verificar si algún jugador ha ganado el juego
    if (yourRoundWins >= roundsRequiredToWin)
    {
        VictoryText.text = "Congratulations! You are the Final Winner!";
        Restart();
    }
    else if (opponentRoundWins >= roundsRequiredToWin)
    {
        VictoryText.text = "Sorry, Opponent is the Final Winner!";
        Restart();
    }
    else
    {
        Restart();
    }
}

 void EndYourTurn()
{
    playerTurnEnded = true; // El jugador ha finalizado su turno
    deck1.GetComponent<Button>().interactable = false; 
    deck2.GetComponent<Button>().interactable = true; // Habilitar el botón del oponente

    // Desactivar zonas del jugador
    for (int j = 0; j < MyZone.Count; j++)
    {
        OponentZone[j].GetComponent<CanvasGroup>().blocksRaycasts = true;
        MyZone[j].GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    DiffenceZone.GetComponent<CanvasGroup>().blocksRaycasts = false;
    DiffenceOponentZone.GetComponent<CanvasGroup>().blocksRaycasts = true;

    // Verificar si ambos jugadores han terminado el turno
    CheckEndOfRound();
}
void EndOponentTurn()
{
    opponentTurnEnded = true; // El oponente ha finalizado su turno
    deck1.GetComponent<Button>().interactable = true;
    deck2.GetComponent<Button>().interactable = false; // Desactivar botón del oponente

    // Desactivar zonas del oponente
    for (int p = 0; p < OponentZone.Count; p++)
    {
        OponentZone[p].GetComponent<CanvasGroup>().blocksRaycasts = false;
        MyZone[p].GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    DiffenceZone.GetComponent<CanvasGroup>().blocksRaycasts = true;
    DiffenceOponentZone.GetComponent<CanvasGroup>().blocksRaycasts = false;

    // Verificar si ambos jugadores han terminado el turno
    CheckEndOfRound();
}
void CheckEndOfRound()
{
    // Verificar si ambos jugadores han terminado su turno
    if (playerTurnEnded && opponentTurnEnded)
    {
        // Proceder con la lógica de fin de ronda
        EndRoundd();
    }
}
void EndRoundd()
{
    Debug.Log("EndRoundd() called");

    // Actualiza los puntos de cada jugador
    pointfinal = 0;
    pointfinal2 = 0;

    for (int i = 0; i < MyZone.Count; i++)
    {
        for (int j = MyZone[i].transform.childCount - 1; j >= 0; j--)
        {
            pointfinal += MyZone[i].transform.GetChild(j).GetComponent<ThisCard>().power;
            MyZone[i].transform.GetChild(j).transform.SetParent(MYGraveyard.transform);
        }
    }

    for (int k = 0; k < OponentZone.Count; k++)
    {
        for (int l = OponentZone[k].transform.childCount - 1; l >= 0; l--)
        {
            pointfinal2 += OponentZone[k].transform.GetChild(l).GetComponent<ThisCard>().power;
            OponentZone[k].transform.GetChild(l).transform.SetParent(OponentGraveyard.transform);
        }
    }

    Debug.Log("Points calculated: Your points = " + pointfinal + ", Opponent points = " + pointfinal2);

    // Determina el ganador de la ronda
    if (pointfinal > pointfinal2)
    {
        VictoryText.text = "You Gained This Round!";
        yourRoundWins++;
    }
    else if (pointfinal < pointfinal2)
    {
        VictoryText.text = "Opponent Gained This Round!";
        opponentRoundWins++;
    }
    else
    {
        VictoryText.text = "Both Gained This Round!";
    }

    // Verifica si el juego ha terminado
    if (yourRoundWins >= roundsRequiredToWin || opponentRoundWins >= roundsRequiredToWin)
    {
        Debug.Log("Calling WaitBeforeRestart()");
        StartCoroutine(WaitBeforeRestart(3f)); // Espera de 3 segundos antes de reiniciar completamente
    }
    else
    {
        // Reinicia para la próxima ronda
        Restart();
    }

    // Reiniciar variables
    playerTurnEnded = false;
    opponentTurnEnded = false;
}



  

    void Restart()
    {
        isYourTurn = true;
        isOponentTurn = 0;
        EndRound = false;
        YourGainedRound = 0;
        OponentGainedRound = 0;
        YourTotalPower = 0;
        OponentTotalPower = 0;
        PassCounter = 0;
        pointfinal = 0;
        pointfinal2 = 0;
        TurnText.text = "Your Turn";
       

       // Regresar las cartas del cementerio al mazo
        ReturnGraveyardToDeck(MYGraveyard, deck1);
         ReturnGraveyardToDeck(OponentGraveyard, deck2);
        for (int i = 0; i < MyZone.Count; i++)
        {
            MyZoneCount[i] = 0;
        }
        for (int k = 0; k < OponentZone.Count; k++)
        {
            OponentZoneCount[k] = 0;
        }
        void ReturnGraveyardToDeck(GameObject graveyard, GameObject deck)
     {
       while (graveyard.transform.childCount > 0)
      {
        Transform card = graveyard.transform.GetChild(0);
        card.SetParent(deck.transform);
       }
        }
    }


   void Victory()
   {
     if (YourTotalPower>OponentTotalPower && PassCounter%2==0)
      {
        VictoryText.text="You Gained This Turn";
        YourGainedRound++;
        deck1.GetComponent<Draw>().cardsDrawnThisTurn=0;
        deck1.GetComponent<Button>().interactable=true;
        deck2.GetComponent<Draw>().cardsDrawnThisTurn=0;
        deck2.GetComponent<Button>().interactable=true;
      }
      else if (YourTotalPower<OponentTotalPower && PassCounter%2==0)
      {
        VictoryText.text="Oponent Gained This Turn";
         OponentGainedRound++;
        deck1.GetComponent<Draw>().cardsDrawnThisTurn=0;
        deck1.GetComponent<Button>().interactable=true;
        deck2.GetComponent<Draw>().cardsDrawnThisTurn=0;
        deck2.GetComponent<Button>().interactable=true;
      }
      else if(YourTotalPower==OponentTotalPower && PassCounter%2==0)
      {
        VictoryText.text="Both Gained This Turn";
        OponentGainedRound++;
        deck1.GetComponent<Draw>().cardsDrawnThisTurn=0;
        deck1.GetComponent<Button>().interactable=true;
        deck2.GetComponent<Draw>().cardsDrawnThisTurn=0;
        deck2.GetComponent<Button>().interactable=true;
      }
   
  }



}  */


  
 
  
  

 



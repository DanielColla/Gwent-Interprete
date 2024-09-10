using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR;


public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<GameObject> MyZone = new List<GameObject>();
    public List<GameObject> OponentZone = new List<GameObject>();
    public GameObject Hand;
    public GameObject HandOponent;
    public enum Mycardenum { R, M, S, clima, aumento, despeje, lider, Handcard, graveyard }
    public Mycardenum tipozona;
    public Transform parentToReturnTo = null;
    public int totalfilaR;
    public int totalfilaS;
    public int totalfilaM;
    private int[] MyZoneCount = new int[9];
    private int[] OponentZoneCount = new int[9];
    public GameObject MYGraveyard;
    public GameObject OponentGraveyard;
 private GameController gameController;
 
    void Start()
    {
        Hand = GameObject.Find("Hand");
        HandOponent = GameObject.Find("Hand Oponent");
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

   /* public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (this.transform.parent.parent == GameObject.Find("Board").transform && this.transform.parent != Hand.transform && this.transform.parent != HandOponent.transform)
        {
            Effect();
            Debug.Log("Efecto Activado");
        }
    }*/
 public void OnEndDrag(PointerEventData eventData)
{
    Debug.Log("OnEndDrag");
    this.transform.SetParent(parentToReturnTo);
    GetComponent<CanvasGroup>().blocksRaycasts = true;

    // Verifica si la carta está en el campo
    if (this.transform.parent.parent == GameObject.Find("Board").transform && 
        this.transform.parent != Hand.transform && 
        this.transform.parent != HandOponent.transform)
    {
        // Llamar a la función Effect después de verificar que la carta está en el campo
        Effect();
        Debug.Log("Efecto Activado");
        // Cambiar el turno automáticamente después de colocar la carta
            gameController.ChangeTurn();
    }
   
}
  void Effect()
    {
        switch (this.GetComponent<ThisCard>().descriptionHability)
        {
            case "addaument":
                HandleAddAument();
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "addclimate":
                HandleAddClimate();
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "deletemax":
                HandleDeleteMax();
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "deletemin":
                HandleDeleteMin();
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "addcard":
                HandleAddCard();
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "sendToGraveyard":
                HandleSendToGraveyard();
                Debug.Log("Parent actual de la carta: " + this.transform.parent.name);
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "effectclima":
                HandleEffectClima();
                Debug.Log("Parent actual de la carta: " + this.transform.parent.name);
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                break;

            case "effectboost":
                HandleEffectBoost();
                Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
                Debug.Log("Parent actual de la carta: " + this.transform.parent.name);
                break;

            case "nuevaHabilidad":
                HandleNuevaHabilidad();
                break;
        }
    }

    public IEnumerable<GameObject> Selector()
    {
        if (gameController.isYourTurn) // Usa la instancia de gameController
        {
            return gameController.MyZone; // Devuelve la zona del jugador
        }
        else
        {
            return gameController.OponentZone; // Devuelve la zona del oponente
        }
    }

    public GameObject GetGraveyard()
    {
        if (gameController.isYourTurn) // Usa la instancia de gameController
        {
             return gameController.Graveyards[0]; 
        }
        else
        {
            return gameController.Graveyards[0]; 
        }
      
    }

    void HandleAddAument()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        if (IsInAllowedPositions(zone, 0, 1, 2))
        {
            MoveCardToZone(Hand, zone, 3, 4, 5);
        }
    }

    void HandleAddClimate()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        if (IsInAllowedPositions(zone, 0, 1, 2))
        {
            MoveCardToZone(Hand, zone, 7);
        }
    }

    void HandleDeleteMax()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        GameObject targetCard = FindCardWithMaxPower(zone);
        if (targetCard != null)
        {
            targetCard.transform.SetParent(GetGraveyard().transform);
        }
    }

    void HandleDeleteMin()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        GameObject targetCard = FindCardWithMinPower(zone);
        if (targetCard != null)
        {
            targetCard.transform.SetParent(GetGraveyard().transform);
        }
    }

    void HandleAddCard()
    {
        Draw drawScript = FindObjectOfType<Draw>();
        Button drawButton = drawScript.GetComponent<Button>();
        if (drawButton != null)
        {
            drawButton.interactable = true;
        }
    }

    void HandleSendToGraveyard()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        SendZoneToGraveyard(zone, GetGraveyard());
    }

    void HandleEffectClima()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        SendZoneToGraveyard(zone, GetGraveyard());
    }

    void HandleEffectBoost()
    {
        List<GameObject> zone = new List<GameObject>(Selector());
        BoostZone(zone, 0, 2);
    }

    void HandleNuevaHabilidad()
    {
        // Implementar la lógica específica para esta nueva habilidad
    }

    // Métodos auxiliares (sin cambios)
    bool IsInAllowedPositions(List<GameObject> zone, params int[] positions)
    {
        foreach (int pos in positions)
        {
            if (pos >= 0 && pos < zone.Count)
            {
                if (this.transform.parent == zone[pos].transform)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void MoveCardToZone(GameObject hand, List<GameObject> zone, params int[] targetPositions)
    {
        foreach (Transform cardTransform in hand.transform)
        {
            ThisCard cardComponent = cardTransform.GetComponent<ThisCard>();
            if (cardComponent != null)
            {
                cardTransform.SetParent(zone[targetPositions[0]].transform);
                return;
            }
        }
    }

    GameObject FindCardWithMaxPower(List<GameObject> zone)
    {
        int maxPower = 0;
        GameObject targetCard = null;
        for (int i = 0; i <= 2; i++)
        {
            foreach (Transform cardTransform in zone[i].transform)
            {
                ThisCard cardComponent = cardTransform.GetComponent<ThisCard>();
                if (cardComponent != null && cardComponent.power > maxPower)
                {
                    maxPower = cardComponent.power;
                    targetCard = cardTransform.gameObject;
                }
            }
        }
        return targetCard;
    }

    GameObject FindCardWithMinPower(List<GameObject> zone)
    {
        int minPower = int.MaxValue;
        GameObject targetCard = null;
        for (int i = 0; i <= 2; i++)
        {
            foreach (Transform cardTransform in zone[i].transform)
            {
                ThisCard cardComponent = cardTransform.GetComponent<ThisCard>();
                if (cardComponent != null && cardComponent.power > 0 && cardComponent.power < minPower)
                {
                    minPower = cardComponent.power;
                    targetCard = cardTransform.gameObject;
                }
            }
        }
        return targetCard;
    }

    void SendZoneToGraveyard(List<GameObject> zone, GameObject graveyard)
    {
       
        
        for (int i = 0; i <= 2; i++)
        {
            foreach (Transform cardTransform in zone[i].transform)
            {
                cardTransform.SetParent(graveyard.transform);
            }
        }
    }

    void BoostZone(List<GameObject> zone, int targetPosition, int boostAmount)
    {
        foreach (Transform cardTransform in zone[targetPosition].transform)
        {
            ThisCard cardComponent = cardTransform.GetComponent<ThisCard>();
            cardComponent.power += boostAmount;
            cardComponent.powerText.text = cardComponent.power.ToString();
        }
    }

   
}
  
 



 



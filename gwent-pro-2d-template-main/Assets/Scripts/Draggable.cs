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

    void Start()
    {
        Hand = GameObject.Find("Hand");
        HandOponent = GameObject.Find("Hand Oponent");
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

        case "sendToGraveyard"://despeje
            HandleSendToGraveyard();
            Debug.Log("Parent actual de la carta: " + this.transform.parent.name);
            Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);

            break;

        case "effectclima":  //clima
            HandleEffectClima();
            Debug.Log("Parent actual de la carta: " + this.transform.parent.name);
            Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);

            break;

        case "effectboost":   // aumento
            HandleEffectBoost();
            Debug.Log("Activando efecto: " + this.GetComponent<ThisCard>().descriptionHability);
            Debug.Log("Parent actual de la carta: " + this.transform.parent.name);
            break;

        case "nuevaHabilidad":
            HandleNuevaHabilidad();
            break;     
    }
}

 void HandleAddAument()
{
    if (IsInAllowedPositions(MyZone, 0, 1, 2))
    {
        MoveCardToZone(Hand, MyZone, 3, 4, 5);
    }
    else if (IsInAllowedPositions(OponentZone, 0, 1, 2))
    {
        MoveCardToZone(HandOponent, OponentZone, 3, 4, 5);
    }
}

 void HandleAddClimate()
{
    if (IsInAllowedPositions(MyZone, 0, 1, 2))
    {
        MoveCardToZone(Hand, MyZone, 7);
    }
    else if (IsInAllowedPositions(OponentZone, 0, 1, 2))
    {
        MoveCardToZone(HandOponent, OponentZone, 7);
    }
}

 void HandleDeleteMax()
{
    if (IsInAllowedPositions(MyZone, 0, 1, 2))
    {
        GameObject targetCard = FindCardWithMaxPower(OponentZone);
        if (targetCard != null)
        {
            targetCard.transform.SetParent(OponentGraveyard.transform);
        }
    }
    else if (IsInAllowedPositions(OponentZone, 0, 1, 2))
    {
        GameObject targetCard = FindCardWithMaxPower(MyZone);
        if (targetCard != null)
        {
            targetCard.transform.SetParent(MYGraveyard.transform);
        }
    }
}

 void HandleDeleteMin()
{
    if (IsInAllowedPositions(MyZone, 0, 1, 2))
    {
        GameObject targetCard = FindCardWithMinPower(OponentZone);
        if (targetCard != null)
        {
            targetCard.transform.SetParent(OponentGraveyard.transform);
        }
    }
    else if (IsInAllowedPositions(OponentZone, 0, 1, 2))
    {
        GameObject targetCard = FindCardWithMinPower(MyZone);
        if (targetCard != null)
        {
            targetCard.transform.SetParent(MYGraveyard.transform);
        }
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
    if (IsInAllowedPositions(MyZone, 0, 1, 2))
    {
        SendZoneToGraveyard(OponentZone, OponentGraveyard);
    }
    else if (IsInAllowedPositions(OponentZone, 0, 1, 2))
    {
        SendZoneToGraveyard(MyZone, MYGraveyard);
    }
}

 void HandleEffectClima()
{
    if (IsInAllowedPositions(MyZone, 6))
    {
        SendZoneToGraveyard(OponentZone, OponentGraveyard);
    }
    else if (IsInAllowedPositions(OponentZone, 6))
    {
        SendZoneToGraveyard(MyZone, MYGraveyard);
    }
}

 void HandleEffectBoost()
  {
    if (IsInAllowedPositions(MyZone, 0,1, 2))
    {
        BoostZone(MyZone, 0, 2);
    }
    else if (IsInAllowedPositions(OponentZone, 0, 2))
    {
        BoostZone(OponentZone, 0, 2);
    }
}
     void HandleNuevaHabilidad()
    {
    // Implementar la lógica específica para esta nueva habilidad
    }
  bool IsInAllowedPositions(List<GameObject> zone, params int[] positions)
{
    foreach (int pos in positions)
    {
        if (pos >= 0 && pos < zone.Count)
        {
            Debug.Log("Verificando posición: " + pos + ", Zona: " + zone[pos].name);
            if (this.transform.parent == zone[pos].transform)
            {
                Debug.Log("La carta está en la posición permitida: " + pos);
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
 



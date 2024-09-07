using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThisCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public List<Card> thisCard = new List<Card>();
    public int thisId;
    public int id;
    public string cardName;
    public int power;
    public string descriptionHability;
    public string typeCard;
    public string typeAttack;
    public string legion;
   
    public TMP_Text idText;
    public TMP_Text nameText;
    public TMP_Text powerText;
    public TMP_Text descriptionText;
    public TMP_Text typeCardText;
    public TMP_Text typeAttackText;
    public TMP_Text legionText;
    public Image thatImage;
    public Sprite thisSprite;
    public GameObject Zoom;

    void Start()
    {
        thisCard.Add(CardDataBase.cardList[thisId]);
        Zoom = GameObject.Find("Zoom");
   
    }

    void Update()
    {
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        power = thisCard[0].power;
        descriptionHability = thisCard[0].descriptionHability;
        typeAttack = thisCard[0].typeAttack;
        typeCard = thisCard[0].typeCard;
        legion = thisCard[0].legion;
        thisSprite = thisCard[0].thisImage;

        idText.text = "" + id;
        nameText.text = "" + cardName;
        powerText.text = "" + power;
        descriptionText.text = "" + descriptionHability;
        typeAttackText.text = "" + typeAttack;
        typeCardText.text = "" + typeCard;
        legionText.text = "" + legion;
        thatImage.sprite = thisSprite;
    }
 public void OnPointerEnter(PointerEventData eventData){
    if(this.gameObject.name != "Zoom"){
        Zoom.GetComponent<ThisCard>().thisCard[0].id = this.GetComponent<ThisCard>().id;
        Zoom.GetComponent<ThisCard>().thisCard[0].cardName = this.GetComponent<ThisCard>().cardName;
        Zoom.GetComponent<ThisCard>().thisCard[0].power = this.GetComponent<ThisCard>().power;
        Zoom.GetComponent<ThisCard>().thisCard[0].descriptionHability = this.GetComponent<ThisCard>().descriptionHability;
        Zoom.GetComponent<ThisCard>().thisCard[0].typeAttack = this.GetComponent<ThisCard>().typeAttack;
        Zoom.GetComponent<ThisCard>().thisCard[0].typeCard = this.GetComponent<ThisCard>().typeCard;
        Zoom.GetComponent<ThisCard>().thisCard[0].legion = this.GetComponent<ThisCard>().legion;
        Zoom.GetComponent<ThisCard>().thisCard[0].thisImage = this.GetComponent<ThisCard>().thisSprite;

    }
 }
  public void OnPointerExit(PointerEventData eventData){

  }
}


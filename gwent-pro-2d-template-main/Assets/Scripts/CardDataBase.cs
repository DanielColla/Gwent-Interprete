using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    
    public static List<Card> cardList= new List<Card>();
    void Awake()
    {//sendToGraveyard,effectclima,effectboost
     cardList.AddRange(CardPersistence.LoadGeneratedCards()); 
      cardList.Add(new Card(37,"Mash",0,"effectboost",Resources.Load<Sprite>("36"),"aumento","none","none"));
     cardList.Add(new Card(38,"Mash",0,"effectboost",Resources.Load<Sprite>("37"),"aumento","none","none"));
     cardList.Add(new Card(23,"Mash",0,"effectclima",Resources.Load<Sprite>("22"),"clima","none","none"));
     cardList.Add(new Card(1/*24*/,"Mash",0,"effectclima",Resources.Load<Sprite>("23"),"clima","none","none"));
     cardList.Add(new Card(2/*25*/,"Mash",0,"effectclima",Resources.Load<Sprite>("24"),"clima","none","none"));
     cardList.Add(new Card(3/*26*/,"Mash",0,"effectclima",Resources.Load<Sprite>("25"),"clima","none","none"));
     cardList.Add(new Card(4/*27*/,"Mash",0,"effectclima",Resources.Load<Sprite>("26"),"clima","none","none"));
    //Despeje
     cardList.Add(new Card(5/*28*/,"Mash",0,"sendToGraveyard",Resources.Load<Sprite>("27"),"despeje","none","none"));
     cardList.Add(new Card(6/*29*/,"Mash",0,"sendToGraveyard",Resources.Load<Sprite>("28"),"despeje","none","none"));
     cardList.Add(new Card(7/*30*/,"Mash",0,"sendToGraveyard",Resources.Load<Sprite>("29"),"despeje","none","none"));
     cardList.Add(new Card(8/*31*/,"Mash",0,"sendToGraveyard",Resources.Load<Sprite>("30"),"despeje","none","none"));
//Master
     //oro
     //distancia R
     cardList.Add(new Card(1234,"Rin Tohsaka",4,"espada",Resources.Load<Sprite>("0"),"lider","R","Master"));
     cardList.Add(new Card(28590,"Tokiomi Tohsaka",9,"magia",Resources.Load<Sprite>("1"),"oro","R","Master"));
     cardList.Add(new Card(389,"Matou Kariya",8,"arco",Resources.Load<Sprite>("2"),"oro","R","Master"));
     cardList.Add(new Card(444,"Matou Zouken",8,"arco",Resources.Load<Sprite>("3"),"oro","R","Master"));
     
     //plata
     //cuerpo a cuerpo M
     cardList.Add(new Card(555,"Emiya Shirou",3,"lo mismo q archer",Resources.Load<Sprite>("4"),"plata","M","Master"));
     cardList.Add(new Card(666,"Sisigou Kairi",5,"arco",Resources.Load<Sprite>("5"),"plata","M","Master"));
     cardList.Add(new Card(755,"Noah",7,"arco",Resources.Load<Sprite>("6"),"plata","M","Master"));


//servant
     //oro

     //cuerpo a cuerpo M
     cardList.Add(new Card(844,"Saber alter",6,"arco",Resources.Load<Sprite>("7"),"oro","M","Servant")); 
     cardList.Add(new Card(944,"Saber",7,"arco",Resources.Load<Sprite>("8"),"oro","M","Servant"));
     cardList.Add(new Card(1044,"Neron",8,"arco",Resources.Load<Sprite>("9"),"oro","M","Servant"));
     //asedio S
     cardList.Add(new Card(1144,"Joan of Arc",3,"arco",Resources.Load<Sprite>("10"),"oro","S","Servant"));
     cardList.Add(new Card(1244,"Saber Lancer",9,"arco",Resources.Load<Sprite>("11"),"oro","S","Servant"));
     cardList.Add(new Card(1344,"Joan of Arc alter",4,"arco",Resources.Load<Sprite>("12"),"oro","S","Servant"));
     cardList.Add(new Card(1444,"Enkidu",8,"arco",Resources.Load<Sprite>("13"),"oro","S","Servant"));
     //a distancia R
     cardList.Add(new Card(15,"Gilgamesh",9,"arco",Resources.Load<Sprite>("14"),"oro","R","Servant"));
     cardList.Add(new Card(16,"Merlin",4,"arco",Resources.Load<Sprite>("15"),"oro","R","Servant"));

     //plata

    //distancia R
     cardList.Add(new Card(17,"Nikola Tesla",8,"arco",Resources.Load<Sprite>("16"),"plata","R","Servant"));
     cardList.Add(new Card(18,"Morgan le Fay",4,"arco",Resources.Load<Sprite>("17"),"plata","R","Servant"));
     //asedio S
     cardList.Add(new Card(19,"Alexander Magnus",6,"arco",Resources.Load<Sprite>("18"),"plata","S","Servant"));
     cardList.Add(new Card(20,"Twice H. Pieceman",7,"arco",Resources.Load<Sprite>("19"),"plata","S","Servant"));
     //cuerpo a cuerpo
     cardList.Add(new Card(21,"Senji.Muramasa",6,"arco",Resources.Load<Sprite>("20"),"plata","M","Servant"));
     cardList.Add(new Card(22,"Mash",3,"arco",Resources.Load<Sprite>("21"),"plata","M","Servant"));

//Magicas


    //Clima
     cardList.Add(new Card(23,"Mash",0,"effectclima",Resources.Load<Sprite>("22"),"clima","none","none"));
     cardList.Add(new Card(24,"Mash",0,"effectclima",Resources.Load<Sprite>("23"),"clima","none","none"));
     cardList.Add(new Card(25,"Mash",0,"effectclima",Resources.Load<Sprite>("24"),"clima","none","none"));
     cardList.Add(new Card(26,"Mash",0,"effectclima",Resources.Load<Sprite>("25"),"clima","none","none"));
     cardList.Add(new Card(27,"Mash",0,"effectclima",Resources.Load<Sprite>("26"),"clima","none","none"));
    //Despeje
     cardList.Add(new Card(28,"Mash",0,"eliminaclima",Resources.Load<Sprite>("27"),"despeje","none","none"));
     cardList.Add(new Card(29,"Mash",0,"eliminaclima",Resources.Load<Sprite>("28"),"despeje","none","none"));
     cardList.Add(new Card(30,"Mash",0,"eliminaclima",Resources.Load<Sprite>("29"),"despeje","none","none"));
     cardList.Add(new Card(31,"Mash",0,"eliminaclima",Resources.Load<Sprite>("30"),"despeje","none","none"));
    //señuelo
     cardList.Add(new Card(32,"Mash",0,"colocarCard0",Resources.Load<Sprite>("31"),"señuelo","none","none"));
     cardList.Add(new Card(33,"Mash",0,"colocarCard0",Resources.Load<Sprite>("32"),"señuelo","none","none"));
     cardList.Add(new Card(34,"Mash",0,"colocarCard0",Resources.Load<Sprite>("33"),"señuelo","none","none"));
     cardList.Add(new Card(35,"Mash",0,"colocarCard0",Resources.Load<Sprite>("34"),"señuelo","none","none"));
     cardList.Add(new Card(36,"Mash",0,"colocarCard0",Resources.Load<Sprite>("35"),"señuelo","none","none"));
     //aumento
     cardList.Add(new Card(37,"Mash",0,"effectboost",Resources.Load<Sprite>("36"),"aumento","none","none"));
     cardList.Add(new Card(38,"Mash",0,"effectboost",Resources.Load<Sprite>("37"),"aumento","none","none"));
     cardList.Add(new Card(38,"Mash",0,"effectboost",Resources.Load<Sprite>("38"),"aumento","none","none"));
     cardList.Add(new Card(39,"Mash",0,"effectboost",Resources.Load<Sprite>("39"),"aumento","none","none"));
     cardList.Add(new Card(40,"Mash",0,"effectboost",Resources.Load<Sprite>("40"),"aumento","none","none"));
    }
 
        
    
}

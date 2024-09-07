using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Card
{
    public int id;
    public string cardName;
    public int power;
    public string descriptionHability;
    public string typeCard; // oro o plata
    public string typeAttack; // cuerpo, distancia, asalto
    public string legion; // servant o Master
    public Sprite thisImage;

    public Card() { }

    public Card(int Id, string CardName, int Power, string DescriptionHability, Sprite ThisImage, string TypeCard, string TypeAttack, string Legion)
    {
        id = Id;
        cardName = CardName;
        power = Power;
        descriptionHability = DescriptionHability;
        thisImage = ThisImage;
        typeCard = TypeCard;
        typeAttack = TypeAttack;
        legion = Legion;
    }
}

public class CardPersistence
{
    private static string savePath = Application.persistentDataPath + "/generatedCards.json";

    public static void SaveGeneratedCards(List<Card> generatedCards)
    {
        string json = JsonUtility.ToJson(new CardListWrapper { cards = generatedCards });
        File.WriteAllText(savePath, json);
    }

    public static List<Card> LoadGeneratedCards()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CardListWrapper wrapper = JsonUtility.FromJson<CardListWrapper>(json);
            return wrapper.cards;
        }
        return new List<Card>();
    }

    [System.Serializable]
    private class CardListWrapper
    {
        public List<Card> cards;
    }

    public void CompileAndSaveNewCard(string input)
    {
        Card newCard = InterpretLanguageInput(input); // Convierte el input en una instancia de Card
        CardDataBase.cardList.Add(newCard);
        SaveGeneratedCards(CardDataBase.cardList);
    }

    private Card InterpretLanguageInput(string input)
    {
        // Aquí iría tu lógica de interpretación para convertir el input en una instancia de Card
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize(input);

        var parser = new Parser(tokens);
        var ast = parser.Parse();

        foreach (var node in ast)
        {
            if (node is CardNode cardNode)
            {
                return new Card(
                    Id: GenerateUniqueId(),
                    CardName: cardNode.Name,
                    Power: cardNode.Power,
                    DescriptionHability: "", // Ajusta según sea necesario
                    ThisImage: null, // Ajusta según sea necesario
                    TypeCard: cardNode.Type,
                    TypeAttack: "", // Ajusta según sea necesario
                    Legion: cardNode.Faction
                );
            }
        }
        return null;
    }

    private int GenerateUniqueId()
{
    return 41 + CardDataBase.cardList.Count; // Generación de ID a partir de 41
}

}


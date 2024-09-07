using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Interpreter : MonoBehaviour
{
    public TMP_InputField inputField; // El campo de entrada para el código DSL
    public TMP_Text terminal;         // Donde se mostrarán los mensajes del terminal

    public void Compile()
    {
        string dsl = inputField.text; // Obtiene el código DSL del campo de entrada

        try
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(dsl);

            var parser = new Parser(tokens);
            var ast = parser.Parse();

            // Aquí puedes agregar lógica para traducir el AST a objetos del juego
            terminal.text = "Compilación exitosa!";
            // Procesar el AST para crear cartas
            ProcessAST(ast);
        }
        catch (System.Exception e)
        {
            terminal.text = "Error: " + e.Message;
        }
    }

    private void ProcessAST(List<Node> ast)
{
    foreach (var node in ast)
    {
        if (node is CardNode cardNode)
        {
            // Crear una nueva instancia de la clase Card usando los datos de cardNode
            Card newCard = new Card(
                Id: GenerateUniqueId(),
                CardName: cardNode.Name,
                Power: cardNode.Power,
                DescriptionHability: GetEffectAbility(cardNode.OnActivation), // Vincula el efecto a la habilidad
                ThisImage: null,
                TypeCard: cardNode.Type,
                TypeAttack: "", 
                Legion: cardNode.Faction
            );

            // Añadir la nueva carta a la base de datos
            CardDataBase.cardList.Add(newCard);

            Debug.Log($"Carta creada: {newCard.cardName} de tipo {newCard.typeCard}");
        }
    }
}

private string GetEffectAbility(List<ActivationNode> activations)
{
    foreach (var activation in activations)
    {
        if (activation.Effect != null)
        {
            switch (activation.Effect.Name)
            {
                case "boost":
                    return "effectboost"; // Vincular el efecto "boost" con la habilidad "effectboost"
                case "clima":
                    return "effectclima"; // Vincular el efecto "clima" con la habilidad "effectclima"
                case "deletemax":
                    return "deletemax"; // Vincular "deletemax" con su habilidad
                case "deletemin":
                    return "deletemin"; // Vincular "deletemin" con su habilidad
                case "addcard":
                    return "addcard"; // Vincular "addcard" con su habilidad
                case "sendToGraveyard":
                    return "sendToGraveyard"; // Vincular "sendToGraveyard" con su habilidad
                default:
                    return "nuevaHabilidad"; // Un caso por defecto
            }
        }
    }

    return "nuevaHabilidad"; // Retorno por defecto si no hay efecto
}


    private int GenerateUniqueId()
{
    return CardDataBase.cardList.Count + 41;
}

}

// Definición de tipos de token
public enum TokenType
{
    Keyword, Identifier, Number, String, Boolean, Symbol, Whitespace
}

// Definición de clase Token
public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString() => $"{Type}: {Value}";
}

// Tokenizador
public class Tokenizer
{
    private static readonly Dictionary<TokenType, string> Patterns = new Dictionary<TokenType, string>
    {
        { TokenType.Keyword, @"\b(effect|card|Type|Name|Faction|Power|Range|OnActivation|Params|Action|Selector|Source|Single|Predicate|PostAction)\b" },
        { TokenType.Identifier, @"\b[a-zA-Z_]\w*\b" },
        { TokenType.Number, @"\b\d+\b" },
        { TokenType.String, "\"[^\"]*\"" },
        { TokenType.Boolean, @"\b(true|false)\b" },
        { TokenType.Symbol, @"[{}:;,\.\(\)=><\-\*\+/]" },
        { TokenType.Whitespace, @"\s+" }
    };

    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int position = 0;

        while (position < input.Length)
        {
            Token token = null;

            foreach (var pattern in Patterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(input.Substring(position), "^" + pattern.Value);
                if (match.Success)
                {
                    token = new Token(pattern.Key, match.Value);
                    position += match.Length;
                    if (pattern.Key != TokenType.Whitespace)
                        tokens.Add(token);
                    break;
                }
            }

            if (token == null)
                throw new Exception($"Unexpected character at position {position}: {input[position]}");
        }

        return tokens;
    }
}

// Nodos AST
public abstract class Node { }

public class EffectNode : Node
{
    public string Name { get; set; }
    public Dictionary<string, string> Params { get; set; }
    public ActionNode Action { get; set; }
}

public class CardNode : Node
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Faction { get; set; }
    public int Power { get; set; }
    public List<string> Range { get; set; }
    public List<ActivationNode> OnActivation { get; set; }
}

public class ActivationNode : Node
{
    public EffectNode Effect { get; set; }
    public SelectorNode Selector { get; set; }
    public ActivationNode PostAction { get; set; }
}

public class SelectorNode : Node
{
    public string Source { get; set; }
    public bool Single { get; set; }
    public Func<CardNode, bool> Predicate { get; set; }
}

public class ActionNode : Node
{
    public List<string> Commands { get; set; }
}

// Parser
public class Parser
{
    private List<Token> tokens;
    private int currentTokenIndex;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        this.currentTokenIndex = 0;
    }

    private Token CurrentToken => currentTokenIndex < tokens.Count ? tokens[currentTokenIndex] : null;

    private Token Consume(TokenType expectedType)
    {
        var token = CurrentToken;
        if (token == null || token.Type != expectedType)
            throw new Exception($"Unexpected token: {token?.Value}");
        currentTokenIndex++;
        return token;
    }

    public List<Node> Parse()
    {
        var nodes = new List<Node>();

        while (currentTokenIndex < tokens.Count)
        {
            if (CurrentToken.Type == TokenType.Keyword && CurrentToken.Value == "effect")
                nodes.Add(ParseEffect());
            else if (CurrentToken.Type == TokenType.Keyword && CurrentToken.Value == "card")
                nodes.Add(ParseCard());
            else
                throw new Exception($"Unexpected top-level construct: {CurrentToken.Value}");
        }

        return nodes;
    }

    private EffectNode ParseEffect()
    {
        Consume(TokenType.Keyword); // "effect"
        Consume(TokenType.Symbol); // "{"

        var effectNode = new EffectNode { Params = new Dictionary<string, string>() };

        while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "}")
        {
            var key = Consume(TokenType.Keyword).Value;
            Consume(TokenType.Symbol); // ":"
            switch (key)
            {
                case "Name":
                    effectNode.Name = Consume(TokenType.String).Value.Trim('"');
                    break;
                case "Params":
                    Consume(TokenType.Symbol); // "{"
                    while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "}")
                    {
                        var paramName = Consume(TokenType.Identifier).Value;
                        Consume(TokenType.Symbol); // ":"
                        var paramType = Consume(TokenType.Identifier).Value;
                        effectNode.Params[paramName] = paramType;
                        if (CurrentToken.Type == TokenType.Symbol && CurrentToken.Value == ",")
                            Consume(TokenType.Symbol); // ","
                    }
                    Consume(TokenType.Symbol); // "}"
                    break;
                case "Action":
                    effectNode.Action = ParseAction();
                    break;
                default:
                    throw new Exception($"Unknown key in effect: {key}");
            }
        }

        Consume(TokenType.Symbol); // "}"
        return effectNode;
    }

    private ActionNode ParseAction()
    {
        Consume(TokenType.Symbol); // "("
        Consume(TokenType.Identifier); // Parámetro "Targets"
        Consume(TokenType.Symbol); // ","
        Consume(TokenType.Identifier); // Parámetro "context"
        Consume(TokenType.Symbol); // ")"
        Consume(TokenType.Symbol); // "=>"

        var actionNode = new ActionNode { Commands = new List<string>() };

        if (CurrentToken.Type == TokenType.Symbol && CurrentToken.Value == "{")
        {
            Consume(TokenType.Symbol); // "{"
            while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "}")
            {
                actionNode.Commands.Add(Consume(TokenType.Identifier).Value);
            }
            Consume(TokenType.Symbol); // "}"
        }
        else
        {
            actionNode.Commands.Add(Consume(TokenType.Identifier).Value);
        }

        return actionNode;
    }

    private CardNode ParseCard()
    {
        Consume(TokenType.Keyword); // "card"
        Consume(TokenType.Symbol); // "{"

        var cardNode = new CardNode
        {
            Range = new List<string>(),
            OnActivation = new List<ActivationNode>()
        };

        while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "}")
        {
            var key = Consume(TokenType.Keyword).Value;
            Consume(TokenType.Symbol); // ":"
            switch (key)
            {
                case "Type":
                    cardNode.Type = Consume(TokenType.String).Value.Trim('"');
                    break;
                case "Name":
                    cardNode.Name = Consume(TokenType.String).Value.Trim('"');
                    break;
                case "Faction":
                    cardNode.Faction = Consume(TokenType.String).Value.Trim('"');
                    break;
                case "Power":
                    cardNode.Power = int.Parse(Consume(TokenType.Number).Value);
                    break;
                case "Range":
                    Consume(TokenType.Symbol); // "["
                    while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "]")
                    {
                        cardNode.Range.Add(Consume(TokenType.String).Value.Trim('"'));
                        if (CurrentToken.Type == TokenType.Symbol && CurrentToken.Value == ",")
                            Consume(TokenType.Symbol); // ","
                    }
                    Consume(TokenType.Symbol); // "]"
                    break;
                case "OnActivation":
                    Consume(TokenType.Symbol); // "["
                    while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "]")
                    {
                        cardNode.OnActivation.Add(ParseActivation());
                        if (CurrentToken.Type == TokenType.Symbol && CurrentToken.Value == ",")
                            Consume(TokenType.Symbol); // ","
                    }
                    Consume(TokenType.Symbol); // "]"
                    break;
                default:
                    throw new Exception($"Unknown key in card: {key}");
            }
        }

        Consume(TokenType.Symbol); // "}"
        return cardNode;
    }

    private ActivationNode ParseActivation()
    {
        Consume(TokenType.Symbol); // "{"
        var activationNode = new ActivationNode();

        while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "}")
        {
            var key = Consume(TokenType.Keyword).Value;
            Consume(TokenType.Symbol); // ":"
            switch (key)
            {
                case "Effect":
                    activationNode.Effect = ParseEffect();
                    break;
                case "Selector":
                    activationNode.Selector = ParseSelector();
                    break;
                case "PostAction":
                    activationNode.PostAction = ParseActivation();
                    break;
                default:
                    throw new Exception($"Unknown key in activation: {key}");
            }
        }

        Consume(TokenType.Symbol); // "}"
        return activationNode;
    }

    private SelectorNode ParseSelector()
    {
        Consume(TokenType.Symbol); // "{"
        var selectorNode = new SelectorNode();

        while (CurrentToken.Type != TokenType.Symbol || CurrentToken.Value != "}")
        {
            var key = Consume(TokenType.Keyword).Value;
            Consume(TokenType.Symbol); // ":"
            switch (key)
            {
                case "Source":
                    selectorNode.Source = Consume(TokenType.String).Value.Trim('"');
                    break;
                case "Single":
                    selectorNode.Single = bool.Parse(Consume(TokenType.Boolean).Value);
                    break;
                case "Predicate":
                    selectorNode.Predicate = card => card.Power > 5; // Ejemplo de predicado
                    break;
                default:
                    throw new Exception($"Unknown key in selector: {key}");
            }
        }

        Consume(TokenType.Symbol); // "}"
        return selectorNode;
    }
}

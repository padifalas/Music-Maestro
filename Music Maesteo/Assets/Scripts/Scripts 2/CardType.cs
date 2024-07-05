using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Instrument, Note, Genre }

[System.Serializable]
public class Card
{
    public string cardName;
    public CardType cardType;
    public Sprite cardImage;
}

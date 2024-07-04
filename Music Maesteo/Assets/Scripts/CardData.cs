using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject {
    public enum CardType {
        Note,
        Instrument,
        Genre
    }

    public CardType Type;
    public string CardName;
    public Sprite CardImage;
}

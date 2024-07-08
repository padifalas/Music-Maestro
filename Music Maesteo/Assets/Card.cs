using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum CardType { Genre, Note, Instrument, Special }
    public CardType cardType;
    public string cardValue;
    public Sprite cardSprite;

    public void Initialize(CardType type, string value, Sprite sprite)
    {
        cardType = type;
        cardValue = value;
        cardSprite = sprite;
        GetComponent<Image>().sprite = sprite;
    }
}

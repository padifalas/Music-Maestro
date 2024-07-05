using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public TMP_Text nameText;
    public Image artworkImage;
    private Card card;

    public void Setup(Card newCard)
    {
        card = newCard;
        nameText.text = card.cardName;
        artworkImage.sprite = card.cardImage;
    }
}
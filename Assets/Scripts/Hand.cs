using System.Collections.Generic;

public class Hand
{
    private List<Card> cards = new List<Card>();

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public int GetValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (Card card in cards)
        {
            if (card.Rank == Rank.Ace)
            {
                aceCount++;
                value += 11;
            }
            else if (card.Rank >= Rank.Jack)
            {
                value += 10;
            }
            else
            {
                value += (int)card.Rank;
            }
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10;
            aceCount--;
        }

        return value;
    }

    public int CardCount()
    {
        return cards.Count;
    }

    public bool IsBusted()
    {
        return GetValue() > 21;
    }

    public bool IsBlackjack()
    {
        return GetValue() == 21 && CardCount() == 2;
    }

    public Card GetCard(int index)
    {
        if (index < 0 || index >= cards.Count) return null;
        return cards[index];
    }

    public string ToDisplayString(bool hideSecondCard = false)
    {
        if (cards.Count == 0) return "(sin cartas)";

        if (hideSecondCard && cards.Count >= 2)
        {
            return $"{cards[0]} + [Carta Oculta]";
        }

        string result = "";
        for (int i = 0; i < cards.Count; i++)
        {
            result += cards[i].ToString();
            if (i < cards.Count - 1) result += ", ";
        }
        return result;
    }


}

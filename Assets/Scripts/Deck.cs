using System.Collections.Generic;

public class Deck
{
    private List<Card> cards = new List<Card>();

    public Deck()
    {
        CreateDeck();
        Shuffle();
    }

    private void CreateDeck()
    {
        cards.Clear();

        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    private void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
            return null;

        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_Text dealerText;
    public TMP_Text statusText;

    public Button hitButton;
    public Button standButton;

    private Deck deck;
    private Hand playerHand;
    private Hand dealerHand;

    private bool playerTurn = true;
    private bool roundOver = false;

    void Start()
    {
        StartRound();

        if (hitButton != null)
            hitButton.onClick.AddListener(PlayerHit);

        if (standButton != null)
            standButton.onClick.AddListener(PlayerStand);

        UpdateUI(hideDealerSecondCard: true);
    }


    void Update()
    {
        if (roundOver) return;

        if (playerTurn)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                PlayerHit();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                PlayerStand();
            }
        }
    }

    private void StartRound()
    {
        deck = new Deck();
        playerHand = new Hand();
        dealerHand = new Hand();

        playerHand.AddCard(deck.DrawCard());
        playerHand.AddCard(deck.DrawCard());

        dealerHand.AddCard(deck.DrawCard());
        dealerHand.AddCard(deck.DrawCard());

        Debug.Log("=== NUEVA RONDA ===");
        Debug.Log("Jugador: " + playerHand.ToDisplayString() + " | Valor: " + playerHand.GetValue());
        Debug.Log("Dealer: " + dealerHand.ToDisplayString(hideSecondCard: true));


        if (playerHand.IsBlackjack())
        {
            Debug.Log("Blackjack del jugador. Ganas automáticamente.");
            roundOver = true;
        }

        statusText.text = "Tu turno: Hit o Stand";
        UpdateUI(hideDealerSecondCard: true);

    }

    private void PlayerHit()
    {
        playerHand.AddCard(deck.DrawCard());
        Debug.Log("Jugador pide. Ahora tiene: " + playerHand.GetValue());

        if (playerHand.IsBusted())
        {
            Debug.Log("Te pasaste de 21. Pierdes.");
            roundOver = true;
        }
        else if (playerHand.GetValue() == 21)
        {
            Debug.Log("Llegaste a 21. Te plantas automáticamente.");
            PlayerStand();
        }
    }

    private void PlayerStand()
    {
        playerTurn = false;
        Debug.Log("Jugador se planta con: " + playerHand.GetValue());

        DealerPlay();
        DecideWinner();
        roundOver = true;
    }

    private void DealerPlay()
    {
        Debug.Log("Turno del dealer. Revela mano: " + dealerHand.ToDisplayString() + " | Valor: " + dealerHand.GetValue());

        while (dealerHand.GetValue() < 17)
        {
            dealerHand.AddCard(deck.DrawCard());
            Debug.Log("Dealer roba. Ahora tiene: " + dealerHand.GetValue());
        }

        if (dealerHand.IsBusted())
        {
            Debug.Log("Dealer se pasó de 21.");
        }
        else
        {
            Debug.Log("Dealer se planta con: " + dealerHand.GetValue());
        }
    }

    private void DecideWinner()
    {
        int playerValue = playerHand.GetValue();
        int dealerValue = dealerHand.GetValue();

        if (dealerValue > 21)
        {
            statusText.text = "Ganas. El dealer se pasó de 21.";
        }
        else if (playerValue > dealerValue)
        {
            statusText.text = "Ganas. Tu mano es mayor.";
        }
        else if (playerValue < dealerValue)
        {
            statusText.text = "Pierdes. La mano del dealer es mayor.";
        }
        else
        {
            statusText.text = "Empate.";
        }

        roundOver = true;
        UpdateUI(hideDealerSecondCard: false);
    }

    private void UpdateUI(bool hideDealerSecondCard)
    {
        playerText.text = "Jugador: " + playerHand.ToDisplayString() + "\nValor: " + playerHand.GetValue();
        dealerText.text = "Dealer: " + dealerHand.ToDisplayString(hideDealerSecondCard);

        if (roundOver)
        {
            if (hitButton != null) hitButton.interactable = false;
            if (standButton != null) standButton.interactable = false;
        }
    }

}

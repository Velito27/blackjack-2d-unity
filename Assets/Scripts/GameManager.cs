using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_Text dealerText;
    public TMP_Text statusText;
    public Transform playerCardsContainer;
public Transform dealerCardsContainer;
public TMP_FontAsset cardFont;



    public Button hitButton;
    public Button standButton;
    public Button newRoundButton;


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

        if (newRoundButton != null)
{
    newRoundButton.onClick.AddListener(StartNewRound);
    newRoundButton.gameObject.SetActive(false);
}


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
        if (newRoundButton != null) newRoundButton.gameObject.SetActive(true);
       UpdateUI(hideDealerSecondCard: true);
StartCoroutine(PopText(playerText));

if (playerHand.IsBusted())
{
    statusText.text = "Te pasaste de 21. Pierdes.";
    roundOver = true;

    if (newRoundButton != null) newRoundButton.gameObject.SetActive(true);
    UpdateUI(hideDealerSecondCard: false);
}


    }

   private void PlayerStand()
{
    playerTurn = false;
    roundOver = true;
    StartCoroutine(DealerPlayRoutine());
}


    private System.Collections.IEnumerator DealerPlayRoutine()
    {
        statusText.text = "Turno del dealer...";
        UpdateUI(hideDealerSecondCard: false);
        StartCoroutine(PopText(statusText));

        yield return new WaitForSeconds(0.6f);

        while (dealerHand.GetValue() < 17)
        {
            dealerHand.AddCard(deck.DrawCard());
            UpdateUI(hideDealerSecondCard: false);
            StartCoroutine(PopText(dealerText));

            yield return new WaitForSeconds(0.6f);
        }

        DecideWinner();
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
        if (newRoundButton != null) newRoundButton.gameObject.SetActive(true);

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
        DrawHand(playerHand, playerCardsContainer, false);
DrawHand(dealerHand, dealerCardsContainer, hideDealerSecondCard);

    }
    private void StartNewRound()
    {
        playerTurn = true;
        roundOver = false;

        if (hitButton != null) hitButton.interactable = true;
        if (standButton != null) standButton.interactable = true;

        if (newRoundButton != null) newRoundButton.gameObject.SetActive(false);

        StartRound();
    }
    private System.Collections.IEnumerator PopText(TMP_Text text, float scaleUp = 1.15f, float duration = 0.08f)
    {
        if (text == null) yield break;

        Transform t = text.transform;
        Vector3 original = t.localScale;

        t.localScale = original * scaleUp;
        yield return new WaitForSeconds(duration);

        t.localScale = original;
    }

private void ClearCards(Transform container)
{
    foreach (Transform child in container)
    {
        Destroy(child.gameObject);
    }
}
private void DrawHand(Hand hand, Transform container, bool hideSecondCard)
{
    ClearCards(container);

    for (int i = 0; i < hand.CardCount(); i++)
    {
        if (hideSecondCard && i == 1)
        {
            CreateCardText("🂠", container);
        }
        else
        {
            CreateCardText(hand.GetCard(i).ToString(), container);
        }
    }
}
private void CreateCardText(string text, Transform container)
{
    GameObject go = new GameObject("Card");
    go.transform.SetParent(container, false);

    TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
    tmp.text = text;
    tmp.fontSize = 48;
    tmp.alignment = TextAlignmentOptions.Center;
    tmp.color = Color.white;

    if (cardFont != null)
        tmp.font = cardFont;

    RectTransform rt = tmp.rectTransform;
    rt.sizeDelta = new Vector2(60, 80);
    rt.localScale = Vector3.one;
}



}

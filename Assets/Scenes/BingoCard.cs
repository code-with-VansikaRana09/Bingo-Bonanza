using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BingoCard : MonoBehaviour
{
    [SerializeField] private GameObject winPanel; // Drag WinPanel here in Inspector
    [SerializeField] private TextMeshProUGUI winText; // Drag WinText here in Inspector
    public Button[] numberButtons; // Array to hold the 48 buttons
    [SerializeField] private TextMeshProUGUI randomNumberText; // Drag a UI Text here
    private HashSet<int> clickedNumbers = new HashSet<int>(); // Track clicked numbers
    private int currentTargetNumber = -1; // Current number to find

    void Start()
    {
        GenerateNumbers();

        // Add click event to each button
        foreach (Button btn in numberButtons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }

        GenerateNewNumber();
    }

    void OnButtonClick(Button clickedButton)
    {
        int clickedNumber = int.Parse(clickedButton.GetComponentInChildren<TextMeshProUGUI>().text);

        // If the clicked number is the current target
        if (clickedNumber == currentTargetNumber)
        {
            // Change button color when clicked
            ColorBlock colors = clickedButton.colors;
            colors.normalColor = Color.gray;
            clickedButton.colors = colors;

            // Disable the button after clicking
            clickedButton.interactable = false;

            // Track the clicked number
            clickedNumbers.Add(clickedNumber);

            Debug.Log($"Button {clickedNumber} clicked!");

            // Check for BINGO
            CheckForWin();

            // Generate a new number that hasn't been clicked
            GenerateNewNumber();
        }
    }

    void GenerateNewNumber()
    {
        List<int> availableNumbers = new List<int>();

        foreach (Button btn in numberButtons)
        {
            int number = int.Parse(btn.GetComponentInChildren<TextMeshProUGUI>().text);
            if (!clickedNumbers.Contains(number))
            {
                availableNumbers.Add(number);
            }
        }

        if (availableNumbers.Count > 0)
        {
            currentTargetNumber = availableNumbers[Random.Range(0, availableNumbers.Count)];
            randomNumberText.text = "Find: " + currentTargetNumber;
        }
        else
        {
            randomNumberText.text = "All numbers clicked!";
        }
    }

    void CheckForWin()
    {
        int gridSize = 6;  // 6 rows in a 6x8 grid

        for (int row = 0; row < gridSize; row++)
        {
            bool rowComplete = true;
            for (int col = 0; col < 8; col++)
            {
                int index = row * 8 + col;
                if (numberButtons[index].interactable)
                {
                    rowComplete = false;
                    break;
                }
            }

            if (rowComplete)
            {
                Debug.Log("🎉 BINGO! Row completed!");
                ShowWinMessage();
                return;
            }
        }
    }

    void ShowWinMessage()
    {
        winPanel.SetActive(true);
        winText.text = "🎉 BINGO! You Win! 🎉";
    }

    public void RestartGame()
    {
        winPanel.SetActive(false);
        clickedNumbers.Clear();

        foreach (Button btn in numberButtons)
        {
            btn.interactable = true;
            ColorBlock colors = btn.colors;
            colors.normalColor = Color.white;
            btn.colors = colors;
        }

        GenerateNumbers();
        GenerateNewNumber();
    }

    void GenerateNumbers()
    {
        int[] numbers = new int[48];
        for (int i = 0; i < 48; i++)
        {
            numbers[i] = i + 1;
        }

        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        for (int i = 0; i < numberButtons.Length; i++)
        {
            numberButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = numbers[i].ToString();
        }
    }
}




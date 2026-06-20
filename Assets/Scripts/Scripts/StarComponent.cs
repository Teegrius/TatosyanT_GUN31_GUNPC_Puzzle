using UnityEngine;
using TMPro; // Обязательно подключаем библиотеку TextMeshPro

public class StarComponent : MonoBehaviour
{
    public static int starsCollected = 0;
    
    // Ссылка на компонент TextMeshPro
    private static TextMeshProUGUI scoreText; 

    private void Start()
    {
        if (scoreText == null)
        {
            GameObject textObj = GameObject.Find("ScoreText");
            if (textObj != null)
            {
                scoreText = textObj.GetComponent<TextMeshProUGUI>();
                UpdateUI();
            }
            else
            {
                Debug.LogWarning("UI текст 'ScoreText' не найден на сцене!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            starsCollected++;
            UpdateUI(); 
            
            Debug.Log($"Звезда собрана! Всего: {starsCollected}");
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Души: {starsCollected}";
        }
    }
}
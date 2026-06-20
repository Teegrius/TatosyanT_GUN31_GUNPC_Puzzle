using UnityEngine;

public class StarComponent : MonoBehaviour
{
    // Переменная для подсчета собранных звезд
    public static int starsCollected = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что коснулись именно игрока
        if (collision.CompareTag("Player"))
        {
            starsCollected++;
            Debug.Log($"Звезда собрана! Всего в копилке: {starsCollected}");
            
            // Уничтожаем объект звезды
            Destroy(gameObject);
        }
    }
}
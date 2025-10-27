using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }
}

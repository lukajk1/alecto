using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void OnEnable()
    {
        Game.ScoreUpdated += ScoreUpdated;
    }

    private void OnDisable()
    {
         Game.ScoreUpdated -= ScoreUpdated;
    }
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void ScoreUpdated()
    {
        text.text = Game.Score.ToString();
    }
}

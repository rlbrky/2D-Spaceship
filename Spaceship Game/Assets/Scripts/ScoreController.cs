using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] int score;
    private Text updatedText;

    private void Start()
    {
        ResetScore();
        //Text component'ýna sahip obje zaten bu scripte sahip olduðu için direkt bu þekilde ulaþabiliyoruz.
        updatedText = GetComponent<Text>();
    }

    //Düþman kontrolünde çaðýracaðýmýz için public olmalý.
    public void AddScore(int points)
    {
        score += points;
        updatedText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        updatedText.text = score.ToString();
    }
}

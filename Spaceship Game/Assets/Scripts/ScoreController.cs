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
        //Text component'�na sahip obje zaten bu scripte sahip oldu�u i�in direkt bu �ekilde ula�abiliyoruz.
        updatedText = GetComponent<Text>();
    }

    //D��man kontrol�nde �a��raca��m�z i�in public olmal�.
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

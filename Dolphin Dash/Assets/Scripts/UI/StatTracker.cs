// SID 1901981
// Date:  11/12/2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatTracker : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI multiplierText;

    void Update()
    {
        pointsText.text = GameController.Points.ToString(); // updates pointx text in hud
        scoreText.text = Mathf.Round(GameController.Score).ToString(); // update score text in hud

        // Round to 2 decimal places
        multiplierText.text = (Mathf.Round(GameController.Multiplier * 100f) * 0.01f).ToString(); // updates multiplier  txt in hud
    }
}   


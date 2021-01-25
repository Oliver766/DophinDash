// SID 1901981
// Date : 11/12/2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOver : MonoBehaviour
{
    /// <summary>
    /// Gets Text object for ditance and score
    /// </summary>
    public TextMeshProUGUI Distance;
    public TextMeshProUGUI Score;



    private void Start()
    {
        Distance.text = Mathf.Round(GameController.Distance).ToString() + "m"; // Writes distance on screen as number is converted into a whole number
        Score.text = Mathf.Round(GameController.Score).ToString();// Writes score on screen as number 

    }

}

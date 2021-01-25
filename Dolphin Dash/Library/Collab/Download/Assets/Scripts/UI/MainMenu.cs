using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Controls;
    public GameObject Credits;
    // public GameObject options;
    



    public void Play() // function for play game
    {
        GameController.Begin(); // begins game
    }



    public void Quit() // function to exit the game
    {

        Application.Quit(); // quits the application


    }

    public void Creditsmenu() // function for selecting  credits in mainmenu
    {
        Credits.SetActive(true); // sets credit canvas active
        

    }

    public void BackButton() //Function to go back to menu
    {
        Credits.SetActive(false);// sets credit canvas false

        Controls.SetActive(false);// sets controls canvas false
        // options.SetActive(false);


    }

    public void HowToPlaymenu()// function for selecting  Controls  in mainmenu
    {

        Controls.SetActive(true);// sets controls canvas true

    }

    // public void Options()
    // {

    //     options.SetActive(true);


    // }

}




   

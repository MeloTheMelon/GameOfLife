using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour {

    //Board Generator Object
    public BoardGen bg;
    //Textinput-Felder
    public Text rA, rB, rC1, rC2, rD, timeDelay, length, height;
    //UI-Elemente die ein bzw ausgeschalten werden
    public GameObject settingsScreen, startScreen, tutScreen, buttonOverlay, background;


    //Schaltet das Start-UI aus und das Tutorial-Popup ein
    //Generiert auch das Raster
    public void startGame()
    {
        startScreen.SetActive(false);
        background.SetActive(false);
        buttonOverlay.SetActive(true);
        tutScreen.SetActive(true);
        bg.generateBoard();
    }

    //Schließt das Tutorial-Popup
    public void tutButton()
    {
        tutScreen.SetActive(false);
        bg.tutGone = true;
    }

    //Startet die Simulation
    public void startSim()
    {
        StartCoroutine(bg.GameLogic());
    }

    //Setzt das Spielfeld zurück
    public void resetField()
    {
        bg.stopGame();
        bg.resetBoard();
    }

    //Übernimmt geänderte Einstellungen
    public void submitButton()
    {
        if (rA.text.Equals(""))
            bg.ruleA = 3;
        else
            bg.ruleA = int.Parse(rA.text);

        if (rB.text.Equals(""))
            bg.ruleB = 2;
        else
            bg.ruleB = int.Parse(rB.text);

        if (rC1.text.Equals(""))
            bg.ruleC1 = 2;
        else
            bg.ruleC1 = int.Parse(rC1.text);

        if (rC2.text.Equals(""))
            bg.ruleC2 = 3;
        else
            bg.ruleC2 = int.Parse(rC2.text);

        if (rD.text.Equals(""))
            bg.ruleD = 3;
        else
            bg.ruleD = int.Parse(rD.text);

        if (length.text.Equals(""))
            bg.length = 5;
        else
            bg.length = int.Parse(length.text);

        if (height.text.Equals(""))
            bg.height = 5;
        else
            bg.height = int.Parse(height.text);

        if (timeDelay.text.Equals(""))
            bg.delayTime = 1;
        else
            bg.delayTime = int.Parse(timeDelay.text);

        settingsScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    //Beendet das Programm
    public void quitButton()
    {
        Application.Quit();
    }

    //Öffnet das Settings Menü
    public void settingsButton()
    {
        startScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    //Beendet die Simulation und lädt das Hauptmenü
    public void exitGameButton()
    {
        SceneManager.LoadScene(0);
    }

}

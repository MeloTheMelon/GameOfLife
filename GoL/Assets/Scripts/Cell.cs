using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    //Status der Zelle
    bool alive = false;

    //Texturen um den Status anzuzeigen
    public Material redMat, greenMat;

    void Update () {

        //Passt das Material dem Zustand an
        if (alive)
        {
            this.GetComponent<Renderer>().material = greenMat;
        }
        else
        {
            this.GetComponent<Renderer>().material = redMat;
        }
    }

    //Getter und Setter für den Status

    public bool getStatus()
    {
        return alive;
    }

    public void setStatus(bool status)
    {
        alive = status;
    }

}

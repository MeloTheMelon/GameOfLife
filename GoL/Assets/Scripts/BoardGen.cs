using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGen : MonoBehaviour {

    //Zellen Prefab
    public GameObject cell;
    //Maße des Rasters
    public int length, height;
    //Zeitabstand zwischen den Generationen
    public float delayTime = 1;

    //Werte der einzelnen Spielregeln
    public int ruleA = 3;
    public int ruleB = 2; 
    public int ruleC1 = 2, ruleC2 = 3;
    public int ruleD = 3;

    //Boolean welcher anzeigt ob das Tutorial-Popup noch geöffnet ist
    public bool tutGone = false;

    //Array mit allen Zellen
    GameObject[,] board;

    //Booleans zur Fehlervermeidung
    bool boardGenerated = false;
    bool started = false;

	void Update () {

       //Klicke auf Zellen um den Status zu ändern     
       if (boardGenerated && tutGone && Input.GetMouseButtonDown(0))
       {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.GetComponent<Cell>().setStatus(!hit.transform.GetComponent<Cell>().getStatus());
            }
        }
	}

    //Erstellt das Spielfeld
    public void generateBoard()
    {
        board = new GameObject[length, height];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                board[i, j] = Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity);
            }
        }
        boardGenerated = true;
        setupCamera();
    }

    //Passt die Kameraposition an damit immer das ganze Spielfeld zu sehen ist
    void setupCamera()
    {

        float s = (((float)Mathf.Max(length, height) + 2) / 2) / Mathf.Cos((60 * Mathf.PI) / 180);
        Camera.main.transform.position = new Vector3(((float)length) / 2, (((float)height) / 2) - 0.5f, -s);
        
    }

    //Modulo-Funktion die auch negative Zahlen berücksichtigt
    int mod(int val, int boundary)
    {

        if (val < 0)
        {
            val += boundary;
        }

        return val % boundary;

    }

    //Gibt den Wert einer Zelle des Arrays zurück und berücksichtigt die Randfälle
    int getValueAt(GameObject[,] boardCopy,int x, int y)
    {
        
        if (boardCopy[mod(x,length),mod(y,height)].GetComponent<Cell>().getStatus())
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }

    //Berechnet den neuen Status der Zelle, mit den Regeln des Spieles
    bool evaluateCellStatus(bool cellStatus, int aliveCellsSurrounding)
    {

        if (!cellStatus && aliveCellsSurrounding == ruleA)
            return true;

        if (cellStatus && aliveCellsSurrounding <ruleB)
            return false;

        if (cellStatus && aliveCellsSurrounding >= ruleC1 && aliveCellsSurrounding <= ruleC2)
            return true;

        if (cellStatus && aliveCellsSurrounding > ruleD)
            return false;

        return cellStatus;

    }

    //Setzt das Spielfeld zurück
    public void resetBoard()
    {
        foreach(GameObject g in board)
        {
            g.GetComponent<Cell>().setStatus(false);
        }
    }

    //Pausiert die Simulation
    public void stopGame()
    {
        started = false;
    }

    //Führt jede Generation alle nötigen Schritte aus
    public IEnumerator GameLogic()
    {
        if (boardGenerated && !started)
        {
            started = true;
            while (started)
            {
                bool[,] tempArray = new bool[length, height];
                //Berechnung der Anzahl der lebenden umliegenden Zellen
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        int counter = 0;
                        counter += getValueAt(board, i + 1, j);
                        counter += getValueAt(board, i - 1, j);
                        counter += getValueAt(board, i, j + 1);
                        counter += getValueAt(board, i, j - 1);
                        counter += getValueAt(board, i + 1, j + 1);
                        counter += getValueAt(board, i + 1, j - 1);
                        counter += getValueAt(board, i - 1, j + 1);
                        counter += getValueAt(board, i - 1, j - 1);
                        //Speichern des neuen Status der Zelle
                        tempArray[i, j] = (evaluateCellStatus(board[i, j].GetComponent<Cell>().getStatus(), counter));
                    }
                }

                //Übertragung der Auswertung auf das Spielfeld (Druch Verwendung von einem 2. Spielfeld wäre diese Scheife unnötig, aber durch das Delay zwischen den Generationen ist die Laufzeit nicht sehr wichtig, Speicherbedarf wäre der selbe da die Werte nicht zwischen gespeichert werden müssten)
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        board[i, j].GetComponent<Cell>().setStatus(tempArray[i, j]);
                    }
                }

                yield return new WaitForSeconds(delayTime);

            }
        }
    }
}

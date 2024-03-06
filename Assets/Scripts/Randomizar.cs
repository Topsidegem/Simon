using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Randomizar : MonoBehaviour
{
    public List<int> sequence = new List<int>();
    public float sequenceDelay = 1f;
    public float buttonHighlightDuration = 0.5f;
    public bool pActivate = false;
    private Simon actions;

    public int maxNumSec = 1;
    public int clickIndex = 0;

    //public GameObject Verde;
    //public GameObject Rojo;
    //public GameObject Amarillo;
    //public GameObject Azul;

    private List<int> playerSequence = new List<int>();
    private bool playerTurn = false;

    private Material normal;
    private Material resaltado;

    void Start()
    {
        GenerateNumber();
        actions = new Simon();
        actions.Enable();
        actions.Raycast.Click.performed += Ray;
    }

    private void GenerateNumber()
    {
        sequence.Add(Random.Range(0, 4));
        pActivate = true;
        clickIndex = 0;
    }

    private void Ray(InputAction.CallbackContext ctx)
    {
        if (pActivate)
        {
            var ray = Camera.main.ScreenPointToRay(actions.Raycast.MousePosition.ReadValue<Vector2>());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(sequence[clickIndex] == 0 && hit.transform.tag == "Verde" ||
                    sequence[clickIndex] == 1 && hit.transform.tag == "Rojo" ||
                    sequence[clickIndex] == 2 && hit.transform.tag == "Amarillo"||
                    sequence[clickIndex] == 3 && hit.transform.tag == "Azul")
                {
                    if(clickIndex >= sequence.Count)
                    {
                        pActivate = false;
                        GenerateNumber();
                    }
                    else
                    {
                        clickIndex ++;
                    }
                }
                else
                {
                    //reiniciar el arreglo
                    //desactivar al jugador
                    //mandar mensaje de perdiste
                    // menu
                }

            }
        }
       
    }

    IEnumerator ShowSequence()
    {
        foreach (int colorIndex in sequence)
        {
            HighlightButton(colorIndex);
            yield return new WaitForSeconds(buttonHighlightDuration);
            UnhighlightButton(colorIndex);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void HighlightButton(int index)
    {
        if (normal == null)
        {
            normal = GetComponent<Renderer>().material;
        }


        GetComponent<Renderer>().material = resaltado;
        Debug.Log("Iluminar botón " + index);
    }

    void UnhighlightButton(int index)
    {
        if (normal != null)
        {
            GetComponent<Renderer>().material = normal;
        }
        Debug.Log("Apagar botón " + index);
    }


    public void RegisterButtonPress(int buttonIndex)
    {
        if (playerTurn)
        {
            playerSequence.Add(buttonIndex);
        }
    }
}

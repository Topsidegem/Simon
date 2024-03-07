using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Randomizar : MonoBehaviour
{
    public List<int> sequence = new List<int>();
    public bool pActivate = false;
    private Simon actions;

    public int clickIndex = 0;

    private Menu menuScript;

    [SerializeField] GameObject[] buttons;

    Color originalColor;
    void Start()
    {
        GenerateNumber();
        actions = new Simon();
        actions.Enable();
        actions.Raycast.Click.performed += Ray;

        menuScript = GameObject.FindObjectOfType<Menu>();
        if (menuScript == null)
        {
            Debug.LogError("No se pudo encontrar el script Menu en la escena.");
        }
    }

    private void GenerateNumber()
    {
        sequence.Add(Random.Range(0, 4));
        pActivate = true;
        clickIndex = 0;

        StartCoroutine(ShowSequence());
    }

    private void Ray(InputAction.CallbackContext ctx)
    {
        if (pActivate)
        {
            var ray = Camera.main.ScreenPointToRay(actions.Raycast.MousePosition.ReadValue<Vector2>());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (sequence[clickIndex] == 0 && hit.transform.tag == "Verde" ||
                    sequence[clickIndex] == 1 && hit.transform.tag == "Rojo" ||
                    sequence[clickIndex] == 2 && hit.transform.tag == "Amarillo" ||
                    sequence[clickIndex] == 3 && hit.transform.tag == "Azul")
                {
                    clickIndex++;
                    if (clickIndex >= sequence.Count)
                    {
                        pActivate = false;
                        GenerateNumber();
                    }
                }
                else
                {
                    menuScript.Perdiste();

                    Debug.Log("¡Has perdido! Reiniciando el juego...");
                    ResetGame();
                }

            }
        }

    }

    IEnumerator ShowSequence()
    {
        foreach (int colorIndex in sequence)
        {
            
            HighlightButton(colorIndex);
            yield return new WaitForSeconds(1f); 
            UnhighlightButton(colorIndex);
            yield return new WaitForSeconds(0.5f); 
        }
        pActivate = true; 
    }

    void HighlightButton(int index)
    {
        Renderer buttonRenderer = GetButtonRenderer(index);

        originalColor = buttons[index].GetComponent<MeshRenderer>().material.color;
        
        buttonRenderer.material.color = Color.magenta;

        Debug.Log("Iluminar botón " + index);
    }

    void UnhighlightButton(int index)
    {
        Renderer buttonRenderer = GetButtonRenderer(index);

        buttons[index].GetComponent<MeshRenderer>().material.color = originalColor;

        buttonRenderer.material.color = originalColor;

        Debug.Log("Apagar botón " + index);
    }

    Renderer GetButtonRenderer(int index)
    {
        
        if (index >= 0 && index < buttons.Length)
        {
            
            Renderer buttonRenderer = buttons[index].GetComponent<Renderer>();
            if (buttonRenderer != null)
            {
                return buttonRenderer;
            }
            else
            {
                Debug.LogError("El botón en el índice " + index + " no tiene un componente Renderer.");
                return null;
            }
        }
        else
        {
            Debug.LogError("El índice está fuera del rango de botones.");
            return null;
        }
    }

    public void ResetGame()
    {
        sequence.Clear();
        clickIndex = 0;
        pActivate = false;
        GenerateNumber();
    }
}

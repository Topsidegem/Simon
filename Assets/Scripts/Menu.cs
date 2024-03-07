using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Menu : MonoBehaviour
{
    public GameObject menu;
    public GameObject menuPerdedor;

    private Randomizar randomizarScript;

    private bool isPaused = true;

    private void Awake()
    {
        Time.timeScale = 0;
    }
    
    void Start()
    {
        randomizarScript = GameObject.FindObjectOfType<Randomizar>();
        if (randomizarScript == null)
        {
            Debug.LogError("No se pudo encontrar el script Randomizar en la escena.");
        }
    }

    public void Pause()
    {
        
        isPaused = !isPaused;

        if (isPaused)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            menuPerdedor.SetActive(false);
        }
    }
    
    public void ReiniciarJuego()
    {
        if (randomizarScript != null)
        {
            randomizarScript.ResetGame();
            
        }
        else
        {
            Debug.Log("No se puede reiniciar el juego porque el script Randomizar no está asignado.");
        }

        menuPerdedor.SetActive(false);
        Time.timeScale = 1;
    }

    public void Perdiste()
    {
        //Time.timeScale = 0;
        menuPerdedor.SetActive(true);
    }



    public static void QuitGame()
    {
        EditorApplication.isPlaying = false;
    }
}

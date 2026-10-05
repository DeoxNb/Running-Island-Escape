using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuInicioUI : MonoBehaviour
{
    [Header("Configuración de UI")]
    public GameObject panelAjustes;
    public string nombreEscenaJuego = "SeleccionPersonaje";

    [Header("Configuración de Audio")]
    public AudioSource musicaMenu;
    public Slider sliderVolumen;

    void Start()
    {
    
        if (panelAjustes != null)
        {
            panelAjustes.SetActive(false);
        }


        if (musicaMenu != null && sliderVolumen != null)
        {
            sliderVolumen.value = musicaMenu.volume;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void AbrirAjustes()
    {
        if (panelAjustes != null)
        {
            panelAjustes.SetActive(true); 
        }
    }

    
    public void CerrarAjustes()
    {
        if (panelAjustes != null)
        {
            panelAjustes.SetActive(false);
        }
    }

    public void CambiarVolumen(float valor)
    {
        if (musicaMenu != null)
        {
            musicaMenu.volume = valor;
        }
    }
}
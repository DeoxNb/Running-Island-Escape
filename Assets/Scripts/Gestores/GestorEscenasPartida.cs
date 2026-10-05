using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorEscenasPartida : MonoBehaviour
{
    public static GestorEscenasPartida instancia;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

   
    public void FinalizarPartida(bool esVictoria)
    {
       
        Time.timeScale = 1f;

       
        if (esVictoria)
        {
            PlayerPrefs.SetString("EstadoFinal", "Victoria");
        }
        else
        {
            PlayerPrefs.SetString("EstadoFinal", "Derrota");
        }

      
        PlayerPrefs.Save();

        Debug.Log($"[SISTEMA] Cargando escena final. Resultado: " + (esVictoria ? "Victoria" : "Derrota"));

       
        SceneManager.LoadScene("PantallaFinal");
    }
}
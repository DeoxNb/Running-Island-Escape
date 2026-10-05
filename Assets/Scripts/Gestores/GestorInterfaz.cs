using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using RunningIsland.Datos;
using RunningIsland.Entidades;

public class GestorInterfaz : MonoBehaviour
{
    public static GestorInterfaz instancia;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoRutasActivas;
    public UnityEngine.UI.Image pantallaFeedback;

    [Header("Sistema de Vida")]
    public UnityEngine.UI.Slider barraVida;

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

        Time.timeScale = 1f;
    }

    public void ActualizarTextoHUD(string mensaje)
    {
        if (textoRutasActivas != null)
        {
            textoRutasActivas.text = mensaje;
        }
    }

    public void ActualizarVidas(int vidas)
    {
        if (barraVida != null)
        {
            barraVida.value = vidas;
        }
    }

    public void IrAlFinal(bool victoria)
    {
        PlayerPrefs.SetString(CamposPartida.ESTADO_FINAL, victoria ? "Victoria" : "Derrota");
        PlayerPrefs.Save();

        if (GestorDatos.instancia != null)
        {
            GestorDatos.instancia.RegistrarPartida(victoria ? "Victoria" : "Derrota");
        }

        StartCoroutine(SincronizarYSalir());
    }

    private IEnumerator SincronizarYSalir()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(CamposPartida.ESCENA_FINAL);
    }
}
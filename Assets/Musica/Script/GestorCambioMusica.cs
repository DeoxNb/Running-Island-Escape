using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorCambioMusica : MonoBehaviour
{
    public AudioClip musicaMenuYSeleccion;
    public AudioClip musicaJuego;
    public AudioClip musicaFinal;

    [Header("Ajustes de Velocidad y Volumen")]
    public float velocidadFade = 3.0f;
    [Range(0, 1)] public float volMenu = 0.8f;
    [Range(0, 1)] public float volJuego = 0.8f;
    [Range(0, 1)] public float volFinal = 0.8f;

    private void Start()
    {
        if (ControladorMusica.instancia != null)
        {
            AudioSource source = ControladorMusica.instancia.GetComponent<AudioSource>();
            string escena = SceneManager.GetActiveScene().name;

            AudioClip clipDestino = null;
            float volDestino = 1.0f;

            switch (escena)
            {
                case "Inicio":
                case "SeleccionPersonaje":
                    clipDestino = musicaMenuYSeleccion;
                    volDestino = volMenu;
                    break;
                case "Juego":
                    clipDestino = musicaJuego;
                    volDestino = volJuego;
                    break;
                case "PantallaFinal":
                    clipDestino = musicaFinal;
                    volDestino = volFinal;
                    break;
            }

            if (clipDestino != null && source.clip != clipDestino)
            {
                StopAllCoroutines();
                StartCoroutine(CambiarMusicaPro(source, clipDestino, volDestino));
            }
        }
    }

    private IEnumerator CambiarMusicaPro(AudioSource source, AudioClip nuevo, float volObjetivo)
    {
        while (source.volume > 0)
        {
            source.volume -= velocidadFade * Time.deltaTime;
            yield return null;
        }

        source.clip = nuevo;
        source.Play();

        while (source.volume < volObjetivo)
        {
            source.volume += velocidadFade * Time.deltaTime;
            yield return null;
        }

        source.volume = volObjetivo;
    }
}
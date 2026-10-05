using UnityEngine;
using RunningIsland.Datos;
using RunningIsland.UI;

namespace RunningIsland.Entidades
{
    public class SaludJugador : MonoBehaviour
    {
        private int _vidaMaxima = 100;
        private int _vidaActual;

        public int VidaActual => _vidaActual;
        public int VidaMaxima => _vidaMaxima;

        public void InicializarVida(int maximo)
        {
            _vidaMaxima = maximo;
            _vidaActual = _vidaMaxima;

            if (GestorHUD.instancia != null)
            {
                GestorHUD.instancia.ActualizarSaludVisual(_vidaActual, _vidaMaxima);
            }
        }

        public void RecibirDaño(int cantidad)
        {
            string personajeActual = PlayerPrefs.GetString(CamposPartida.PERSONAJE_SELECCIONADO, "AGENTE");
            float multiplicador = 1.0f;

           
            switch (personajeActual.ToUpper())
            {
                case "CIENTIFICA":
                    multiplicador = 0.75f; 
                    break;
                case "AGENTE":
                case "ARQUEOLOGO":
                case "PILOTO":
                default:
                    multiplicador = 1.0f; 
                    break;
            }

            int dañoFinal = Mathf.RoundToInt(cantidad * multiplicador);

            _vidaActual -= dañoFinal;
            _vidaActual = Mathf.Clamp(_vidaActual, 0, _vidaMaxima);

            if (GestorHUD.instancia != null)
            {
                GestorHUD.instancia.ActualizarSaludVisual(_vidaActual, _vidaMaxima);
                GestorHUD.instancia.EfectoRecibirDaño();
            }

            if (_vidaActual <= 0)
            {
                ProcesarDerrota();
            }
        }

        private void ProcesarDerrota()
        {
            PlayerPrefs.SetString(CamposPartida.ESTADO_FINAL, "Derrota");
            PlayerPrefs.SetString(CamposPartida.VIA_ESCAPE, CamposPartida.VIA_NINGUNA);
            PlayerPrefs.Save();

            if (GestorDatos.instancia != null)
            {
                GestorDatos.instancia.RegistrarPartida("Derrota");
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(CamposPartida.ESCENA_FINAL);
        }
    }
}
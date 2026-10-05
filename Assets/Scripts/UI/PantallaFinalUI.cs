using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using RunningIsland.Datos;

namespace RunningIsland.UI
{
    [DisallowMultipleComponent]
    public class PantallaFinalUI : MonoBehaviour
    {
        [Header("Arrastra aquí los textos de ContenedorFinal")]
        [SerializeField] private TextMeshProUGUI textoTitulo;
        [SerializeField] private TextMeshProUGUI textoPersonaje;
        [SerializeField] private TextMeshProUGUI textoViaEscape;

        private void Start()
        {
            Time.timeScale = 1f;
            ConfigurarInterfazResultado();
        }

        private void ConfigurarInterfazResultado()
        {
            int indicePartida = PlayerPrefs.GetInt("TotalPartidas", 1);
            string nombre = PlayerPrefs.GetString($"Partida_{indicePartida}_Nombre", "Jugador");
            float tiempo = PlayerPrefs.GetFloat($"Partida_{indicePartida}_Tiempo", 0f);
            string estado = PlayerPrefs.GetString(CamposPartida.ESTADO_FINAL, "Derrota");
            string via = PlayerPrefs.GetString(CamposPartida.VIA_ESCAPE, "Ninguna");
            string personaje = PlayerPrefs.GetString(CamposPartida.PERSONAJE_SELECCIONADO, "Agente");

            int minutos = Mathf.FloorToInt(tiempo / 60F);
            int segundos = Mathf.FloorToInt(tiempo % 60);
            string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

            if (textoPersonaje != null)
            {
                textoPersonaje.text = $"SUPERVIVIENTE: {nombre.ToUpper()}\nPERSONAJE: {personaje.ToUpper()}";
            }

            string descripcionNarrativa = "";

            if (estado == "Victoria")
            {
                if (textoTitulo != null) textoTitulo.text = "¡ESCAPE EXITOSO!";
                descripcionNarrativa = via == "Barca"
                    ? "Reparaste la barca con madera, tela y cuerdas, y huiste remando antes de la explosión volcánica."
                    : "Rescate aéreo coordinado en el helipuerto.";
            }
            else if (estado == "VictoriaVerdadera")
            {
                if (textoTitulo != null) textoTitulo.text = "BUCLE ROTO";
                descripcionNarrativa = "Unificaste las estatuas y detuviste el tiempo.";
            }
            else if (estado == "SecretoRevelado")
            {
                if (textoTitulo != null) textoTitulo.text = "SECRETO DESVELADO";
                descripcionNarrativa = "Sobreviviste en el búnker. Continuará en la parte II.";
            }
            else
            {
                if (textoTitulo != null) textoTitulo.text = "HAS MUERTO";
                descripcionNarrativa = "Sucumbiste a la lava del volcán anómalo.";
            }

            if (textoViaEscape != null)
            {
                textoViaEscape.text = $"{descripcionNarrativa}\n\nTiempo total: {tiempoFormateado}";
            }
        }

     

        public void JugarDeNuevo()
        {
            SceneManager.LoadScene("Juego");
        }

        public void VerLeaderboard()
        {
            SceneManager.LoadScene("Leaderboard");
        }

        public void SalirAlMenu()
        {
            SceneManager.LoadScene(CamposPartida.ESCENA_INICIO);
        }

     
        public void ExportarDatosCSV()
        {
            
            if (GestorDatos.instancia != null)
            {
                GestorDatos.instancia.EjecutarExportacion();
            }
        }
    }
}
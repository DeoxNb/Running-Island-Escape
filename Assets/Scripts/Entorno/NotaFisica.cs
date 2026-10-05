using UnityEngine;
using RunningIsland.Datos;
using RunningIsland.UI;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class NotaFisica : MonoBehaviour
    {
        [SerializeField] private NotaSuperviviente datosNota;

        private bool _jugadorEnRango = false;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (_jugadorEnRango && Input.GetKeyDown(KeyCode.E))
            {
                LeerEIntegrarNota();
            }
        }

        private void LeerEIntegrarNota()
        {
            if (datosNota == null) return;

            ControladorManualUI manual = GameObject.FindFirstObjectByType<ControladorManualUI>();
            if (manual != null)
            {
                manual.CargarYMostrarNota(datosNota);
            }

            string claveNota = $"Nota_Descubierta_{datosNota.IdNota}";
            PlayerPrefs.SetInt(claveNota, 1);
            PlayerPrefs.Save();

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _jugadorEnRango = true;
                if (GestorInterfaz.instancia != null)
                {
                    GestorInterfaz.instancia.ActualizarTextoHUD("Presiona [E] para leer nota");
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _jugadorEnRango = false;
                if (GestorInterfaz.instancia != null)
                {
                    GestorInterfaz.instancia.ActualizarTextoHUD("");
                }
            }
        }
    }
}
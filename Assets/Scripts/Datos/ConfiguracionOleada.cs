using System.Collections.Generic;
using UnityEngine;

namespace RunningIsland.Datos
{
    [System.Serializable]
    public struct ElementoSpawnAmbiental
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float probabilidad;
    }

    [CreateAssetMenu(fileName = "NuevaOleadaFuego", menuName = "Running Island/Configuracion/Oleada Fuego")]
    public class ConfiguracionOleada : ScriptableObject
    {
        [Header("Tiempos de la Fase")]
        [SerializeField] private float duracionOleada = 45f;
        [SerializeField] private float intervaloSpawnInicial = 4f;
        [SerializeField] private float intervaloSpawnMinimo = 1f;
        [SerializeField] private float tasaAceleracionIntervalo = 0.15f;

        [Header("Elementos Disponibles en esta Oleada")]
        [SerializeField] private List<ElementoSpawnAmbiental> poolElementos = new List<ElementoSpawnAmbiental>();

        public float DuracionOleada => duracionOleada;
        public float IntervaloSpawnInicial => intervaloSpawnInicial;
        public float IntervaloSpawnMinimo => intervaloSpawnMinimo;
        public float TasaAceleracionIntervalo => tasaAceleracionIntervalo;
        public IReadOnlyList<ElementoSpawnAmbiental> PoolElementos => poolElementos;

        public GameObject ObtenerElementoAleatorio()
        {
            if (poolElementos == null || poolElementos.Count == 0) return null;

            float pesoTotal = 0f;
            foreach (var elemento in poolElementos) pesoTotal += elemento.probabilidad;

            float valorAleatorio = Random.Range(0f, pesoTotal);
            float pesoAcumulado = 0f;

            foreach (var elemento in poolElementos)
            {
                pesoAcumulado += elemento.probabilidad;
                if (valorAleatorio <= pesoAcumulado) return elemento.prefab;
            }

            return poolElementos[0].prefab;
        }
    }
}
using UnityEngine;
using RunningIsland.Datos;

namespace RunningIsland.Gestores
{
    [DisallowMultipleComponent]
    public class GestorOleadas : MonoBehaviour
    {
        [Header("Configuración de Fases")]
        [SerializeField] private ConfiguracionOleada configuracionFase1;
        [SerializeField] private ConfiguracionOleada configuracionFase2;

        [Header("Configuración de Spawn")]
        [SerializeField] private GameObject prefabFuego;
     
        [SerializeField] private PolygonCollider2D areaSpawn;

        [Header("Configuración de Expansión")]
        public float velocidadExpansion = 0.5f;
        public float tamanoMaximo = 3f;

        private float _cronometroFase = 0f;
        private float _cronometroSpawn = 0f;
        private float _intervaloSpawnActual = 0f;
        private int _faseActual = 1;
        private ConfiguracionOleada _configuracionActiva;

        private void Start()
        {
            _configuracionActiva = configuracionFase1;
            if (_configuracionActiva != null)
                _intervaloSpawnActual = _configuracionActiva.IntervaloSpawnInicial;
        }

        private void Update()
        {
            if (_configuracionActiva == null) return;

            _cronometroFase += Time.deltaTime;
            _cronometroSpawn += Time.deltaTime;

            if (_cronometroSpawn >= _intervaloSpawnActual)
            {
                _cronometroSpawn = 0f;
                GenerarLavaYExpandir();
                AcelerarFrecuenciaSpawn();
            }

            if (_faseActual == 1 && _cronometroFase >= _configuracionActiva.DuracionOleada)
                CambiarAFase2();
        }

        private void GenerarLavaYExpandir()
        {
            if (prefabFuego == null || areaSpawn == null) return;

            Bounds bounds = areaSpawn.bounds;
            Vector3 pos = Vector3.zero;
            bool posicionValida = false;
            int intentosSeguridad = 0;

       
            while (!posicionValida && intentosSeguridad < 15)
            {
                pos = new Vector3(
                    UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                    UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                    0f
                );

               
                if (areaSpawn.OverlapPoint(pos))
                {
                    posicionValida = true;
                }
                intentosSeguridad++;
            }

            GameObject lava = Instantiate(prefabFuego, pos, Quaternion.identity);
            var expansion = lava.AddComponent<LavaExpansiva>();
            expansion.Configurar(velocidadExpansion, tamanoMaximo);
        }

        private void CambiarAFase2()
        {
            if (configuracionFase2 == null) return;
            _faseActual = 2;
            _cronometroFase = 0f;
            _configuracionActiva = configuracionFase2;
            _intervaloSpawnActual = _configuracionActiva.IntervaloSpawnInicial;
        }

        private void AcelerarFrecuenciaSpawn()
        {
            _intervaloSpawnActual = Mathf.Max(_configuracionActiva.IntervaloSpawnMinimo, _intervaloSpawnActual - _configuracionActiva.TasaAceleracionIntervalo);
        }

        public class LavaExpansiva : MonoBehaviour
        {
            private float _vel, _max;
            public void Configurar(float vel, float max) { _vel = vel; _max = max; }

            void Update()
            {
                if (transform.localScale.x < _max) transform.localScale += Vector3.one * _vel * Time.deltaTime;
            }

            private void OnTriggerEnter2D(Collider2D col)
            {
                var estado = col.GetComponent<EstadoObjeto>();
                if (estado != null) estado.MarcarComoQuemado();
            }
        }
    }
}
using UnityEngine;
using RunningIsland.Datos;

namespace RunningIsland.Entidades
{
    public class PoolFactoriaEntidades : MonoBehaviour
    {
        public static PoolFactoriaEntidades Instancia { get; private set; }

        [SerializeField] private GameObject prefabEnemigo;
        [SerializeField] private int tamañoPoolInicial = 10;

        private System.Collections.Generic.List<GameObject> _poolEnemigos = new System.Collections.Generic.List<GameObject>();

        private void Awake()
        {
            if (Instancia == null)
            {
                Instancia = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            LlenarPoolInicial();
        }

        private void LlenarPoolInicial()
        {
            for (int i = 0; i < tamañoPoolInicial; i++)
            {
                GameObject enemigo = Instantiate(prefabEnemigo, transform);
                enemigo.SetActive(false);
                _poolEnemigos.Add(enemigo);
            }
        }

        public GameObject ObtenerEnemigoDelPool(Vector3 posicion, Quaternion rotacion)
        {
            for (int i = 0; i < _poolEnemigos.Count; i++)
            {
                if (_poolEnemigos[i] != null && !_poolEnemigos[i].activeInHierarchy)
                {
                    GameObject enemigoExistente = _poolEnemigos[i];
                    enemigoExistente.transform.position = posicion;
                    enemigoExistente.transform.rotation = rotacion;
                    enemigoExistente.SetActive(true);
                    return enemigoExistente;
                }
            }

            GameObject nuevoEnemigo = Instantiate(prefabEnemigo, posicion, rotacion, transform);
            _poolEnemigos.Add(nuevoEnemigo);
            return nuevoEnemigo;
        }

        public void RecuperarEnemigoAlPool(GameObject enemigo)
        {
            if (enemigo == null) return;
            enemigo.SetActive(false);
        }
    }
}
using System.Collections;
using UnityEngine;

public class GeneradorIncendios : MonoBehaviour
{
    public static GeneradorIncendios instancia;

    [Header("Prefab del Fuego")]
    public GameObject prefabFocoFuego;

    [Header("Tiempos de Brotes")]
    public float tiempoPrimerBrote = 5f;
    public float intervaloBrotes = 40f;

    [Header("Límites del Mapa (Rango de Spawn)")]
    public float minX = -20f;
    public float maxX = 20f;
    public float minY = -20f;
    public float maxY = 20f;

    private void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(RutinaCicloIncendios());
    }

    private IEnumerator RutinaCicloIncendios()
    {
        yield return new WaitForSeconds(tiempoPrimerBrote);
        SpawnearFocoAleatorio();

        while (true)
        {
            yield return new WaitForSeconds(intervaloBrotes);
            SpawnearFocoAleatorio();
        }
    }

    private void SpawnearFocoAleatorio()
    {
        if (prefabFocoFuego == null)
        {
            Debug.LogError("[GENERADOR] No has asignado el prefab del foco de fuego en el Inspector.");
            return;
        }

        float posXAleatoria = Random.Range(minX, maxX);
        float posYAleatoria = Random.Range(minY, maxY);
        Vector3 posicionSpawn = new Vector3(posXAleatoria, posYAleatoria, 0f);

       
        Instantiate(prefabFocoFuego, posicionSpawn, Quaternion.identity);

        Debug.Log($"<color=orange>[VOLCÁN]</color> ¡Ha brotado un incendio en las coordenadas: {posicionSpawn}!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 centro = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 tamano = new Vector3(maxX - minX, maxY - minY, 1);
        Gizmos.DrawWireCube(centro, tamano);
    }
}
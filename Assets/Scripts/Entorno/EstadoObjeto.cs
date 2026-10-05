using UnityEngine;

public class EstadoObjeto : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool estaQuemado = false;

    public void MarcarComoQuemado()
    {
        if (estaQuemado) return;

        estaQuemado = true;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.black;

   
        MonoBehaviour item = GetComponent<MonoBehaviour>();
        if (item != null && item.GetType().Name == "ItemFisico")
        {
            item.enabled = false;
        }
    }
}
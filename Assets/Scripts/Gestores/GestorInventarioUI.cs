using UnityEngine;

namespace RunningIsland.UI
{
    public class InventarioUI : MonoBehaviour
    {
        [SerializeField] private GameObject panelInventario;
        [SerializeField] private KeyCode teclaInventario = KeyCode.I;

        private void Start()
        {
            if (panelInventario != null)
            {
                panelInventario.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(teclaInventario))
            {
                AlternarInventario();
            }
        }

        private void AlternarInventario()
        {
            if (panelInventario != null)
            {
                bool estadoActual = panelInventario.activeSelf;
                panelInventario.SetActive(!estadoActual);
            }
        }
    }
}
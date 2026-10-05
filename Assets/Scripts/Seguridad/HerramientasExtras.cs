using System;
using System.IO;
using UnityEngine;

public class HerramientasExtras
{
    public void ExportarHistorialLocal()
    {
       
        string rutaOrigen = Path.Combine(UnityEngine.Application.dataPath, "../", "datos_globales.csv");
        string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string rutaDestino = Path.Combine(rutaEscritorio, "Copia_Seguridad_IslaND.csv");

        if (File.Exists(rutaOrigen))
        {
            File.Copy(rutaOrigen, rutaDestino, true);
          
            UnityEngine.Debug.Log("Exportado_Al_Escritorio");
        }
    }
}
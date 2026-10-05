using System;
using System.Collections.Generic;

namespace RunningIsland.Datos
{
    [Serializable]
    public class DatosPartida
    {
        public string nombreJugador;
        public string horaInicio;
        public string personajeElegido;
        public List<string> itemsRecogidos;
        public List<string> notasDescubiertas;
        public string rutaEscape;
        public float tiempoJugado;
        public string desenlace;
    }
}
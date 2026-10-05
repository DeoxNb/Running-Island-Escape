namespace RunningIsland.Datos
{
    public enum TipoFinal { Derrota, Victoria, VictoriaVerdadera, SecretoRevelado }

    public static class CamposPartida
    {
        public const string ESTADO_FINAL = "EstadoFinal";
        public const string VIA_ESCAPE = "ViaEscapeUsada";
        public const string PERSONAJE_SELECCIONADO = "PersonajeSeleccionado";

        public const string VIA_PORTAL = "Portal del Bosque";
        public const string VIA_HELICOPTERO = "Helipuerto (Rescate)";
        public const string VIA_BARCA = "Barca de Madera";
        public const string VIA_BUNKER = "Búnker (Secreto Desvelado)";
        public const string VIA_NINGUNA = "Ninguna (Calcinado)";

        public const string ESCENA_INICIO = "Inicio";
        public const string ESCENA_SELECCION = "SeleccionPersonaje";
        public const string ESCENA_JUEGO = "Juego";
        public const string ESCENA_FINAL = "PantallaFinal";
    }
}
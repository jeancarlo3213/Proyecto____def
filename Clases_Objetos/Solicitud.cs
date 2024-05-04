namespace Proyecto____def.Clases_Objetos
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public int IdCliente { get; set; }
        public int IdOpcion { get; set; }
        public string DescripcionProblema { get; set; }
        public string Estado { get; set; } = "En espera";
        public int IdTecnico { get; set; } = 0;
        public string Calificacion { get; set; } = "Ninguna";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.Now;
        public string NombreCreador { get; set; }
        public string DescripcionDetallada { get; set; }
        public string Prioridad { get; set; }
        public DateTime? FechaResolucion { get; set; }
    }
}

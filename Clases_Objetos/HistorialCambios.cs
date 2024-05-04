namespace Proyecto____def.Clases_Objetos
{
    public class HistorialCambios
    {
        public int IdHistorial { get; set; }
        public int IdSolicitud { get; set; }
        public DateTime FechaHoraCambio { get; set; } = DateTime.Now;
        public string DescripcionCambio { get; set; }
        public string Comentarios { get; set; }
    }
}

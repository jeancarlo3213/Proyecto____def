namespace Proyecto____def.Estructura
{
    public class Nodo
    {
        public object Informacion { get; set; }
        public Nodo Referencia { get; set; }

        public Nodo()
        {
            Informacion = null;
            Referencia = null;
        }

        public Nodo(object informacion)
        {
            Informacion = informacion;
            Referencia = null;
        }

        public Nodo(object informacion, Nodo referencia)
        {
            Informacion = informacion;
            Referencia = referencia;
        }

        public override string ToString()
        {
            return $"{Informacion}";
        }
    }
}

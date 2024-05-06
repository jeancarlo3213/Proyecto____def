using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.Arm;

namespace Proyecto____def.Clases_Objetos
{
    public class SearchModel
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string DPI { get; set; }
        public int? NumeroOficina { get; set; }
        public string Puesto { get; set; }
        public bool IsEmpty()
        {
            return !Id.HasValue && string.IsNullOrEmpty(Nombre) &&
                   string.IsNullOrEmpty(DPI) && !NumeroOficina.HasValue &&
                   string.IsNullOrEmpty(Puesto);
        }
    }
 
}

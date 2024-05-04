using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Proyecto____def.Estructura;
using Proyecto____def.Clases_Objetos;

namespace Proyecto____def.Servicios
{
    public class SolicitudService
    {
        private readonly string _connectionString;

        public SolicitudService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener las opciones disponibles para crear solicitudes
        public async Task<ListaEnlazadaSimple> ObtenerOpcionesAsync()
        {
            ListaEnlazadaSimple opciones = new ListaEnlazadaSimple();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT IdOpcion, Descripcion FROM Opciones", connection);
                connection.Open();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var opcion = new Opcion
                        {
                            IdOpcion = reader.GetInt32(0),
                            Descripcion = reader.GetString(1)
                        };
                        opciones.AgregarAlFinal(opcion);
                    }
                }
            }
            return opciones;
        }
        public async Task AgregarNuevaOpcionAsync(Opcion nuevaOpcion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Opciones (Descripcion) VALUES (@Descripcion)", connection);
                cmd.Parameters.AddWithValue("@Descripcion", nuevaOpcion.Descripcion);

                connection.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Método para agregar una solicitud a la base de datos
        public async Task AgregarSolicitudAsync(Solicitud solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Solicitudes (IdCliente, IdOpcion, DescripcionProblema, Estado, IdTecnico, Calificacion, FechaCreacion, FechaUltimaActualizacion, NombreCreador, DescripcionDetallada, Prioridad, FechaResolucion) VALUES (@IdCliente, @IdOpcion, @DescripcionProblema, @Estado, @IdTecnico, @Calificacion, @FechaCreacion, @FechaUltimaActualizacion, @NombreCreador, @DescripcionDetallada, @Prioridad, @FechaResolucion)", connection);
                cmd.Parameters.AddWithValue("@IdCliente", solicitud.IdCliente);
                cmd.Parameters.AddWithValue("@IdOpcion", solicitud.IdOpcion);
                cmd.Parameters.AddWithValue("@DescripcionProblema", solicitud.DescripcionProblema);
                cmd.Parameters.AddWithValue("@Estado", solicitud.Estado);
                cmd.Parameters.AddWithValue("@IdTecnico", solicitud.IdTecnico);
                cmd.Parameters.AddWithValue("@Calificacion", solicitud.Calificacion);
                cmd.Parameters.AddWithValue("@FechaCreacion", solicitud.FechaCreacion);
                cmd.Parameters.AddWithValue("@FechaUltimaActualizacion", solicitud.FechaUltimaActualizacion);
                cmd.Parameters.AddWithValue("@NombreCreador", solicitud.NombreCreador);
                cmd.Parameters.AddWithValue("@DescripcionDetallada", solicitud.DescripcionDetallada);
                cmd.Parameters.AddWithValue("@Prioridad", solicitud.Prioridad);
                cmd.Parameters.AddWithValue("@FechaResolucion", solicitud.FechaResolucion ?? (object)DBNull.Value);
                connection.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Métodos adicionales para manejar el historial de cambios y otras operaciones pueden ser implementados aquí.
    }
}

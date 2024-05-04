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

        public async Task<Solicitud> ObtenerSolicitudPorId(int idSolicitud)
        {
            Solicitud solicitud = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT IdSolicitud, IdCliente, IdOpcion, DescripcionProblema, Estado, FechaCreacion, FechaUltimaActualizacion, NombreCreador, DescripcionDetallada, Prioridad FROM Solicitudes WHERE IdSolicitud = @IdSolicitud", connection);
                cmd.Parameters.AddWithValue("@IdSolicitud", idSolicitud);

                connection.Open();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        solicitud = new Solicitud
                        {
                            IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            IdOpcion = reader.GetInt32(reader.GetOrdinal("IdOpcion")),
                            DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            FechaUltimaActualizacion = reader.GetDateTime(reader.GetOrdinal("FechaUltimaActualizacion")),
                            NombreCreador = reader.GetString(reader.GetOrdinal("NombreCreador")),
                            DescripcionDetallada = reader.GetString(reader.GetOrdinal("DescripcionDetallada")),
                            Prioridad = reader.GetString(reader.GetOrdinal("Prioridad"))
                        };
                    }
                }
            }

            return solicitud;
        }

        public async Task<int> AgregarSolicitudAsync(Solicitud solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Solicitudes (IdCliente, IdOpcion, DescripcionProblema, Estado, FechaCreacion, FechaUltimaActualizacion, NombreCreador, DescripcionDetallada, Prioridad) VALUES (@IdCliente, @IdOpcion, @DescripcionProblema, @Estado, @FechaCreacion, @FechaUltimaActualizacion, @NombreCreador, @DescripcionDetallada, @Prioridad); SELECT SCOPE_IDENTITY();", connection);
                cmd.Parameters.AddWithValue("@IdCliente", solicitud.IdCliente);
                cmd.Parameters.AddWithValue("@IdOpcion", solicitud.IdOpcion);
                cmd.Parameters.AddWithValue("@DescripcionProblema", solicitud.DescripcionProblema);
                cmd.Parameters.AddWithValue("@Estado", solicitud.Estado);
                cmd.Parameters.AddWithValue("@FechaCreacion", solicitud.FechaCreacion);
                cmd.Parameters.AddWithValue("@FechaUltimaActualizacion", solicitud.FechaUltimaActualizacion);
                cmd.Parameters.AddWithValue("@NombreCreador", solicitud.NombreCreador);
                cmd.Parameters.AddWithValue("@DescripcionDetallada", solicitud.DescripcionDetallada);
                cmd.Parameters.AddWithValue("@Prioridad", solicitud.Prioridad);

                connection.Open();
                int idSolicitudGenerada = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                return idSolicitudGenerada;
            }
        }


        // Métodos adicionales para manejar el historial de cambios y otras operaciones pueden ser implementados aquí.
    }
}

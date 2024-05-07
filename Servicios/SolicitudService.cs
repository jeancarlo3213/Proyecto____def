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
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorUsuario(int idUsuario)
        {
            ListaEnlazadaSimple solicitudes = new ListaEnlazadaSimple();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Solicitudes WHERE IdCliente = @IdCliente ORDER BY FechaCreacion DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idUsuario);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            Solicitud solicitud = new Solicitud
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
                            solicitudes.AgregarAlFinal(solicitud);
                        }
                    }
                }
            }
            return solicitudes;
        }
        
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorTecnico(int idTecnico)
        {
            ListaEnlazadaSimple solicitudes = new ListaEnlazadaSimple();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Solicitudes WHERE IdTecnico = @IdTecnico ORDER BY FechaCreacion DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTecnico", idTecnico);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            Solicitud solicitud = new Solicitud
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
                                // Añade más campos según sea necesario.
                            };
                            solicitudes.AgregarAlFinal(solicitud);
                        }
                    }
                }
            }
            return solicitudes;
        }


        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorTecnicoYEstado(int idTecnico, string estado1, string estado2, string estado3)
        {
            ListaEnlazadaSimple solicitudes = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"
SELECT * FROM Solicitudes 
WHERE IdTecnico = @IdTecnico AND (Estado = @Estado1 OR Estado = @Estado2 OR Estado = @Estado3)
ORDER BY FechaCreacion DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTecnico", idTecnico);
                    command.Parameters.AddWithValue("@Estado1", estado1);
                    command.Parameters.AddWithValue("@Estado2", estado2);
                    command.Parameters.AddWithValue("@Estado3", estado3);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var solicitud = new Solicitud
                            {
                                IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                                DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                // Añadir más campos si son necesarios
                            };
                            solicitudes.AgregarAlFinal(solicitud);
                        }
                    }
                }
            }
            return solicitudes;
        }
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorTecnicoYEstado(int idTecnico, string estado)
        {
            ListaEnlazadaSimple solicitudes = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"
SELECT * FROM Solicitudes 
WHERE IdTecnico = @IdTecnico AND Estado = @Estado
ORDER BY FechaCreacion DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTecnico", idTecnico);
                    command.Parameters.AddWithValue("@Estado", estado);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var solicitud = new Solicitud
                            {
                                IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                                DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                // Añadir más campos si son necesarios
                            };
                            solicitudes.AgregarAlFinal(solicitud);
                        }
                    }
                }
            }
            return solicitudes;
        }


        public async Task ActualizarCalificacionSolicitud(Solicitud solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("UPDATE Solicitudes SET Calificacion = @Calificacion, Estado = @Estado WHERE IdSolicitud = @IdSolicitud", connection);
                cmd.Parameters.AddWithValue("@Calificacion", solicitud.Calificacion);
                cmd.Parameters.AddWithValue("@Estado", solicitud.Estado);
                cmd.Parameters.AddWithValue("@IdSolicitud", solicitud.IdSolicitud);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }


        public async Task<int> ObtenerTecnicoIdPorNombreUsuario(string nombreUsuario)
        {
            int tecnicoId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT Id FROM Usuarios WHERE Usuario = @Usuario";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    await connection.OpenAsync();
                    object result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        tecnicoId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el ID del técnico: {ex.Message}");
                // Manejar la excepción según sea necesario
            }
            return tecnicoId;
        }
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorTecnicoYPrioridad(int tecnicoId, string prioridad)
        {
            ListaEnlazadaSimple solicitudes = new ListaEnlazadaSimple();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Solicitudes WHERE IdTecnico = @TecnicoId AND Prioridad = @Prioridad";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TecnicoId", tecnicoId);
                command.Parameters.AddWithValue("@Prioridad", prioridad);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Solicitud solicitud = new Solicitud
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
                        Prioridad = reader.GetString(reader.GetOrdinal("Prioridad")),
                        FechaResolucion = reader.IsDBNull(reader.GetOrdinal("FechaResolucion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaResolucion"))
                    };

                    solicitudes.AgregarAlFinal(solicitud);
                }

                reader.Close();
            }

            return solicitudes;
        }
        public async Task ActualizarSolicitud(Solicitud solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("UPDATE Solicitudes SET Estado = @Estado, FechaUltimaActualizacion = @FechaUltimaActualizacion, FechaResolucion = @FechaResolucion WHERE IdSolicitud = @IdSolicitud", connection);
                cmd.Parameters.AddWithValue("@Estado", solicitud.Estado);
                cmd.Parameters.AddWithValue("@FechaUltimaActualizacion", solicitud.FechaUltimaActualizacion);
                cmd.Parameters.AddWithValue("@FechaResolucion", solicitud.FechaResolucion ?? (object)DBNull.Value); // Asegura que se maneje correctamente un DateTime nulo
                cmd.Parameters.AddWithValue("@IdSolicitud", solicitud.IdSolicitud);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

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
                var cmd = new SqlCommand("SELECT IdSolicitud, IdCliente, IdOpcion, DescripcionProblema, Estado, FechaCreacion, FechaUltimaActualizacion, NombreCreador, DescripcionDetallada, Prioridad, Calificacion FROM Solicitudes WHERE IdSolicitud = @IdSolicitud", connection);
                cmd.Parameters.AddWithValue("@IdSolicitud", idSolicitud);

                await connection.OpenAsync();
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
                            Prioridad = reader.GetString(reader.GetOrdinal("Prioridad")),
                            Calificacion = reader.IsDBNull(reader.GetOrdinal("Calificacion")) ? null : reader.GetString(reader.GetOrdinal("Calificacion"))
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

        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorEstado(string estado)
        {
            var solicitudes = new ListaEnlazadaSimple();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Solicitudes WHERE Estado = @Estado", connection);
                cmd.Parameters.AddWithValue("@Estado", estado);

                await connection.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var solicitud = new Solicitud
                        {
                            IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            IdOpcion = reader.GetInt32(reader.GetOrdinal("IdOpcion")),
                            DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado")),
                            // Otros campos según necesidad
                        };
                        solicitudes.AgregarAlFinal(solicitud);
                    }
                }
            }

            return solicitudes;
        }

        public async Task AsignarTecnicoASolicitud(int idSolicitud, int idTecnico)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("UPDATE Solicitudes SET IdTecnico = @IdTecnico, Estado = 'Asignado' WHERE IdSolicitud = @IdSolicitud", connection);
                cmd.Parameters.AddWithValue("@IdSolicitud", idSolicitud);
                cmd.Parameters.AddWithValue("@IdTecnico", idTecnico);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Métodos adicionales para manejar el historial de cambios y otras operaciones pueden ser implementados aquí.
    }
}

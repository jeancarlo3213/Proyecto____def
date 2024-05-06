using Microsoft.Extensions.Configuration;
using Proyecto____def.Clases_Objetos;
using Proyecto____def.Estructura;
using Proyecto____def.Jefe;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto____def.Servicios
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Usuario_Persona> ObtenerUsuarioPorId(int id)
        {
            Usuario_Persona usuario = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    usuario = new Usuario_Persona
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        UsuarioNombre = reader.GetString(reader.GetOrdinal("Usuario")),
                        Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        DPI = reader.GetString(reader.GetOrdinal("DPI")),
                        NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                    };
                }
                reader.Close();
            }
            return usuario;
        }
        
        public async Task<ListaEnlazadaSimple> ObtenerTodosLosUsuarios()
        {
            var listaUsuarios = new ListaEnlazadaSimple();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Usuarios", connection);

                await connection.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario_Persona
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            UsuarioNombre = reader.GetString(reader.GetOrdinal("Usuario")),
                            Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                            DPI = reader.GetString(reader.GetOrdinal("DPI")),
                            NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                        };
                        listaUsuarios.AgregarAlFinal(usuario);
                    }
                }
            }

            return listaUsuarios;
        }

        public async Task<bool> AgregarUsuarioConValidacion(Usuario_Persona user)
        {
            // Primero, verificar si ya existe un usuario con el mismo DPI y Puesto
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) FROM Usuarios WHERE DPI = @DPI AND Puesto = @Puesto";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DPI", user.DPI);
                command.Parameters.AddWithValue("@Puesto", user.Puesto);

                await connection.OpenAsync();
                int existingCount = (int)await command.ExecuteScalarAsync();
                if (existingCount > 0)
                {
                    return false;  // No se agrega el usuario porque ya existe uno con el mismo DPI y Puesto
                }
            }

            // Si no existe, agregar el usuario
            return await AgregarUsuarioValidado(user);
        }

        public async Task<ListaEnlazadaSimple> BuscarUsuarios(SearchModel searchModel)
        {
            var resultados = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var query = new StringBuilder("SELECT * FROM Usuarios WHERE 1=1");

                    if (searchModel.Id.HasValue)
                    {
                        query.Append(" AND Id = @Id");
                    }
                    if (!string.IsNullOrEmpty(searchModel.Nombre))
                    {
                        query.Append(" AND Nombre LIKE @Nombre");
                    }
                    if (!string.IsNullOrEmpty(searchModel.DPI))
                    {
                        query.Append(" AND DPI = @DPI");
                    }
                    if (searchModel.NumeroOficina.HasValue)
                    {
                        query.Append(" AND NumeroOficina = @NumeroOficina");
                    }
                    if (!string.IsNullOrEmpty(searchModel.Puesto))
                    {
                        query.Append(" AND Puesto = @Puesto");
                    }

                    using (var command = new SqlCommand(query.ToString(), connection))
                    {
                        if (searchModel.Id.HasValue)
                            command.Parameters.AddWithValue("@Id", searchModel.Id);
                        if (!string.IsNullOrEmpty(searchModel.Nombre))
                            command.Parameters.AddWithValue("@Nombre", $"%{searchModel.Nombre}%");
                        if (!string.IsNullOrEmpty(searchModel.DPI))
                            command.Parameters.AddWithValue("@DPI", searchModel.DPI);
                        if (searchModel.NumeroOficina.HasValue)
                            command.Parameters.AddWithValue("@NumeroOficina", searchModel.NumeroOficina);
                        if (!string.IsNullOrEmpty(searchModel.Puesto))
                            command.Parameters.AddWithValue("@Puesto", searchModel.Puesto);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuario_Persona
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                    UsuarioNombre = reader.GetString(reader.GetOrdinal("Usuario")),
                                    Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                                    DPI = reader.GetString(reader.GetOrdinal("DPI")),
                                    NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                                };
                                resultados.AgregarAlFinal(new Nodo(usuario));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during database operation: {ex.Message}");
                }
            }
            return resultados;
        }
        public async Task<ListaEnlazadaSimple> BuscarPorId(int id)
        {
            var resultados = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Usuarios WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            resultados.AgregarAlFinal(LeerUsuario(reader));
                        }
                    }
                }
            }
            return resultados;
        }
        public async Task<ListaEnlazadaSimple> BuscarPorNombre(string nombre)
        {
            var resultados = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Usuarios WHERE Nombre LIKE @Nombre";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", $"%{nombre}%");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            resultados.AgregarAlFinal(LeerUsuario(reader));
                        }
                    }
                }
            }
            return resultados;
        }

        public async Task<ListaEnlazadaSimple> BuscarPorDPI(string dpi)
        {
            var resultados = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Usuarios WHERE DPI = @DPI";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DPI", dpi);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            resultados.AgregarAlFinal(LeerUsuario(reader));
                        }
                    }
                }
            }
            return resultados;
        }
        public async Task<ListaEnlazadaSimple> BuscarPorNumeroOficina(int numeroOficina)
        {
            var resultados = new ListaEnlazadaSimple();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Usuarios WHERE NumeroOficina = @NumeroOficina";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeroOficina", numeroOficina);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            resultados.AgregarAlFinal(LeerUsuario(reader));
                        }
                    }
                }
            }
            return resultados;
        }
        private Usuario_Persona LeerUsuario(SqlDataReader reader)
        {
            return new Usuario_Persona
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                UsuarioNombre = reader.GetString(reader.GetOrdinal("Usuario")),
                Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                DPI = reader.GetString(reader.GetOrdinal("DPI")),
                NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
            };
        }

        public async Task<bool> AgregarUsuarioValidado(Usuario_Persona user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"
            INSERT INTO Usuarios (Nombre, Apellido, Usuario, Contraseña, Email, Puesto, DPI, NumeroOficina)
            VALUES (@Nombre, @Apellido, @Usuario, @Contraseña, @Email, @Puesto, @DPI, @NumeroOficina)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", user.Nombre);
                command.Parameters.AddWithValue("@Apellido", user.Apellido);
                command.Parameters.AddWithValue("@Usuario", user.UsuarioNombre);
                command.Parameters.AddWithValue("@Contraseña", user.Contraseña);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Puesto", user.Puesto);
                command.Parameters.AddWithValue("@DPI", user.DPI);
                command.Parameters.AddWithValue("@NumeroOficina", user.NumeroOficina);

                await connection.OpenAsync();
                int result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }


        public async Task<Usuario_Persona> ObtenerUsuarioPorNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return null;  // No se ejecuta la consulta si el nombre es nulo o vacío
            }

            Usuario_Persona usuario = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Usuario = @Usuario";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", nombreUsuario);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    usuario = new Usuario_Persona
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        UsuarioNombre = nombreUsuario, // Asegurarse de que este es el correcto
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        DPI = reader.GetString(reader.GetOrdinal("DPI")),
                        NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                    };
                }

                reader.Close();
            }

            return usuario;
        }

        public async Task<bool> ValidarDPI(string dpi)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) FROM Usuarios WHERE DPI = @DPI";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DPI", dpi);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }

        public async Task AgregarUsuario(Usuario_Persona user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO Usuarios (Nombre, Apellido, Usuario, Contraseña, Email, Puesto, DPI, NumeroOficina)
                    VALUES (@Nombre, @Apellido, @Usuario, @Contraseña, @Email, @Puesto, @DPI, @NumeroOficina)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", user.Nombre);
                command.Parameters.AddWithValue("@Apellido", user.Apellido);
                command.Parameters.AddWithValue("@Usuario", user.UsuarioNombre);
                command.Parameters.AddWithValue("@Contraseña", user.Contraseña);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Puesto", user.Puesto);
                command.Parameters.AddWithValue("@DPI", user.DPI);
                command.Parameters.AddWithValue("@NumeroOficina", user.NumeroOficina);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> ValidarCredencialesCliente(string usuario, string contraseña)
        {
            bool credencialesValidas = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña AND Puesto = 'Cliente'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contraseña", contraseña);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                credencialesValidas = count > 0;
            }

            return credencialesValidas;
        }
        public async Task<bool> ValidarCredencialesJefe(string usuario, string contraseña)
        {
            bool credencialesValidas = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Asegúrate de que la consulta sea específica para el puesto de 'Jefe'
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña AND Puesto = 'Jefe'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contraseña", contraseña);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                credencialesValidas = count > 0;
            }

            return credencialesValidas;
        }
        public async Task<ListaEnlazadaSimple> ObtenerTecnicos()
        {
            var tecnicos = new ListaEnlazadaSimple();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Puesto = 'Tecnico'", connection);

                await connection.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var tecnico = new Usuario_Persona
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            // Otros campos según necesidad
                        };
                        tecnicos.AgregarAlFinal(tecnico);
                    }
                }
            }

            return tecnicos;
        }

        public async Task<bool> ValidarCredencialesTecnico(string usuario, string contraseña)
        {
            bool credencialesValidas = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña AND Puesto = 'Tecnico'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contraseña", contraseña);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                credencialesValidas = count > 0;
            }

            return credencialesValidas;
        }
    }
}

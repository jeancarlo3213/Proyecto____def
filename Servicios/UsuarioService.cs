using Microsoft.Extensions.Configuration;
using Proyecto____def.Clases_Objetos;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Proyecto_def.Servicios
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

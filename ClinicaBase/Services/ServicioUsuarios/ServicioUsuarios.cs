using Azure.Core;
using ClinicaBase.Data;
using ClinicaBase.Models.DTOs;
using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using ClinicaBase.Services.ServicioHash;
using Microsoft.EntityFrameworkCore;

namespace ClinicaBase.Services.ServicioUsuarios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly ClinicaBase1Context _context;
        private readonly IServicioHash _servicioHash;

        private const string GeneralError = "Se ha generado un error inesperado";
        private const string UserOrPasswordNotFound = "El usuario o contraseña no coinciden";

        public ServicioUsuarios(ClinicaBase1Context context, IServicioHash servicioHash)
        {
            _context = context;
            _servicioHash = servicioHash;

        }


        public async Task<GeneralResponse> Auth(UsuarioAuthViewModel request)
        {
            GeneralResponse response = new();
            User? user = await _context.Users.Where(u => u.Documento == request.Documento).SingleOrDefaultAsync();
            if (user == null || user.Activo == 0)
            {
                response.Message = UserOrPasswordNotFound;
                return response;
            }

            bool usuarioAuthCorrecto = _servicioHash.VerifyHashPassword(request.Contrasena, user.Sal, user.Contrasena);
            if (!usuarioAuthCorrecto)
            {
                response.Message = UserOrPasswordNotFound;
                return response;
            }

            UserClaimsDTO userClaims = new()
            {
                Documento = user.Documento,
                Nombres = user.Nombres,
                Apellidos = user.Apellidos,
                Rol = user.Rol
            };

            response.Succeed = 1;
            response.Data = userClaims;
            response.Message = null;

            return response;
        }


        public async Task<GeneralResponse> AddUsuario(RegisterViewModel request)
        {
            GeneralResponse response = new();
            User? user = await FindById(request.Documento);
            if (user != null)
            {
                if (user.Activo == 1)
                {
                    response.Succeed = 0;
                    response.Message = "Este usuario ya se encuentra registrado.";
                    return response;
                }
                else
                {
                    response = await ReactivateUser(user, request);
                    return response;
                }
            }
            else
            {
                NewRegisterModel(request, out User? newRequestModel);
                if (newRequestModel == null)
                {
                    response.Succeed = 0;
                    response.Message = GeneralError;
                    return response;
                }

                _servicioHash.CreateHashPassword(newRequestModel.Contrasena, out string? hashPasswordSalt, out string? salt);
                if (hashPasswordSalt == null || salt == null)
                {
                    response.Succeed = 0;
                    response.Message = GeneralError;
                    return response;
                }
                newRequestModel.Contrasena = hashPasswordSalt;
                newRequestModel.Sal = salt;
                response = await CreateUser(newRequestModel);
                return response;
            }
        }
                     

        private void NewRegisterModel(RegisterViewModel request, out User? newRequestModel)
        {
            try
            {
                User usuario = new()
                {
                    Documento = request.Documento,
                    Nombres = request.Nombres,
                    Apellidos = request.Apellidos,
                    Correo = request.Correo,
                    Telefono = request.Telefono,
                    Contrasena = request.Documento.ToString(),
                    CambioContrasena = 0,
                    Sal = DateTime.Now.ToString(),
                    Rol = request.Rol,
                    Activo = 1
                };

                newRequestModel = usuario;
            }
            catch (Exception)
            {
                newRequestModel = null;
            }
        }


        private async Task<GeneralResponse> CreateUser(User newRequestModel)
        {
            GeneralResponse response = new();
            try
            {
                await _context.Users.AddAsync(newRequestModel);
                await _context.SaveChangesAsync();
                response.Succeed = 1;
                response.Message = "Usuario registrado exitosamente.";
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Se ha generado un error inesperado al momento de guardar la información de usuario.";
            }
            return response;
        }


        private async Task<GeneralResponse> ReactivateUser(User user, RegisterViewModel request)
        {
            GeneralResponse response = new();
            try
            {
                _servicioHash.CreateHashPassword(user.Documento.ToString(), out string? hashPasswordSalt, out string? salt);
                if (hashPasswordSalt == null || salt == null)
                {
                    response.Succeed = 0;
                    response.Message = GeneralError;
                    return response;
                }

                user.Contrasena = hashPasswordSalt;
                user.Sal = salt;
                user.Activo = 1;
                user.CambioContrasena = 0;

                user.Nombres = request.Nombres;
                user.Apellidos = request.Apellidos;
                user.Correo = request.Correo;
                user.Telefono = request.Telefono;
                user.Rol = request.Rol;

                _context.Users.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Succeed = 1;
                response.Message = "Usuario agregado con éxito.";
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Ha sucedido un error inesperado";
            }
            return response;
        }


        public async Task<GeneralResponse> FindAllUsers()
        {
            GeneralResponse response = new();
            try
            {
                List<UserDTO>? usuarios = new();
                List<User>? usuariosDB = await _context.Users.Where(u => u.Activo == 1).ToListAsync();
                if (usuariosDB.Count == 0)
                {
                    usuarios = null;
                }
                else
                {
                    foreach (var usuario in usuariosDB)
                    {
                        UserDTO userDTO = new()
                        {
                            Documento = usuario.Documento,
                            Nombres = usuario.Nombres,
                            Apellidos = usuario.Apellidos,
                            Correo = usuario.Correo,
                            Telefono = usuario.Telefono,
                            Rol = usuario.Rol
                        };
                        usuarios.Add(userDTO);
                    }
                }                

                response.Succeed = 1;
                response.Data = usuarios;
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Data = null;
            }
            return response;
        }

        public async Task<User?> FindById(int documento)
        {
            User? usuario = await _context.Users.Where(u => u.Documento == documento).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<GeneralResponse> DeleteUser(EliminarUsuarioViewModel usuarioEliminar, string rolUsuarioAutenticado)
        {
            GeneralResponse response = new();
            
            User? usuario = await FindById(usuarioEliminar.Documento);
            if (usuario != null)
            {
                if (rolUsuarioAutenticado != "Admin" && usuario.Rol == "Admin")
                {
                    response.Succeed = 0;
                    response.Message = "No tiene permisos para eliminar al Administrador";
                    return response;
                }

                usuario.Activo = 0;
                try
                {
                    _context.Users.Entry(usuario).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    response.Succeed = 1;
                    response.Message = "Usuario eliminado con éxito";
                }
                catch (Exception)
                {
                    response.Succeed = 0;
                    response.Message = "Ha ocurrido un error inesperado y no se ha podido eliminar al usuario";
                }                
            }
            else
            {
                response.Succeed = 0;
                response.Message = "No se ha podido encontrar el usuario";
            }
            return response;
        }


        public async Task<GeneralResponse> EditarUsuario(EditarUsuarioViewModel request, string rolUsuarioAutenticado)
        {
            GeneralResponse response = new();
            User? usuario = await FindById(request.Documento);
            if (usuario == null)
            {
                response.Succeed = 0;
                response.Message = "No se ha encontrado el usuario";
                return response;
            }

            if (rolUsuarioAutenticado != "Admin" && usuario.Rol == "Admin")
            {
                response.Succeed = 0;
                response.Message = "No tiene permisos para modificar a este usuario";
                return response;
            }

            try
            {
                usuario.Nombres = request.Nombres;
                usuario.Apellidos = request.Apellidos;
                usuario.Rol = request.Rol;

                _context.Users.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Succeed = 1;
                response.Message = "Usuario modificado exitosamente";
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Se ha generado un error inesperado mientras se intentaba guardar la información, por favor vuelva a intentarlo";
            }
            return response;
        }


        public async Task<GeneralResponse> EditarPerfil(PerfilViewModel perfilViewModel, int documentoUsuarioAutenticado)
        {
            GeneralResponse response = new();

            User? usuario = await FindById(documentoUsuarioAutenticado);
            if (usuario == null)
            {
                response.Succeed = 0;
                response.Message = "No se ha podido encontrar el usuario";
                return response;
            }

            usuario.Correo = perfilViewModel.Correo;
            usuario.Telefono = perfilViewModel.Telefono;

            try
            {
                _context.Users.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Succeed = 1;
                response.Message = "Datos almacenados correctamente";
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Se ha generado un error inesperado, por favor vuelva a intentarlo";
            }

            return response;
        }
    }
}

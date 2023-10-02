using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using APICORE.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace APICORE.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ProductoController(IConfiguration confi) {
            cadenaSQL = confi.GetConnectionString("CadenaSQL");
        
        }
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista() {
            List<Producto> lista = new();
            try {
                using (var conexion = new SqlConnection(cadenaSQL)) { 
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader()) {
                        while (rd.Read()) {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBArra = Convert.ToString(rd["CodigoBarra"]),
                                Nombre= Convert.ToString(rd["Nombre"]),
                                Marca= Convert.ToString(rd["Marca"]),
                                Categoria= Convert.ToString(rd["Categoria"]),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            }) ;
                        } 
                    }
                
                }
                return StatusCode(StatusCodes.Status200OK,
                                  new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { mensaje = ex.Message, response = lista });

            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto}")]
        public IActionResult Obtener(int idProducto)
        {
            List<Producto> lista = new();
            Producto prod  = new();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBArra = Convert.ToString(rd["CodigoBarra"]),
                                Nombre = Convert.ToString(rd["Nombre"]),
                                Marca = Convert.ToString(rd["Marca"]),
                                Categoria = Convert.ToString(rd["Categoria"]),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }
                    }

                }
                prod = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK,
                                  new { mensaje = "ok", response = prod });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { mensaje = ex.Message, response = prod });

            }
        }

        #region Guardar
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {                       
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_producto", conexion);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBArra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }   
                return StatusCode(StatusCodes.Status200OK,
                                  new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { mensaje = ex.Message});

            }
        }
        #endregion

        #region editar
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_producto", conexion);
                    
                    cmd.Parameters.AddWithValue("idProducto", objeto.IdProducto==0?DBNull.Value: objeto.IdProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBArra is null?DBNull.Value:objeto.CodigoBArra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
                return StatusCode(StatusCodes.Status200OK,
                                  new { mensaje = "editado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { mensaje = ex.Message });

            }
        }
        #endregion
        #region eliminar
        [HttpDelete]
        [Route("Eliminar")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conexion);

                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
                return StatusCode(StatusCodes.Status200OK,
                                  new { mensaje = "eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { mensaje = ex.Message });

            }
        }
        #endregion
    }
}

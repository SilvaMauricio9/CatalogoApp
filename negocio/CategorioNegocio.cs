using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.IO.Pipes;

namespace negocio
{
    public class CategorioNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria>lista = new List<Categoria>();
            AccesoDatos datos= new AccesoDatos();

            try
            {
                datos.setearConsulta("select Id as IdArticulo, Descripcion as Articulo from CATEGORIAS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = (int)datos.Lector["IdArticulo"];
                    aux.Descripcion = (string)datos.Lector["Articulo"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using actividadParcialQuilpa.dominio;
using actividadParcialQuilpa.datos;

namespace actividadParcialQuilpa.datos
{
    //3er paso: crear recetaDAO : acceso y definir sus métodos
    //(tamb. método singleton para que haya 1 instancia)
    internal class recetaDAO : accesoDB
    {
        private static recetaDAO instancia;
        public static recetaDAO crearInstancia()
        {
            if(instancia == null)
            {
                return instancia = new recetaDAO();
            }
            return instancia;
        }

        public DataTable listarIngredientes()
        {
            DataTable dt = new DataTable();
            comando.Parameters.Clear();
            conectar();
            comando.CommandText = "SP_CONSULTAR_INGREDIENTES";
            dt.Load(comando.ExecuteReader());
            desconectar();
            return dt;
        }
        public int proximaReceta()
        {
            int verificar = 0;
            SqlParameter p = new SqlParameter("@prox", SqlDbType.Int);

            try
            {
                comando.Parameters.Clear();
                conectar();
                comando.CommandText = "SP_PROXIMA_RECETA";
                p.Direction = ParameterDirection.Output;
                comando.Parameters.Add(p);
                comando.ExecuteNonQuery();
                comando.Parameters.Clear();

                try
                {
                    verificar = (int)p.Value;
                }
                catch
                {
                    return 1;
                }

                return (int)p.Value;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if(conexion.State == ConnectionState.Open)
                {
                    desconectar();
                }
            }
        }

        public bool ejecutarInsert(Receta r)
        {
            SqlTransaction t = null;
            bool ok = true;

            try
            {
                comando.Parameters.Clear();
                conectar();
                t = conexion.BeginTransaction();
                comando.Transaction = t;
                comando.CommandText = "SP_INSERTAR_RECETA";
                comando.Parameters.AddWithValue("@tipo_receta", r.TipoReceta);
                comando.Parameters.AddWithValue("@nombre", r.Nombre);
                if(r.Chef != null)
                {
                    comando.Parameters.AddWithValue("@cheff", r.Chef);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cheff", DBNull.Value);
                }
                comando.ExecuteNonQuery();
                comando.Parameters.Clear();
                int count = 1;

                foreach(DetalleReceta d in r.DetalleRecetas)
                {
                    comando.CommandText = "SP_INSERTAR_DETALLES";
                    comando.Parameters.AddWithValue("@id_receta", r.RecetaNro);
                    comando.Parameters.AddWithValue("@id_ingrediente", d.Ingrediente.IngredienteID);
                    comando.Parameters.AddWithValue("@cantidad", d.Cantidad);
                    count++;
                    comando.ExecuteNonQuery();
                    comando.Parameters.Clear();
                }

                t.Commit();
            }
            catch (Exception)
            {
                t.Rollback();
                ok = false;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    desconectar();
                }
            }

            return ok;
        }
    }
}

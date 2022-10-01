using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using actividadParcialQuilpa.dominio;

namespace actividadParcialQuilpa.datos
{
    //4to paso: definir gestor : interfaz iRecetaDAO y métodos public
    //dentro del código del form se llamará a los métodos de recetaDAO
    internal class gestorDB : iRecetaDAO
    {
        public int getProximaReceta()
        {
            return recetaDAO.crearInstancia().proximaReceta();
        }
        public DataTable getListarIngredientes()
        {
            return recetaDAO.crearInstancia().listarIngredientes();
        }
        public bool getEjecutarInsert(Receta r)
        {
            return recetaDAO.crearInstancia().ejecutarInsert(r);
        }

    }
}

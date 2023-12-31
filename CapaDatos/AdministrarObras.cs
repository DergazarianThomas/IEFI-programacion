﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
	public class AdministrarObras : DatosConexion
	{
        public int abmObras(string accion, Obra objObra)
		{
			int resultado = -1;  // para controlar que se realize la operacion con exito
			string orden = string.Empty; // para guardad consulta sql

			if (accion == "Alta")
			{
				orden = $"insert into Obras (NumeroObra, NombreObra, Direccion, FechaCreacion) values ({objObra.NumeroObra}, '{objObra.NombreObra}','{objObra.Direccion}', '{objObra.FechaCreacion}' );";
			}

			if (accion == "Modificar")

				orden = $"update Obras set NombreObra='{objObra.NombreObra}', Direccion='{objObra.Direccion}', FechaCreacion={objObra.FechaCreacion} WHERE IdObra Like '%{objObra.NumeroObra}%';";


			if (accion == "Borrar")
				orden = "delete * from Obras where IdObra =" + objObra.NumeroObra + ";";


			OleDbCommand cmd = new OleDbCommand(orden, conexion);
			try
			{
				Abrirconexion();
				resultado = cmd.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				throw new Exception($"Error de la accion {accion}", e);
			}
			finally
			{
				Cerrarconexion();
				cmd.Dispose();
			}
			return resultado;
		}

		public DataSet listadoObras(string cual)
		{
			string orden = string.Empty;
			if (cual != "Todos")
				orden = "select * from Obras where IdObras = " + int.Parse(cual) + ";";
			else
				orden = "select * from Obras;";
			OleDbCommand cmd = new OleDbCommand(orden, conexion);
			DataSet ds = new DataSet();
			OleDbDataAdapter da = new OleDbDataAdapter();
			try
			{
				Abrirconexion();
				cmd.ExecuteNonQuery();
				da.SelectCommand = cmd;
				da.Fill(ds);
			}
			catch (Exception e)
			{
				throw new Exception("Error al listar Obras", e);
			}
			finally
			{
				Cerrarconexion();
				cmd.Dispose();
			}
			return ds;
		}

        public List<Obra> ObtenerObras()
        {
            List<Obra> lista = new List<Obra>();
            string orden = "Select IdObra, nombreObra From Obras";
            OleDbCommand cmd = new OleDbCommand(orden, conexion);
            OleDbDataReader dr;
            try
            {
                Abrirconexion();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Obra O = new Obra();// instancio R como un objeto Rubro
                    O.IdObra = dr.GetInt32(0); //se puede acceder a los campos con el tipo de dato, en este caso un entero
                    O.NombreObra = dr.GetString(1);
                    lista.Add(O);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al listar Obras", e);
            }
            finally
            {
                Cerrarconexion();
                cmd.Dispose();
            }
            return lista;
        }

    }
}

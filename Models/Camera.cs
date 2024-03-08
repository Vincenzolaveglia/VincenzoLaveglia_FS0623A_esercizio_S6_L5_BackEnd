using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Gestionale_Albergo.Models
{
    public class Camera
    {
        public int IdCamera { get; set; }
        public int NumeroCamera { get; set; }
        public string Descrizione { get; set; }
        public string Tipo { get; set; }
        public string Stato { get; set; }

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        public void InserisciCamera()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                string query = "INSERT INTO Camere (NumeroCamera, Descrizione, Tipo, Stato) " +
                               "VALUES (@NumeroCamera, @Descrizione, @Tipo, @Stato)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NumeroCamera", NumeroCamera);
                    cmd.Parameters.AddWithValue("@Descrizione", Descrizione);
                    cmd.Parameters.AddWithValue("@Tipo", Tipo);
                    cmd.Parameters.AddWithValue("@Stato", Stato);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AggiornaCamera()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "UPDATE Camere SET NumeroCamera = @NumeroCamera, Descrizione = @Descrizione, " +
                               "Tipo = @Tipo, Stato = @Stato WHERE IdCamera = @IdCamera";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCamera", IdCamera);
                    cmd.Parameters.AddWithValue("@NumeroCamera", NumeroCamera);
                    cmd.Parameters.AddWithValue("@Descrizione", Descrizione);
                    cmd.Parameters.AddWithValue("@Tipo", Tipo);
                    cmd.Parameters.AddWithValue("@Stato", Stato);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminaCamera()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "DELETE FROM Camere WHERE IdCamera = @IdCamera";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCamera", IdCamera);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Camera GetCameraById(int IdCamera)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "SELECT * FROM Camere WHERE IdCamera = @IdCamera";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCamera", IdCamera);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Camera
                            {
                                IdCamera = Convert.ToInt32(reader["IdCamera"]),
                                NumeroCamera = Convert.ToInt32(reader["NumeroCamera"]),
                                Descrizione = reader["Descrizione"].ToString(),
                                Tipo = reader["Tipo"].ToString(),
                                Stato = reader["Stato"].ToString()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public List<SelectListItem> CamereDisponibili { get; set; }

        public static List<SelectListItem> GetCamereDisponibili()
        {
            List<SelectListItem> camereDisponibili = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "SELECT IdCamera, NumeroCamera, Tipo FROM Camere WHERE Stato = @Stato";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Stato", "Disponibile");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idCamera = Convert.ToInt32(reader["IdCamera"]);
                            int numeroCamera = Convert.ToInt32(reader["NumeroCamera"]);
                            string tipo = reader["Tipo"].ToString();

                            string text = $"{numeroCamera} - {tipo}";

                            SelectListItem item = new SelectListItem
                            {
                                Value = numeroCamera.ToString(),
                                Text = text
                            };

                            camereDisponibili.Add(item);
                        }
                    }
                }
            }

            return camereDisponibili;
        }
        public List<Camera> ListaCamera()
        {
            List<Camera> camere = new List<Camera>();

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                string query = "SELECT * FROM Camere";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Camera camera = new Camera
                            {
                                IdCamera = Convert.ToInt32(reader["IdCamera"]),
                                NumeroCamera = Convert.ToInt32(reader["NumeroCamera"]),
                                Descrizione = reader["Descrizione"].ToString(),
                                Tipo = reader["Tipo"].ToString(),
                                Stato = reader["Stato"].ToString()
                            };

                            camere.Add(camera);
                        }
                    }
                }
            }
            return camere;
        }
    }
}
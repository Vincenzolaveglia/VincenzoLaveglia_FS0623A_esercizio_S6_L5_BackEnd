using Gestionale_Albergo.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace Gestionale_Albergo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DB_ConnString"].ConnectionString;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Utente model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))

                    try
                    {
                        conn.Open();

                        string query = " SELECT * FROM Utenti WHERE Username = @Username AND Password = @Password ";

                        SqlCommand sqlCommand = new SqlCommand(query, conn);
                        sqlCommand.Parameters.AddWithValue("Username", model.Username);
                        sqlCommand.Parameters.AddWithValue("Password", model.Password);

                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        if (reader.HasRows)
                        {
                            FormsAuthentication.SetAuthCookie(model.Username, false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.AuthError = "Autenticazione non riuscita, credenziali non corrette";
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
            }
        
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Checkout(int IdPrenotazione)
        {
            Prenotazione prenotazione = Prenotazione.GetPrenotazioneById(IdPrenotazione);

            if (prenotazione == null)
            {
                return HttpNotFound();
            }

            decimal importoServiziAggiuntivi = ServizioAggiuntivo.GetTotaleServiziAggiuntivi(IdPrenotazione);

            decimal importoDaSaldare = prenotazione.TariffaApplicata - prenotazione.CaparraConfirmatoria + importoServiziAggiuntivi;

            List<ServizioAggiuntivo> serviziAggiuntivi = ServizioAggiuntivo.GetServiziAggiuntiviByIdPrenotazione(IdPrenotazione);

            Checkout checkoutModel = new Checkout
            {
                NomeTitolare = prenotazione.CodiceFiscale,
                NumeroStanza = prenotazione.NumeroCamera,
                DataCheckIn = prenotazione.DataInizioSoggiorno,
                DataCheckOut = prenotazione.DataFineSoggiorno,
                CaparraConfirmatoria = prenotazione.CaparraConfirmatoria,
                TariffaApplicata = prenotazione.TariffaApplicata,
                ServiziAggiuntivi = serviziAggiuntivi,
                ImportoDaSaldare = importoDaSaldare
            };

            return View(checkoutModel);
        }
    }
}
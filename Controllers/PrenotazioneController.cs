using Gestionale_Albergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestionale_Albergo.Controllers
{
    public class PrenotazioneController : Controller
    {
        public ActionResult CreaPrenotazione(string CodiceFiscale)
        {
            List<SelectListItem> camereDisponibili = Camera.GetCamereDisponibili();

            Prenotazione model = new Prenotazione();
            model.CodiceFiscale = CodiceFiscale;

            ViewBag.ListaCamere = camereDisponibili;

            return View(model);
        }

        [HttpPost]
        public ActionResult CreaPrenotazione(Prenotazione model)
        {
            if (ModelState.IsValid)
            {
                model.InserisciPrenotazione();
                return RedirectToAction("ListaPrenotazione");
            }

            return View(model);
        }

        public ActionResult DettagliPrenotazione(int IdPrenotazione)
        {
            Prenotazione prenotazione = Prenotazione.GetPrenotazioneById(IdPrenotazione);

            if (prenotazione != null)
            {
                return View(prenotazione);
            }

            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ListaPrenotazione()
        {
            var prenotazione = new Prenotazione().ListaPrenotazioni();
            return View(prenotazione);
        }

        public ActionResult EliminaPrenotazione(int IdPrenotazione)
        {
            Prenotazione prenotazione = Prenotazione.GetPrenotazioneById(IdPrenotazione);

            if (prenotazione != null)
            {
                prenotazione.EliminaPrenotazione(IdPrenotazione);
            }
            return RedirectToAction("ListaPrenotazione");
        }
    }
}
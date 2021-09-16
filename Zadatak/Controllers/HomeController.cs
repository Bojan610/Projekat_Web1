using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;

namespace Zadatak.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> sumnjiviKorisnici = (List<Korisnik>)HttpContext.Application["sumnjiviKorisnici"];
            List<Rezervacija> rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacije"];

            if (rezervacije == null)
            {
                rezervacije = new List<Rezervacija>();
                HttpContext.Application["rezervacije"] = rezervacije;
            }

            if (korisnici == null)
            {
                korisnici = new List<Korisnik>();
                HttpContext.Application["korisnici"] = korisnici;
            }

            if (sumnjiviKorisnici == null)
            {
                sumnjiviKorisnici = new List<Korisnik>();
                HttpContext.Application["sumnjiviKorisnici"] = sumnjiviKorisnici;
            }

            if (aranzmani == null)
            {
                aranzmani = new List<Aranzman>();
                HttpContext.Application["aranzmani"] = aranzmani;
            }

            if (prosliAranzmani == null)
            {
                prosliAranzmani = new List<Aranzman>();
                HttpContext.Application["prosliAranzmani"] = prosliAranzmani;
            }

            if (korisnik == null)
            {
                korisnik = new Korisnik();
                Session["korisnik"] = korisnik;
            }

            foreach (Aranzman item in aranzmani.ToList())
            {
                if (DateTime.Now.Date > item.DatumPocetka)
                {
                    prosliAranzmani.Add(item);
                    HttpContext.Application["prosliAranzmani"] = prosliAranzmani;
                    aranzmani.Remove(item);
                    HttpContext.Application["aranzmani"] = aranzmani;
                    //aranzmani.RemoveAt(aranzmani.IndexOf(item));
                }
            }

            foreach (Aranzman item in prosliAranzmani.ToList())
            {
                if (DateTime.Now.Date < item.DatumPocetka)
                {
                    prosliAranzmani.Remove(item);
                    HttpContext.Application["prosliAranzmani"] = prosliAranzmani;
                    aranzmani.Add(item);
                    HttpContext.Application["aranzmani"] = aranzmani;

                }
            }

            foreach (Korisnik item in korisnici)
            {
                if (item.BrojOtkaza >= 2 && !sumnjiviKorisnici.Contains(item) && !item.IsBlocked)
                {
                    sumnjiviKorisnici.Add(item);
                    HttpContext.Application["sumnjiviKorisnici"] = sumnjiviKorisnici;
                }
            }

            List<Aranzman> retAr = new List<Aranzman>();
            foreach (Aranzman item in aranzmani)
            {
                if (!item.IsDeleted)
                    retAr.Add(item);
            }

            return View(retAr);
        }

        [HttpPost]
        public ActionResult PregledajAranzman(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            if (aranzman == null)
            {
                List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
                aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));
            }

            return View(aranzman);
        }

        [HttpPost]
        public ActionResult PregledajSmestaj(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            if (aranzman == null)
            {
                List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
                aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));
            }

            return View(aranzman.Smestaj);
        }

        public ActionResult PregledajProsleAranzmane()
        {
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];

            List<Aranzman> retAr = new List<Aranzman>();
            foreach (Aranzman item in prosliAranzmani)
            {
                if (!item.IsDeleted)
                    retAr.Add(item);
            }

            return View(retAr);
        }
    }
}
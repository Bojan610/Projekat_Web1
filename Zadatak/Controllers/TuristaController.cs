using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;

namespace Zadatak.Controllers
{
    public class TuristaController : Controller
    {
        // GET: Turista
        public ActionResult Index()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];

            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            if (komentari == null)
            {
                komentari = new List<Komentar>();
                HttpContext.Application["komentari"] = komentari;
            }

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Korisnik = korisnik;

            foreach (Aranzman item in aranzmani.ToList())
            {
                if (DateTime.Now.Date > item.DatumPocetka)
                {
                    prosliAranzmani.Add(item);
                    HttpContext.Application["prosliAranzmani"] = prosliAranzmani;
                    aranzmani.Remove(item);
                    HttpContext.Application["aranzmani"] = aranzmani;
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

            return View(aranzman);
        }

        [HttpPost]
        public ActionResult PregledajSmestaj(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            return View(aranzman.Smestaj);
        }

        [HttpPost]
        public ActionResult RezervisiAranzman(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            return View(aranzman);
        }


        [HttpPost]
        public ActionResult RezervisiSmestaj(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            return View(aranzman);
        }

        [HttpPost]
        public ActionResult Rezervisi(string id, string naziv)
        {
            List<Rezervacija> rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacije"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            SmestajnaJedinica sj = aranzman.Smestaj.SlobodneSmestajneJedinice.Find(u => u.IdSmestajneJedinice.Equals(id));

            sj.IsDeleted = true;
            Rezervacija rez = new Rezervacija(aranzman.Naziv + sj.IdSmestajneJedinice, korisnik, "Aktivna", aranzman, sj);
            bool finded = false;
            foreach (Rezervacija item in rezervacije)
            {
                if (item.IdRezervacije == rez.IdRezervacije)
                {
                    rezervacije[rezervacije.IndexOf(item)] = rez;
                    finded = true;
                    break;
                }
            }

            if (!finded)
                rezervacije.Add(rez);

            HttpContext.Application["rezervacije"] = rezervacije;
            DataWrite.PisiRezervacije("~/App_Data/Rezervacije.txt", rezervacije);

            if (!korisnik.RezervisaniAranzmani.Contains(aranzman))
                korisnik.RezervisaniAranzmani.Add(aranzman);
            Session["korisnik"] = korisnik;

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            DataWrite.WriteUsers("~/App_Data/Korisnici.txt", korisnici);

            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
            smestajneJedinice = Data.CitajSmestajneJedinice("~/App_Data/SmestajneJedinice.txt");
            foreach (SmestajnaJedinica item in smestajneJedinice)
            {
                if (item.IdSmestajneJedinice == id)
                    item.IsDeleted = true;
            }

            DataWrite.PisiSmestajneJedinice("~/App_Data/SmestajneJedinice.txt", smestajneJedinice);

            return RedirectToAction("Index");
        }

        public ActionResult MojeRezervacije()
        {
            List<Rezervacija> rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacije"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            List<Rezervacija> aktivneBuduce = new List<Rezervacija>();
            List<Rezervacija> otkazaneBuduce = new List<Rezervacija>();
            foreach (Rezervacija item in rezervacije)
            {
                if (item.Turista.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    if (DateTime.Now.Date <= item.Aranzman.DatumPocetka)
                    {
                        if (item.Status == "Aktivna")
                            aktivneBuduce.Add(item);
                        else
                            otkazaneBuduce.Add(item);
                    }
                }
            }

            ViewBag.OtkazaneBuduce = otkazaneBuduce;
            return View(aktivneBuduce);
        }

        public ActionResult ProsleRezervacije()
        {
            List<Rezervacija> rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacije"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            List<Rezervacija> prosle = new List<Rezervacija>();
            foreach (Rezervacija item in rezervacije)
            {
                if (item.Turista.KorisnickoIme == korisnik.KorisnickoIme && item.Status == "Aktivna")
                {
                    if (DateTime.Now.Date > item.Aranzman.DatumPocetka)
                        prosle.Add(item);
                }
            }

            return View(prosle);
        }

        [HttpPost]
        public ActionResult OtkaziRezervaciju(string idRezervacije)
        {
            List<Korisnik> sumnjiviKorisnici = (List<Korisnik>)HttpContext.Application["sumnjiviKorisnici"];
            List<Rezervacija> rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacije"];
            Rezervacija rez = rezervacije.Find(u => u.IdRezervacije.Equals(idRezervacije));
          
            rez.SmestajnaJedinica.IsDeleted = false;
            rez.Status = "Otkazana";
            rez.Turista.BrojOtkaza++;

            if (rez.Turista.BrojOtkaza >= 2 && !sumnjiviKorisnici.Contains(rez.Turista))
            {
                sumnjiviKorisnici.Add(rez.Turista);
                HttpContext.Application["sumnjiviKorisnici"] = sumnjiviKorisnici;
            }

            HttpContext.Application["rezervacije"] = rezervacije;
            DataWrite.PisiRezervacije("~/App_Data/Rezervacije.txt", rezervacije);

            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
            smestajneJedinice = Data.CitajSmestajneJedinice("~/App_Data/SmestajneJedinice.txt");
            foreach (SmestajnaJedinica item in smestajneJedinice)
            {
                if (item.IdSmestajneJedinice == rez.SmestajnaJedinica.IdSmestajneJedinice)
                    item.IsDeleted = false;
            }

            DataWrite.PisiSmestajneJedinice("~/App_Data/SmestajneJedinice.txt", smestajneJedinice);

            return RedirectToAction("MojeRezervacije");
        }

        [HttpPost]
        public ActionResult DodajKomentar(string naziv)
        {
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            return View(aranzman);
        }

        [HttpPost]
        public ActionResult DodajKomentar1(int ocena, string komentarTekst, string naziv)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            Komentar komentar = new Komentar(korisnik, aranzman, komentarTekst, ocena);

            komentari.Add(komentar);
            HttpContext.Application["komentari"] = komentari;

            return RedirectToAction("MojeRezervacije");
        }
    }
}
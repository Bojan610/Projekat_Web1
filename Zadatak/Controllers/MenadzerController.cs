using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;
using Microsoft.Win32;
using System.IO;

namespace Zadatak.Controllers
{
    public class MenadzerController : Controller
    {
        // GET: Menadzer
        public ActionResult Index()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            ViewBag.Korisnik = korisnik;

            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            if (komentari == null)
            {
                komentari = new List<Komentar>();
                HttpContext.Application["komentari"] = komentari;
            }

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

        public ActionResult DodajAranzman()
        {
            Aranzman aranzman = (Aranzman)Session["aranzman"];

            if (aranzman == null)
            {
                aranzman = new Aranzman();
                Session["aranzman"] = aranzman;
            }

            Smestaj sm = new Smestaj();
            List<SmestajnaJedinica> sj = new List<SmestajnaJedinica>();
            List<SmestajnaJedinica> ssj = new List<SmestajnaJedinica>();
            List<Komentar> komentari = new List<Komentar>();
            sm.SmestajneJedinice = sj;
            sm.SlobodneSmestajneJedinice = ssj;
            aranzman.Smestaj = sm;
            aranzman.Komentari = komentari;

            Session["aranzman"] = aranzman;
            return View(aranzman);
        }

        [HttpPost]
        public ActionResult DodajAranzman(Aranzman ar, HttpPostedFileBase file)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            Aranzman aranzman = (Aranzman)Session["aranzman"];

            List<Aranzman> aran = new List<Aranzman>();
            aran = Data.CitajAranzmane("~/App_Data/Aranzmani.txt");
            foreach (Aranzman item in aran)
            {
                if (item.Naziv == ar.Naziv)
                {
                    ViewBag.Message = "Neuspesno dodavanje aranzmana. Aranzman sa unetim nazivom vec postoji.";
                    return View(aranzman);
                }
            }

            Korisnik korisnik = (Korisnik)Session["korisnik"];

            string path = "";

            if (file != null && file.ContentLength > 0)
                try
                {
                    path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            aranzman.Naziv = ar.Naziv;
            aranzman.TipAranzmana = ar.TipAranzmana;
            aranzman.TipPrevoza = ar.TipPrevoza;
            aranzman.LokacijaPutovanja = ar.LokacijaPutovanja;
            aranzman.DatumPocetka = ar.DatumPocetka;
            aranzman.DatumZavrsetka = ar.DatumZavrsetka;
            aranzman.VremePolaska = ar.VremePolaska;
            aranzman.MaxBrojPutnika = ar.MaxBrojPutnika;
            aranzman.ProgramPutovanja = ar.ProgramPutovanja;
            aranzman.OpisAranzmana = ar.OpisAranzmana;
            aranzman.Poster = path;
            aranzman.Dana = (uint)(ar.DatumZavrsetka.Date - ar.DatumPocetka.Date).Days;

            aranzmani.Add(aranzman);
            korisnik.KreiraniAranzmani.Add(aranzman);
            DataWrite.WriteUsers("~/App_Data/Korisnici.txt", korisnici);
            HttpContext.Application["aranzmani"] = aranzmani;

            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            List<Aranzman> sviAranzmani = new List<Aranzman>();
            foreach (Aranzman item in prosliAranzmani)
                sviAranzmani.Add(item);

            foreach (Aranzman item in aranzmani)
                sviAranzmani.Add(item);

            DataWrite.PisiAranzmane("~/App_Data/Aranzmani.txt", sviAranzmani);

            Session["aranzman"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult DodajSmestaj()
        {
            Aranzman aranzman = (Aranzman)Session["aranzman"];
            return View(aranzman.Smestaj);
        }

        [HttpPost]
        public ActionResult DodajSmestaj(Smestaj smestaj)
        {
            Aranzman aranzman = (Aranzman)Session["aranzman"];
            List<Smestaj> smestaji = new List<Smestaj>();
            smestaji = Data.CitajSmestaje("~/App_Data/Smestaji.txt");
            foreach (Smestaj item in smestaji)
            {
                if (item.Naziv == smestaj.Naziv)
                {
                    ViewBag.Message = "Neuspesno dodavanje smestaja. Smestaj sa unetim nazivom vec postoji.";
                    return View(aranzman.Smestaj);
                }
            }

            aranzman.Smestaj.TipSmestaja = smestaj.TipSmestaja;
            aranzman.Smestaj.Naziv = smestaj.Naziv;
            aranzman.Smestaj.BrojZvezdica = smestaj.BrojZvezdica;
            aranzman.Smestaj.ImaBazen = smestaj.ImaBazen;
            aranzman.Smestaj.ImaSpaCentar = smestaj.ImaSpaCentar;
            aranzman.Smestaj.ImaWiFi = smestaj.ImaWiFi;
            aranzman.Smestaj.OsobeSaInvaliditetom = smestaj.OsobeSaInvaliditetom;

            smestaji.Add(aranzman.Smestaj);
            DataWrite.PisiSmestaje("~/App_Data/Smestaji.txt", smestaji);

            Session["aranzman"] = aranzman;
            return View("DodajAranzman", aranzman);
        }

        [HttpPost]
        public ActionResult DodajSmestajnuJedinicu(SmestajnaJedinica smestajnaJedinica)
        {
            Aranzman aranzman = (Aranzman)Session["aranzman"];

            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
            smestajneJedinice = Data.CitajSmestajneJedinice("~/App_Data/SmestajneJedinice.txt");
            foreach (SmestajnaJedinica item in smestajneJedinice)
            {
                if (item.IdSmestajneJedinice == smestajnaJedinica.IdSmestajneJedinice)
                {
                    ViewBag.Message = "Neuspesno dodavanje. Smestajna jedinica sa unesenim id-om vec postoji.";
                    return View("DodajSmestaj", aranzman.Smestaj);
                }
            }
       
            smestajneJedinice.Add(smestajnaJedinica);
            DataWrite.PisiSmestajneJedinice("~/App_Data/SmestajneJedinice.txt", smestajneJedinice);

            aranzman.Smestaj.SmestajneJedinice.Add(smestajnaJedinica);
            aranzman.Smestaj.SlobodneSmestajneJedinice.Add(smestajnaJedinica);
            Session["aranzman"] = aranzman;

            return View("DodajSmestaj", aranzman.Smestaj);
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

        public ActionResult MojiAranzmani()
        {
            List<Rezervacija> rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacije"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            List<Aranzman> aranzmani = new List<Aranzman>();
            foreach (Aranzman item in korisnik.KreiraniAranzmani)
                aranzmani.Add(item);

            foreach (Aranzman item in aranzmani.ToList())
            {
                foreach (Rezervacija rezervacija in rezervacije)
                {
                    if (item.Naziv == rezervacija.Aranzman.Naziv && rezervacija.Status == "Aktivna")
                        aranzmani.Remove(item);
                }
            }

            ViewBag.Aranzmani = aranzmani;
            return View(korisnik);
        }

        [HttpPost]
        public ActionResult IzmeniAranzman(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            if (aranzman == null)
                aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            Session["aranzmanZaIzmenu"] = aranzman;

            return View(aranzman);
        }

        [HttpPost]
        public ActionResult SacuvajIzmene(Aranzman ar, HttpPostedFileBase file)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzmanStari = aranzmani.Find(u => u.Naziv.Equals(ar.Naziv));

            if (aranzmanStari == null)
                aranzmanStari = prosliAranzmani.Find(u => u.Naziv.Equals(ar.Naziv));

            Aranzman aranzmanZaIzmenu = (Aranzman)Session["aranzmanZaIzmenu"];

            Korisnik korisnik = (Korisnik)Session["korisnik"];

            string path = "";

            if (file != null && file.ContentLength > 0)
                try
                {
                    path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            aranzmanZaIzmenu.TipAranzmana = ar.TipAranzmana;
            aranzmanZaIzmenu.TipPrevoza = ar.TipPrevoza;
            aranzmanZaIzmenu.LokacijaPutovanja = ar.LokacijaPutovanja;
            aranzmanZaIzmenu.DatumPocetka = ar.DatumPocetka;
            aranzmanZaIzmenu.DatumZavrsetka = ar.DatumZavrsetka;
            aranzmanZaIzmenu.VremePolaska = ar.VremePolaska;
            aranzmanZaIzmenu.MaxBrojPutnika = ar.MaxBrojPutnika;
            aranzmanZaIzmenu.ProgramPutovanja = ar.ProgramPutovanja;
            aranzmanZaIzmenu.OpisAranzmana = ar.OpisAranzmana;
            aranzmanZaIzmenu.Poster = path;
            aranzmanZaIzmenu.Dana = (uint)(ar.DatumZavrsetka.Date - ar.DatumPocetka.Date).Days;

            if (aranzmani.Contains(aranzmanStari))
                aranzmani[aranzmani.IndexOf(aranzmanStari)] = aranzmanZaIzmenu;
            else
                prosliAranzmani[prosliAranzmani.IndexOf(aranzmanStari)] = aranzmanZaIzmenu;

            //korisnik.KreiraniAranzmani[korisnik.KreiraniAranzmani.IndexOf(aranzmanStari)] = aranzmanZaIzmenu;
            foreach (Aranzman item in korisnik.KreiraniAranzmani)
            {
                if (item.Naziv == aranzmanZaIzmenu.Naziv)
                {
                    item.Dana = aranzmanZaIzmenu.Dana;
                    item.DatumPocetka = aranzmanZaIzmenu.DatumPocetka;
                    item.DatumZavrsetka = aranzmanZaIzmenu.DatumZavrsetka;
                    item.Komentari = aranzmanZaIzmenu.Komentari;
                    item.LokacijaPutovanja = aranzmanZaIzmenu.LokacijaPutovanja;
                    item.MaxBrojPutnika = aranzmanZaIzmenu.MaxBrojPutnika;
                    item.MestoNalazenja = aranzmanZaIzmenu.MestoNalazenja;
                    item.OpisAranzmana = aranzmanZaIzmenu.OpisAranzmana;
                    item.Poster = aranzmanZaIzmenu.Poster;
                    item.ProgramPutovanja = aranzmanZaIzmenu.ProgramPutovanja;
                    item.Smestaj = aranzmanZaIzmenu.Smestaj;
                    item.TipAranzmana = aranzmanZaIzmenu.TipAranzmana;
                    item.TipPrevoza = item.TipPrevoza;
                    item.VremePolaska = aranzmanZaIzmenu.VremePolaska;
                }
            }

            List<Aranzman> sviAranzmani = new List<Aranzman>();
            foreach (Aranzman item in prosliAranzmani)
                sviAranzmani.Add(item);

            foreach (Aranzman item in aranzmani)
                sviAranzmani.Add(item);

            DataWrite.PisiAranzmane("~/App_Data/Aranzmani.txt", sviAranzmani);

            HttpContext.Application["aranzmani"] = aranzmani;
            HttpContext.Application["prosliAranzmani"] = prosliAranzmani;
            Session["aranzmanZaIzmenu"] = null;

            return RedirectToAction("MojiAranzmani", korisnik);
        }

        [HttpPost]
        public ActionResult IzmeniSmestaj(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            if (aranzman == null)
                aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            return View(aranzman);
        }

        [HttpPost]
        public ActionResult IzmeniSmestajnuJedinicu(SmestajnaJedinica smestajnaJedinica)
        {
            Aranzman aranzmanZaIzmenu = (Aranzman)Session["aranzmanZaIzmenu"];
            SmestajnaJedinica sj = aranzmanZaIzmenu.Smestaj.SmestajneJedinice.Find(u => u.IdSmestajneJedinice.Equals(smestajnaJedinica.IdSmestajneJedinice));

            aranzmanZaIzmenu.Smestaj.SmestajneJedinice[aranzmanZaIzmenu.Smestaj.SmestajneJedinice.IndexOf(sj)] = smestajnaJedinica;
            //aranzmanZaIzmenu.Smestaj.SlobodneSmestajneJedinice[aranzmanZaIzmenu.Smestaj.SlobodneSmestajneJedinice.IndexOf(sj)] = smestajnaJedinica;
            foreach (SmestajnaJedinica item in aranzmanZaIzmenu.Smestaj.SlobodneSmestajneJedinice)
            {
                if (item.IdSmestajneJedinice == sj.IdSmestajneJedinice)
                {
                    item.Cena = smestajnaJedinica.Cena;
                    item.KucniLjubimci = smestajnaJedinica.KucniLjubimci;
                    item.MaxBrojGostiju = smestajnaJedinica.MaxBrojGostiju;
                }
            }

            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
            smestajneJedinice = Data.CitajSmestajneJedinice("~/App_Data/SmestajneJedinice.txt");
            foreach (SmestajnaJedinica item in smestajneJedinice)
            {
                if (item.IdSmestajneJedinice == smestajnaJedinica.IdSmestajneJedinice)
                {
                    item.Cena = smestajnaJedinica.Cena;
                    item.KucniLjubimci = smestajnaJedinica.KucniLjubimci;
                    item.MaxBrojGostiju = smestajnaJedinica.MaxBrojGostiju;
                }
            }

            DataWrite.PisiSmestajneJedinice("~/App_Data/SmestajneJedinice.txt", smestajneJedinice);

            Session["aranzmanZaIzmenu"] = aranzmanZaIzmenu;
            return View("IzmeniSmestaj", aranzmanZaIzmenu);
        }

        [HttpPost]
        public ActionResult SacuvajIzmeneSmestaj(Smestaj smestaj)
        {
            Aranzman aranzmanZaIzmenu = (Aranzman)Session["aranzmanZaIzmenu"];
            aranzmanZaIzmenu.Smestaj.TipSmestaja = smestaj.TipSmestaja;
            aranzmanZaIzmenu.Smestaj.Naziv = smestaj.Naziv;
            aranzmanZaIzmenu.Smestaj.BrojZvezdica = smestaj.BrojZvezdica;
            aranzmanZaIzmenu.Smestaj.ImaBazen = smestaj.ImaBazen;
            aranzmanZaIzmenu.Smestaj.ImaSpaCentar = smestaj.ImaSpaCentar;
            aranzmanZaIzmenu.Smestaj.ImaWiFi = smestaj.ImaWiFi;
            aranzmanZaIzmenu.Smestaj.OsobeSaInvaliditetom = smestaj.OsobeSaInvaliditetom;

            List<Smestaj> smestaji = new List<Smestaj>();
            smestaji = Data.CitajSmestaje("~/App_Data/Smestaji.txt");
            foreach (Smestaj item in smestaji)
            {
                if (item.Naziv == smestaj.Naziv)
                {
                    item.TipSmestaja = smestaj.TipSmestaja;
                    item.Naziv = smestaj.Naziv;
                    item.BrojZvezdica = smestaj.BrojZvezdica;
                    item.ImaBazen = smestaj.ImaBazen;
                    item.ImaSpaCentar = smestaj.ImaSpaCentar;
                    item.ImaWiFi = smestaj.ImaWiFi;
                    item.OsobeSaInvaliditetom = smestaj.OsobeSaInvaliditetom;
                }
            }

            DataWrite.PisiSmestaje("~/App_Data/Smestaji.txt", smestaji);

            Session["aranzmanZaIzmenu"] = aranzmanZaIzmenu;
            return View("IzmeniAranzman", aranzmanZaIzmenu);
        }

        [HttpPost]
        public ActionResult ObrisiAranzman(string naziv)
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            if (aranzman == null)
                aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            Korisnik korisnik = (Korisnik)Session["korisnik"];

            //aranzmani[aranzmani.IndexOf(aranzman)].IsDeleted = true;
            foreach (Aranzman item in aranzmani)
            {
                if (item.Naziv == aranzman.Naziv)
                    item.IsDeleted = true;
            }
            //prosliAranzmani[prosliAranzmani.IndexOf(aranzman)].IsDeleted = true;
            foreach (Aranzman item in prosliAranzmani)
            {
                if (item.Naziv == aranzman.Naziv)
                    item.IsDeleted = true;
            }
            //korisnik.KreiraniAranzmani[korisnik.KreiraniAranzmani.IndexOf(aranzman)].IsDeleted = true;
            foreach (Aranzman item in korisnik.KreiraniAranzmani)
            {
                if (item.Naziv == aranzman.Naziv)
                    item.IsDeleted = true;
            }

            List<Aranzman> sviAranzmani = new List<Aranzman>();
            foreach (Aranzman item in prosliAranzmani)
                sviAranzmani.Add(item);

            foreach (Aranzman item in aranzmani)
                sviAranzmani.Add(item);

            DataWrite.PisiAranzmane("~/App_Data/Aranzmani.txt", sviAranzmani);

            return RedirectToAction("MojiAranzmani");
        }

        [HttpPost]
        public ActionResult ObrisiSmestajnuJedinicu(string id, string naziv)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Application["aranzmani"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = aranzmani.Find(u => u.Naziv.Equals(naziv));

            if (aranzman == null)
                aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            int count = 0;
            foreach (SmestajnaJedinica item in aranzman.Smestaj.SlobodneSmestajneJedinice)
            {
                if (!item.IsDeleted)
                    count++;
            }

            if (count > 1)
            {
                SmestajnaJedinica sj = aranzman.Smestaj.SmestajneJedinice.Find(u => u.IdSmestajneJedinice.Equals(id));
                sj.IsDeleted = true;

                SmestajnaJedinica sjj = aranzman.Smestaj.SlobodneSmestajneJedinice.Find(u => u.IdSmestajneJedinice.Equals(id));
                sjj.IsDeleted = true;

                /*if (aranzmani.Contains(aranzman))
                {
                    aranzmani[aranzmani.IndexOf(aranzman)].Smestaj.SmestajneJedinice.Remove(sj);
                    aranzmani[aranzmani.IndexOf(aranzman)].Smestaj.SlobodneSmestajneJedinice.Remove(sj);*/
                HttpContext.Application["aranzmani"] = aranzmani;

                List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
                smestajneJedinice = Data.CitajSmestajneJedinice("~/App_Data/SmestajneJedinice.txt");
                foreach (SmestajnaJedinica item in smestajneJedinice)
                {
                    if (item.IdSmestajneJedinice == id)
                        item.IsDeleted = true;
                }

                DataWrite.PisiSmestajneJedinice("~/App_Data/SmestajneJedinice.txt", smestajneJedinice);
                /*}
                else
                {
                    prosliAranzmani[prosliAranzmani.IndexOf(aranzman)].Smestaj.SmestajneJedinice.Remove(sj);
                    prosliAranzmani[prosliAranzmani.IndexOf(aranzman)].Smestaj.SlobodneSmestajneJedinice.Remove(sj);
                    HttpContext.Application["prosliAranzmani"] = prosliAranzmani;
                }*/
            }
            else
            {
                ViewBag.Brisanje = "Neuspesno brisanje. Smestaj mora posedovati bar jednu smestajnu jedinicu!";
            }

            return View("IzmeniSmestaj", aranzman);
        }

        [HttpPost]
        public ActionResult PogledajKomentare(string naziv)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Aranzman> prosliAranzmani = (List<Aranzman>)HttpContext.Application["prosliAranzmani"];
            Aranzman aranzman = prosliAranzmani.Find(u => u.Naziv.Equals(naziv));

            List<Komentar> kom = new List<Komentar>();

            if (aranzman == null)
                return View(kom);

            /*foreach (Komentar item in komentari)
            {
                if (korisnik.KreiraniAranzmani.Contains(item.Aranzman) && item.Aranzman.Naziv == aranzman.Naziv)
                    kom.Add(item);
            }*/

            foreach (Aranzman item in korisnik.KreiraniAranzmani)
            {
                foreach (Komentar ko in komentari)
                {
                    if (item.Naziv == ko.Aranzman.Naziv && ko.Aranzman.Naziv == aranzman.Naziv)
                        kom.Add(ko);
                }
            }

            return View(kom);
        }

        [HttpPost]
        public ActionResult OdobriKomentar(string komentarTekst)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            Komentar komentar = komentari.Find(u => u.KomentarTekst.Equals(komentarTekst));

            komentar.Aranzman.Komentari.Add(komentar);
            komentari.Remove(komentar);
            HttpContext.Application["komentari"] = komentari;

            List<Komentar> kom = new List<Komentar>();
            kom = Data.CitajKomentare("~/App_Data/Komentari.txt");
            kom.Add(komentar);
            DataWrite.PisiKomentare("~/App_Data/Komentari.txt", kom);

            return RedirectToAction("MojiAranzmani");
        }

        [HttpPost]
        public ActionResult OdbijKomentar(string komentarTekst)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            Komentar komentar = komentari.Find(u => u.KomentarTekst.Equals(komentarTekst));

            komentari.Remove(komentar);
            HttpContext.Application["komentari"] = komentari;

            return RedirectToAction("MojiAranzmani");
        }
    }
}
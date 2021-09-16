using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;

namespace Zadatak.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        public ActionResult Index()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> sumnjiviKorisnici = (List<Korisnik>)HttpContext.Application["sumnjiviKorisnici"];
            ViewBag.Korisnik = korisnik;
            ViewBag.Sumnjivi = sumnjiviKorisnici;
            return View(korisnici);
        }

        public ActionResult PregledajKorisnika(string korisnickoIme)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik kor = korisnici.Find(u => u.KorisnickoIme.Equals(korisnickoIme));

            return View(kor);
        }

        public ActionResult DodajMenadzera()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DodajMenadzera(Korisnik korisnikReg)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            foreach (Korisnik korisnik in korisnici)
            {
                if (korisnik.KorisnickoIme == korisnikReg.KorisnickoIme)
                {
                    ViewBag.Message = $"Korisnik {korisnikReg.KorisnickoIme} vec postoji!";
                    return View();
                }
            }

            korisnikReg.KreiraniAranzmani = new List<Aranzman>();
            korisnici.Add(korisnikReg);
            HttpContext.Application["korisnici"] = korisnici;

            DataWrite.WriteUsers("~/App_Data/Korisnici.txt", korisnici);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Pretraga(string ime, string prezime, string uloga)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Korisnik> korisniciRet = new List<Korisnik>();

            if (ime == "" || prezime == "" || uloga == "")
            {
                if ((ime != "") && (prezime != ""))
                {
                    foreach (Korisnik item in korisnici)
                    {
                        if (item.Ime.Contains(ime) && item.Prezime.Contains(prezime))
                            korisniciRet.Add(item);
                    }
                }
                else if (((ime != "") && (uloga != "")))
                {
                    foreach (Korisnik item in korisnici)
                    {
                        if (item.Ime.Contains(ime) && item.Uloga.Equals(uloga))
                            korisniciRet.Add(item);
                    }
                }
                else if (((prezime != "") && (uloga != "")))
                {
                    foreach (Korisnik item in korisnici)
                    {
                        if (item.Prezime.Contains(prezime) && item.Uloga.Equals(uloga))
                            korisniciRet.Add(item);
                    }
                }
                else
                {
                    if (ime != "")
                    {
                        foreach (Korisnik item in korisnici)
                        {
                            if (item.Ime.Contains(ime))
                                korisniciRet.Add(item);
                        }
                    }
                    else if (prezime != "")
                    {
                        foreach (Korisnik item in korisnici)
                        {
                            if (item.Prezime.Contains(prezime))
                                korisniciRet.Add(item);
                        }
                    }
                    else
                    {
                        foreach (Korisnik item in korisnici)
                        {
                            if (item.Uloga.Equals(uloga))
                                korisniciRet.Add(item);
                        }
                    }
                }
            }
            else
            {
                foreach (Korisnik item in korisnici)
                {
                    if (item.Ime.Contains(ime) && item.Prezime.Contains(prezime) && item.Uloga.Equals(uloga))
                        korisniciRet.Add(item);
                }
            }

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            List<Korisnik> sumnjivi = new List<Korisnik>();
            ViewBag.Korisnik = korisnik;
            ViewBag.Sumnjivi = sumnjivi;
            return View("Index", korisniciRet);
        }

        [HttpPost]
        public ActionResult BlokirajKorisnika(string naziv)
        {
            List<Korisnik> sumnjiviKorisnici = (List<Korisnik>)HttpContext.Application["sumnjiviKorisnici"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik kor = korisnici.Find(u => u.KorisnickoIme.Equals(naziv));

            kor.IsBlocked = true;
            sumnjiviKorisnici.Remove(kor);

            HttpContext.Application["korisnici"] = korisnici;
            DataWrite.WriteUsers("~/App_Data/Korisnici.txt", korisnici);
            HttpContext.Application["sumnjiviKorisnici"] = sumnjiviKorisnici;

            return RedirectToAction("Index");
        }
    }
}
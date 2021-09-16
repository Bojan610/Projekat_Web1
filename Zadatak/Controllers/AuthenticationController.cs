using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;

namespace Zadatak.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Korisnik korisnikReg)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            foreach (Korisnik korisnik in korisnici)
            {
                if (korisnik.KorisnickoIme == korisnikReg.KorisnickoIme)
                {
                    ViewBag.Message = $"Korisnik {korisnikReg.KorisnickoIme} vec postoji!";
                    return View("Index");
                }
            }

            korisnikReg.RezervisaniAranzmani = new List<Aranzman>();
            korisnikReg.BrojOtkaza = 0;
            korisnici.Add(korisnikReg);
            HttpContext.Application["korisnici"] = korisnici;
            DataWrite.WriteUsers("~/App_Data/Korisnici.txt", korisnici);
            return View("LogIn");
        }

        public ActionResult LogIn1()
        {
            return View("LogIn");
        }

        [HttpPost]
        public ActionResult LogIn(string korisnickoIme, string lozinka)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = korisnici.Find(u => u.KorisnickoIme.Equals(korisnickoIme) && u.Lozinka.Equals(lozinka));

            if (korisnik == null)
            {
                ViewBag.Message = "Korisnik sa unetim podacima ne postoji!";
                return View();
            }
            else if (korisnik.IsBlocked)
            {
                ViewBag.Message = "Korisnik sa unetim podacima je blokiran!";
                return View();
            }

            Session["korisnik"] = korisnik;

            if (korisnik.Uloga == "Administrator")
                return RedirectToAction("Index", "Administrator");
            else if (korisnik.Uloga == "Menadzer")
                return RedirectToAction("Index", "Menadzer");
            else
                return RedirectToAction("Index", "Turista");

        }

        public ActionResult LogOut()
        {
            Session["korisnik"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
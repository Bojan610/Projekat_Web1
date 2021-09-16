using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Zadatak.Models;

namespace Zadatak
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            List<Aranzman> aranzmani = Data.SpajanjeAranzmani();
            HttpContext.Current.Application["aranzmani"] = aranzmani;

            List<Korisnik> korisnici = Data.SpajanjeKorisnici(aranzmani);
            HttpContext.Current.Application["korisnici"] = korisnici;

            List<Rezervacija> rezervacije = Data.SpajanjeRezervacije(korisnici, aranzmani);
            HttpContext.Current.Application["rezervacije"] = rezervacije;
        }
    }
}

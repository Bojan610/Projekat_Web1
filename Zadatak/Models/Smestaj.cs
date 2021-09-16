using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class Smestaj
    {
        private string tipSmestaja;
        private string naziv;
        private uint brojZvezdica;
        private string imaBazen;
        private string imaSpaCentar;
        private string osobeSaInvaliditetom;
        private string imaWiFi;
        private List<SmestajnaJedinica> smestajneJedinice;
        private List<SmestajnaJedinica> slobodneSmestajneJedinice;
        private bool isDeleted;

        public Smestaj(string tipSmestaja, string naziv, uint brojZvezdica, string imaBazen, string imaSpaCentar,
            string osobeSaInvaliditetom, string imaWiFi)
        {
            this.tipSmestaja = tipSmestaja;
            this.naziv = naziv;
            this.brojZvezdica = brojZvezdica;
            this.imaBazen = imaBazen;
            this.imaSpaCentar = imaSpaCentar;
            this.osobeSaInvaliditetom = osobeSaInvaliditetom;
            this.imaWiFi = imaWiFi;
            this.smestajneJedinice = new List<SmestajnaJedinica>();
            this.slobodneSmestajneJedinice = new List<SmestajnaJedinica>();
            this.isDeleted = false;
        }

        public Smestaj() { }

        public string TipSmestaja { get => tipSmestaja; set => tipSmestaja = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public uint BrojZvezdica { get => brojZvezdica; set => brojZvezdica = value; }
        public string ImaBazen { get => imaBazen; set => imaBazen = value; }
        public string ImaSpaCentar { get => imaSpaCentar; set => imaSpaCentar = value; }
        public string OsobeSaInvaliditetom { get => osobeSaInvaliditetom; set => osobeSaInvaliditetom = value; }
        public string ImaWiFi { get => imaWiFi; set => imaWiFi = value; }
        public List<SmestajnaJedinica> SmestajneJedinice { get => smestajneJedinice; set => smestajneJedinice = value; }
        public List<SmestajnaJedinica> SlobodneSmestajneJedinice { get => slobodneSmestajneJedinice; set => slobodneSmestajneJedinice = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class SmestajnaJedinica
    {
        private uint maxBrojGostiju;
        private string kucniLjubimci;
        private double cena;
        private string idSmestajneJedinice;
        private bool isDeleted;

        public SmestajnaJedinica(uint maxBrojGostiju, string kucniLjubimci, double cena, string idSmestajneJedinice)
        {
            this.maxBrojGostiju = maxBrojGostiju;
            this.kucniLjubimci = kucniLjubimci;
            this.cena = cena;
            this.idSmestajneJedinice = idSmestajneJedinice;
            this.isDeleted = false;
        }

        public SmestajnaJedinica() { }

        public uint MaxBrojGostiju { get => maxBrojGostiju; set => maxBrojGostiju = value; }
        public string KucniLjubimci { get => kucniLjubimci; set => kucniLjubimci = value; }
        public double Cena { get => cena; set => cena = value; }
        public string IdSmestajneJedinice { get => idSmestajneJedinice; set => idSmestajneJedinice = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
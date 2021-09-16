using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class MestoNalazenja
    {
        private string adresa;
        private string geografskaDuzina;
        private string geografskaSirina;

        public MestoNalazenja(string adresa, string geografskaDuzina, string geografskaSirina)
        {
            this.adresa = adresa;
            this.geografskaDuzina = geografskaDuzina;
            this.geografskaSirina = geografskaSirina;
        }

        public MestoNalazenja() { }

        public string Adresa { get => adresa; set => adresa = value; }
        public string GeografskaDuzina { get => geografskaDuzina; set => geografskaDuzina = value; }
        public string GeografskaSirina { get => geografskaSirina; set => geografskaSirina = value; }
    }
}
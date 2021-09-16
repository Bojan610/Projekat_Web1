using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class Komentar
    {
        private Korisnik turista;
        private Aranzman aranzman;
        private string komentarTekst;
        private int ocena;

        public Komentar(Korisnik turista, Aranzman aranzman, string komentarTekst, int ocena)
        {
            this.turista = turista;
            this.aranzman = aranzman;
            this.komentarTekst = komentarTekst;
            this.ocena = ocena;
        }

        public Komentar()
        {
            this.turista = new Korisnik();
            this.aranzman = new Aranzman();
        }

        public Korisnik Turista { get => turista; set => turista = value; }
        public Aranzman Aranzman { get => aranzman; set => aranzman = value; }
        public string KomentarTekst { get => komentarTekst; set => komentarTekst = value; }
        public int Ocena { get => ocena; set => ocena = value; }
    }
}
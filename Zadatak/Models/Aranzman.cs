using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class Aranzman
    {
        private string naziv;
        private string tipAranzmana;
        private string tipPrevoza;
        private string lokacijaPutovanja;
        private DateTime datumPocetka;
        private DateTime datumZavrsetka;
        private uint dana;
        private MestoNalazenja mestoNalazenja;
        private DateTime vremePolaska;
        private uint maxBrojPutnika;
        private string opisAranzmana;
        private string programPutovanja;
        private string poster;
        private Smestaj smestaj;
        private List<Komentar> komentari;
        private bool isDeleted;

        public Aranzman(string naziv, string tipAranzmana, string tipPrevoza, string lokacijaPutovanja,
            DateTime datumPocetka, DateTime datumZavrsetka, DateTime vremePolaska,
            uint maxBrojPutnika, string opisAranzmana, string programPutovanja, string poster)
        {
            this.naziv = naziv;
            this.tipAranzmana = tipAranzmana;
            this.tipPrevoza = tipPrevoza;
            this.lokacijaPutovanja = lokacijaPutovanja;
            this.datumPocetka = datumPocetka;
            this.datumZavrsetka = datumZavrsetka;
            this.vremePolaska = vremePolaska;
            this.maxBrojPutnika = maxBrojPutnika;
            this.opisAranzmana = opisAranzmana;
            this.programPutovanja = programPutovanja;
            this.poster = poster;
            this.smestaj = new Smestaj();
            this.mestoNalazenja = new MestoNalazenja();
            this.komentari = new List<Komentar>();
            this.isDeleted = false;
        }

        public Aranzman() { }

        public string Naziv { get => naziv; set => naziv = value; }
        public string TipAranzmana { get => tipAranzmana; set => tipAranzmana = value; }
        public string TipPrevoza { get => tipPrevoza; set => tipPrevoza = value; }
        public string LokacijaPutovanja { get => lokacijaPutovanja; set => lokacijaPutovanja = value; }
        public DateTime DatumPocetka { get => datumPocetka; set => datumPocetka = value; }
        public DateTime DatumZavrsetka { get => datumZavrsetka; set => datumZavrsetka = value; }
        public uint Dana { get => dana; set => dana = value; }
        public MestoNalazenja MestoNalazenja { get => mestoNalazenja; set => mestoNalazenja = value; }
        public DateTime VremePolaska { get => vremePolaska; set => vremePolaska = value; }
        public uint MaxBrojPutnika { get => maxBrojPutnika; set => maxBrojPutnika = value; }
        public string OpisAranzmana { get => opisAranzmana; set => opisAranzmana = value; }
        public string ProgramPutovanja { get => programPutovanja; set => programPutovanja = value; }
        public string Poster { get => poster; set => poster = value; }
        public Smestaj Smestaj { get => smestaj; set => smestaj = value; }
        public List<Komentar> Komentari { get => komentari; set => komentari = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
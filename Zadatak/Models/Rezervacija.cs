using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class Rezervacija
    {
        private string idRezervacije;
        private Korisnik turista;
        private string status;
        private Aranzman aranzman;
        private SmestajnaJedinica smestajnaJedinica;

        public Rezervacija(string idRezervacije, Korisnik turista, string status, Aranzman aranzman,
            SmestajnaJedinica smestajnaJedinica)
        {
            this.idRezervacije = idRezervacije;
            this.turista = turista;
            this.status = status;
            this.aranzman = aranzman;
            this.smestajnaJedinica = smestajnaJedinica;
        }

        public Rezervacija()
        {
            this.turista = new Korisnik();
            this.aranzman = new Aranzman();
            this.smestajnaJedinica = new SmestajnaJedinica();
        }

        public string IdRezervacije { get => idRezervacije; set => idRezervacije = value; }
        public Korisnik Turista { get => turista; set => turista = value; }
        public string Status { get => status; set => status = value; }
        public Aranzman Aranzman { get => aranzman; set => aranzman = value; }
        public SmestajnaJedinica SmestajnaJedinica { get => smestajnaJedinica; set => smestajnaJedinica = value; }
    }
}
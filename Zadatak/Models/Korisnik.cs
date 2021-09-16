using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public class Korisnik
    {
        private string korisnickoIme;
        private string lozinka;
        private string ime;
        private string prezime;
        private string pol;
        private string email;
        private string datumRodjenja;
        private string uloga;
        private List<Aranzman> rezervisaniAranzmani;
        private List<Aranzman> kreiraniAranzmani;
        private int brojOtkaza;
        private bool isBlocked;

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email,
            string datumRodjenja, string uloga)
        {
            this.korisnickoIme = korisnickoIme;
            this.lozinka = lozinka;
            this.ime = ime;
            this.prezime = prezime;
            this.pol = pol;
            this.email = email;
            this.datumRodjenja = datumRodjenja;
            this.uloga = uloga;
            this.rezervisaniAranzmani = new List<Aranzman>();
            this.kreiraniAranzmani = new List<Aranzman>();
            this.brojOtkaza = 0;
            this.isBlocked = false;
        }

        public Korisnik()
        {
            korisnickoIme = "";
            lozinka = "";
            isBlocked = false;
        }

        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public string DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public string Uloga { get => uloga; set => uloga = value; }
        public List<Aranzman> RezervisaniAranzmani { get => rezervisaniAranzmani; set => rezervisaniAranzmani = value; }
        public List<Aranzman> KreiraniAranzmani { get => kreiraniAranzmani; set => kreiraniAranzmani = value; }
        public int BrojOtkaza { get => brojOtkaza; set => brojOtkaza = value; }
        public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Zadatak.Models
{
    public class Data
    {
        public static List<Korisnik> ReadUsers(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != "**")
            {
                string[] tokens = line.Split(';');
                Korisnik p = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7]);
                korisnici.Add(p);
            }

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Korisnik p = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7]);

                p.BrojOtkaza = Int32.Parse(tokens[8]);
                p.IsBlocked = bool.Parse(tokens[9]);
                List<Aranzman> kreirani = new List<Aranzman>();
                List<Aranzman> rezervisani = new List<Aranzman>();
                Aranzman aranzman;
                while ((line = sr.ReadLine()) != "**")
                {
                    aranzman = new Aranzman();
                    aranzman.Naziv = line.ToString();
                    kreirani.Add(aranzman);
                }

                while ((line = sr.ReadLine()) != "**")
                {
                    aranzman = new Aranzman();
                    aranzman.Naziv = line.ToString();
                    rezervisani.Add(aranzman);
                }

                p.KreiraniAranzmani = kreirani;
                p.RezervisaniAranzmani = rezervisani;

                korisnici.Add(p);
            }

            sr.Close();
            stream.Close();

            return korisnici;
        }

        public static List<Aranzman> CitajAranzmane(string path)
        {
            List<Aranzman> aranzmani = new List<Aranzman>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            line = sr.ReadLine();
            while ((line != null) && (line != ""))
            {
                string[] tokens = line.Split(';');
                Aranzman p = new Aranzman(tokens[0], tokens[1], tokens[2], tokens[3], DateTime.Parse(tokens[4]), DateTime.Parse(tokens[5]),
                    DateTime.Parse(tokens[6]), UInt32.Parse(tokens[7]), tokens[8], tokens[9], tokens[10]);

                p.Smestaj.Naziv = tokens[11];
                p.Dana = (uint)(DateTime.Parse(tokens[5]).Date - DateTime.Parse(tokens[4]).Date).Days;
                p.IsDeleted = bool.Parse(tokens[12]);

                aranzmani.Add(p);
                line = sr.ReadLine();
            }

            sr.Close();
            stream.Close();

            return aranzmani;
        }

        public static List<Smestaj> CitajSmestaje(string path)
        {
            List<Smestaj> smestaji = new List<Smestaj>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            line = sr.ReadLine();
            while ((line != null) && (line != ""))
            {
                string[] tokens = line.Split(';');
                Smestaj p = new Smestaj(tokens[0], tokens[1], UInt32.Parse(tokens[2]), tokens[3], tokens[4], tokens[5], tokens[6]);
                p.IsDeleted = bool.Parse(tokens[7]);

                List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
                List<SmestajnaJedinica> slobodneSmestajneJedinice = new List<SmestajnaJedinica>();
                SmestajnaJedinica sj;
                while ((line = sr.ReadLine()) != "**")
                {
                    sj = new SmestajnaJedinica();
                    sj.IdSmestajneJedinice = line.ToString();
                    smestajneJedinice.Add(sj);
                }

                while ((line = sr.ReadLine()) != "**")
                {
                    sj = new SmestajnaJedinica();
                    sj.IdSmestajneJedinice = line.ToString();
                    slobodneSmestajneJedinice.Add(sj);
                }

                p.SmestajneJedinice = smestajneJedinice;
                p.SlobodneSmestajneJedinice = slobodneSmestajneJedinice;

                smestaji.Add(p);
                line = sr.ReadLine();
            }

            sr.Close();
            stream.Close();

            return smestaji;
        }

        public static List<SmestajnaJedinica> CitajSmestajneJedinice(string path)
        {
            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            line = sr.ReadLine();
            while ((line != null) && (line != ""))
            {
                string[] tokens = line.Split(';');
                SmestajnaJedinica p = new SmestajnaJedinica(UInt32.Parse(tokens[0]), tokens[1], Double.Parse(tokens[2]), tokens[3]);
                p.IsDeleted = bool.Parse(tokens[4]);
                smestajneJedinice.Add(p);
                line = sr.ReadLine();
            }

            sr.Close();
            stream.Close();

            return smestajneJedinice;
        }

        public static List<Komentar> CitajKomentare(string path)
        {
            List<Komentar> komentari = new List<Komentar>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";


            line = sr.ReadLine();
            while ((line != null) && (line != ""))
            {
                string[] tokens = line.Split(';');
                Komentar p = new Komentar();
                p.Turista.KorisnickoIme = tokens[0];
                p.Aranzman.Naziv = tokens[1];
                p.KomentarTekst = tokens[2];
                p.Ocena = Int32.Parse(tokens[3]);
                komentari.Add(p);
                line = sr.ReadLine();
            }

            sr.Close();
            stream.Close();

            return komentari;
        }

        public static List<Rezervacija> CitajRezervacije(string path)
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            line = sr.ReadLine();
            while ((line != null) && (line != ""))
            {
                string[] tokens = line.Split(';');
                Rezervacija p = new Rezervacija();
                p.IdRezervacije = tokens[0];
                p.Turista.KorisnickoIme = tokens[1];
                p.Status = tokens[2];
                p.Aranzman.Naziv = tokens[3];
                p.SmestajnaJedinica.IdSmestajneJedinice = tokens[4];

                rezervacije.Add(p);
                line = sr.ReadLine();
            }

            sr.Close();
            stream.Close();

            return rezervacije;
        }

        public static List<Aranzman> SpajanjeAranzmani()
        {
            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();
            smestajneJedinice = CitajSmestajneJedinice("~/App_Data/SmestajneJedinice.txt");

            List<Smestaj> smestaji = new List<Smestaj>();
            smestaji = CitajSmestaje("~/App_Data/Smestaji.txt");

            foreach (Smestaj item in smestaji)
            {
                foreach (SmestajnaJedinica itemSj in item.SmestajneJedinice)
                {
                    foreach (SmestajnaJedinica sj in smestajneJedinice)
                    {
                        if (itemSj.IdSmestajneJedinice == sj.IdSmestajneJedinice)
                        {
                            itemSj.Cena = sj.Cena;
                            itemSj.KucniLjubimci = sj.KucniLjubimci;
                            itemSj.MaxBrojGostiju = sj.MaxBrojGostiju;
                            itemSj.IsDeleted = sj.IsDeleted;
                        }
                    }
                }

                foreach (SmestajnaJedinica itemSj in item.SlobodneSmestajneJedinice)
                {
                    foreach (SmestajnaJedinica sj in smestajneJedinice)
                    {
                        if (itemSj.IdSmestajneJedinice == sj.IdSmestajneJedinice)
                        {
                            itemSj.Cena = sj.Cena;
                            itemSj.KucniLjubimci = sj.KucniLjubimci;
                            itemSj.MaxBrojGostiju = sj.MaxBrojGostiju;
                            itemSj.IsDeleted = sj.IsDeleted;
                        }
                    }
                }
            }

            List<Aranzman> aranzmani = Data.CitajAranzmane("~/App_Data/Aranzmani.txt");

            foreach (Aranzman item in aranzmani)
            {
                foreach (Smestaj sm in smestaji)
                {
                    if (item.Smestaj.Naziv == sm.Naziv)
                    {
                        item.Smestaj.BrojZvezdica = sm.BrojZvezdica;
                        item.Smestaj.ImaBazen = sm.ImaBazen;
                        item.Smestaj.ImaSpaCentar = sm.ImaSpaCentar;
                        item.Smestaj.ImaWiFi = sm.ImaWiFi;
                        item.Smestaj.OsobeSaInvaliditetom = sm.OsobeSaInvaliditetom;
                        item.Smestaj.SmestajneJedinice = sm.SmestajneJedinice;
                        item.Smestaj.SlobodneSmestajneJedinice = sm.SlobodneSmestajneJedinice;
                        item.Smestaj.TipSmestaja = sm.TipSmestaja;
                        item.Smestaj.IsDeleted = sm.IsDeleted;
                    }
                }
            }

            return aranzmani;
        }

        public static List<Korisnik> SpajanjeKorisnici(List<Aranzman> aranzmani)
        {
            List<Korisnik> korisnici = Data.ReadUsers("~/App_Data/Korisnici.txt");

            foreach (Korisnik kor in korisnici)
            {
                if (kor.Uloga == "Menadzer")
                {
                    foreach (Aranzman item in kor.KreiraniAranzmani)
                    {
                        foreach (Aranzman ar in aranzmani)
                        {
                            if (item.Naziv == ar.Naziv)
                            {
                                item.Dana = ar.Dana;
                                item.DatumPocetka = ar.DatumPocetka;
                                item.DatumZavrsetka = ar.DatumZavrsetka;
                                item.Komentari = ar.Komentari;
                                item.LokacijaPutovanja = ar.LokacijaPutovanja;
                                item.MaxBrojPutnika = ar.MaxBrojPutnika;
                                item.MestoNalazenja = ar.MestoNalazenja;
                                item.OpisAranzmana = ar.OpisAranzmana;
                                item.Poster = ar.Poster;
                                item.ProgramPutovanja = ar.ProgramPutovanja;
                                item.Smestaj = ar.Smestaj;
                                item.TipAranzmana = ar.TipAranzmana;
                                item.TipPrevoza = ar.TipPrevoza;
                                item.VremePolaska = ar.VremePolaska;
                                item.IsDeleted = ar.IsDeleted;
                            }
                        }
                    }
                }
                else if (kor.Uloga == "Turista")
                {
                    foreach (Aranzman item in kor.RezervisaniAranzmani)
                    {
                        foreach (Aranzman ar in aranzmani)
                        {
                            if (item.Naziv == ar.Naziv)
                            {
                                item.Dana = ar.Dana;
                                item.DatumPocetka = ar.DatumPocetka;
                                item.DatumZavrsetka = ar.DatumZavrsetka;
                                item.Komentari = ar.Komentari;
                                item.LokacijaPutovanja = ar.LokacijaPutovanja;
                                item.MaxBrojPutnika = ar.MaxBrojPutnika;
                                item.MestoNalazenja = ar.MestoNalazenja;
                                item.OpisAranzmana = ar.OpisAranzmana;
                                item.Poster = ar.Poster;
                                item.ProgramPutovanja = ar.ProgramPutovanja;
                                item.Smestaj = ar.Smestaj;
                                item.TipAranzmana = ar.TipAranzmana;
                                item.TipPrevoza = ar.TipPrevoza;
                                item.VremePolaska = ar.VremePolaska;
                                item.IsDeleted = ar.IsDeleted;
                            }
                        }
                    }
                }
            }

            List<Komentar> komentari = Data.CitajKomentare("~/App_Data/Komentari.txt");

            foreach (Komentar kom in komentari)
            {
                foreach (Korisnik kor in korisnici)
                {
                    if (kor.KorisnickoIme == kom.Turista.KorisnickoIme)
                    {
                        kom.Turista = kor;
                        break;
                    }
                }
                foreach (Aranzman ar in aranzmani)
                {
                    if (ar.Naziv == kom.Aranzman.Naziv)
                    {
                        ar.Komentari.Add(kom);
                        kom.Aranzman = ar;
                        break;
                    }
                }
            }

            return korisnici;
        }

        public static List<Rezervacija> SpajanjeRezervacije(List<Korisnik> korisnici, List<Aranzman> aranzmani)
        {
            List<Rezervacija> rezervacije = Data.CitajRezervacije("~/App_Data/Rezervacije.txt");

            foreach (Rezervacija rez in rezervacije)
            {
                foreach (Korisnik kor in korisnici)
                {
                    if (kor.KorisnickoIme == rez.Turista.KorisnickoIme)
                    {
                        rez.Turista = kor;

                        foreach (Aranzman ar in kor.RezervisaniAranzmani)
                        {
                            if (ar.Naziv == rez.Aranzman.Naziv)
                            {
                                rez.Aranzman = ar;

                                foreach (SmestajnaJedinica sj in ar.Smestaj.SmestajneJedinice)
                                {
                                    if (sj.IdSmestajneJedinice == rez.SmestajnaJedinica.IdSmestajneJedinice)
                                    {
                                        rez.SmestajnaJedinica = sj;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return rezervacije;
        }

    }
}
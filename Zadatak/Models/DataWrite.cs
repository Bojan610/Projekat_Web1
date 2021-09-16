using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Zadatak.Models
{
    public class DataWrite
    {
        public static void WriteUsers(string path, List<Korisnik> korisnici)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);
            string text = "";

            foreach (Korisnik item in korisnici)
            {
                if (item.Uloga == "Administrator")
                {
                    text += item.KorisnickoIme + ";";
                    text += item.Lozinka + ";";
                    text += item.Ime + ";";
                    text += item.Prezime + ";";
                    text += item.Pol + ";";
                    text += item.Email + ";";
                    text += item.DatumRodjenja + ";";
                    text += item.Uloga + "\n";
                }
            }
            text += "**";

            foreach (Korisnik item in korisnici)
            {
                if (item.Uloga != "Administrator")
                {
                    text += "\n";
                    text += item.KorisnickoIme + ";";
                    text += item.Lozinka + ";";
                    text += item.Ime + ";";
                    text += item.Prezime + ";";
                    text += item.Pol + ";";
                    text += item.Email + ";";
                    text += item.DatumRodjenja + ";";
                    text += item.Uloga + ";";
                    text += item.BrojOtkaza + ";";
                    text += item.IsBlocked + "\n";
                    if (item.KreiraniAranzmani != null)
                    {
                        foreach (Aranzman ar in item.KreiraniAranzmani)
                        {
                            text += ar.Naziv + "\n";
                        }
                    }

                    text += "**" + "\n";

                    if (item.RezervisaniAranzmani != null)
                    {
                        foreach (Aranzman ar in item.RezervisaniAranzmani)
                        {
                            text += ar.Naziv + "\n";
                        }
                    }
                    text += "**";
                }
            }

            sw.Write(text);

            sw.Close();
            stream.Close();
        }

        public static void PisiAranzmane(string path, List<Aranzman> aranzmani)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);

            string text = "";

            foreach (Aranzman item in aranzmani)
            {
                text += item.Naziv + ";";
                text += item.TipAranzmana + ";";
                text += item.TipPrevoza + ";";
                text += item.LokacijaPutovanja + ";";
                text += item.DatumPocetka + ";";
                text += item.DatumZavrsetka + ";";
                text += item.VremePolaska + ";";
                text += item.MaxBrojPutnika + ";";
                text += item.OpisAranzmana + ";";
                text += item.ProgramPutovanja + ";";
                text += item.Poster + ";";
                text += item.Smestaj.Naziv + ";";
                text += item.IsDeleted + "\n";
            }

            sw.Write(text);

            sw.Close();
            stream.Close();
        }


        public static void PisiSmestaje(string path, List<Smestaj> smestaji)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);
            string text = "";

            foreach (Smestaj item in smestaji)
            {
                text += item.TipSmestaja + ";";
                text += item.Naziv + ";";
                text += item.BrojZvezdica + ";";
                text += item.ImaBazen + ";";
                text += item.ImaSpaCentar + ";";
                text += item.OsobeSaInvaliditetom + ";";
                text += item.ImaWiFi + ";";
                text += item.IsDeleted + "\n";

                foreach (SmestajnaJedinica sj in item.SmestajneJedinice)
                {
                    text += sj.IdSmestajneJedinice + "\n";
                }

                text += "**" + "\n";
                foreach (SmestajnaJedinica ssj in item.SlobodneSmestajneJedinice)
                {
                    text += ssj.IdSmestajneJedinice + "\n";
                }

                text += "**" + "\n";
            }

            sw.Write(text);

            sw.Close();
            stream.Close();
        }

        public static void PisiSmestajneJedinice(string path, List<SmestajnaJedinica> smestajneJedinice)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);
            string text = "";

            foreach (SmestajnaJedinica sj in smestajneJedinice)
            {
                text += sj.MaxBrojGostiju + ";";
                text += sj.KucniLjubimci + ";";
                text += sj.Cena + ";";
                text += sj.IdSmestajneJedinice + ";";
                text += sj.IsDeleted + "\n";
            }

            sw.Write(text);

            sw.Close();
            stream.Close();
        }

        public static void PisiRezervacije(string path, List<Rezervacija> rezervacije)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);
            string text = "";

            foreach (Rezervacija item in rezervacije)
            {
                text += item.IdRezervacije + ";";
                text += item.Turista.KorisnickoIme + ";";
                text += item.Status + ";";
                text += item.Aranzman.Naziv + ";";
                text += item.SmestajnaJedinica.IdSmestajneJedinice + "\n";
            }

            sw.Write(text);

            sw.Close();
            stream.Close();
        }

        public static void PisiKomentare(string path, List<Komentar> komentari)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);
            string text = "";

            foreach (Komentar item in komentari)
            {
                text += item.Turista.KorisnickoIme + ";";
                text += item.Aranzman.Naziv + ";";
                text += item.KomentarTekst + ";";
                text += item.Ocena + "\n";
            }

            sw.Write(text);

            sw.Close();
            stream.Close();
        }
    }
}
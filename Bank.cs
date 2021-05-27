using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProjekt
{
    class Bank
    {
        private class Szamla
        {
            private string nev;
            private string szamlaszam;
            private ulong egyenleg;
            public string Nev
            {
                get
                {
                    return nev;
                }
            }
            public string Szamlaszam
            {
                get
                {
                    return szamlaszam;
                }
            }
            public ulong Egyenleg
            {
                get
                {
                    return egyenleg;
                }
                set
                {
                    egyenleg = value;
                }
            }
            public Szamla(string nev, string szamlaszam)
            {
                this.nev = nev;
                this.szamlaszam = szamlaszam;
                egyenleg = 0;
            }
        }
        private List<Szamla> szamlak = new List<Szamla>();
        private Szamla SzamlaKeres(string szamlaszam)
        {
            foreach (Szamla szamla in szamlak)
            {
                if (szamla.Szamlaszam == szamlaszam)
                {
                    return szamla;
                }
            }
            throw new HibasSzamlaszamException(szamlaszam);
        }
        // Egy létező számlára pénzt helyez
        public void EgyenlegFeltolt(string szamlaszam, ulong osszeg)
        {
            SzamlaKeres(szamlaszam).Egyenleg += osszeg;
        }

        // Új számlát nyit a megadott névvel, számlaszámmal
        public void UjSzamla(string nev, string szamlaszam)
        {
            if (szamlak.Count > 0)
            {
                foreach (Szamla szamla in szamlak)
                {
                    if (szamla.Szamlaszam == szamlaszam)
                    {
                        throw new ArgumentException(
                            "A megadott számlaszámmal már létezik számla", "szamlaszam");
                    }
                }
            }
            szamlak.Add(new Szamla(nev, szamlaszam));
        }

        // Két számla között utal.
        // Ha nincs elég pénz a forrás számlán, akkor
        public bool Utal(string honnan, string hova, ulong osszeg)
        {
            Szamla egyik = SzamlaKeres(honnan);
            Szamla masik = SzamlaKeres(hova);
            if (egyik == masik)
            {
                throw new ArgumentException("A két számlaszám megyegyezik!");
            }
            if (egyik.Egyenleg < osszeg)
            {
                return false; 
            }
            egyik.Egyenleg -= osszeg;
            masik.Egyenleg += osszeg;
            return true;
        }

        // Lekérdezi az adott számlán lévő pénzösszeget
        public ulong Egyenleg(string szamlaszam)
        {
            return SzamlaKeres(szamlaszam).Egyenleg;
        }
    }

    // Nem létező számla esetén dobhatjuk bármely függvényből
    class HibasSzamlaszamException : Exception
    {
        public HibasSzamlaszamException(string szamlaszam)
            : base("Hibas szamlaszam: " + szamlaszam)
        {
        }
    }
}

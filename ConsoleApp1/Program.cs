using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lotto_szovegfajlbol
{
    /**
     * A program bekér öt egész számot 1-90 között. Gondoskodik róla, hogy ne fordulhasson elő ismétlődő érték!
     * Az eddig kihúzott nyerőszámokkal összehasonlítja és megmondja hányszor lett volna 2-es, 3-as, 4-gyes vagy 5-ös találata
     * A szükséges aadatok a https://bet.szerencsejatek.hu/jatekok/otoslotto/sorsolasok webhelyről letölthetők,
     * de mellékelve megkapja.
     * Az adatfájlban pontosvesszővel elválasztva 16 adat van soronként.
     * Év;Hét;Húzásdátum;5 találat (db);5 találat (Ft);4 találat (db);4 találat (Ft);3 találat (db);3 találat (Ft);2 találat (db);2 találat (Ft);Számok;;;;
     */
    class Lotto_szovegfajlbolm
    {
        static string forras = @"..\..\otos.csv";
        static int[] talalatok = { 0, 0, 0, 0, 0, 0 };
        static int[] tippek = new int[5];
        static List<int[]> sorsoltak = new List<int[]>();
        const int TIPPEKSZAMA = 5, MIN = 1, MAX = 90;
        static void Main(string[] args)
        {
            //-- Bekér --------------
            for (int i = 0; i < TIPPEKSZAMA; i++)
            {
                tippek[i] = szamotBeker(MIN, MAX);
            }
            Beolvas();
            for (int i = 0; i < sorsoltak.Count; i++)
            {
                talalatok[Talalat(sorsoltak[i], tippek)]++;
            }
            Console.WriteLine($"{sorsoltak.Count} sorsolásból:");
            for (int i = 0; i < talalatok.Length; i++)
            {
                Console.WriteLine($"\t{i} találat: {talalatok[i],5}");
            }
            Console.WriteLine("Program vége!");
            Console.ReadKey();
        }
        static void Beolvas()
        {
            if (File.Exists(forras))
            {
                using (StreamReader sr = new StreamReader(forras))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sor = sr.ReadLine().Split(';');
                        int[] s = { int.Parse(sor[sor.Length - 5]), int.Parse(sor[sor.Length - 4]), int.Parse(sor[sor.Length - 3]), int.Parse(sor[sor.Length - 2]), int.Parse(sor[sor.Length - 1]) };
                        sorsoltak.Add(s);
                    }
                }
            }
            else
            {
                Console.WriteLine("Nem található a forrás fájl!");
                Environment.Exit(0);
            }
        }

         static int Talalat(int[] sorsolt, int[] tippelt)
        {
            int talalat = 0;
            for (int i = 0; i < sorsolt.Length; i++)
            {
                for (int j = 0; j < tippelt.Length; j++)
                {
                    talalat += sorsolt[i] == tippelt[j] ? 1 : 0;
                }
            }
            return talalat;
        }

        static int szamotBeker(int tol, int ig)
        {
            int szam;
            Console.Write($"Kérek egy számot {tol} és {ig} között: ");
            while (!(int.TryParse(Console.ReadLine(), out szam) && szam <= ig && szam >= tol) || tippek.Contains(szam))
            {
                Console.WriteLine($"Ne szórakozz! Adj egy számot {tol} és {ig} között: ");
            }
            return szam;
        }
    }
}

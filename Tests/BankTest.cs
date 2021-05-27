using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BankProjekt.Tests
{
    [TestFixture]
    class BankTest
    {
        Bank b;

        [SetUp]
        public void Setup()
        {
            b = new Bank();
        }

        [TestCase]
        public void UjjSzamlaHibaNelkulLetrejon()
        {
            Assert.DoesNotThrow(() => b.UjSzamla("Teszt Elek", "1234"));
        }

        [TestCase]
        public void UjjSzamlaDuplikaltSzamlaszam()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<ArgumentException>(() => b.UjSzamla("Kovács Elek", "1234"));
        }

        [TestCase]
        public void UjszamlaLetezoNevvelNincsHiba()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.DoesNotThrow(() => b.UjSzamla("Teszt Elek", "4321"));
        }

        [TestCase]
        public void EgyenlegFeltoltNemLetezoEgyenleg()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<HibasSzamlaszamException>(() => b.EgyenlegFeltolt("Kenyér", 10000));
        }

        [TestCase]
        public void EgyenlegFeltoltMegfeleloErtekek()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.DoesNotThrow(() => b.EgyenlegFeltolt("1234", 10000));
        }

        [TestCase]
        public void EgyenlegLekerdezNemLetezoEgyenleg()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<HibasSzamlaszamException>(() => b.Egyenleg("Kenyér"));
        }

        [TestCase]
        public void EgyenlegLekerdezMegfeleloErtekek()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.AreEqual(0, EgyenlegszerzoTeszthez(b, "1234"));
        }
        private ulong EgyenlegszerzoTeszthez(Bank b, string szamla)
        {
            return b.Egyenleg(szamla);
        }

        [TestCase]
        public void UtalasElsoSzamlaNemJo()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<HibasSzamlaszamException>(() => b.Utal("Kenyér", "1234", 0));
        }

        [TestCase]
        public void UtalasMasodikSzamlaNemJo()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<HibasSzamlaszamException>(() => b.Utal("1234", "Kenyér", 0));
        }

        [TestCase]
        public void UtalasASzamlakMegyegyeznek()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<ArgumentException>(() => b.Utal("1234", "1234", 0));
        }

        [TestCase]
        public void UtalasNincsEgyenleg()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Teszt Béla", "4321");
            Assert.IsFalse(UtalasEllenorzoTeszthez(b));
        }
        private bool UtalasEllenorzoTeszthez(Bank b)
        {
            return b.Utal("1234", "4321", 200);
        }

        [TestCase]
        public void UtalasMegfeleloErtekekkel()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Teszt Béla", "4321");
            b.EgyenlegFeltolt("1234", 200);
            Assert.IsTrue(UtalasEllenorzoTeszthez(b));
        }
    }
}

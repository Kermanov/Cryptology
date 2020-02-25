using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static TrithemiusCipher.Logic.TrithemiusCipher;

namespace TrithemiusCipher.Tests
{
    [TestClass]
    public class TrithemiusCipherTests
    {
        private string engAlphabet = " 'abcdefghijklmnopqrstuvwxyz";
        //private string ukrAlphabet = " 'абвгґдеєжзиіїйклмнопрстуфхцчшщьюя";

        private string GetRandomString(string alphabet)
        {
            var rand = new Random();
            int length = rand.Next(1, 256);
            var stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; ++i)
            {
                stringBuilder.Append(alphabet[rand.Next(alphabet.Length)]);
            }

            return stringBuilder.ToString();
        }

        private void GetRandomLinearKey(out int coefA, out int coefB)
        {
            var rand = new Random();
            coefA = rand.Next(-100, 100);
            coefB = rand.Next(-100, 100);
        }

        private void GetRandomQuadraticKey(out int coefA, out int coefB, out int coefC)
        {
            GetRandomLinearKey(out coefA, out coefB);
            var rand = new Random();
            coefC = rand.Next(-100, 100);
        }

        private string GetRandomMotoKey(string alphabet)
        {
            return GetRandomString(alphabet);
        }

        [TestMethod]
        public void TestEncryptAndDecryptLinear()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, Logic.Alphabet.English, -50, -3);
            string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, -50, -3);

            Assert.AreNotEqual(inputText, encryptedText);
            Assert.AreEqual(inputText, decryptedText);
        }

        [TestMethod]
        public void TestEncryptAndDecryptQuadratic()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, Logic.Alphabet.English, -50, -3, 2);
            string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, -50, -3, 2);

            Assert.AreNotEqual(inputText, encryptedText);
            Assert.AreEqual(inputText, decryptedText);
        }

        [TestMethod]
        public void TestEncryptAndDecryptMoto()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, Logic.Alphabet.English, "moto");
            string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, "moto");

            Assert.AreNotEqual(inputText, encryptedText);
            Assert.AreEqual(inputText, decryptedText);
        }

        [TestMethod]
        public void TestRandomEncryptAndDecryptLinearEnglish()
        {
            int iterations = 100;
            for (int i = 0; i < iterations; ++i)
            {
                string inputText = GetRandomString(engAlphabet);
                GetRandomLinearKey(out int coefA, out int coefB);
                string encryptedText = Encrypt(inputText, Logic.Alphabet.English, coefA, coefB);
                string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, coefA, coefB);

                Assert.AreNotEqual(inputText, encryptedText);
                Assert.AreEqual(inputText, decryptedText);
            }
        }

        [TestMethod]
        public void TestCrackMoto()
        {
            string key = "qwer";
            string decryptedText = "test hello world message";
            string encryptedText = Encrypt(decryptedText, Logic.Alphabet.English, key);

            var crackedKey = CrackMoto(encryptedText, decryptedText, Logic.Alphabet.English);

            if (crackedKey == null)
            {
                Assert.IsNull(crackedKey);
                System.Diagnostics.Trace.WriteLine("null");
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(crackedKey);
                var keys = crackedKey.Split('\n');
                Assert.IsTrue(key == keys[0] || key == keys[1]);
            }
        }
    }
}

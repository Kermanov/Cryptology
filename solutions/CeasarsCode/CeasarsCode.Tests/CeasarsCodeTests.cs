using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static CeasarsCode.Logic.CeasarsCode;

namespace CeasarsCode.Tests
{
    [TestClass]
    public class CeasarsCodeTests
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

        private int GetRandomKey(string alphabet)
        {
            var rand = new Random();
            return rand.Next(alphabet.Length);
        }

        [TestMethod]
        public void TestEncryptAndDecrytEnglish()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, 14 ,Logic.Alphabet.English);
            string decryptedText = Decrypt(encryptedText, 14, Logic.Alphabet.English);

            Assert.AreNotEqual(inputText, encryptedText);
            Assert.AreEqual(inputText, decryptedText);
        }

        [TestMethod]
        public void TestRandomEncryptAndDecryptEnglish()
        {
            int iterations = 100;
            for (int i = 0; i < iterations; ++i)
            {
                var engText = GetRandomString(engAlphabet);
                var engKey = GetRandomKey(engAlphabet);

                var encryptedEngText = Encrypt(engText, engKey, Logic.Alphabet.English);
                var decryptedEngText = Decrypt(encryptedEngText, engKey, Logic.Alphabet.English);

                Assert.AreNotEqual(engText, encryptedEngText);
                Assert.AreEqual(engText, decryptedEngText);
            }
        }
    }
}

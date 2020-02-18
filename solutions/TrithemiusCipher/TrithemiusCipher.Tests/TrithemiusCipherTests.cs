using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static TrithemiusCipher.Logic.TrithemiusCipher;

namespace TrithemiusCipher.Tests
{
    [TestClass]
    public class TrithemiusCipherTests
    {
        [TestMethod]
        public void TestEncryptAndDecryptLinear()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, Logic.Alphabet.English, -50, -3);
            string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, -50, -3);

            Assert.AreEqual(inputText, decryptedText);
        }

        [TestMethod]
        public void TestEncryptAndDecryptQuadratic()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, Logic.Alphabet.English, -50, -3, 2);
            string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, -50, -3, 2);

            Assert.AreEqual(inputText, decryptedText);
        }

        [TestMethod]
        public void TestEncryptAndDecryptMoto()
        {
            string inputText = "hello world";
            string encryptedText = Encrypt(inputText, Logic.Alphabet.English, "moto");
            string decryptedText = Decrypt(encryptedText, Logic.Alphabet.English, "moto");

            Assert.AreEqual(inputText, decryptedText);
        }
    }
}

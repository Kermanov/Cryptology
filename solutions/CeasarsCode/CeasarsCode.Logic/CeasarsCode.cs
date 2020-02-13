using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeasarsCode.Logic
{
    public enum Alphabet
    {
        English,
        Ukrainian
    }

    public static class CeasarsCode
    {
        private static Dictionary<Alphabet, string> alphabets;

        static CeasarsCode()
        {
            alphabets = new Dictionary<Alphabet, string>
            {
                { Alphabet.English, " abcdefghijkmnopqrstuvwxyz" },
                { Alphabet.Ukrainian, " абвгґдеєжзиіїйклмнопрстуфхцчшщьюя" }
            };
        }

        public static bool IsKeyValid(int key, Alphabet alphabet)
        {
            return key < alphabets[alphabet].Length;
        }

        public static bool IsTextValid(string text, Alphabet alphabet)
        {
            foreach (var symbol in text)
            {
                if (!alphabets[alphabet].Contains(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        private static string Shift(string text, int shift, Alphabet alphabet)
        {
            var stringBuilder = new StringBuilder(text.Length);
            foreach (var symbol in text)
            {
                int index = alphabets[alphabet].IndexOf(symbol);
                index = (index + shift) % alphabets[alphabet].Length;
                stringBuilder.Append(alphabets[alphabet][index]);
            }

            return stringBuilder.ToString();
        }

        public static string Encrypt(string text, int key, Alphabet alphabet)
        {
            return Shift(text, key, alphabet);
        }

        public static string Decrypt(string text, int key, Alphabet alphabet)
        {
            return Shift(text, -key, alphabet);
        }
    }
}

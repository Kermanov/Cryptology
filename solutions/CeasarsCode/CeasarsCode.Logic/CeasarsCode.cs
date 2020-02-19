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
        Ukrainian,
        Binary
    }

    public static class CeasarsCode
    {
        private static Dictionary<Alphabet, string> alphabets;

        static CeasarsCode()
        {
            alphabets = new Dictionary<Alphabet, string>
            {
                { Alphabet.English, " 'abcdefghijklmnopqrstuvwxyz" },
                { Alphabet.Ukrainian, " 'абвгґдеєжзиіїйклмнопрстуфхцчшщьюя" }
            };
        }

        public static bool IsKeyValid(int key, Alphabet alphabet)
        {
            return key >= 0 && key < alphabets[alphabet].Length;
        }

        public static bool IsBinaryKeyValid(int key)
        {
            return key >= 0 && key < 256;
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
                index = (index + shift + alphabets[alphabet].Length) % alphabets[alphabet].Length;
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

        private static byte[] BiteShift(byte[] bytes, int shift)
        {
            var shiftedBytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; ++i)
            {
                shiftedBytes[i] = (byte)((bytes[i] + 256 + shift) % 256);
            }

            return shiftedBytes;
        }

        public static byte[] EncryptBytes(byte[] bytes, byte key)
        {
            return BiteShift(bytes, key);
        }

        public static byte[] DecryptBytes(byte[] bytes, byte key)
        {
            return BiteShift(bytes, -key);
        }
    }
}

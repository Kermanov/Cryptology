using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher.Logic
{
    public enum Alphabet
    {
        English,
        Ukrainian
    }

    public enum Mode
    {
        Encrypt,
        Decrypt
    }


    public static class VigenereCipher
    {
        private static Dictionary<Alphabet, string> alphabets;

        static VigenereCipher()
        {
            alphabets = new Dictionary<Alphabet, string>
            {
                { Alphabet.English, " 'abcdefghijklmnopqrstuvwxyz" },
                { Alphabet.Ukrainian, " 'абвгґдеєжзиіїйклмнопрстуфхцчшщьюя" }
            };
        }

        public static bool IsKeyValid(string key, Alphabet alphabet)
        {
            return IsTextValid(key, alphabet);
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

        private static char ShiftChar(char textChar, char keyChar, Alphabet alphabet, Mode mode)
        {
            var alph = alphabets[alphabet];

            int newIndex; 
            if (mode == Mode.Encrypt)
            {
                newIndex = (alph.IndexOf(textChar) + alph.IndexOf(keyChar)) % alph.Length;
            }
            else
            {
                newIndex = (alph.IndexOf(textChar) - alph.IndexOf(keyChar) + alph.Length) % alph.Length;
            }

            return alph[newIndex];
        }

        private static string ShiftString(string text, string key, Alphabet alphabet, Mode mode)
        {
            var stringBuilder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; ++i)
            {
                stringBuilder.Append(ShiftChar(text[i], key[i % key.Length], alphabet, mode));
            }

            return stringBuilder.ToString();
        }

        public static string Encrypt(string text, string key, Alphabet alphabet)
        {
            return ShiftString(text, key, alphabet, Mode.Encrypt);
        }

        public static string Decrypt(string text, string key, Alphabet alphabet)
        {
            return ShiftString(text, key, alphabet, Mode.Decrypt);
        }
    }
}

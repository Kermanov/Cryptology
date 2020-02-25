using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrithemiusCipher.Logic
{
    public enum Alphabet
    {
        English,
        Ukrainian
    }


    public static class TrithemiusCipher
    {
        private static Dictionary<Alphabet, string> alphabets;
        static TrithemiusCipher()
        {
            alphabets = new Dictionary<Alphabet, string>
            {
                { Alphabet.English, " 'abcdefghijklmnopqrstuvwxyz" },
                { Alphabet.Ukrainian, " 'абвгґдеєжзиіїйклмнопрстуфхцчшщьюя" }
            };
        }

        public static bool IsKeyValid(string coef, out int intCoef)
        {
            return int.TryParse(coef, out intCoef);
        }

        public static bool IsKeyValid(string moto, Alphabet alphabet)
        {
            return IsTextValid(moto, alphabet);
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

        private static string ShiftViaFunction(string text, Alphabet alphabet, Func<int, int> func)
        {
            var stringBuilder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; ++i)
            {
                int index = alphabets[alphabet].IndexOf(text[i]);
                int pos = index + func(i);
                while (pos < 0)
                {
                    pos += alphabets[alphabet].Length;
                }

                index = pos % alphabets[alphabet].Length;
                stringBuilder.Append(alphabets[alphabet][index]);
            }

            return stringBuilder.ToString();
        }

        public static string Encrypt(string text, Alphabet alphabet, int coefA, int coefB)
        {
            return ShiftViaFunction(text, alphabet, p => coefA * p + coefB);
        }

        public static string Encrypt(string text, Alphabet alphabet, int coefA, int coefB, int coefC)
        {
            return ShiftViaFunction(text, alphabet, p => coefA * p * p + coefB * p + coefC);
        }

        public static string Encrypt(string text, Alphabet alphabet, string moto)
        {
            return ShiftViaFunction(text, alphabet, p =>
            {
                return alphabets[alphabet].IndexOf(moto[p % moto.Length]);
            });
        }

        public static string Decrypt(string text, Alphabet alphabet, int coefA, int coefB)
        {
            return ShiftViaFunction(text, alphabet, p => -(coefA * p + coefB));
        }

        public static string Decrypt(string text, Alphabet alphabet, int coefA, int coefB, int coefC)
        {
            return ShiftViaFunction(text, alphabet, p => -(coefA * p * p + coefB * p + coefC));
        }

        public static string Decrypt(string text, Alphabet alphabet, string moto)
        {
            return ShiftViaFunction(text, alphabet, p =>
            {
                return -alphabets[alphabet].IndexOf(moto[p % moto.Length]);
            });
        }

        private static bool PatternCanFillString(string pattern, string str)
        {
            var replaced = str.Replace(pattern, "");
            return replaced.Length < pattern.Length && pattern.Contains(replaced);
        }

        public static string CrackMoto(string encryptedText, string decryptedText, Alphabet alphabet)
        {
            var deltaStringBuilder = new StringBuilder(encryptedText.Length);
            var alph = alphabets[alphabet];
            for (int i = 0; i < encryptedText.Length; ++i)
            {
                int delta = (alph.IndexOf(encryptedText[i]) - alph.IndexOf(decryptedText[i]) + alph.Length) % alph.Length;
                deltaStringBuilder.Append(alph[delta]);
            }

            var deltaString = deltaStringBuilder.ToString();
            for (int i = 1; i < deltaString.Length + 1; ++i)
            {
                var pattern = deltaString.Substring(0, i);
                if (PatternCanFillString(pattern, deltaString))
                {
                    return pattern;
                }
            }

            return null;
        }

        public static string CrackLinear(string encryptedText, string decryptedText, Alphabet alphabet)
        {
            var alph = alphabets[alphabet];
            int coefB = alph.IndexOf(encryptedText[0]) - alph.IndexOf(decryptedText[0]);
            int coefA = alph.IndexOf(encryptedText[1]) - alph.IndexOf(decryptedText[1]) - coefB;
            return $"{coefA} {coefB}";
        }

        public static string CrackQuadratic(string encryptedText, string decryptedText, Alphabet alphabet)
        {
            var alph = alphabets[alphabet];
            int coefC = alph.IndexOf(encryptedText[0]) - alph.IndexOf(decryptedText[0]);
            int coefA = (alph.IndexOf(encryptedText[2]) - alph.IndexOf(decryptedText[2]) - coefC) / 2 
                - alph.IndexOf(encryptedText[1]) + alph.IndexOf(decryptedText[1]) + coefC;
            int coefB = alph.IndexOf(encryptedText[1]) - alph.IndexOf(decryptedText[1]) - coefA - coefC;

            return $"{coefA} {coefB} {coefC}";
        }
    }
}

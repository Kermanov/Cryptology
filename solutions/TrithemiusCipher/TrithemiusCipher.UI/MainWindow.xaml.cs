using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrithemiusCipher.Logic;

using static TrithemiusCipher.Logic.TrithemiusCipher;

namespace TrithemiusCipher.UI
{
    enum KeyType
    {
        Linear,
        Quadratic,
        Moto
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filterString = "Text file (*.txt)|*.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private enum Mode
        {
            Encrypt,
            Decrypt
        }

        private void transformButton_Click(object sender, RoutedEventArgs e)
        {

            if (CheckIsTextValid() && CheckIsKeyValid())
            {
                var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;
                var keyType = (KeyType)keyTypeComboBox.SelectedIndex;

                if (keyType == KeyType.Linear)
                {
                    var coefs = keyTextBox.Text.Split();
                    int coefA = int.Parse(coefs[0]);
                    int coefB = int.Parse(coefs[1]);

                    if (modeComboBox.SelectedIndex == 0)
                    {
                        outputTextBox.Text = Encrypt(inputTextBox.Text, alphabet, coefA, coefB);
                    }
                    else
                    {
                        outputTextBox.Text = Decrypt(inputTextBox.Text, alphabet, coefA, coefB);
                    }
                }
                else if (keyType == KeyType.Quadratic)
                {
                    var coefs = keyTextBox.Text.Split();
                    int coefA = int.Parse(coefs[0]);
                    int coefB = int.Parse(coefs[1]);
                    int coefC = int.Parse(coefs[2]);

                    if (modeComboBox.SelectedIndex == 0)
                    {
                        outputTextBox.Text = Encrypt(inputTextBox.Text, alphabet, coefA, coefB, coefC);
                    }
                    else
                    {
                        outputTextBox.Text = Decrypt(inputTextBox.Text, alphabet, coefA, coefB, coefC);
                    }
                }
                else if (keyType == KeyType.Moto)
                {
                    if (modeComboBox.SelectedIndex == 0)
                    {
                        outputTextBox.Text = Encrypt(inputTextBox.Text, alphabet, keyTextBox.Text);
                    }
                    else
                    {
                        outputTextBox.Text = Decrypt(inputTextBox.Text, alphabet, keyTextBox.Text);
                    }
                }
            }
        }

        private bool CheckIsKeyValid()
        {
            var keyString = keyTextBox.Text;
            var keyType = (KeyType)keyTypeComboBox.SelectedIndex;

            bool isKeyValid = false;
            if (keyType == KeyType.Linear)
            {
                var coefs = keyString.Split();
                isKeyValid = coefs.Length == 2 
                    && IsKeyValid(coefs[0], out int _) 
                    && IsKeyValid(coefs[1], out int _);
            }
            else if (keyType == KeyType.Quadratic)
            {
                var coefs = keyString.Split();
                isKeyValid = coefs.Length == 3 
                    && IsKeyValid(coefs[0], out int _) 
                    && IsKeyValid(coefs[1], out int _) 
                    && IsKeyValid(coefs[1], out int _);
            }
            else if (keyType == KeyType.Moto)
            {
                var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;
                isKeyValid = IsKeyValid(keyString, alphabet);
            }

            if (isKeyValid)
            {
                keyTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 200, 0));
            }
            else
            {
                keyTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 0, 0));
            }

            return isKeyValid;
        }

        private bool CheckIsTextValid()
        {
            var inputText = GetInputText();
            var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;

            var isTextValid = IsTextValid(inputText, alphabet);
            if (isTextValid)
            {
                inputTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 200, 0));
            }
            else
            {
                inputTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 0, 0));
            }

            return isTextValid;
        }

        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIsTextValid();
        }

        private void keyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIsKeyValid();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = filterString
            };

            if (openFileDialog.ShowDialog().Value)
            {
                inputTextBox.Text = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
            }
        }

        private void SaveToFile(string text)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filterString
            };


            if (saveFileDialog.ShowDialog().Value)
            {
                File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        private void SaveInputMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile(inputTextBox.Text);
        }

        private void SaveOutputMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile(outputTextBox.Text);
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            inputTextBox.Clear();
            outputTextBox.Clear();
            keyTextBox.Clear();

            modeComboBox.SelectedIndex = 0;
            alphabetComboBox.SelectedIndex = 0;
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void bruteforceButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private string GetInputText()
        {
            return inputTextBox.Text.ToLower();
        }

        private void alphabetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIsTextValid();
        }

        private void crackButton_Click(object sender, RoutedEventArgs e)
        {
            var alphabet = (Alphabet)crackAlphabetComboBox.SelectedIndex;
            var keyType = (KeyType)crackKeyTypeComboBox.SelectedIndex;

            if (keyType == KeyType.Linear)
            {
                crackedKeyTextBox.Text = CrackLinear(encryptedTextBox.Text, decryptedTextBox.Text, alphabet);
            }
            else if (keyType == KeyType.Quadratic)
            {
                crackedKeyTextBox.Text = CrackQuadratic(encryptedTextBox.Text, decryptedTextBox.Text, alphabet);
            }
            else if (keyType == KeyType.Moto)
            {
                crackedKeyTextBox.Text = CrackMoto(encryptedTextBox.Text, decryptedTextBox.Text, alphabet) ?? "Can not find key";
            }
        }
    }
}

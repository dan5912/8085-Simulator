using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _8050_Simulator
{
    public partial class Form1 : Form
    {
        private void assemble()
        {
            hexTextbox.Text = "";
            string [,] assemblyCode = parseAssemblyText();
            int ub = assemblyCode.GetLength(0);
            for (int row = 0; row < assemblyCode.GetLength(0); row++)
            {
                switch (assemblyCode[row, 0].ToUpper())
                {
                    case "MOV":
                        switch (assemblyCode[row, 1].ToUpper())
                        {
                            case "A":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "7F ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "78 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "79 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "7A ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "7B ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "7C ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "7D ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "7E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "B":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "47 ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "40 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "41 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "42 ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "43 ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "44 ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "45 ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "46 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "C":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "4F ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "48 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "49 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "4A ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "4B ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "4C ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "4D ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "4E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "D":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "57 ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "50 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "51 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "52 ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "53 ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "54 ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "55 ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "56 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "E":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "5F ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "58 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "59 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "5A ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "5B ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "5C ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "5D ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "5E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "H":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "67 ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "60 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "61 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "62 ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "63 ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "64 ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "65 ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "66 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "L":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "6F ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "68 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "69 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "6A ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "6B ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "6C ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "6D ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "6E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;

                            case "M":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexTextbox.Text += "77 ";
                                        break;

                                    case "B":
                                        hexTextbox.Text += "70 ";
                                        break;

                                    case "C":
                                        hexTextbox.Text += "71 ";
                                        break;

                                    case "D":
                                        hexTextbox.Text += "72 ";
                                        break;

                                    case "E":
                                        hexTextbox.Text += "73 ";
                                        break;

                                    case "H":
                                        hexTextbox.Text += "74 ";
                                        break;

                                    case "L":
                                        hexTextbox.Text += "75 ";
                                        break;

                                    case "M":
                                        hexTextbox.Text += "76 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return;
                                }
                                break;



                             
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

            }
        }

        private string[,] parseAssemblyText()
        {

            char[] s1 = assemblyTextbox.Text.ToCharArray();
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i].Equals(','))
                {
                    s1[i] = ' ';
                }
            }

            string[,] s2 = new string[s1.Length, 3];

            int row = 0;
            int col = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i].Equals('\n') || s1[i].Equals('\r'))
                {
                    if (i + 1 < s1.Length)
                    {
                        while (s1[i + 1].Equals('\r') || s1[i + 1].Equals('\n'))
                        {
                            if (i + 2 < s1.Length)
                            {
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        while (s1[i + 1].Equals(' '))
                        {
                            if (i + 2 < s1.Length)
                            {
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    row++;
                    col = 0;
                    continue;
                }
                else if (s1[i].Equals(' '))
                {
                    if (s2[row, col] != null)
                    {
                        col++;
                    }
                    if (i + 1 < s1.Length)
                    {
                        while (s1[i + 1].Equals(' '))
                        {
                            i++;
                        }
                    }
                    continue;
                }
                else
                {
                    s2[row, col] += s1[i];
                }

            }
            int count = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s2[i, 1] != null)
                {
                    count++;
                }
            }

            string[,] s3 = new string[count, 3];

            for (int i = 0; i < count; i++)
            {
                s3[i, 0] = s2[i, 0];
                s3[i, 1] = s2[i, 1];
                s3[i, 2] = s2[i, 2];
            }

            return s3;
        }

        private void handleError(int row, int col)
        {

        }
    }
}
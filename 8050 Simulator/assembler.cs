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
            string[,] labels = new string[100, 2];
            hexTextbox.Text = "";
            bool return_ok = false;
            string [,] assemblyCode = parseAssemblyText();
            if (assemblyCode == null)
            {
                return;
            }

            string hexCode = processAssembly(ref assemblyCode, ref labels);
            if (hexCode == null)
            {
                return;
            }
            string[] hexCodeSplit = hexCode.Split(' ');
            for (int i = 0; i < hexCodeSplit.GetLength(0); i++)
            {
                if (hexCodeSplit[i] != null && hexCodeSplit[i] != "")
                {
                    if (hexCodeSplit[i][0].Equals('%'))
                    {
                        return_ok = false;
                        string s1 = hexCodeSplit[i].Remove(0,1);
                        for (int j = 0; j < labels.GetLength(0); j++)
                        {
                            if (s1.Equals(labels[j, 0]))
                            {
                                hexCodeSplit[i] = labels[j, 1];
                                return_ok = true;
                            }
                        }
                        if (!return_ok)
                        {
                            hexTextbox.Text = "Unrecognized label: " + (string)s1;
                            return;
                        }
                    }
                }
            }
            hexCode = String.Join(" ", hexCodeSplit);
            hexTextbox.Text = hexCode;   

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
                else if (col > 2)
                {
                    handleError(row, 2);
                    hexTextbox.Text += " Too many arguments";
                    return new string[1, 1];
                }
                else
                {
                    s2[row, col] += s1[i];
                }

            }
            int count = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s2[i, 0] != null)
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

            for (row = 0; row < s3.GetLength(0); row++)
            {
                for (col = 0; col < 3; col++)
                {
                    if (s3[row, col] == null)
                    {
                        s3[row, col] = "";
                    }
                }
            }

            return s3;
        }

        private void handleError(int row, int col)
        {
            hexTextbox.Text = "";
            switch (col)
            {
                case 0:
                    hexTextbox.Text += "Unrecognized mnemonic - Line: " + (row+1);
                    break;

                case 1:
                    hexTextbox.Text += "Unrecognized argument - Line: " + (row+1) + " Argument: 1";
                    break;

                case 2:
                    hexTextbox.Text += "Unrecognized argument - Line: " + (row+1) + " Argument: 2";
                    break;
                    
            }
        }

        private void handleError(int row, int col, string errorMessage)
        {
            hexTextbox.Text = "";
            switch (col)
            {
                case 0:
                    hexTextbox.Text += "Unrecognized mnemonic - Line: " + (row + 1);
                    break;

                case 1:
                    hexTextbox.Text += "Unrecognized argument - Line: " + (row + 1) + " Argument: 1";
                    break;

                case 2:
                    hexTextbox.Text += "Unrecognized argument - Line: " + (row + 1) + " Argument: 2";
                    break;

            }
            hexTextbox.Text += " " + errorMessage;
        }

        private uint? parseNumber(string numberString)
        {
            if (numberString[numberString.Length - 1] == 'h')
            {
                numberString = numberString.Remove(numberString.Length - 1);
                return Convert.ToUInt16(numberString, 16);
            }
            else if (numberString[numberString.Length - 1] == 'o')
            {
                numberString = numberString.Remove(numberString.Length - 1);

                return Convert.ToUInt16(numberString, 8);
            }
            else
            {
                for (int i = 0; i < numberString.Length; i++)
                {
                    if (numberString[i] > '9' || numberString[i] < '0')
                    {
                        return null;
                    }
                }
                return Convert.ToUInt16(numberString, 10);
            }
        }

        private int processLabel(string s1, int row, int currentIndex, ref string [,] labels, ref int labelsNextIndex)
        {
            if (s1[s1.Length - 1] == ':')
            {
                labels[labelsNextIndex,0] = s1.Remove(s1.Length - 1);
                labels[labelsNextIndex, 1] = ((programStartAddr + currentIndex) & 0xFF).ToString("X2") + " " + (((programStartAddr + currentIndex) & 0xFF00) >> 16).ToString("X2");
                labelsNextIndex++;
                return 0;
            }
            return -1;
        }

        private string replaceLabel(string label, ref string[,] labels)
        {
            string result = null;
            for (int i = 0; i < labels.GetLength(0); i++)
            {
                if (label == labels[i, 0])
                {
                    result = labels[i, 1] + " ";
                    break;
                }
            }
            return result;
        }

        private string processAssembly(ref string[,] assemblyCode, ref string[,] labels)
        {
            int labelsNextIndex = 0;
            int currentIndex = 0;
            uint? nullTemp = null;
            string hexCode = null;
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
                                        hexCode += "7F ";
                                        break;

                                    case "B":
                                        hexCode += "78 ";
                                        break;

                                    case "C":
                                        hexCode += "79 ";
                                        break;

                                    case "D":
                                        hexCode += "7A ";
                                        break;

                                    case "E":
                                        hexCode += "7B ";
                                        break;

                                    case "H":
                                        hexCode += "7C ";
                                        break;

                                    case "L":
                                        hexCode += "7D ";
                                        break;

                                    case "M":
                                        hexCode += "7E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "B":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "47 ";
                                        break;

                                    case "B":
                                        hexCode += "40 ";
                                        break;

                                    case "C":
                                        hexCode += "41 ";
                                        break;

                                    case "D":
                                        hexCode += "42 ";
                                        break;

                                    case "E":
                                        hexCode += "43 ";
                                        break;

                                    case "H":
                                        hexCode += "44 ";
                                        break;

                                    case "L":
                                        hexCode += "45 ";
                                        break;

                                    case "M":
                                        hexCode += "46 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "C":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "4F ";
                                        break;

                                    case "B":
                                        hexCode += "48 ";
                                        break;

                                    case "C":
                                        hexCode += "49 ";
                                        break;

                                    case "D":
                                        hexCode += "4A ";
                                        break;

                                    case "E":
                                        hexCode += "4B ";
                                        break;

                                    case "H":
                                        hexCode += "4C ";
                                        break;

                                    case "L":
                                        hexCode += "4D ";
                                        break;

                                    case "M":
                                        hexCode += "4E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "D":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "57 ";
                                        break;

                                    case "B":
                                        hexCode += "50 ";
                                        break;

                                    case "C":
                                        hexCode += "51 ";
                                        break;

                                    case "D":
                                        hexCode += "52 ";
                                        break;

                                    case "E":
                                        hexCode += "53 ";
                                        break;

                                    case "H":
                                        hexCode += "54 ";
                                        break;

                                    case "L":
                                        hexCode += "55 ";
                                        break;

                                    case "M":
                                        hexCode += "56 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "E":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "5F ";
                                        break;

                                    case "B":
                                        hexCode += "58 ";
                                        break;

                                    case "C":
                                        hexCode += "59 ";
                                        break;

                                    case "D":
                                        hexCode += "5A ";
                                        break;

                                    case "E":
                                        hexCode += "5B ";
                                        break;

                                    case "H":
                                        hexCode += "5C ";
                                        break;

                                    case "L":
                                        hexCode += "5D ";
                                        break;

                                    case "M":
                                        hexCode += "5E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "H":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "67 ";
                                        break;

                                    case "B":
                                        hexCode += "60 ";
                                        break;

                                    case "C":
                                        hexCode += "61 ";
                                        break;

                                    case "D":
                                        hexCode += "62 ";
                                        break;

                                    case "E":
                                        hexCode += "63 ";
                                        break;

                                    case "H":
                                        hexCode += "64 ";
                                        break;

                                    case "L":
                                        hexCode += "65 ";
                                        break;

                                    case "M":
                                        hexCode += "66 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "L":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "6F ";
                                        break;

                                    case "B":
                                        hexCode += "68 ";
                                        break;

                                    case "C":
                                        hexCode += "69 ";
                                        break;

                                    case "D":
                                        hexCode += "6A ";
                                        break;

                                    case "E":
                                        hexCode += "6B ";
                                        break;

                                    case "H":
                                        hexCode += "6C ";
                                        break;

                                    case "L":
                                        hexCode += "6D ";
                                        break;

                                    case "M":
                                        hexCode += "6E ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            case "M":
                                switch (assemblyCode[row, 2].ToUpper())
                                {
                                    case "A":
                                        hexCode += "77 ";
                                        break;

                                    case "B":
                                        hexCode += "70 ";
                                        break;

                                    case "C":
                                        hexCode += "71 ";
                                        break;

                                    case "D":
                                        hexCode += "72 ";
                                        break;

                                    case "E":
                                        hexCode += "73 ";
                                        break;

                                    case "H":
                                        hexCode += "74 ";
                                        break;

                                    case "L":
                                        hexCode += "75 ";
                                        break;

                                    case "M":
                                        hexCode += "76 ";
                                        break;


                                    default:
                                        handleError(row, 2);
                                        return null;
                                }
                                break;

                            default:
                                handleError(row, 1);
                                return null;
                        }
                        currentIndex++;
                        break;

                    case "HLT":
                        hexCode += "76 ";
                        currentIndex++;
                        break;

                    case "NOP":
                        hexCode += "00 ";
                        currentIndex++;
                        break;

                    case "MVI":
                        switch (assemblyCode[row, 1].ToUpper())
                        {
                            case "A":
                                hexCode += "3E ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "B":
                                hexCode += "06 ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "C":
                                hexCode += "0E ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "D":
                                hexCode += "16 ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "E":
                                hexCode += "1E ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "H":
                                hexCode += "26 ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "L":
                                hexCode += "2E ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            case "M":
                                hexCode += "36 ";
                                nullTemp = parseNumber(assemblyCode[row, 2]);
                                if (nullTemp == null || nullTemp > 0xFF)
                                {
                                    handleError(row, 2);
                                    return null;
                                }
                                hexCode += ((int)nullTemp).ToString("X2") + " ";
                                break;

                            default:
                                handleError(row, 1);
                                return null;
                        }
                        currentIndex += 2;
                        break;

                    case "ACI":
                        nullTemp = parseNumber(assemblyCode[row, 1]);
                        if (nullTemp == null || nullTemp > 0xFF)
                        {
                            handleError(row, 1);
                            return null;
                        }
                        hexCode += "CE " + ((int)nullTemp).ToString("X2") + " ";
                        currentIndex += 2;
                        break;

                    case "ADC":
                        switch (assemblyCode[row, 1])
                        {
                            case "A":
                                hexCode += "8F ";
                                break;

                            case "B":
                                hexCode += "88 ";
                                break;

                            case "C":
                                hexCode += "89 ";
                                break;

                            case "D":
                                hexCode += "8A ";
                                break;

                            case "E":
                                hexCode += "8B ";
                                break;

                            case "H":
                                hexCode += "8C ";
                                break;

                            case "L":
                                hexCode += "8D ";
                                break;

                            case "M":
                                hexCode += "8E ";
                                break;

                            default:
                                handleError(row, 1);
                                return null;
                        }
                        currentIndex++;
                        break;

                    case "ADD":
                        switch (assemblyCode[row, 1])
                        {
                            case "A":
                                hexCode += "87 ";
                                break;

                            case "B":
                                hexCode += "80 ";
                                break;

                            case "C":
                                hexCode += "81 ";
                                break;

                            case "D":
                                hexCode += "82 ";
                                break;

                            case "E":
                                hexCode += "83 ";
                                break;

                            case "H":
                                hexCode += "84 ";
                                break;

                            case "L":
                                hexCode += "85 ";
                                break;

                            case "M":
                                hexCode += "86 ";
                                break;

                            default:
                                handleError(row, 1);
                                return null;
                        }
                        currentIndex++;
                        break;

                    case "ADI":
                        nullTemp = parseNumber(assemblyCode[row, 1]);
                        if (nullTemp == null || nullTemp > 0xFF)
                        {
                            handleError(row, 1);
                            return null;
                        }
                        hexCode += "C6 " + ((int)nullTemp).ToString("X2") + " ";
                        currentIndex += 2;
                        break;

                    case "ANA":
                        switch (assemblyCode[row, 1])
                        {
                            case "A":
                                hexCode += "A7 ";
                                break;

                            case "B":
                                hexCode += "A0 ";
                                break;

                            case "C":
                                hexCode += "A1 ";
                                break;

                            case "D":
                                hexCode += "A2 ";
                                break;

                            case "E":
                                hexCode += "A3 ";
                                break;

                            case "H":
                                hexCode += "A4 ";
                                break;

                            case "L":
                                hexCode += "A5 ";
                                break;

                            case "M":
                                hexCode += "A6 ";
                                break;

                            default:
                                handleError(row, 1);
                                return null;
                        }
                        currentIndex++;
                        break;

                    case "ANI":
                        nullTemp = parseNumber(assemblyCode[row, 1]);
                        if (nullTemp == null || nullTemp > 0xFF)
                        {
                            handleError(row, 1);
                            return null;
                        }
                        hexCode += "E6 " + ((int)nullTemp).ToString("X2") + " ";
                        currentIndex += 2;
                        break;

                    case "RET":
                        hexCode += "C9 ";
                        currentIndex++;
                        break;

                    case "JMP":
                        hexCode += "C3 ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;


                    case "CALL":
                        hexCode += "CD ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CC":
                        hexCode += "DC ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CM":
                        hexCode += "FC ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CNC":
                        hexCode += "D4 ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CNZ":
                        hexCode += "C4 ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CP":
                        hexCode += "F4 ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CPO":
                        hexCode += "E4 ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;

                    case "CZ":
                        hexCode += "CC ";
                        hexCode += "%" + assemblyCode[row, 1] + " ";
                        currentIndex += 3;
                        break;



                    default:
                        if (processLabel(assemblyCode[row, 0], row, currentIndex, ref labels, ref labelsNextIndex) == -1)
                        {
                            handleError(row, 0);
                            return null;
                        }
                        break;
                }

            }
            return hexCode;
        }
    }
}
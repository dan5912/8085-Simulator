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
        static int CARRY = 0;
        static int PARITY = 2;
        static int AUX_CARRY = 4;
        static int ZERO = 6;
        static int SIGN = 7;
        int currentByte = 0;
        string[] memory = new string[0xFFFF];
        string[] programMemory = { "76"};
        string[] dataMemory = {""};
        List<int> stack = new List<int>();
        UInt16 dataMemoryStartAddr = 0x0F00;
        UInt16 programStartAddr = 0x0000;
        int stackPointer = 0xFFFF;
        int flags = 0;
        int regA = 0, regB = 0, regC = 0, regD = 0, regE = 0, regH = 0x00, regL = 0x00;
        
        int inputPort = 0;
        int outputPort = 0;
        bool halted = true;
        int speed = 0;
        BackgroundWorker runWorker = new BackgroundWorker();
        
        private void initializeRegToZero()
        {
            regA = 0; 
            regB = 0;
            regC = 0;
            regD = 0;
            regE = 0;
            regH = 0;
            regL = 0;
            flags = 0;
            out11(0);
            stackPointer = 0xFFFF;
        }

        public Form1()
        {
            InitializeComponent();
            
            for (int i = 0; i < dataMemory.Length; i++)
            {
                memory[dataMemoryStartAddr+ i] = dataMemory[i];
            }

            runWorker.DoWork += new DoWorkEventHandler(run);
            runWorker.ProgressChanged += new ProgressChangedEventHandler(runWorker_ProgressChanged);
            runWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runWorker_Completed);
            runWorker.WorkerReportsProgress = true;
            runWorker.WorkerSupportsCancellation = true;
        }

        private void button1_Click(object sender, EventArgs e) //run button
        {
            if (runWorker.IsBusy != true)
            {
                runButton.Enabled = false;
                runWorker.RunWorkerAsync();
            }
        }

        private void run(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            currentByte = programStartAddr;
            initializeRegToZero();
            halted = false;
            while (!halted)
            {
                stepForward();
                worker.ReportProgress(0);
                if (speed > 0)
                {
                    Thread.Sleep(200/speed);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            worker.ReportProgress(0);
        }

        private void runWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            updateRegUI();
            updateIO_UI(outputPort, inputPort);
            updateStackUI();
        }

        private void runWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            runButton.Enabled = true;
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            if (currentByte < memory.Length)
            {
                halted = false;
                stepForward();
                halted = true;
            }
            updateRegUI();
            updateIO_UI(outputPort, inputPort);
            updateStackUI();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            runWorker.CancelAsync();
            currentByte = 0;
            initializeRegToZero();
            halted = true;
            updateRegUI();
            updateIO_UI(outputPort, inputPort);
            updateStackUI();
        }

        private void loadHex()
        {
            programMemory = hexTextbox.Text.Split(' ');
            for (int i = 0; i < programMemory.Length; i++)
            {
                memory[programStartAddr + i] = programMemory[i];
            }
        }

        private void stepForward()
        {
            int M = 0;
            int temp = 0;




            {
                switch (memory[currentByte])
                {
                    case "CE":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        ADC(temp);
                        break;

                    case "8F":
                        ADC(regA);
                        break;

                    case "88":
                        ADC(regB);
                        break;

                    case "89":
                        ADC(regC);
                        break;

                    case "8A":
                        ADC(regD);
                        break;

                    case "8B":
                        ADC(regE);
                        break;

                    case "8C":
                        ADC(regE);
                        break;

                    case "8D":
                        ADC(regL);
                        break;

                    case "8E":
                        M = regL;
                        M |= (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        ADC(temp);
                        break;

                    case "87":
                        ADD(regA);
                        break;

                    case "80":
                        ADD(regB);
                        break;

                    case "81":
                        ADD(regC);
                        break;

                    case "82":
                        ADD(regD);
                        break;

                    case "83":
                        ADD(regE);
                        break;

                    case "84":
                        ADD(regH);
                        break;
                    case "85":
                        ADD(regL);
                        break;

                    case "86":
                        M = regL;
                        M |= (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        ADD(temp);
                        break;

                    case "C6":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        ADD(temp);
                        break;

                    case "A7":
                        ANA(regA);
                        break;

                    case "A0":
                        ANA(regB);
                        break;

                    case "A1":
                        ANA(regC);
                        break;

                    case "A2":
                        ANA(regD);
                        break;

                    case "A3":
                        ANA(regE);
                        break;

                    case "A4":
                        ANA(regH);
                        break;

                    case "A5":
                        ANA(regL);
                        break;

                    case "A6":
                        M = regL;
                        M |= (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        ANA(temp);
                        break;

                    case "E6":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        ANA(temp);
                        break;

                    case "CD":
                        currentByte = callFromMem(currentByte);
                        break;

                    case "DC":  // call on carry
                        if (checkCarry())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "FC":  // call on minus
                        if (checkSignPositive())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "2F":
                        CMA();
                        break;

                    case "3F": // CMC
                        if (checkCarry())
                        {
                            resetCarry();
                        }
                        else
                        {
                            setCarry();
                        }
                        break;

                    case "BF":
                        CMP(regA);
                        break;

                    case "B8":
                        CMP(regB);
                        break;

                    case "B9":
                        CMP(regC);
                        break;

                    case "BA":
                        CMP(regD);
                        break;

                    case "BB":
                        CMP(regE);
                        break;

                    case "BC":
                        CMP(regH);
                        break;

                    case "BD":
                        CMP(regL);
                        break;

                    case "BE":
                        M = regL;
                        M |= (regH << 8);
                        CMP(Convert.ToInt16(memory[M], 16));
                        break;

                    case "D4": // Call no Carry
                        if ((flags & (1 << CARRY)) == 0)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "C4": // Call no Zero
                        if (!checkZero())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "F4":
                        if (checkSignPositive())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "EC": // call on parity 1
                        if (checkParityEven())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "E4": //call on parity 0
                        if (!checkParityEven())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "FE": // compare imediate
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        CMP(temp);
                        break;

                    case "CC": // jump on zero
                        if (checkZero())
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "09": // Add BC to HL
                        DAD(regB, regC);
                        break;

                    case "19": // Add DE to HL
                        DAD(regD, regE);
                        break;

                    case "29": // Add HL to HL
                        DAD(regH, regL);
                        break;

                    case "39": // Add SP to HL
                        DAD((stackPointer & 0xFF00), (stackPointer & 0xFF));
                        break;

                    case "3D": // decrement A
                        DCR(ref regA);
                        break;

                    case "05": // decrement B
                        DCR(ref regB);
                        break;

                    case "0D": // decrement C
                        DCR(ref regC);
                        break;

                    case "15": // decrement D
                        DCR(ref regD);
                        break;

                    case "1D": // decrement E
                        DCR(ref regE);
                        break;

                    case "25": // decrement H
                        DCR(ref regH);
                        break;

                    case "2D": // decrement L
                        DCR(ref regL);
                        break;

                    case "35": // decrement M
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        DCR(ref temp);
                        memory[M] = temp.ToString("X2");
                        break;

                    case "0B":
                        DCX(ref regB, ref regC);
                        break;

                    case "1B":
                        DCX(ref regD, ref regE);
                        break;

                    case "2B":
                        DCX(ref regH, ref regL);
                        break;

                    case "3B": // DCX SP
                        stackPointer--;
                        if (stackPointer < 0)
                        {
                            stackPointer = 0xFFFF;
                        }
                        break;

                    case "76": //HLT
                        halted = true;
                        break;

                    case "DB": // IN port 12
                        currentByte++;
                        if (Convert.ToInt16(memory[currentByte], 16) == 0x12)
                        {
                            regA = inputPort;
                        }
                        break;

                    case "3C": //Increment A
                        INR(ref regA);
                        break;

                    case "04"://Increment B
                        INR(ref regB);
                        break;

                    case "0C"://Increment C
                        INR(ref regC);
                        break;

                    case "14"://Increment D
                        INR(ref regD);
                        break;

                    case "1C"://Increment E
                        INR(ref regE);
                        break;

                    case "24"://Increment H
                        INR(ref regH);
                        break;

                    case "2C"://Increment L
                        INR(ref regL);
                        break;

                    case "34"://Increment M
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        INR(ref temp);
                        memory[M] = temp.ToString("X2");
                        break;

                    case "03": //increment BC
                        INX(ref regB, ref regC);
                        break;

                    case "13": //increment DE
                        INX(ref regD, ref regE);
                        break;

                    case "23": //increment HL
                        INX(ref regH, ref regL);
                        break;

                    case "33": //increment SP
                        stackPointer++;
                        if (stackPointer >= 0x10000)
                        {
                            stackPointer = 0;
                        }
                        break;

                    case "DA":
                        if (checkCarry())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "FA":
                        if (!checkSignPositive())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "C3":
                        currentByte = jumpFromMemory(currentByte);
                        break;

                    case "D2":
                        if (!checkCarry())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "C2":
                        if (!checkZero())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "F2":
                        if (checkSignPositive())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "EA":
                        if (checkParityEven())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "E2":
                        if (checkParityEven())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "CA":
                        if (checkZero())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "3A": // Load A from Memory
                        currentByte++;
                        M = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        M += (Convert.ToInt16(memory[currentByte], 16) << 8);
                        regA = Convert.ToInt16(memory[M], 16);
                        break;

                    case "0A": // Load A from Memory (location specified by BC)
                        M = regC;
                        M += (regB << 8);
                        regA = Convert.ToInt16(memory[M], 16);
                        break;

                    case "1A": // Load A from Memory (location specified by DE)
                        M = regE;
                        M += (regD << 8);
                        regA = Convert.ToInt16(memory[M], 16);
                        break;

                    case "2A": // Load H and L from Memory (location specified by memory)
                        currentByte++;
                        M = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        M += (Convert.ToInt16(memory[currentByte], 16) << 8);
                        regL = Convert.ToInt16(memory[M], 16);
                        regH = Convert.ToInt16(memory[M + 1], 16);
                        break;

                    case "01": // Load BC from memory
                        currentByte++;
                        regC = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        regB = (Convert.ToInt16(memory[currentByte], 16));
                        break;

                    case "11": // Load DE from memory
                        currentByte++;
                        regE = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        regD = (Convert.ToInt16(memory[currentByte], 16));
                        break;

                    case "21": // Load HL from memory
                        currentByte++;
                        regL = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        regH = (Convert.ToInt16(memory[currentByte], 16));
                        break;

                    case "31": // Load stackpointer from memory
                        currentByte++;
                        stackPointer = Convert.ToUInt16(memory[currentByte], 16);
                        currentByte++;
                        stackPointer += (UInt16)(Convert.ToUInt16(memory[currentByte], 16) << 8);
                        break;

                    case "78": // Move B to A
                        MOV(ref regB, ref regA);
                        break;

                    case "79": // Move C to A
                        MOV(ref regC, ref regA);
                        break;

                    case "7A": // Move D to A
                        MOV(ref regD, ref regA);
                        break;

                    case "7B": // Move E to A
                        MOV(ref regE, ref regA);
                        break;

                    case "7C": // Move H to A
                        MOV(ref regH, ref regA);
                        break;

                    case "7D": // Move L to A
                        MOV(ref regL, ref regA);
                        break;

                    case "7E": // Move M to A
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regA);
                        break;

                    case "47": // Move A to B
                        MOV(ref regA, ref regB);
                        break;

                    case "41": // Move C to B
                        MOV(ref regC, ref regB);
                        break;

                    case "42": // Move D to B
                        MOV(ref regD, ref regB);
                        break;

                    case "43": // Move E to B
                        MOV(ref regE, ref regB);
                        break;

                    case "44": // Move H to B
                        MOV(ref regH, ref regB);
                        break;

                    case "45": // Move L to B
                        MOV(ref regL, ref regB);
                        break;

                    case "46": // Move M to B
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regB);
                        break;

                    case "4F": // Move A to C
                        MOV(ref regA, ref regC);
                        break;

                    case "48": // Move B to C
                        MOV(ref regB, ref regC);
                        break;

                    case "4A": // Move D to C
                        MOV(ref regD, ref regC);
                        break;

                    case "4B": // Move E to C
                        MOV(ref regE, ref regC);
                        break;

                    case "4C": // Move H to C
                        MOV(ref regH, ref regC);
                        break;

                    case "4D": // Move L to C
                        MOV(ref regL, ref regC);
                        break;

                    case "4E": // Move M to C
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regC);
                        break;

                    case "57": // Move A
                        MOV(ref regA, ref regD);
                        break;

                    case "50": // Move B 
                        MOV(ref regB, ref regD);
                        break;

                    case "51": // Move C
                        MOV(ref regC, ref regD);
                        break;

                    case "53": // Move E
                        MOV(ref regE, ref regD);
                        break;

                    case "54": // Move H
                        MOV(ref regH, ref regD);
                        break;

                    case "55": // Move L
                        MOV(ref regL, ref regD);
                        break;

                    case "56": // Move M
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regD);
                        break;

                    case "D3":
                        currentByte++;
                        if (memory[currentByte] == "11")
                        {
                            out11(regA);
                        }
                        break;

                    case "5F":
                        MOV(ref regA, ref regE);
                        break;

                    case "58":
                        MOV(ref regB, ref regE);
                        break;

                    case "59":
                        MOV(ref regC, ref regE);
                        break;

                    case "5A":
                        MOV(ref regD, ref regE);
                        break;

                    case "5C":
                        MOV(ref regE, ref regE);
                        break;

                    case "5D":
                        MOV(ref regH, ref regE);
                        break;

                    case "5E":
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regE);
                        break;

                    case "67":
                        MOV(ref regA, ref regH);
                        break;

                    case "60":
                        MOV(ref regB, ref regH);
                        break;

                    case "61":
                        MOV(ref regC, ref regH);
                        break;

                    case "62":
                        MOV(ref regD, ref regH);
                        break;

                    case "63":
                        MOV(ref regE, ref regH);
                        break;

                    case "65":
                        MOV(ref regL, ref regH);
                        break;

                    case "66":
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regH);
                        break;

                    case "6F":
                        MOV(ref regA, ref regL);
                        break;

                    case "68":
                        MOV(ref regB, ref regL);
                        break;

                    case "69":
                        MOV(ref regC, ref regL);
                        break;

                    case "6A":
                        MOV(ref regD, ref regL);
                        break;

                    case "6B":
                        MOV(ref regE, ref regL);
                        break;

                    case "6C":
                        MOV(ref regH, ref regL);
                        break;

                    case "6E":
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        MOV(ref temp, ref regL);
                        break;

                    case "77":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regA.ToString("X2");
                        break;

                    case "70":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regB.ToString("X2");
                        break;

                    case "71":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regC.ToString("X2");
                        break;


                    case "72":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regD.ToString("X2");
                        break;

                    case "73":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regE.ToString("X2");
                        break;

                    case "74":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regH.ToString("X2");
                        break;

                    case "75":
                        M = regL;
                        M += (regH << 8);
                        memory[M] = regL.ToString("X2");
                        break;

                    case "3E":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regA);
                        break;

                    case "06":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regB);
                        break;

                    case "0E":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regC);
                        break;

                    case "16":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regD);
                        break;

                    case "1E":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regE);
                        break;

                    case "26":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regH);
                        break;

                    case "2E":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        MOV(ref temp, ref regL);
                        break;

                    case "36":
                        currentByte++;
                        M = regL;
                        M += (regH << 8);
                        memory[M] = memory[currentByte];
                        break;

                    case "B7": // ORA A
                        regA |= regA;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B0":
                        regA |= regB;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B1":
                        regA |= regC;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B2":
                        regA |= regD;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B3":
                        regA |= regE;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B4":
                        regA |= regH;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B5":
                        regA |= regL;
                        setFlagsZPSForReg(regA);
                        break;

                    case "B6":// ORA M
                        M = regL;
                        M += (regH << 8);
                        regA |= Convert.ToInt32(memory[M], 16);
                        setFlagsZPSForReg(regA);
                        break;

                    case "F6": // ORI Data
                        currentByte++;
                        regA |= Convert.ToInt32(memory[currentByte], 16);
                        setFlagsZPSForReg(regA);
                        break;

                    case "E9":
                        currentByte = regL;
                        currentByte += (regH << 8);
                        break;

                    case "C1":
                        regC = pop();
                        regB = pop();
                        break;

                    case "D1":
                        regE = pop();
                        regD = pop();
                        break;

                    case "E1":
                        regL = pop();
                        regH = pop();
                        break;

                    case "F1":
                        flags = pop();
                        regA = pop();
                        break;

                    case "C5":
                        push(regB);
                        push(regC);
                        break;

                    case "D5":
                        push(regD);
                        push(regE);
                        break;

                    case "E5":
                        push(regH);
                        push(regL);
                        break;

                    case "F5":
                        push(regA);
                        push(flags);
                        break;

                    case "17":
                        temp = (checkCarry_Int());
                        if ((regA & 0x80) != 0)
                        {
                            setCarry();
                        }
                        else
                        {
                            resetCarry();
                        }
                        regA = ((regA << 1) & 0xFF) + temp;
                        break;

                    case "1F":
                        temp = regA + (checkCarry_Int() << 8);
                        if ((regA & 0x01) != 0)
                        {
                            setCarry();
                        }
                        else
                        {
                            resetCarry();
                        }
                        regA = ((temp >> 1) & 0xFF);
                        break;

                    case "D8": // Return on Carry
                        if (checkCarry())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "C9": // Return
                        currentByte = returnFromCall() - 1;
                        break;

                    case "07": //Rotate left and set carry if D7 = 1
                        regA = ((regA << 1) | ((regA & 0x80) >> 7)) & 0xFF;
                        if ((regA & 0x01) == 1)
                        {
                            setCarry();
                        }
                        else
                        {
                            resetCarry();
                        }
                        //noFlagUpdates = 1;
                        break;

                    case "F8": // Return on Minus
                        if (checkSignPositive())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "D0": // Return on No Carry
                        if (!checkCarry())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "C0": // Return on NZ
                        if (!checkZero())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "F0": // Return on Positive
                        if (checkSignPositive())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "E8": // Return on PE
                        if (checkParityEven())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "E0": // Return on PO
                        if (!checkParityEven())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "0F": //Rotate right and set carry if D0 = 1
                        temp = regA;
                        regA = ((temp & 0xFE) >> 1);
                        regA |= ((temp & 0x01) << 7);
                        if ((regA & 0x01) == 1)
                        {
                            setCarry();
                        }
                        else
                        {
                            resetCarry();
                        }
                        //noFlagUpdates = 1;
                        break;

                    case "C8": // Return on NZ
                        if (checkZero())
                        {
                            currentByte = returnFromCall() - 1; // corrects for byte increment
                        }
                        break;

                    case "9F": // subtract with borrow
                        regA += ((~regA - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regA & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "98": // subtract with borrow
                        regA += ((~regB - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regB & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "99": // subtract with borrow
                        regA += ((~regC - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regC & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "9A": // subtract with borrow
                        regA += ((~regD - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regD & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "9B": // subtract with borrow
                        regA += ((~regE - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regE & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "9C": // subtract with borrow
                        regA += ((~regH - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regH & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "9D": // subtract with borrow
                        regA += ((~regL - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (regL & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "9E": // subtract with borrow
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        regA += ((~temp - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (temp & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "DE": // subtract with borrow
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        regA += ((~temp - checkCarry_Int()) & 0x1FF);
                        setAuxCarry(((regA & 0xF) + (temp & 0xF)) >> 4);
                        setFlagsZPSForReg(regA);
                        break;

                    case "22":
                        currentByte++;
                        M = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        M += (Convert.ToInt16(memory[currentByte], 16) << 8);
                        memory[M] = regL.ToString("X2");
                        memory[M + 1] = regH.ToString("X2");
                        break;

                    case "F9":
                        stackPointer = regL;
                        stackPointer += (regH << 8);
                        break;

                    case "32":
                        currentByte++;
                        M = Convert.ToInt16(memory[currentByte], 16);
                        currentByte++;
                        M += (Convert.ToInt16(memory[currentByte], 16) << 8);
                        memory[M] = regA.ToString("X2");
                        break;

                    case "02":
                        M = regC;
                        M += regB;
                        memory[M] = regA.ToString("X2");
                        break;

                    case "12":
                        M = regE;
                        M += regD;
                        memory[M] = regA.ToString("X2");
                        break;

                    case "37":
                        setCarry();
                        //noFlagUpdates = 1;
                        break;

                    case "97": // Sub no borrow
                        regA += ((~regA + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "90": // Sub no borrow
                        regA += ((~regB + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "91": // Sub no borrow
                        regA += ((~regC + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "92": // Sub no borrow
                        regA += ((~regD + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "93": // Sub no borrow
                        regA += ((~regE + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "94": // Sub no borrow
                        regA += ((~regH + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "95": // Sub no borrow
                        regA += ((~regL + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "96": // Sub no borrow
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        regA += ((~temp + 1) & 0x1FF);
                        setFlagsZPSForReg(regA);
                        break;

                    case "D6":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        regA += ((~temp + 1) & 0x1FF);
                        break;

                    case "EB": // exchange DE with HL
                        temp = regL;
                        regL = regE;
                        regE = temp;
                        temp = regD;
                        regD = regB;
                        regB = temp;
                        break;

                    case "AF":
                        XRA(regA);
                        break;

                    case "A8":
                        XRA(regB);
                        break;

                    case "A9":
                        XRA(regC);
                        break;

                    case "AA":
                        XRA(regD);
                        break;

                    case "AB":
                        XRA(regE);
                        break;

                    case "AC":
                        XRA(regH);
                        break;

                    case "AD":
                        XRA(regL);
                        break;

                    case "AE":
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        XRA(temp);
                        break;

                    case "EE":
                        currentByte++;
                        temp = Convert.ToInt16(memory[M], 16);
                        XRA(temp);
                        break;

                    case "E3":
                        temp = regH;
                        regH = Convert.ToInt16(memory[stackPointer + 1], 16);
                        memory[stackPointer + 1] = temp.ToString("X2");
                        temp = regL;
                        regL = Convert.ToInt16(memory[stackPointer], 16);
                        memory[stackPointer] = temp.ToString("X2");
                        break;

                    default:
                        break;

                }


                currentByte++;
            }
        }

        private void adjustCarry()
        {
            if (regA > 0xFF)
            {
                setCarry();
                regA -= 0x100;
            }
            else
            {
                resetCarry();
            }
        }

        private void ADC(int regX)
        {
            setAuxCarry((regA & 0xF) + (regX & 0xF) + checkCarry_Int());
            regA += regX + checkCarry_Int();
            setFlagsZPSForReg(regA);
        }

        private void ADD(int regX)
        {
            setAuxCarry(((regA & 0xF) + (regX & 0xF)) >> 4);
            regA += regX;
            if (regA >= 0x100)
            {
                regA -= 0x100;
            }
            setFlagsZPSForReg(regA);
        }

        private void ANA(int regX)
        {
            regA &= regX;
            resetCarry();
            setAuxCarry(1);
            setFlagsZPSForReg(regA);
        }

        private void CMA()
        {
            regA = (~regA & 0xFF);
        }

        private void DAD(int regxH, int regxL) 
        {
            int priv_temp = regL;
            priv_temp += (regH << 8);
            priv_temp += (regC);
            priv_temp += (regB << 8);

            if (priv_temp > 0xFFFF)
            {
                setCarry();
                priv_temp -= 0X10000;
            }

            regH = ((priv_temp & 0xFF00) >> 8);
            regL = (priv_temp & 0x00FF);
        }

        private void DCR(ref int regX)
        {
            regX--;
            setFlagsZPSForReg(regX);
        }

        private void DCX(ref int regxH, ref int regxL)
        {
            short priv_temp = (short) regxL;
            priv_temp += (short)(regxH << 8);
            priv_temp--;
            regxL = priv_temp & 0xFF;
            regxH = (priv_temp & 0xFF00) >> 8;
        }

        private void INR(ref int regX)
        {
            regX = regX + 1;
            if (regX == 0x100)
            {
                regX -= 0x100;
            }
            setFlagsZPSForReg(regX);
        }

        private void INX(ref int regxH, ref int regxL)
        {
            int priv_temp = regxL;
            priv_temp += (regxH << 8);
            priv_temp++;
            if (priv_temp >= 0x10000)
            {
                priv_temp -= 0x10000;
            }
            regxL = (priv_temp & 0xFF);
            regxH = ((priv_temp & 0xFF00) >> 8);
        }

        private void MOV(ref int regFrom, ref int regTo)
        {
            regTo = regFrom;
        }

        private void XRA(int regX)
        {
            regA ^= regX;
            setFlagsZPSForReg(regX);
        }

        private void push(int byteToPush)
        {
            stackPointer--;
            memory[stackPointer] = byteToPush.ToString("X2");            
        }

        private int pop()
        {
            int value = Convert.ToInt16(memory[stackPointer], 16);
            stackPointer++;
            if (stackPointer > 0xFFFF)
            {
                halted = true;
                stackPointer = 0xFFFF;
            }
            return value;
        }

        private void setCarry()
        {
            flags |= (1 << CARRY);
        }

        private void resetCarry()
        {
            flags &= ~(1 << CARRY);
        }

        private void setZeroFlag()
        {
            flags |= (1 << ZERO);
        }

        private void resetZeroFlag()
        {
            flags &= ~(1 << ZERO);
        }

        private void setAuxCarry(int AC)
        {
            if (AC == 1)
            {
                flags |= (1 << AUX_CARRY);
            }
            else
            {
                flags &= ~(1 << AUX_CARRY);
            }
        }

        private void setSignFlag()
        {
            flags |= (1 << SIGN);
        }

        private void resetSignFlag()
        {
            flags &= ~(1 << SIGN);
        }

        private void setParityFlag()
        {
            flags |= (1 << PARITY);
        }

        private void resetParityFlag()
        {
            flags &= ~(1 << PARITY);
        }

        private void CMP(int regX)
        {
            if (regA < regX)
            { 
                setCarry();
                resetZeroFlag();
            }
            else if (regA == regX) 
            {
                setZeroFlag();
                resetCarry();
            }
            else
            {
                resetZeroFlag();
                resetCarry();
            }
        }

        private int jumpFromMemory(int mCurrentByte)
        {
            int M;
            mCurrentByte++;
            M = Convert.ToInt16(memory[mCurrentByte], 16);
            mCurrentByte++;
            M |= (Convert.ToInt16(memory[mCurrentByte], 16) << 8);
            return mCurrentByte = M - 1; // -1 corrects for increment at the end of loop
        }

        private int callFromMem(int mCurrentByte)
        {
            int M;
            mCurrentByte++;
            M = Convert.ToInt16(memory[mCurrentByte], 16);
            mCurrentByte++;
            M |= (Convert.ToInt16(memory[mCurrentByte], 16) << 8);
            mCurrentByte++;
            push(((mCurrentByte & 0xFF00) >> 8));
            push((mCurrentByte & 0xFF));
            return mCurrentByte = M - programStartAddr - 1; // -1 corrects for increment at the end of loop
        }

        private int returnFromCall()
        {
            int mCurrentByte = pop();
            mCurrentByte += (pop() << 8);
            return mCurrentByte;
        }

        private void setFlagsZPSForReg(int regX)
        {
            if (regX == 0)
            {
                setZeroFlag();
            }
            else
            {
                resetZeroFlag();
            }
            if (hasEvenParity(regX))
            {
                setParityFlag();
            }
            else
            {
                resetParityFlag();
            }
            if (regX < 0)
            {
                setSignFlag();
            }
            else
            {
                resetSignFlag();
            }
            
        }

        private bool hasEvenParity(int regX)
        {
            int count = 0;
            for (int i = 0; i < 32; i++)
            {
                if ((regX & (1<<i)) != 0)
                {
                    count++;
                }
            }
            if (count % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool checkCarry()
        {
            return ((flags & (1 << CARRY)) != 0);
        }

        private int checkCarry_Int()
        {
            return (flags & (1 << CARRY));
        }

        private bool checkSignPositive()
        {
            return ((flags & (1 << SIGN)) == 0);
        }

        private bool checkZero()
        {
            return ((flags & (1 << ZERO)) != 0);
        }

        private bool checkParityEven()
        {
            return ((flags & (1 << PARITY)) != 0);
        }

        private void updateRegUI()
        {
            sbyte regAc = (sbyte)regA;
            sbyte regBc = (sbyte)regB;
            sbyte regCc = (sbyte)regC;
            sbyte regDc = (sbyte)regD;
            sbyte regEc = (sbyte)regE;
            sbyte regHc = (sbyte)regH;
            sbyte regLc = (sbyte)regL;
            
            regATextbox.Text = regAc.ToString("X2") + "h";
            regBTextbox.Text = regBc.ToString("X2") + "h";
            regCTextbox.Text = regCc.ToString("X2") + "h";
            regDTextbox.Text = regDc.ToString("X2") + "h";
            regETextbox.Text = regEc.ToString("X2") + "h";
            regHTextbox.Text = regHc.ToString("X2") + "h";
            regLTextbox.Text = regLc.ToString("X2") + "h";
            flagsTextbox.Text = Convert.ToString(flags, 2).PadLeft(8,'0');
            haltedCheckbox.Checked = halted;
            pcTextbox.Text = currentByte.ToString("X4") + "h";
        }

        private void updateStackUI()
        {
            while (0xFFFF - stackPointer < StackListBox.Items.Count)
            {
                StackListBox.Items.RemoveAt(StackListBox.Items.Count-1);
            }
            while (0xFFFF - stackPointer > StackListBox.Items.Count)
            {
                if (memory[0xFFFE - StackListBox.Items.Count] == null)
                {
                    memory[0xFFFE - StackListBox.Items.Count] = "00";
                }
                StackListBox.Items.Insert(0,"0x" + (0xFFFE - StackListBox.Items.Count).ToString("X4") + "   0x" + memory[0xFFFE - StackListBox.Items.Count]);
            }
        }

        private void updateCompletedAssemblyUI()
        {

        }

        private void out11(int regX)
        {
            outputPort = regX;
        }

        private void updateIO_UI(int out11, int in12)
        {
            outP0.Checked = ((out11 & 0X01) != 0);
            outP1.Checked = ((out11 & 0X02) != 0);
            outP2.Checked = ((out11 & 0X04) != 0);
            outP3.Checked = ((out11 & 0X08) != 0);
            outP4.Checked = ((out11 & 0X10) != 0);
            outP5.Checked = ((out11 & 0X20) != 0);
            outP6.Checked = ((out11 & 0X40) != 0);
            outP7.Checked = ((out11 & 0X80) != 0);

            inputPort = Convert.ToInt32(in0.Checked);
            inputPort |= Convert.ToInt32(in1.Checked) << 1;
            inputPort |= Convert.ToInt32(in2.Checked) << 2;
            inputPort |= Convert.ToInt32(in3.Checked) << 3;
            inputPort |= Convert.ToInt32(in4.Checked) << 4;
            inputPort |= Convert.ToInt32(in5.Checked) << 5;
            inputPort |= Convert.ToInt32(in6.Checked) << 6;
            inputPort |= Convert.ToInt32(in7.Checked) << 7;

        }

        private void speedScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            speed = speedScrollbar.Value;
        }

        private void loadHexButton_Click(object sender, EventArgs e)
        {
            loadHex();
        }

        private void assembleButton_Click(object sender, EventArgs e)
        {
            assemble();
        }

        
    }

}

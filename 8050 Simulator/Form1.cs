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
        string[] programMemory = { "3C", "3C", "D3", "11","C3", "00", "00", "76"};
        string[] dataMemory = { "0A", "0A" };
        List<int> stack = new List<int>();
        UInt16 dataMemoryStartAddr = 0x0F00;
        UInt16 programStartAddr = 0x0000;
        UInt16 stackPointer = 0xFFFF;
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
        }

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < programMemory.Length; i++)
            {
                memory[programStartAddr + i] = programMemory[i];
            }
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
                    Thread.Sleep(200);
                }
            }
            worker.ReportProgress(0);
        }

        private void runWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            updateRegUI();
            updateOut_UI(outputPort);  
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
            updateOut_UI(outputPort);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            runWorker.CancelAsync();
            currentByte = 0;
            initializeRegToZero();
            halted = true;
            updateRegUI();
            updateOut_UI(outputPort);
        }

        private void assemble()
        {
        
        }

        private void stepForward()
        {
            int noFlagUpdates = 0;
            int M = 0;
            int temp = 0;
            
            

            
            {
                switch (memory[currentByte])
                {
                    case "CE":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        setAuxCarry(((regA & 0xF) + (temp & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += temp + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "8F":
                        setAuxCarry(((regA & 0xF) + (regA & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regA + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;
                   
                    case "88":
                        setAuxCarry(((regA & 0xF) + (regB & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regB + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "89":
                        setAuxCarry(((regA & 0xF) + (regC & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regC + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "8A":
                        setAuxCarry(((regA & 0xF) + (regD & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regD + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "8B":
                        setAuxCarry(((regA & 0xF) + (regD & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regD + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "8C":
                        setAuxCarry(((regA & 0xF) + (regH & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regH + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "8D":
                        setAuxCarry(((regA & 0xF) + (regL & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += regL + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "8E":
                        M = regL;
                        M |= (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        setAuxCarry(((regA & 0xF) + (temp & 0xF) + (flags & (1 << CARRY))) >> 4);
                        regA += temp + (flags & (1 << CARRY) >> CARRY);
                        setFlagsZPSForReg(regA);
                        break;

                    case "87":
                        setAuxCarry(((regA & 0xF) + (regA & 0xF)) >> 4);
                        regA += regA;
                        setFlagsZPSForReg(regA);
                        break;

                    case "80":
                        setAuxCarry(((regA & 0xF) + (regB & 0xF)) >> 4);
                        regA += regB;
                        setFlagsZPSForReg(regA);
                        break;

                    case "81":
                        setAuxCarry(((regA & 0xF) + (regC & 0xF)) >> 4);
                        regA += regC;
                        setFlagsZPSForReg(regA);
                        break;

                    case "82":
                        setAuxCarry(((regA & 0xF) + (regD & 0xF)) >> 4);
                        regA += regD;
                        setFlagsZPSForReg(regA);
                        break;

                    case "83":
                        setAuxCarry(((regA & 0xF) + (regE & 0xF)) >> 4);
                        regA += regE;
                        setFlagsZPSForReg(regA);
                        break;

                    case "84":
                        setAuxCarry(((regA & 0xF) + (regH & 0xF)) >> 4);
                        regA += regH;
                        setFlagsZPSForReg(regA);
                        break;
                    case "85":
                        setAuxCarry(((regA & 0xF) + (regL & 0xF)) >> 4);
                        regA += regL;
                        setFlagsZPSForReg(regA);
                        break;

                    case "86":
                        M = regL;
                        M |= (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        setAuxCarry(((regA & 0xF) + (temp & 0xF)) >> 4);
                        regA += temp;
                        setFlagsZPSForReg(regA);
                        break;

                    case "C6":
                        currentByte++;
                        temp = Convert.ToInt16(memory[currentByte], 16);
                        setAuxCarry(((regA & 0xF) + (temp & 0xF)) >> 4);
                        regA += temp;
                        setFlagsZPSForReg(regA);
                        break;

                    case "A7":
                        regA &= regA;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A0":
                        regA &= regB;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A1":
                        regA &= regC;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A2":
                        regA &= regD;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A3":
                        regA &= regE;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A4":
                        regA &= regH;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A5":
                        regA &= regL;
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "A6":
                        M = regL;
                        M |= (regH<<8);
                        regA &= Convert.ToInt16(memory[M], 16);
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "E6":
                        currentByte++;
                        regA &= Convert.ToInt16(memory[currentByte], 16);
                        resetCarry();
                        setAuxCarry(1);
                        setFlagsZPSForReg(regA);
                        break;

                    case "CD":
                        currentByte = callFromMem(currentByte);
                        break;

                    case "DC":  // call on carry
                        if ((flags & (1 << CARRY)) == 1)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "FC":  // call on minus
                        if ((flags & (1 << SIGN)) == 1)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "2F":
                        regA = ~regA;
                        noFlagUpdates = 1;
                        break;

                    case "3F":
                        if ((flags & (1 << CARRY)) == 0)
                        {
                            setCarry();
                        }
                        else
                        {
                            resetCarry();
                        }
                        break;

                    case "BF":
                        compareToA(regA);
                        noFlagUpdates = 1;
                        break;

                    case "B8":
                        compareToA(regB);
                        noFlagUpdates = 1;
                        break;

                    case "B9":
                        compareToA(regC);
                        noFlagUpdates = 1;
                        break;

                    case "BA":
                        compareToA(regD);
                        noFlagUpdates = 1;
                        break;

                    case "BB":
                        compareToA(regE);
                        noFlagUpdates = 1;
                        break;

                    case "BC":
                        compareToA(regH);
                        noFlagUpdates = 1;
                        break;

                    case "BD":
                        compareToA(regL);
                        noFlagUpdates = 1;
                        break;

                    case "BE":
                        M = regL;
                        M |= (regH << 8);
                        compareToA(Convert.ToInt16(memory[M], 16));
                        noFlagUpdates = 1;
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
                        if ((flags & (1 << ZERO)) == 0)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;
                
                    case "F4":
                        if ((flags & (1 << SIGN)) != 0)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "EC": // call on parity 1
                        if ((flags & (1 << PARITY)) != 0)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "E4": //call on parity 0
                        if ((flags & (1 << PARITY)) == 0)
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
                        temp = Convert.ToInt16(memory[currentByte],16);
                        compareToA(temp);
                        noFlagUpdates = 1;
                        break;

                    case "CC": // jump on zero
                        if ((flags & (1 << ZERO)) != 0)
                        {
                            currentByte = callFromMem(currentByte);
                        }
                        else
                        {
                            currentByte += 2;
                        }
                        break;

                    case "09": // Add BC to HL
                        temp = regL;
                        temp += (regH << 8);
                        temp += (regC);
                        temp += (regB << 8);

                        if (temp > 0xFFFF)
                        {
                            setCarry();
                            temp -= 0X10000;
                        }

                        regH = ((temp & 0xFF00) >> 8);
                        regL = (temp & 0x00FF);
                        break;

                    case "19": // Add DE to HL
                        temp = regL;
                        temp += (regH << 8);
                        temp += (regE);
                        temp += (regD << 8);

                        if (temp > 0xFFFF)
                        {
                            setCarry();
                            temp -= 0X10000;
                        }

                        regH = ((temp & 0xFF00) >> 8);
                        regL = (temp & 0x00FF);
                        break;

                    case "29": // Add HL to HL
                        temp = regL;
                        temp += (regH << 8);
                        temp += (regL);
                        temp += (regH << 8);

                        if (temp > 0xFFFF)
                        {
                            setCarry();
                            temp -= 0X10000;
                        }

                        regH = ((temp & 0xFF00) >> 8);
                        regL = (temp & 0x00FF);
                        break;

                    case "39": // Add SP to HL
                        temp = regL;
                        temp += (regH << 8);
                        temp += stackPointer;

                        if (temp > 0xFFFF)
                        {
                            setCarry();
                            temp -= 0X10000;
                        }

                        regH = ((temp & 0xFF00) >> 8);
                        regL = (temp & 0x00FF);
                        break;

                    case "3D": // decrement A
                        regA--;
                        setFlagsZPSForReg(regA);
                        break;

                    case "05": // decrement B
                        regB--;
                        setFlagsZPSForReg(regB);
                        break;

                    case "0D": // decrement C
                        regC--;
                        setFlagsZPSForReg(regC);
                        break;

                    case "15": // decrement D
                        regD--;
                        setFlagsZPSForReg(regD);
                        break;

                    case "1D": // decrement E
                        regE--;
                        setFlagsZPSForReg(regE);
                        break;

                    case "25": // decrement H
                        regH--;
                        setFlagsZPSForReg(regH);
                        break;

                    case "2D": // decrement L
                        regL--;
                        setFlagsZPSForReg(regL);
                        break;

                    case "35": // decrement M
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        temp--;
                        memory[M] = temp.ToString("X2");
                        setFlagsZPSForReg(temp);
                        break;

                    case "0B":
                        temp = regC;
                        temp += (regB << 8);
                        temp--;
                        if (temp < 0)
                        {
                            temp = 0xFFFF;
                        }
                        regC = (temp & 0xFF);
                        regB = ((temp & 0XFF00) >> 8);
                        break;

                    case "1B":
                        temp = regE;
                        temp += (regD << 8);
                        temp--;
                        if (temp < 0)
                        {
                            temp = 0xFFFF;
                        }
                        regE = (temp & 0xFF);
                        regD = ((temp & 0XFF00) >> 8);
                        break;

                    case "2B":
                        temp = regL;
                        temp += (regH << 8);
                        temp--;
                        if (temp < 0)
                        {
                            temp = 0xFFFF;
                        }
                        regL = (temp & 0xFF);
                        regH = ((temp & 0XFF00) >> 8);
                        break;

                    case "3B":
                        stackPointer--;
                        if (stackPointer < 0)
                        {
                            stackPointer = 0xFFFF;
                        }
                        break;

                    case "76":
                        halted = true;
                        break;

                    case "DB":
                        currentByte++;
                        if (Convert.ToInt16(memory[currentByte], 16) == 0x12)
                        {
                            regA = inputPort;
                        }
                        break;

                    case "3C": //Increment A
                        regA++;
                        if (regA > 0xFF)
                        {
                            regA -= 0X100;
                        }
                        setFlagsZPSForReg(regA);
                        break;

                    case "04"://Increment B
                        regB++;
                        if (regB > 0xFF)
                        {
                            regB -= 0X100;
                        }
                        setFlagsZPSForReg(regB);
                        break;

                    case "0C"://Increment C
                        regC++;
                        if (regC > 0xFF)
                        {
                            regC -= 0X100;
                        }
                        setFlagsZPSForReg(regC);
                        break;

                    case "14"://Increment D
                        regD++;
                        if (regD > 0xFF)
                        {
                            regD -= 0X100;
                        }
                        setFlagsZPSForReg(regD);
                        break;

                    case "1C"://Increment E
                        regE++;
                        if (regE > 0xFF)
                        {
                            regE -= 0X100;
                        }
                        setFlagsZPSForReg(regE);
                        break;

                    case "24"://Increment H
                        regH++;
                        if (regH > 0xFF)
                        {
                            regH -= 0X100;
                        }
                        setFlagsZPSForReg(regH);
                        break;

                    case "2C"://Increment L
                        regL++;
                        if (regL > 0xFF)
                        {
                            regL -= 0X100;
                        }
                        setFlagsZPSForReg(regL);
                        break;

                    case "34"://Increment M
                        M = regL;
                        M += (regH << 8);
                        temp = Convert.ToInt16(memory[M], 16);
                        temp++;
                        if (temp > 0xFF)
                        {
                            temp -= 0X100;
                        }
                        memory[M] = temp.ToString("X2");
                        setFlagsZPSForReg(regA);
                        break;
                        
                    case "03": //increment BC
                        temp = regC;
                        temp += (regB << 8);
                        temp++;
                        if (temp > 0xFFFF)
                        {
                            temp -= 0x10000;
                        }
                        regC = (temp & 0xFF);
                        regB = ((temp & 0xFF00) >> 8);
                        break;

                    case "13": //increment DE
                        temp = regE;
                        temp += (regD << 8);
                        temp++;
                        if (temp > 0xFFFF)
                        {
                            temp -= 0x10000;
                        }
                        regE = (temp & 0xFF);
                        regD = ((temp & 0xFF00) >> 8);
                        break;

                    case "23": //increment HL
                        temp = regL;
                        temp += (regH << 8);
                        temp++;
                        if (temp > 0xFFFF)
                        {
                            temp -= 0x10000;
                        }
                        regL = (temp & 0xFF);
                        regH = ((temp & 0xFF00) >> 8);
                        break;

                    case "33": //increment SP
                        stackPointer++;
                        if (stackPointer > 0xFFFF)
                        {
                            stackPointer =0;
                        }
                        break;

                    case "DA":
                        if (checkCarry())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        break;

                    case "FA":
                        if (!checkSignPositive())
                        {
                            currentByte = jumpFromMemory(currentByte);
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
                        break;

                    case "C2":
                        if (!checkZero())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        break;

                    case "F2":
                        if (checkSignPositive())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        break;

                    case "EA":
                        if (checkParityEven())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        break;

                    case "E2":
                        if (checkParityEven())
                        {
                            currentByte = jumpFromMemory(currentByte);
                        }
                        break;

                    case "CA":
                        if (checkZero())
                        {
                            currentByte = jumpFromMemory(currentByte);
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
                        regA = Convert.ToInt16(memory[M],16);
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
                        stackPointer += (UInt16) (Convert.ToUInt16(memory[currentByte], 16) << 8);
                        break;

                    case "78": // Move B to A
                        regA = regB;
                        break;

                    case "79": // Move C to A
                        regA = regC;
                        break;

                    case "7A": // Move D to A
                        regA = regD;
                        break;

                    case "7B": // Move E to A
                        regA = regE;
                        break;

                    case "7C": // Move H to A
                        regA = regH;
                        break;

                    case "7D": // Move L to A
                        regA = regL;
                        break;

                    case "7E": // Move M to A
                        M = regL;
                        M += (regH << 8);
                        regA = Convert.ToInt16(memory[M], 16);
                        break;

                    case "47": // Move A to B
                        regB = regA;
                        break;

                    case "41": // Move C to B
                        regB = regC;
                        break;

                    case "42": // Move D to B
                        regB = regD;
                        break;

                    case "43": // Move E to B
                        regB = regE;
                        break;

                    case "44": // Move H to B
                        regB = regH;
                        break;

                    case "45": // Move L to B
                        regB = regL;
                        break;

                    case "46": // Move M to B
                        M = regL;
                        M += (regH << 8);
                        regB = Convert.ToInt16(memory[M], 16);
                        break;

                    case "4F": // Move A to C
                        regC = regA;
                        break;

                    case "48": // Move B to C
                        regC = regB;
                        break;

                    case "4A": // Move D to C
                        regC = regD;
                        break;

                    case "4B": // Move E to C
                        regC = regE;
                        break;

                    case "4C": // Move H to C
                        regC = regH;
                        break;

                    case "4D": // Move L to C
                        regC = regL;
                        break;

                    case "4E": // Move M to C
                        M = regL;
                        M += (regH << 8);
                        regC = Convert.ToInt16(memory[M], 16);
                        break;

                    case "57": // Move A
                        regD = regA;
                        break;

                    case "50": // Move B 
                        regD = regB;
                        break;

                    case "51": // Move C
                        regD = regC;
                        break;

                    case "53": // Move E
                        regD = regE;
                        break;

                    case "54": // Move H
                        regD = regH;
                        break;

                    case "55": // Move L
                        regD = regL;
                        break;

                    case "56": // Move M
                        M = regL;
                        M += (regH << 8);
                        regD = Convert.ToInt16(memory[M], 16);
                        break;

                    case "D3":
                        currentByte++;
                        if (memory[currentByte] == "11")
                        {
                            out11(regA);
                        }
                        break;

                    case "5F":
                        regE = regA;
                        break;

                    case "58":
                        regE = regB;
                        break;

                    case "59":
                        regE = regC;
                        break;

                    case "5A":
                        regE = regD;
                        break;

                    case "5C":
                        regE = regH;
                        break;

                    case "5D":
                        regE = regL;
                        break;

                    case "5E":
                        M = regL;
                        M += (regH << 8);
                        regE = Convert.ToInt16(memory[M] ,16);
                        break;

                    case "67":
                        regH = regA;
                        break;

                    case "60":
                        regH = regB;
                        break;

                    case "61":
                        regH = regC;
                        break;

                    case "62":
                        regH = regD;
                        break;

                    case "63":
                        regH = regE;
                        break;

                    case "65":
                        regH = regL;
                        break;

                    case "66":
                        M = regL;
                        M += (regH << 8);
                        regH = Convert.ToInt16(memory[M], 16);
                        break;

                    case "6F":
                        regL = regA;
                        break;

                    case "68":
                        regL = regB;
                        break;

                    case "69":
                        regL = regC;
                        break;

                    case "6A":
                        regL = regD;
                        break;

                    case "6B":
                        regL = regE;
                        break;

                    case "6C":
                        regL = regH;
                        break;

                    case "6E":
                        M = regL;
                        M += (regH << 8);
                        regE = Convert.ToInt16(memory[M], 16);
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
                        regA = Convert.ToInt16(memory[currentByte], 16);
                        break;

                    case "06":
                        currentByte++;
                        regB = Convert.ToInt16(memory[currentByte], 16);
                        break;

                    case "0E":
                        currentByte++;
                        regC = Convert.ToInt16(memory[currentByte], 16);
                        break;

                    case "16":
                        currentByte++;
                        regD = Convert.ToInt16(memory[currentByte], 16);
                        break;

                    case "1E":
                        currentByte++;
                        regE = Convert.ToInt16(memory[currentByte], 16);
                        break;

                    case "26":
                        currentByte++;
                        regH = Convert.ToInt16(memory[currentByte], 16);
                        break;

                    case "2E":
                        currentByte++;
                        regL = Convert.ToInt16(memory[currentByte], 16);
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
                        temp = regA;
                        if ((regA & 0x80) != 0)
                        {
                            setCarry();
                        }
                        else
                        {
                            resetCarry();
                        }
                        regA = ((temp << 1) & 0xFF);
                        break;

                    case "1F":
                        temp = regA;
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








                    default:
                        break;
                       
                }


                // Adjust Flags
                
                if (noFlagUpdates == 0)
                {
                    //Set or Unset Carry Flag
                    if (regA > 0xFF)
                    {
                        regA = regA - 0x100;
                        setCarry();
                    }
                    else
                    {
                        resetCarry();
                    }
                }
                else
                {
                    noFlagUpdates = 0;
                }

                currentByte++;        
            }
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

        private void compareToA(int regX)
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
            mCurrentByte++;
            return mCurrentByte = M - programStartAddr - 1; // -1 corrects for increment at the end of loop
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

        private void updateCompletedAssemblyUI()
        {

        }

        private void out11(int regX)
        {
            outputPort = regX;
        }

        private void updateOut_UI(int out11)
        {
            outP0.Checked = ((out11 & 0X01) != 0);
            outP1.Checked = ((out11 & 0X02) != 0);
            outP2.Checked = ((out11 & 0X04) != 0);
            outP3.Checked = ((out11 & 0X08) != 0);
            outP4.Checked = ((out11 & 0X10) != 0);
            outP5.Checked = ((out11 & 0X20) != 0);
            outP6.Checked = ((out11 & 0X40) != 0);
            outP7.Checked = ((out11 & 0X80) != 0);
        }

        private void speedScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            speed = speedScrollbar.Value;
        }

        
    }
}

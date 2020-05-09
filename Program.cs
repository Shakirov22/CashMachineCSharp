using System;
using System.Collections;
using System.Text;
using System.Linq;

namespace CashMachine
{
    public class CounterOptions
    {
        private int index;
        private int remainder;
        private int faceValue;
        private ArrayList indexArray;
        private ArrayList remainderArray;
        private int recursionIndex;
        private StringBuilder output;
        private long numberOptions;
        private ArrayList faceValuesArray;
        private int sum;
        private bool flagPrinting;

        public CounterOptions(ArrayList faceValuesArray, int sum)
        {
            this.indexArray = new ArrayList();
            this.remainderArray = new ArrayList();
            this.faceValuesArray = faceValuesArray;
            this.flagPrinting = true;

            this.sum = sum;
            this.recursionIndex = 0;
            this.numberOptions = 0;
            this.output = new StringBuilder("");
        }

        private bool checkAndPrintLowestFaceValue()
        {
            if (this.remainder % this.faceValue == 0)
            {
                if (flagPrinting)
                {
                    this.output.AppendFormat(string.Concat(Enumerable.Range(0, this.remainder / this.faceValue).Select(i => this.faceValue.ToString() + " ")));
                    Console.WriteLine(this.output);
                }
                return true;
            }
            return false;
        }

        private void goToPreviousRecursionIndex()
        {
            this.recursionIndex--;
            this.indexArray[this.recursionIndex] =  (int)this.indexArray[this.recursionIndex] - 1;
            if (flagPrinting)
            {
                this.output.Remove(2 * this.recursionIndex, this.output.Length - 2 * this.recursionIndex);
            }
        }

        private bool checkExitEvent()
        {
            if (this.recursionIndex == 0)
            {
                return true;
            }
            else
            {
                this.goToPreviousRecursionIndex();
                return false;
            }
        }

        private bool checkTheLowestFaceValue()
        {
            if (this.checkAndPrintLowestFaceValue())
            {
                this.numberOptions++;
            }
            return checkExitEvent();
        }

        private void checkIndexArraySize()
        {
            if (this.recursionIndex >= this.indexArray.Count)
            {
                this.indexArray.Add(0);
                this.remainderArray.Add(0);
            }
        }

        private void goToNextRecursionIndex()
        {
            this.recursionIndex++;
            this.checkIndexArraySize();
            this.indexArray[this.recursionIndex] = this.index;
            remainderArray[this.recursionIndex] = this.remainder - this.faceValue;
            if (flagPrinting)
            {
                this.output.AppendFormat(this.faceValue.ToString() + " ");
            }
        }

        private void checkRemainderGoToNext()
        {
            if (this.remainder - this.faceValue == 0)
            {
                if (flagPrinting)
                {
                    Console.WriteLine(this.output + this.faceValue.ToString());
                }
                this.numberOptions++;
            }
            this.indexArray[this.recursionIndex] = this.index - 1;
        }

        private void checkNotTheLowestFaceValue()
        {
            if (this.remainder - this.faceValue > 0)
            {
                this.goToNextRecursionIndex();
            }
            else
            {
                this.checkRemainderGoToNext();
            }
        }

        private void countAndPrintOptions()
        {
            bool exitFlag = false;

            this.indexArray.Add(this.faceValuesArray.Count - 1);
            this.remainderArray.Add(this.sum);

            while (!exitFlag)
            {
                this.index = (int) this.indexArray[this.recursionIndex];
                this.faceValue = (int) this.faceValuesArray[this.index];
                this.remainder = (int) this.remainderArray[this.recursionIndex];

                if (this.index == 0)
                {
                    exitFlag = this.checkTheLowestFaceValue();
                }
                else
                {
                    this.checkNotTheLowestFaceValue();
                }
            }
        }

        public long getNumberOptions()
        {
            this.countAndPrintOptions();
            return this.numberOptions;
        }
    }

    class Program
    {
        private static void checkForArguments(String[] args)
        {
            if (args.Count() <= 1)
            {
                throw new Exception("Недостаточно аргументов");
            }
        }

        private static void checkSum(int sum)
        {
            if (sum < 0)
            {
                throw new Exception("Сумма должна быть неотрицательна");
            }
        }

        private static int getSum(String[] args)
        {
            int sum = 0;
            try
            {
                sum = int.Parse(args[0]);
                checkSum(sum);
            }
            catch (FormatException e)
            {
                throw new Exception("Все аргументы должны быть целыми числами");
            }
            return sum;
        }

        private static void checkFaceValue(int faceValue)
        {
            if (faceValue <= 0)
            {
                throw new Exception("Номинал монеты должен быть положительным");
            }
        }

        private static ArrayList getFaceValuesArray(String[] args)
        {
            ArrayList faceValuesArray = new ArrayList();
            try
            {
                for (int i = 1; i < args.Count(); i++)
                {
                    int faceValue = int.Parse(args[i]);
                    checkFaceValue(faceValue);
                    if (!faceValuesArray.Contains(faceValue))
                    {
                        faceValuesArray.Add(faceValue);
                    }
                }
            }
            catch (FormatException e)
            {
                throw new Exception("Все аргументы должны быть целыми числами");
            }
            return faceValuesArray;
        }

        private static bool checkNullSum(int sum)
        {
            if (sum == 0)
            {
                Console.WriteLine(0);
                return true;
            }
            return false;
        }

        private static void printNumberOptions(long numberOptions)
        {
            Console.WriteLine("Всего вариантов выдачи: " + numberOptions);
        }

        static void Main(string[] args)
        {
            checkForArguments(args);
            int sum = getSum(args);
            ArrayList faceValuesArray = getFaceValuesArray(args);
            if (!checkNullSum(sum))
            {
                faceValuesArray.Sort();
                CounterOptions counterOptions = new CounterOptions(faceValuesArray, sum);
                long numberOptions = counterOptions.getNumberOptions();
                printNumberOptions(numberOptions);
            }
            while (true) {

            }
        }
    }
}

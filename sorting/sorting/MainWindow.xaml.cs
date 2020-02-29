using System;
using System.Collections.Generic;
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

namespace sorting
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class Heap
    {
        int[] arr;
        public int count;

        public Heap(int degree)
        {
            count = 0;
            arr = new int[degree + 1];
        }
        public void insert(int item)
        {
            int i;
            i = ++count;

            while (i != 1 && item < arr[i / 2])
            {
                arr[i] = arr[i / 2];
                i /= 2;
            }
            arr[i] = item;
        }
        public int delete()
        {
            int parent, child;
            int item, tmp;

            item = arr[1];
            tmp = arr[count--];
            parent = 1;
            child = 2;

            while (child <= count)
            {
                if (child < count && arr[child] > arr[child + 1])
                {
                    child++;
                }

                if (tmp <= arr[child])
                {
                    break;
                }

                arr[parent] = arr[child];
                parent = child;
                child = child * 2;
            }
            arr[parent] = tmp;
            return item;
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool hasRandomVal = false;
        string result;
        DateTime start;
        TimeSpan end;
        private string input;
        string[] split_text;
        public int[] insert;
        public int[] select;
        public int[] bubble;
        public int[] shell;
        public int[] quick;
        public int[] merge;
        public int[] heap;
        public int[] radix;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!hasRandomVal)
            {
                input = Data.Text;
                split_text = Data.Text.Split(',');
                insert = new int[split_text.Length];
                select = new int[split_text.Length];
                bubble = new int[split_text.Length];
                shell = new int[split_text.Length];
                quick = new int[split_text.Length];
                merge = new int[split_text.Length];
                heap = new int[split_text.Length];
                radix = new int[split_text.Length];
                for (int i = 0; i < split_text.Length; i++)
                {
                    insert[i] = select[i] = bubble[i] = shell[i] = quick[i] = merge[i] = heap[i] = radix[i] = Convert.ToInt32(split_text[i]);
                }
            }
            if (Data.Text == "")
            {
                MessageBox.Show("숫자를 입력해 주세요.");
                return;
            }

            insertSort(insert);
            selectionSort(select);
            bubbleSort(bubble);
            shellSort(shell);
            start = DateTime.Now;
            quickSort(quick, 0, quick.Length - 1);
            start = DateTime.Now;
            mergesort(merge, 0, merge.Length - 1);
            heapSort(heap);
            radixSort(radix, 10);
        }

        public void insertSort(int[] arr)
        {
            DateTime start = DateTime.Now;
            result = "";
            int j, temp, size = arr.Length;
            for (int i = 1; i < size; i++)
            {
                temp = arr[i];
                for (j = i - 1; j >= 0; j--)
                {
                    if (arr[j] < temp) break;
                    arr[j + 1] = arr[j];
                }
                arr[j + 1] = temp;
            }
            for (int k = 0; k < size; k++)
            {
                result = result + arr[k] + ", ";
            }
            result += "\n";
            end = DateTime.Now - start;
            result = result + "실행 시간 : " + end.Milliseconds + "ms";
            Insertion.Text = result;
        }
        public void selectionSort(int[] arr)
        {
            DateTime start = DateTime.Now;
            result = "";
            int i, j, min, temp, size = arr.Length;
            for (i = 0; i < size; i++)
            {
                min = i;
                for (j = i + 1; j < size; j++)
                {
                    if (arr[min] > arr[j]) min = j;
                }
                temp = arr[min];
                arr[min] = arr[i];
                arr[i] = temp;
            }
            for (int k = 0; k < size; k++)
            {
                result = result + arr[k] + ", ";
            }
            result += "\n";
            end = DateTime.Now - start;
            result = result + "실행 시간 : " + end.Milliseconds + "ms";
            Selection.Text = result;
        }
        public void bubbleSort(int[] arr)
        {
            DateTime start = DateTime.Now;
            result = "";
            int i, j, temp, size = arr.Length;
            for (i = 0; i < size - 1; i++)
            {
                for (j = 1; j < size - i; j++)
                {
                    if (arr[j - 1] > arr[j])
                    {
                        temp = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            for (int k = 0; k < size; k++)
            {
                result = result + arr[k] + ", ";
            }
            result += "\n";
            end = DateTime.Now - start;
            result = result + "실행 시간 : " + end.Milliseconds + "ms";
            Bubble.Text = result;
        }
        public void shellSort(int[] arr)
        {
            DateTime start = DateTime.Now;
            result = "";
            int i, j, temp, size = arr.Length;
            int gap = size / 2;
            while (gap > 0)
            {
                for (i = gap; i < size; i++)
                {
                    temp = arr[i];
                    j = i;
                    while (j >= gap && arr[j - gap] > temp)
                    {
                        arr[j] = arr[j - gap];
                        j -= gap;
                    }
                    arr[j] = temp;
                }
                gap /= 2;
            }
            for (int k = 0; k < size; k++)
            {
                result = result + arr[k] + ", ";
            }
            result += "\n";
            end = DateTime.Now - start;
            result = result + "실행 시간 : " + end.Milliseconds + "ms";
            Shell.Text = result;
        }
        public void quickSort(int[] arr, int Left, int Right)
        {
            result = "";
            int left, right, pivot = arr[Left];
            for (left = Left, right = Right; left < right; right--)
            {
                while (arr[right] >= pivot && left < right) right--;
                if (left < right) arr[left] = arr[right];
                while (arr[left] <= pivot && left < right) left++;
                if (left >= right) break;
                arr[right] = arr[left];
            }
            arr[left] = pivot;
            if (left > Left) quickSort(arr, Left, left - 1);
            if (left < Right) quickSort(arr, left + 1, Right);
            if (Right - Left + 1 == arr.Length)
            {
                for (int k = 0; k < Right - Left; k++)
                {
                    result = result + arr[k] + ", ";
                }
                result += "\n";
                end = DateTime.Now - start;
                result = result + "실행 시간 : " + end.Milliseconds + "ms";
                Quick.Text = result;
            }
        }
        public void mergesort(int[] arr, int left, int right)
        {
            result = "";
            if (left >= right) return;
            int middle = (left + right - 1) / 2;
            mergesort(arr, left, middle);
            mergesort(arr, middle + 1, right);
            int arr1 = middle + 1 - left;
            int arr2 = right - middle;
            int[] L = new int[arr1], R = new int[arr2];
            int i, j, k;
            for (i = 0; i < arr1; i++) L[i] = arr[left + i];
            for (i = 0; i < arr2; i++) R[i] = arr[middle + 1 + i];
            i = j = 0;
            k = left;
            while (i < arr1 && j < arr2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                    k++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                    k++;
                }
            }
            while (i < arr1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }
            while (j < arr2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
            if (arr.Length <= right - left + 1)
            {
                for (int a = 0; a < right - left; a++)
                {
                    result = result + arr[a] + ", ";
                }
                result += "\n";
                end = DateTime.Now - start;
                result = result + "실행 시간 : " + end.Milliseconds + "ms";
                Merge.Text = result;
            }
        }
        public void heapSort(int[] arr)
        {
            DateTime start = DateTime.Now;
            result = "";
            Heap heap = new Heap(arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                heap.insert(arr[i]);
            }
            for (int i = 0; i < arr.Length; i++)
            {
                result = result + heap.delete() + ", ";
            }
            result += "\n";
            end = DateTime.Now - start;
            result = result + "실행 시간 : " + end.Milliseconds + "ms";
            Heap.Text = result;
        }
        public void radixSort(int[] arr, int bs)
        {
            DateTime start = DateTime.Now;
            result = "";
            bs = 10;
            int max = arr.Max();
            int exp, i, j, size = arr.Length;
            for (exp = 1; exp <= max; exp *= bs)
            {
                int[] count = new int[bs];
                int[] output = new int[size];
                for (i = 0; i < size; i++) count[(arr[i] / exp) % bs]++;
                for (i = 1; i < bs; i++) count[i] += count[i - 1];
                for (i = size - 1; i > -1; i--)
                {
                    j = (arr[i] / exp) % bs;
                    output[count[j] - 1] = arr[i];
                    count[j]--;
                }
                for (i = 0; i < size; i++)
                    arr[i] = output[i];
            }
            for (int k = 0; k < size; k++)
            {
                result = result + arr[k] + ", ";
            }
            result += "\n";
            end = DateTime.Now - start;
            result = result + "실행 시간 : " + end.Milliseconds + "ms";
            Radix.Text = result;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Data.Text = Bubble.Text = Insertion.Text = Selection.Text = Quick.Text = Merge.Text = Radix.Text = Heap.Text = Shell.Text = "";
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            hasRandomVal = true;
            Random rand = new Random();
            int n;
            if (int.TryParse(RandomNum.Text, out n))
            {
                if (n <= 0)
                {
                    MessageBox.Show("양의 정수를 입력해 주세요!");
                    RandomNum.Text = "";
                    return;
                }
                if (n > 1000000000)
                {
                    MessageBox.Show("10억개 이하의 수만 입력 가능합니다.");
                    RandomNum.Text = "";
                    return;
                }
                insert = new int[n];
                select = new int[n];
                bubble = new int[n];
                shell = new int[n];
                quick = new int[n];
                merge = new int[n];
                heap = new int[n];
                radix = new int[n];
                for (int i = 0; i < n; i++)
                {
                    insert[i] = select[i] = bubble[i] = shell[i] = quick[i] = merge[i] = heap[i] = radix[i] = rand.Next(0, n);
                    Data.Text = Data.Text + insert[i] + ",";
                }     
            }
            else
            {
                MessageBox.Show("양의 정수를 입력해 주세요!");
                RandomNum.Text = "";
            }
        }
    }
}

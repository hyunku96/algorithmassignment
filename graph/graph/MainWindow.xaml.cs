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

namespace graph
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class edge
    {
        public int start;
        public int end;
        public int weight;

        public edge(int s, int e, int w)
        {
            start = s;
            end = e;
            weight = w;
        }
    }
    public class Heap
    {
        edge[] arr;
        public int count;
        
        public Heap(int degree)
        {
            count = 0;
            arr = new edge[degree * degree + 1];            
        }
        public void insert(edge item)
        {
            int i;
            i = ++count;

            while (i != 1 && item.weight < arr[i / 2].weight)
            {
                arr[i] = arr[i / 2];
                i /= 2;
            }
            arr[i] = item;
        }
        public edge delete()
        {
            int parent, child;
            edge item, tmp;

            item = arr[1];
            tmp = arr[count--];
            parent = 1;
            child = 2;

            while (child <= count)
            {
                if (child < count && arr[child].weight > arr[child + 1].weight)
                {
                    child++;
                }
                
                if (tmp.weight <= arr[child].weight)
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

        int degree = 0;
        int[] graph;
        int[] mst;

        string result;
        

        private void VertexNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(VertexNum.Text, out degree))
            {
                graph = new int[degree * degree];
                graph.Initialize();
            }
            else
            {
                if (VertexNum.Text != "")
                {
                    MessageBox.Show("숫자(정수)만 입력 가능합니다.");
                    degree = 0;
                    VertexNum.Text = "0";
                    Weights.Text = "";                   
                }
            }
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            if (Vertex1.Text != "" && Vertex2.Text != "" && Edge.Text != "")
            {
                int startV, endV, weight;
                if (int.TryParse(Vertex1.Text, out startV) && int.TryParse(Vertex2.Text, out endV) && int.TryParse(Edge.Text, out weight))
                {
                    if (startV < degree && endV < degree)
                    {
                        graph[startV * degree + endV] = weight;
                        Weights.Text = "";
                        for (int i = 0; i < degree; i++)
                        {
                            for (int j = 0; j < degree; j++)
                            {
                                Weights.Text += graph[i * degree + j];
                                Weights.Text += " ";
                            }
                            Weights.Text += "\n";
                        }
                    }
                    else
                    {
                        MessageBox.Show("배열 범위 초과!");
                    }
                }
                else
                {
                    MessageBox.Show("숫자(정수)만 입력 가능합니다.");
                    Vertex1.Text = "";
                    Vertex2.Text = "";
                    Edge.Text = "";
                }
            }
            else
            {
                MessageBox.Show("값을 입력해 주세요.");
                Vertex1.Text = "";
                Vertex2.Text = "";
                Edge.Text = "";
            }
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            int startnum = -1;
            if (int.TryParse(start.Text, out startnum))
            {
                if (0 <= startnum && startnum < degree)
                {
                    switch (Select.SelectedIndex)
                    {
                        case 0://DFS
                            DFS(startnum);
                            break;
                        case 1://BFS
                            BFS(startnum);
                            break;
                        case 2://Prim
                            Prim(startnum);
                            break;
                        case 3://Kruskal
                            Kruskal();
                            break;
                        case 4://Sollin
                            Sollin();
                            break;
                        case 5://Dijkstra
                            Dijkstra(startnum);
                            break;
                        case 6://Bellman-Ford
                            BF(startnum);
                            break;
                        case 7://Floyd-Warshall                       
                            FW();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("없는 노드입니다.");
                    start.Text = "";
                }
            }
            else
            {
                if (start.Text == "")
                {
                    MessageBox.Show("시작정점(숫자)을 입력해 주세요.");
                    start.Text = "";
                }
                else
                {
                    MessageBox.Show("숫자(정수)만 입력 가능합니다.");
                    start.Text = "";
                }
            }
        }

        private void DFS(int startnum)
        {
            result = "";
            bool[] visited = new bool[degree];
            visited.Initialize();
            result += startnum;
            visited[startnum] = true;
            DFS_iter(startnum, visited);
            Result.Text = result;
        }

        private void DFS_iter(int startnum, bool[] visited)
        {
            for (int i = 0; i < degree; i++)
            {
                if (graph[startnum * degree + i] != 0 && !visited[i])
                {
                    visited[i] = true;
                    result += " -> " + i;
                    DFS_iter(i, visited);
                }
            }
        }

        private void BFS(int startnum)
        {
            result = "";
            bool finished = false;
            Queue<int> buffer = new Queue<int>();
            bool[] visited = new bool[degree];
            bool[] in_q = new bool[degree];
            in_q.Initialize();
            visited.Initialize();

            while (!finished)
            {
                result += startnum;
                visited[startnum] = true;
                for (int i = 0; i < degree; i++)
                {
                    if (!visited[i] && !in_q[i])
                    {
                        if (graph[startnum * degree + i] != 0)
                        {
                            buffer.Enqueue(i);
                            in_q[i] = true;
                        }
                    }
                }
                if (buffer.Count == 0)//진행 불가능하면 끝
                {
                    finished = true;
                    Result.Text = result;
                }
                else
                {
                    result += " -> ";
                    startnum = buffer.Dequeue();
                }
            }
        }

        private void Prim(int startnum)
        {
            result = "";
            mst = new int[degree * degree];
            mst.Initialize();
            Heap heap = new Heap(degree);
            bool[] in_tree = new bool[degree];
            in_tree.Initialize();
            bool finished = false;
            in_tree[startnum] = true;

            while (!finished)
            {
                for (int i = 0; i < degree; i++)
                {
                    if (graph[startnum * degree + i] != 0 && !in_tree[i])
                    {
                        heap.insert(new edge(startnum, i, graph[startnum * degree + i]));
                    }
                }
                if (heap.count > 0)
                {
                    edge end;
                    do
                    {
                        end = heap.delete();
                    }
                    while (in_tree[end.end] && heap.count > 0);
                    if (!in_tree[end.end])
                    {
                        mst[end.start * degree + end.end] = graph[end.start * degree + end.end];
                        in_tree[end.end] = true;
                        startnum = end.end;
                    }                    
                }
                else
                {
                    finished = true;
                }
            }
            for (int i = 0; i < degree; i++)//양방향 그래프화
            {
                for (int j = 0; j < degree; j++)
                {
                    if (mst[i * degree + j] != 0)
                    {
                        mst[j * degree + i] = mst[i * degree + j];
                    }
                }
            }
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    result = result + mst[i * degree + j] + " ";
                }
                result += "\n";
            }

            MST_Result.Text = result;
        }

        private void Kruskal()//no cycle n - 1 edges
        {
            int[] union = new int[degree];
            union.Initialize();
            List<edge> edges = new List<edge>();
            int countEdges = 0;
            int countSet = 0;
            mst = new int[degree * degree];
            mst.Initialize();
            result = "";

            for (int i = 0; i < degree; i++)//input edge to edges
            {
                for (int j = 0; j < degree; j++)
                {
                    if (graph[i * degree + j] != 0)
                    {
                        edges.Add(new edge(i, j, graph[i * degree + j]));
                    }
                }
            }
            edge tmp;
            for (int i = 0; i < edges.Count(); i++)//sort edges
            {
                for (int j = 0; j < edges.Count() - i - 1; j++)
                {
                    if (edges[j].weight > edges[j + 1].weight)
                    {
                        tmp = edges[j];
                        edges[j] = edges[j + 1];
                        edges[j + 1] = tmp;
                    }
                }
            }

            for (int i = 0; i < edges.Count(); i++)
            {
                if (countEdges < degree - 1)
                {
                    if (union[edges[i].start] == 0 && union[edges[i].end] == 0)
                    {
                        countSet++;
                        union[edges[i].start] = union[edges[i].end] = countSet;
                        mst[edges[i].start * degree + edges[i].end] = graph[edges[i].start * degree + edges[i].end];
                        countEdges++;
                    }
                    else if (union[edges[i].start] == 0 && union[edges[i].end] != 0)
                    {
                        union[edges[i].start] = union[edges[i].end];
                        mst[edges[i].start * degree + edges[i].end] = graph[edges[i].start * degree + edges[i].end];
                        countEdges++;
                    }
                    else if (union[edges[i].start] != 0 && union[edges[i].end] == 0)
                    {
                        union[edges[i].end] = union[edges[i].start];
                        mst[edges[i].start * degree + edges[i].end] = graph[edges[i].start * degree + edges[i].end];
                        countEdges++;
                    }
                    else
                    {
                        if (union[edges[i].start] < union[edges[i].end])
                        {
                            countSet--;
                            union[edges[i].end] = union[edges[i].start];
                            mst[edges[i].start * degree + edges[i].end] = graph[edges[i].start * degree + edges[i].end];
                            countEdges++;
                        }
                        else if (union[edges[i].start] > union[edges[i].end])
                        {
                            countSet--;
                            union[edges[i].start] = union[edges[i].end];
                            mst[edges[i].start * degree + edges[i].end] = graph[edges[i].start * degree + edges[i].end];
                            countEdges++;
                        }
                    }
                }
            }
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    if (mst[i * degree + j] != 0)
                    {
                        mst[j * degree + i] = mst[i * degree + j];
                    }
                }
            }
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    result = result + mst[i * degree + j] + " ";
                }
                result += "\n";
            }
            MST_Result.Text = result;            
        }

        private void Sollin()
        {
            result = "";
            mst = new int[degree * degree];
            mst.Initialize();
            int[] union = new int[degree];
            union.Initialize();
            Heap heap = new Heap(degree);
            List<edge> choosen = new List<edge>();
            int setCount = 0;
            bool finished = false;

            while (!finished)
            {
                for (int i = 0; i < degree; i++)
                {
                    for (int j = 0;j < degree; j++)
                    {
                        if (graph[i * degree + j] != 0)
                        {
                            heap.insert(new edge(i, j, graph[i * degree + j]));
                        }
                    }
                    if (heap.count > 0)
                    {
                        choosen.Add(heap.delete());
                        heap.count = 0;
                    }
                }
                for (int i = 0; i < choosen.Count(); i++)
                {
                    if (union[choosen[i].start] == 0 && union[choosen[i].end] == 0)
                    {
                        union[choosen[i].start] = union[choosen[i].end] = ++setCount;
                        mst[choosen[i].start * degree + choosen[i].end] = graph[choosen[i].start * degree + choosen[i].end];
                    }
                    else
                    {
                        if (union[choosen[i].start] != union[choosen[i].end])
                        {
                            if (union[choosen[i].start] == 0 && union[choosen[i].end] != 0)
                            {
                                union[choosen[i].start] = union[choosen[i].end];
                                mst[choosen[i].start * degree + choosen[i].end] = graph[choosen[i].start * degree + choosen[i].end];
                            }
                            else if (union[choosen[i].start] != 0 && union[choosen[i].end] == 0)
                            {
                                union[choosen[i].end] = union[choosen[i].start];
                                mst[choosen[i].start * degree + choosen[i].end] = graph[choosen[i].start * degree + choosen[i].end];
                            }
                            else
                            {
                                if (union[choosen[i].start] < union[choosen[i].end])
                                {
                                    for (int k = 0; k < degree; k++)
                                    {
                                        if (k == choosen[i].end)
                                        {
                                            union[k] = union[choosen[i].start];
                                        }
                                    }
                                    mst[choosen[i].start * degree + choosen[i].end] = graph[choosen[i].start * degree + choosen[i].end];
                                }
                                else
                                {
                                    for (int k = 0; k < degree; k++)
                                    {
                                        if (k == choosen[i].start)
                                        {
                                            union[k] = union[choosen[i].end];
                                        }
                                    }
                                    mst[choosen[i].start * degree + choosen[i].end] = graph[choosen[i].start * degree + choosen[i].end];
                                }
                            }
                        }
                    }
                }
                choosen.Clear();
                finished = true;
                for (int i = 0; i < degree - 1; i++)
                {
                    if (union[i] != union[i + 1])
                    {
                        finished = false;
                    }
                }
            }
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    if (mst[i * degree + j] != 0)
                    {
                        mst[j * degree + i] = mst[i * degree + j];
                    }
                }
            }
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    result = result + mst[i * degree + j] + " ";
                }
                result += "\n";
            }
            MST_Result.Text = result;
        }

        private void Dijkstra(int startnum)
        {
            result = "";            
            bool finished = false;
            Heap fringe = new Heap(degree);
            int[] previous = new int[degree];
            int[] distance = new int[degree];
            for (int i = 0; i < degree; i++)
            {
                distance[i] = -1;
            }
            previous[0] = -1;
            distance[startnum] = 0;

            while (!finished)
            {                
                for (int i = 0; i < degree; i++)
                {
                    if (graph[startnum * degree + i] < 0)
                    {
                        MessageBox.Show("간선에 음수가 있습니다!");
                        return;
                    }
                    if (graph[startnum * degree + i] != 0 && i != previous[i])
                    {                        
                        if (distance[i] == -1)//initial state, not in q
                        {
                            fringe.insert(new edge(startnum, i, graph[startnum * degree + i]));
                            previous[i] = startnum;
                            distance[i] = distance[startnum] + graph[startnum * degree + i];
                        }
                        else//in q
                        {
                            if (distance[startnum] + graph[startnum * degree + i] < distance[i])
                            {
                                distance[i] = distance[startnum] + graph[startnum * degree + i];
                            }
                        }
                    }
                }
                for (int i = 0; i < degree; i++)
                {
                    result = result + i + " : " + distance[i] + ", ";
                }
                result += "\n";
                if (fringe.count != 0)
                {                   
                    startnum = fringe.delete().end;
                }
                else
                {
                    finished = true;
                }
            }

            Path_Result.Text = result;
        }

        private void BF(int startnum)
        {
            result = "";
            bool finished = false;
            Heap fringe = new Heap(degree);
            int[] previous = new int[degree];
            int[] distance = new int[degree];
            for (int i = 0; i < degree; i++)
            {
                distance[i] = int.MaxValue;
                previous[i] = -1;
            }            
            distance[startnum] = 0;
            int maxIter = 0;

            while (!finished)
            {
                if (maxIter > degree * (degree - 1) / 2)
                {
                    MessageBox.Show("음의 사이클 발견");
                    return;
                }
                for (int i = 0; i < degree; i++)
                {
                    if (graph[startnum * degree + i] != 0)
                    {
                        if (distance[i] == int.MaxValue)
                        {
                            distance[i] = distance[startnum] + graph[startnum * degree + i];
                            fringe.insert(new edge(startnum, i, graph[startnum * degree + i]));
                            maxIter++;
                        }
                        else
                        {
                            if (distance[i] > distance[startnum] + graph[startnum * degree + i])
                            {
                                distance[i] = distance[startnum] + graph[startnum * degree + i];
                                fringe.insert(new edge(startnum, i, graph[startnum * degree + i]));
                                maxIter++;
                            }
                        }
                    }
                }
                for (int i = 0; i < degree; i++)
                {
                    result = result + i + " : " + distance[i] + ", ";
                }
                result += "\n";
                if (fringe.count > 0)
                {
                    startnum = fringe.delete().end;
                }
                else
                {
                    finished = true;
                }
            }

            Path_Result.Text = result;
        }

        private void FW()
        {
            result = "";
            int[] distance = new int[degree * degree];
            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    if (graph[i * degree + j] != 0)
                    {
                        distance[i * degree + j] = graph[i * degree + j];
                    }
                    else
                    {
                        distance[i * degree + j] = 1000000000;
                    }
                }
            }

            for (int k = 0; k < degree; k++)
            {
                for (int i = 0; i < degree; i++)
                {
                    for (int j = 0; j < degree; j++)
                    {
                        if (i != j)
                        {
                            if (distance[i * degree + k] + distance[k * degree + j] < distance[i * degree + j])
                            {
                                distance[i * degree + j] = distance[i * degree + k] + distance[k * degree + j];
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < degree; i++)
            {
                for (int j = 0; j < degree; j++)
                {
                    if (distance[i * degree + j] == 1000000000)
                    {
                        result = result + 0 + " ";
                    }
                    else
                    {
                        result = result + distance[i * degree + j] + " ";
                    }
                }
                result += "\n";
            }
            Path_Result.Text = result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)//read txt
        {
            result = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            string[] split_text;
            dlg.DefaultExt = ".txt";

            bool? success = dlg.ShowDialog();

            if (success == true)
            {
                string textValue = System.IO.File.ReadAllText(dlg.FileName);
                split_text = textValue.Split(',', '\n');
                degree = (int)Math.Sqrt(split_text.Length);
                graph = new int[degree * degree];
                graph.Initialize();
                VertexNum.Text = degree.ToString();
                for (int i = 0; i < degree; i++)
                {
                    for (int j = 0; j < degree; j++)
                    {
                        graph[i * degree + j] = Convert.ToInt32(split_text[i * degree + j]);
                        result = result + graph[i * degree + j] + " ";
                    }
                    result += "\n";
                }
                Weights.Text = result;
            }
            else
            {
                MessageBox.Show("파일을 선택하세요.");
            }
        }
    }
}

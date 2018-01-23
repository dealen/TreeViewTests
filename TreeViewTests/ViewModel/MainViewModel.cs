using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace TreeViewTests.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            var topNodes = nodes.Where(x => x.Parent == null).ToList();
            BuildTree(topNodes);
        }

        private List<Node> nodes = new List<Node>
            {
                new Node(1, "One"),
                new Node(2, "Two", 1),
                new Node(3, "Three", 2),
                new Node(4, "Four"),
                new Node(5, "Five", 4),
                new Node(6, "Six"),
                new Node(7, "Seven", 6),
                new Node(8, "Eight", 7),
                new Node(9, "Nine", 7),
                new Node(10, "Ten", 8),
            };

        private void BuildTree(List<Node> topNodes)
        {
            var listOfNodes = new List<Node>();
            Task task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < topNodes.Count(); i++)
                {
                    var node = topNodes[i];
                    PopulateTree(ref node, nodes);
                    if (listOfNodes.Find(x => x.ID == node.ID) == null)
                        listOfNodes.Add(node);
                }
            }).ContinueWith((e) =>
            {
                TopNodes = CollectionViewSource.GetDefaultView(new ObservableCollection<Node>(listOfNodes));
            });
        }

        public string Filtr
        {
            get { return _filtr; }
            set
            {
                _filtr = value;
                _FilterNodes(_filtr);
                RaisePropertyChanged("Filtr");
            }
        }

        private void _FilterNodes(string filter)
        {
            var filteredNodes = nodes.Where(
                x =>
                    (x.Name.Contains(filter) || (x.Children != null && 
                    x.Children.FindAll(y => y.Name.Contains(filter)).Any()))).ToList();
            var topNodes = filteredNodes.Where(x => x.Parent == null).ToList();
            BuildTree(topNodes);
        }

        /// <summary>
        /// https://stackoverflow.com/a/22035349/1237309
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodes"></param>
        public void PopulateTree(ref Node root, List<Node> nodes)
        {
            if (root == null)
            {
                root = new Node();
                // get all departments in the list with parent is null
                var details = nodes.Where(t => t.Parent == null);
                foreach (var detail in details)
                {
                    var child = new Node()
                    {
                        Name = detail.Name,
                        ID = detail.ID,
                    };
                    PopulateTree(ref child, nodes);
                    if (root.Children == null) root.Children = new List<Node>();
                    if (root.Children.Find(x => x.ID == child.ID) == null)
                        root.Children.Add(child);
                }
            }
            else
            {
                var id = (int)root.ID;
                var details = nodes.Where(t => t.Parent == id);
                foreach (var detail in details)
                {
                    var child = new Node()
                    {
                        Name = detail.Name,
                        ID = detail.ID,
                    };
                    PopulateTree(ref child, nodes);
                    if (root.Children == null) root.Children = new List<Node>();
                    if (root.Children.Find(x => x.ID == child.ID) == null)
                        root.Children.Add(child);
                }
            }
        }

        public ICollectionView TopNodes
        {
            get { return _TopNodes; }
            set
            {
                _TopNodes = value;
                this.RaisePropertyChanged("TopNodes");
            }
        }
        
        private ICollectionView _TopNodes;
        private string _filtr;
    }
}
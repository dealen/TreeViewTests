using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TreeViewTests
{
    public class Node
    {
        private List<Node> _children;

        public int ID { get; set; }
        public string Name { get; set; }
        public int? Parent { get; set; }

        public Node()
        {
        }

        public List<Node> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                ObservableCollection<Node> oo = new ObservableCollection<Node>(_children);
                ChildrenView = CollectionViewSource.GetDefaultView(oo);
            }
        }

        public ICollectionView ChildrenView { get; set; }
        //public List<Node> Children { get; set; }


        public Node(int id, string name)
        {
            ID = id;
            Name = name;
            Parent = null;
        }

        public Node(int id, string name, int? parent)
        {
            ID = id;
            Name = name;
            Parent = parent;
        }        
    }
}

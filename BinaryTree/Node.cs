namespace BinaryTree
{
    public class Node
    {
        public Node Parent { get; set; }
        public Node RightChild { get; set; }
        public Node LeftChild { get; set; }
        public int Value { get; }
        public int RightChildrenCount { get; set; }

        public Node(Node parent, int value)
        {
            Parent = parent;
            Value = value;
            RightChild = null;
            LeftChild = null;
            RightChildrenCount = 0;
        }

        ~Node()
        {
            Parent = null;
            RightChild = null;
            LeftChild = null;
        }
    }
}
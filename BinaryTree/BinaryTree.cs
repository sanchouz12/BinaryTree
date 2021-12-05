using System;
using System.Collections.Generic;

namespace BinaryTree
{
    public class Forest
    {
        private List<Node> Nodes { get; }
        private Node ParentNode { get; set; }

        public Forest()
        {
            Nodes = new List<Node>();
        }

        ~Forest()
        {
            Nodes.Clear();
        }

        public Node GetParentNode()
        {
            return ParentNode;
        }

        private Node DefineParent(int value)
        {
            try
            {
                Node res = ParentNode;
                
                // Looping until we reach a node, that doesn't have any children
                // Parent node at first doesn't have one, so second time we call this function, it'll always
                // return parent node, so additional check by value needed
                while (!(res.LeftChild == null && res.RightChild == null))
                {
                    if (res.Value > value)
                    {
                        if (res.LeftChild != null)
                        {
                            res = res.LeftChild;
                        }
                        // If our value should be on left size and current node doesn't have left child, we stop
                        // Now our parent will be current node
                        else
                        {
                            break;
                        }
                    }
                    else if (res.Value < value)
                    {
                        if (res.RightChild != null)
                        {
                            res = res.RightChild;
                        }
                        // If our value should be on right size and current node doesn't have right child, we stop
                        // Now our parent will be current node
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        res = null;
                        break;
                    }
                }

                return res;
            }
            // To catch a situation, when parent node doesn't exist
            catch (NullReferenceException e)
            {
                throw e;
            }
        }

        public bool AddItem(int value)
        {
            Node newItem;
            
            try
            {
                Node newItemParent = DefineParent(value);

                if (newItemParent != null)
                {
                    newItem = new Node(newItemParent, value);

                    if (newItemParent.Value > value)
                    {
                        newItemParent.LeftChild = newItem;
                    }
                    else if (newItemParent.Value < value)
                    {
                        newItemParent.RightChild = newItem;
                    }
                    else
                    {
                        // Cannot add existing item
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                // First item in the tree
                newItem = new Node(null, value);
                ParentNode = newItem;
            }
            
            Nodes.Add(newItem);
            return true;
        }

        public void RemoveItem(int value)
        {
            foreach (Node node in Nodes)
            {
                if (node.Value == value)
                {
                    Node rightChild = node.RightChild;
                    Node leftChild = node.LeftChild;
                    Node parent = node.Parent;
                    Node lastChild = rightChild;
                    
                    Nodes.Remove(node);

                    // If we're not trying to delete parent node
                    if (parent != null)
                    {
                        // Shifting current node's right child into nodes place
                        if (parent.LeftChild == node)
                        {
                            // If current node doesn't have right child, just shifting left child
                            if (rightChild == null)
                            {
                                parent.LeftChild = leftChild;
                                if (leftChild != null)
                                {
                                    leftChild.Parent = parent;
                                }
                                return;
                            }
                        
                            parent.LeftChild = rightChild;
                            rightChild.Parent = parent;
                        }
                        else
                        {
                            // If current node doesn't have right child, just shifting left child
                            if (rightChild == null)
                            {
                                parent.RightChild = leftChild;
                                if (leftChild != null)
                                {
                                    leftChild.Parent = parent;
                                }
                                return;
                            }
                        
                            parent.RightChild = rightChild;
                            rightChild.Parent = parent;
                        }
                        
                        // Finding the smallest node in right side to add left side to it
                        while (lastChild != null)
                        {
                            if (lastChild.LeftChild != null)
                            {
                                lastChild = lastChild.LeftChild;
                            }
                            else
                            {
                                break;
                            }
                        }
                    
                        lastChild.LeftChild = leftChild;
                        if (leftChild != null)
                        {
                            leftChild.Parent = lastChild;
                        }
                    }
                    else
                    {
                        if (rightChild != null)
                        {
                            ParentNode = rightChild;
                            rightChild.Parent = null;
                            
                            // Finding the smallest node in right side to add left side to it
                            while (lastChild != null)
                            {
                                if (lastChild.LeftChild != null)
                                {
                                    lastChild = lastChild.LeftChild;
                                }
                                else
                                {
                                    break;
                                }
                            }
                    
                            lastChild.LeftChild = leftChild;
                            if (leftChild != null)
                            {
                                leftChild.Parent = lastChild;
                            }
                        }
                        // Just shifting left child
                        else if (leftChild != null)
                        {
                            ParentNode = leftChild;
                            leftChild.Parent = null;
                        }
                        // Just deleting parent node with no children
                        else
                        {
                            ParentNode = null;
                        }
                    }

                    // If we don't force stop of the loop, it'll continue and give error
                    break;
                }
            }
        }
    }
}
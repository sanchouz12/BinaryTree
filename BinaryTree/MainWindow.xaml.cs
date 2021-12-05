using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BinaryTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int CircleWidth = 40;
        private const int CircleHeight = 40;
        private const int CirclePadding = 20;
        private double _startX = 20;
        private double _startY = 20;
        private readonly double _leftBorderX;
        private readonly double _topBorderY;
        private readonly double _rightBorderX;
        private readonly double _bottomBorderY;
        private bool _isMoving;
        private Point _mouseDownPos;
        
        private readonly Forest _forest;
        
        public MainWindow()
        {
            Random rand = new Random();
            
            InitializeComponent();
            _forest = new Forest();
            _leftBorderX = 0;
            _topBorderY = 0;
            _rightBorderX = Canv.Width;
            _bottomBorderY = Canv.Height;

            for (int i = 0; i < 1000; i++)
            {
                _forest.AddItem(rand.Next(-5000, 5000));
            }
            DrawForest();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_forest.AddItem(IntegerUpDown.Value.GetValueOrDefault())
                ? "New item was successfully added"
                : "This item was already added to the tree");
            
            DrawForest();
        }

        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            _forest.RemoveItem(IntegerUpDown.Value.GetValueOrDefault());

            MessageBox.Show("Item was removed");
            
            DrawForest();
        }

        private void DrawTreeNode(double x, double y, Node node)
        {
            Ellipse ellipse = new Ellipse
            {
                // Inside color
                Fill = new SolidColorBrush(Colors.Blue),
                // Border (???)
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                // Size
                Width = CircleWidth,
                Height = CircleHeight
            };
            Label text = new Label
            {
                Content = $"{node.Value}",
                FontSize = 12,
                Foreground = Brushes.White
            };
            text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size textSize = text.DesiredSize;
            double textX = x + (CircleWidth - 2) / 2.0 - textSize.Width / 2;
            double textY = y + (CircleHeight - 2) / 2.0 - textSize.Height / 2;

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            Canvas.SetLeft(text, textX);
            Canvas.SetTop(text, textY);
            
            Canv.Children.Add(ellipse);
            Canv.Children.Add(text);
        }

        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            Canv.Children.Add(new Line
            {
                X1 = x1, X2 = x2, Y1 = y1, Y2 = y2,
                Stroke = Brushes.Black, StrokeThickness = 1
            });
        }

        private int CountRightChildren(Node currentNode)
        {
            int count = 0;
            if (currentNode.RightChild != null)
            {
                count++;
                count += CountRightChildren(currentNode.RightChild);
            }

            if (currentNode.LeftChild != null)
            {
                count += CountRightChildren(currentNode.LeftChild);
            }

            return count;
        }

        private bool CheckBounds(double x, double y)
        {
            bool testX = (x  >= _leftBorderX - CircleWidth + 1) && (x  <= _rightBorderX - 1);
            // It just works
            bool testY = (y >= _topBorderY - CircleHeight + 1) && (y <= _bottomBorderY - CircleHeight - 1);

            return testX && testY;
        }

        private void DrawForestRecur(double x, double y, Node node, ref int renderingCount)
        {
            // Checking if we need to render the node
            if (CheckBounds(x, y))
            {
                DrawTreeNode(x, y, node);
                renderingCount++;
            }

            if (node.LeftChild != null)
            {
                double lx = x,
                    ly = y + CircleHeight + CirclePadding;
                
                DrawLine(
                    x + CircleWidth / 2.0, y + CircleHeight,
                    lx + CircleWidth / 2.0, ly);
                DrawForestRecur(lx, ly, node.LeftChild, ref renderingCount);
            }
            if (node.RightChild != null)
            {
                int rightChildrenCount = node.LeftChild == null ? 0 : CountRightChildren(node.LeftChild);
                double rx = x + CircleWidth + CirclePadding +
                            (CircleWidth + CirclePadding) * rightChildrenCount,
                    ry = y + CircleHeight + CirclePadding;
                
                DrawLine(
                    x + CircleWidth / 2.0, y + CircleHeight,
                    rx + CircleWidth / 2.0, ry);
                DrawForestRecur(rx, ry, node.RightChild, ref renderingCount);
            }
        }

        private void DrawForest()
        {
            Canv.Children.Clear();
            
            Node parentNode = _forest.GetParentNode();
            int renderingCount = 0;

            // parentNode can be null, if tree doesn't have any values
            if (parentNode != null)
            {
                DrawForestRecur(_startX, _startY, parentNode, ref renderingCount);
            }
            
            Debug.Print($"Rendering count: {renderingCount}");
        }

        private void Canv_OnMouseMove(object sender, MouseEventArgs e)
        {
            // Checking left button to prevent locking in moving state
            if (_isMoving && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currPos = e.GetPosition(Canv);
                double dx = _mouseDownPos.X - currPos.X;
                double dy = _mouseDownPos.Y - currPos.Y;

                // With + it'll have inverse movement
                _startX -= dx;
                _startY -= dy;
                _mouseDownPos = currPos;

                DrawForest();
            }
        }

        private void Canv_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMoving = true;
            _mouseDownPos = e.GetPosition(Canv);
        }

        private void Canv_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMoving = false;
        }
    }
}
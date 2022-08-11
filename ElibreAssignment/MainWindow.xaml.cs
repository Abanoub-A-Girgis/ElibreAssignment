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
using System.Windows.Ink;

namespace ElibreAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += KeyDownHandler;
        }

        // Bool to define whether the user is drawing or not
        bool drawing = false;

        bool MouseNotOverChild = true;
        // Empty Point to hold initial mouse click location
        Point StartPoint = new Point();
        // Empty Point to hold final mouse click location
        Point EndPoint = new Point();
        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Right Click Events
            if (e.RightButton == MouseButtonState.Pressed)
            {
                drawing = false;
                //Removes any remaining guideline  on screen
                var GuideLines = DrawingCanvas.Children.Cast<UIElement>().Where(c => c.Uid.Contains("GuideLine")).ToList();
                if (GuideLines.Count() > 0)
                    foreach (var GL in GuideLines)
                    {
                        DrawingCanvas.Children.Remove(GL);

                    }

            }
            // Left Click Events
            if (e.LeftButton == MouseButtonState.Pressed && MouseNotOverChild)
            { 
                if (drawing == false)
                {
                    StartPoint = e.GetPosition(this);
                    drawing = true;

                }
                else
                {
                    //DrawingCanvas.Children.RemoveAt(DrawingCanvas.Children.Count - 1);
                    Line WallLine = new Line();

                    WallLine.Stroke = Brushes.Black;
                    WallLine.StrokeThickness = 2;
                    WallLine.Uid = $"WallLine {StartPoint.X} {StartPoint.Y}";

                    WallLine.X1 = StartPoint.X;
                    WallLine.Y1 = StartPoint.Y;
                    WallLine.X2 = EndPoint.X;
                    WallLine.Y2 = EndPoint.Y;
                    WallLine.MouseEnter += HighlightWalls;
                    WallLine.MouseLeave += UnHighlightWalls;
                    WallLine.MouseDown += DrawDoors;

                    DrawingCanvas.Children.Add(WallLine);

                    StartPoint.X = EndPoint.X;
                    StartPoint.Y = EndPoint.Y;

                }
            }
        }
        
        // Method to Highlight walls blue on hover
        private void HighlightWalls(object sender, MouseEventArgs e)
        {
            MouseNotOverChild = false;
            //Debugging.Text = e.GetPosition(this).X.ToString();
            ((Line)sender).Stroke = Brushes.Blue;

        }
        // Method to Change the color back to Black once mouse leaves the wall line
        private void UnHighlightWalls(object sender, MouseEventArgs e)
        {
            MouseNotOverChild = true;
            //Debugging.Text = e.GetPosition(this).X.ToString();
            ((Line)sender).Stroke = Brushes.Black;

        }


        //Method to Draw Doors
        private void DrawDoors(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Debugging.Text = e.GetPosition(this).X.ToString();
                Line SenderAsLine = ((Line)sender);
                if (SenderAsLine.Y1 == SenderAsLine.Y2)
                {
                    Path path = new Path();
                    path.Stroke = Brushes.Black;
                    path.StrokeThickness = 2;
                    path.Uid = "Horizontal " + Guid.NewGuid().ToString();
                    GeometryGroup geometryGroup = new GeometryGroup();
                    PathGeometry pathGeometry = new PathGeometry();
                    PathFigure pathFigure = new PathFigure();

                    ArcSegment arcSegment = new ArcSegment();
                    //arcSegment.IsLargeArc = false; //default value is flase
                    pathFigure.StartPoint = new Point(e.GetPosition(this).X + 50, SenderAsLine.Y1);
                    arcSegment.Point = new Point(e.GetPosition(this).X + 4, SenderAsLine.Y1 + 50);
                    arcSegment.Size = new Size(100, 100);
                    arcSegment.SweepDirection = SweepDirection.Clockwise;

                    Rect rect = new Rect(e.GetPosition(this).X, SenderAsLine.Y1, 4, 50);
                    pathFigure.Segments.Add(arcSegment);
                    pathGeometry.Figures.Add(pathFigure);
                    geometryGroup.Children.Add(pathGeometry);
                    geometryGroup.Children.Add(new RectangleGeometry(rect)); ;

                    path.Data = geometryGroup;
                    Canvas.SetZIndex(path, 2);
                    DrawingCanvas.Children.Add(path);

                    //Drawing an invisble rectangle over the Door to handle click events
                    Rectangle rectangle = new Rectangle
                    {
                        Width = 50,
                        Height = 54,
                        Stroke = Brushes.Transparent,
                        StrokeThickness = 0,
                        Fill = Brushes.White
                    };
                    rectangle.Uid = path.Uid;

                    Canvas.SetLeft(rectangle, e.GetPosition(this).X);
                    Canvas.SetTop(rectangle, SenderAsLine.Y1 - 2);
                    Canvas.SetZIndex(rectangle, 1);

                    DrawingCanvas.Children.Add(rectangle);
                    rectangle.MouseDown += DrawWindows;
                    rectangle.MouseEnter += HighlightDoors;
                    rectangle.MouseLeave += UnHighlightDoors;

                }
                else
                {
                    Path path = new Path();
                    path.Stroke = Brushes.Black;
                    path.StrokeThickness = 2;
                    path.Uid = "Vertical " + Guid.NewGuid().ToString();
                    GeometryGroup geometryGroup = new GeometryGroup();
                    PathGeometry pathGeometry = new PathGeometry();
                    PathFigure pathFigure = new PathFigure();

                    ArcSegment arcSegment = new ArcSegment();
                    //arcSegment.IsLargeArc = false; //default value is flase
                    pathFigure.StartPoint = new Point(SenderAsLine.X1, e.GetPosition(this).Y + 50);
                    arcSegment.Point = new Point(SenderAsLine.X1 + 50, e.GetPosition(this).Y + 4);
                    arcSegment.Size = new Size(100, 100);
                    arcSegment.SweepDirection = SweepDirection.Counterclockwise;

                    Rect rect = new Rect(SenderAsLine.X1, e.GetPosition(this).Y, 50, 4);
                    pathFigure.Segments.Add(arcSegment);
                    pathGeometry.Figures.Add(pathFigure);
                    geometryGroup.Children.Add(pathGeometry);
                    geometryGroup.Children.Add(new RectangleGeometry(rect)); ;

                    path.Data = geometryGroup;
                    Canvas.SetZIndex(path, 2);
                    DrawingCanvas.Children.Add(path);

                    //Drawing an invisble rectangle over the Door to handle click events
                    Rectangle rectangle = new Rectangle
                    {
                        Width = 54,
                        Height = 50,
                        Stroke = Brushes.Transparent,
                        StrokeThickness = 0,
                        Fill = Brushes.White
                    };
                    rectangle.Uid = path.Uid;

                    Canvas.SetLeft(rectangle, SenderAsLine.X1 - 2);
                    Canvas.SetTop(rectangle, e.GetPosition(this).Y);
                    Canvas.SetZIndex(rectangle, 1);

                    DrawingCanvas.Children.Add(rectangle);
                    rectangle.MouseDown += DrawWindows;
                    rectangle.MouseEnter += HighlightDoors;
                    rectangle.MouseLeave += UnHighlightDoors;

                }
            }
        }

        // Method to Highlight doors blue on hover
        private void HighlightDoors(object sender, MouseEventArgs e)
        {
            MouseNotOverChild = false;
            //Debugging.Text = e.GetPosition(this).X.ToString();
            Path? path = DrawingCanvas.Children.OfType<Path>().FirstOrDefault(p => p.Uid == ((Rectangle)sender).Uid);
            if (path != null)
                path.Stroke = Brushes.Blue;
        }
        // Method to Change the color back to Black once mouse leaves the door
        private void UnHighlightDoors(object sender, MouseEventArgs e)
        {
            MouseNotOverChild = true;
            //Debugging.Text = e.GetPosition(this).X.ToString();
            Path? path = DrawingCanvas.Children.OfType<Path>().FirstOrDefault(p => p.Uid == ((Rectangle)sender).Uid);
            if (path != null)
                path.Stroke = Brushes.Black;
        }

        //Method to Draw Windows
        private void DrawWindows(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Debugging.Text = ((UIElement)sender).Uid;
                Rectangle SenderAsRectange = ((Rectangle)sender);
                //Logic to draw a path consisting of a gemotryGroup of an Arc and a Rectangle to simulate a door symbol
                if (SenderAsRectange.Uid.Contains("Horizontal"))
                {
                    Path path = new Path();
                    path.Stroke = Brushes.Black;
                    path.StrokeThickness = 2;
                    path.Uid = Guid.NewGuid().ToString();
                    GeometryGroup geometryGroup = new GeometryGroup();
                    path.Fill = Brushes.White;

                    Rect rect1 = new Rect(Canvas.GetLeft(SenderAsRectange), Canvas.GetTop(SenderAsRectange) - 3, 10, 10);
                    Rect rect2 = new Rect(Canvas.GetLeft(SenderAsRectange) + 10, Canvas.GetTop(SenderAsRectange) - 3, 30, 10);
                    Rect rect3 = new Rect(Canvas.GetLeft(SenderAsRectange) + 40, Canvas.GetTop(SenderAsRectange) - 3, 10, 10);
                    geometryGroup.Children.Add(new RectangleGeometry(rect1));
                    geometryGroup.Children.Add(new RectangleGeometry(rect2));
                    geometryGroup.Children.Add(new RectangleGeometry(rect3));
                    geometryGroup.Children.Add(new LineGeometry(new Point(rect2.TopLeft.X, rect2.TopLeft.Y + 5), new Point(rect2.TopRight.X, rect2.TopRight.Y + 5)));
                    path.Data = geometryGroup;
                    path.MouseEnter += HighlightWindows;
                    path.MouseLeave += UnHighlightWindows;

                    DrawingCanvas.Children.Add(path);

                }
                else
                {
                    Path path = new Path();
                    path.Stroke = Brushes.Black;
                    path.StrokeThickness = 2;
                    path.Uid = Guid.NewGuid().ToString();
                    GeometryGroup geometryGroup = new GeometryGroup();
                    path.Fill = Brushes.White;

                    Rect rect1 = new Rect(Canvas.GetLeft(SenderAsRectange) - 3, Canvas.GetTop(SenderAsRectange), 10, 10);
                    Rect rect2 = new Rect(Canvas.GetLeft(SenderAsRectange) - 3, Canvas.GetTop(SenderAsRectange) + 10, 10, 30);
                    Rect rect3 = new Rect(Canvas.GetLeft(SenderAsRectange) - 3, Canvas.GetTop(SenderAsRectange) + 40, 10, 10);
                    geometryGroup.Children.Add(new RectangleGeometry(rect1));
                    geometryGroup.Children.Add(new RectangleGeometry(rect2));
                    geometryGroup.Children.Add(new RectangleGeometry(rect3));
                    geometryGroup.Children.Add(new LineGeometry(new Point(rect2.TopLeft.X + 5, rect2.TopLeft.Y), new Point(rect2.BottomLeft.X + 5, rect2.BottomLeft.Y)));
                    path.Data = geometryGroup;
                    path.MouseEnter += HighlightWindows;
                    path.MouseLeave += UnHighlightWindows;

                    DrawingCanvas.Children.Add(path);
                }
                DeleteChild();
            }

        }

        // Method to Highlight Windows blue on hover
        private void HighlightWindows(object sender, MouseEventArgs e)
        {
            MouseNotOverChild = false;
            //Debugging.Text = e.GetPosition(this).X.ToString();
            ((Path)sender).Stroke = Brushes.Blue;
        }
        // Method to Change the color back to Black once mouse leaves the Windows
        private void UnHighlightWindows(object sender, MouseEventArgs e)
        {
            MouseNotOverChild = true;
            //Debugging.Text = e.GetPosition(this).X.ToString();
            ((Path)sender).Stroke = Brushes.Black;
        }


        //Method that draws Guidlines
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Checks if drawing mode is on and draws a guideline for the user
            if (drawing == true)
            {
                double angle;

                Line GuideLine = new Line();

                GuideLine.Stroke = Brushes.Gray;
                GuideLine.StrokeThickness = 1;
                GuideLine.Uid = "GuideLine";
                //Using Atan2 function to get the angle between the 2 points
                angle = Math.Atan2(StartPoint.Y - e.GetPosition(this).Y, StartPoint.X - e.GetPosition(this).X) * 180 / Math.PI;
                if (angle <= 45 && angle >= -45 || angle >= 135 || angle <= -135)
                {
                    GuideLine.X1 = StartPoint.X;
                    GuideLine.Y1 = StartPoint.Y;
                    EndPoint.X = e.GetPosition(this).X;
                    EndPoint.Y = StartPoint.Y;
                    GuideLine.X2 = EndPoint.X;
                    GuideLine.Y2 = EndPoint.Y;
                }
                else
                {
                    GuideLine.X1 = StartPoint.X;
                    GuideLine.Y1 = StartPoint.Y;
                    EndPoint.X = StartPoint.X;
                    EndPoint.Y = e.GetPosition(this).Y;
                    GuideLine.X2 = StartPoint.X;
                    GuideLine.Y2 = e.GetPosition(this).Y;
                }

                DrawingCanvas.Children.Add(GuideLine);
                //Debugging.Text = angle.ToString();
            }
            //Checks if 2 or more guidelines will be drawn and removes the older ones
            var GuideLines = DrawingCanvas.Children.Cast<UIElement>().Where( c => c.Uid.Contains("GuideLine")).SkipLast(1).ToList();
            if (GuideLines.Count() > 0)
                foreach( var GL in GuideLines)
                {
                    DrawingCanvas.Children.Remove(GL);
                }
                
            //Debugging.Text = DrawingCanvas.Children.Count.ToString();
        }

        //Method that handles KeyDowns
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteChild();
            }
        }

        //Method that deletes elements from Canvas
        private void DeleteChild()
        {
            var Elements = DrawingCanvas.Children.Cast<UIElement>().Where(c => c.Uid == ((UIElement)Mouse.DirectlyOver).Uid).ToList();
            if (Elements.Count() > 0)
                foreach (var element in Elements)
                {
                    DrawingCanvas.Children.Remove(element);
                }
        }

    }
}

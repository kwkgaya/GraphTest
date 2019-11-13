using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;

using Xamarin.Forms;

namespace GraphTest.Views
{
    public class GraphView : SKCanvasView
    {
        public readonly float[] Points = new float[] { 2, 10, 4, 8, 1, 8 };
        private static readonly SKPaint lineColor = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = SKColors.Blue, StrokeWidth = 2 };
        private static readonly SKPaint pointColor = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Black, StrokeWidth = 1 };

        public GraphView()
        {
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;

            PaintSurface += Redraw;
        }

        private void Redraw(object sender, SKPaintSurfaceEventArgs e)
        {
            var margin = 30;
            var canvas = e.Surface.Canvas;
            float width = e.Info.Width;
            float height = e.Info.Height;
            canvas.Clear();

            var range = Points.Max() - Points.Min() + 1;
            var dY = (height - margin * 2) / range;
            var dX = (width - margin * 2) / (Points.Length - 1);
            float GetY(float y) => margin + (dY * y);

            float x = margin;

            for (int i = 0; i < Points.Length; i++) 
            {
                float yCurrent = GetY(Points[i]);

                if (i > 0)
                {
                    float yPrevious = GetY(Points[i - 1]);
                    canvas.DrawLine(x, yPrevious, x += dX, yCurrent, lineColor);
                }

                canvas.DrawCircle(x, yCurrent, 8, pointColor);
            }
        }
    }
}
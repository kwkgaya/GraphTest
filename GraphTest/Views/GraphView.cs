using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace GraphTest.Views
{
    public class GraphView : SKCanvasView
    {
        private const float PointCircleRadius = 4f;
        private const float PathThickness = 2f;

        public readonly float[] Points = new float[] { 2, 10, 4, 8, 1, 8 };
        private static readonly SKPaint lineColor = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Blue, IsAntialias = true };
        private static readonly SKPaint pointColor = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = SKColors.Black, IsAntialias = true };
        private List<ToolTipView> toolTips = new List<ToolTipView>();

        public GraphView()
        {
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;

            PaintSurface += Redraw;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            toolTips.Clear();
            foreach (var point in Points)
            {
                var toolTip = new ToolTipView($"y: {point}");
                //((Grid)Parent).Children.Add(toolTip);
                toolTips.Add(toolTip);
            }
        }

        private void Redraw(object sender, SKPaintSurfaceEventArgs e)
        {
            var margin = 30;
            var canvas = e.Surface.Canvas;
            float width = e.Info.Width;
            float height = e.Info.Height;
            var scale = (float)(height / ((SKCanvasView)sender).Height);
            canvas.Clear();

            var range = Points.Max() - Points.Min() + 1;
            var dY = (height - margin * 2) / range;
            var dX = (width - margin * 2) / (Points.Length - 1);
            float GetY(float y) => margin + (dY * y);

            float x = margin;

            var path = new SKPath();
            var points = new List<SKPoint>();
            for (int i = 0; i < Points.Length; i++) 
            {
                float y = GetY(Points[i]);

                if (i == 0)
                    path.MoveTo(x , y);
                else
                    path.LineTo(x += dX, y);

                points.Add(new SKPoint(x, y));
            }

            lineColor.StrokeWidth = PathThickness * scale;

            canvas.DrawPath(path, lineColor);

            for (int i = 0; i < points.Count; i++)
            {
                SKPoint point = points[i];
                canvas.DrawCircle(point, scale * PointCircleRadius, pointColor);
                toolTips[i].Redraw(point, e.Surface.Canvas, scale);
            }
        }
    }
}
using SkiaSharp;

namespace GraphTest.Views
{
    public class ToolTipView
    {
        private static float Scale = 1;
        private static float TriangleWidth => 4f * Scale;
        private static float ValueLabelPadding => 5f * Scale;
        private static float ValueLabelFontSize => 14f * Scale;
        private static float ToolTipCornerRadius => 4f * Scale;
        private static float BorderThickness => 1f * Scale;

        private readonly static SKPaint FramePaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        };

        private readonly static SKPaint FillPaint = new SKPaint
        {
            Color = SKColors.LightGray,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true
        };

        SKPaint TextPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextAlign = SKTextAlign.Left,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName(
    "Arial",
    SKFontStyleWeight.ExtraBold,
    SKFontStyleWidth.Normal,
    SKFontStyleSlant.Upright)
        };

        public string Text { get; set; }

        public ToolTipView(string text)
        {
            Text = text;
        }

        public void Redraw(SKPoint tipPoint, SKCanvas canvas, float scale)
        {
            Scale = scale;

            // Not creating new paint objects for each draw, therefore scale the properties here.
            TextPaint.TextSize = ValueLabelFontSize;
            FramePaint.StrokeWidth = BorderThickness;

            // Move the tip little away from the dot
            var tipX = tipPoint.X + 8 * scale;
            var tipY = tipPoint.Y;

            SKRect frameRect = new SKRect();
            TextPaint.MeasureText(Text, ref frameRect);
            var textHeight = frameRect.Top;
            // Inflate for padding
            frameRect.Inflate(ValueLabelPadding, ValueLabelPadding);


            // Start from the tip then move clockwise. Triangle is on negetive side
            //                 |    
            //               c1|     ------------------------------     . c2
            //                 |    /                               \
            //                 |   /                                 \
            //                 |  /                                   \
            //                 | /                                     \
            //                 |                                       |
            //                 |                                       |
            //                 |                                       |
            //                 |                                       |
            //                /|                                       |
            //               / |                                       |
            //              /  |                                       |
            //             /   |                                       |
            //---------------------------------------------------------|---------------------
            //             \   |                                       |
            //              \  |                                       |


            SKPoint GetPoint(float x, float y) => new SKPoint(tipX + x + TriangleWidth, tipY + y);

            using (var path = new SKPath())
            {
                var corners = new SKPoint[] {
                    GetPoint(0, -frameRect.Height / 2),
                    GetPoint(frameRect.Width, -frameRect.Height / 2),
                    GetPoint(frameRect.Width, frameRect.Height / 2),
                    GetPoint(0, frameRect.Height / 2)
                };

                path.MoveTo(GetPoint(-TriangleWidth, 0));
                path.LineTo(GetPoint(0, -TriangleWidth));
                path.ArcTo(corners[0], corners[1], ToolTipCornerRadius);
                path.ArcTo(corners[1], corners[2], ToolTipCornerRadius);
                path.ArcTo(corners[2], corners[3], ToolTipCornerRadius);
                path.ArcTo(corners[3], corners[0], ToolTipCornerRadius);
                path.LineTo(GetPoint(0, TriangleWidth));
                path.Close();
                canvas.DrawPath(path, FillPaint);
                canvas.DrawPath(path, FramePaint);
            }

            var xText = tipX + TriangleWidth + ValueLabelPadding;
            var yText = tipY - frameRect.Height / 2 + ValueLabelPadding - textHeight;

            canvas.DrawText(Text, xText, yText, TextPaint);
        }
    }
}
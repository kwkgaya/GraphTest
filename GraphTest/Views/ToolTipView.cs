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
            Style = SKPaintStyle.Fill,
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
            var xFrame = tipX + TriangleWidth;
            var yFrame = tipY - frameRect.Height / 2;
            frameRect.Location = new SKPoint(xFrame, yFrame);

            canvas.DrawRoundRect(frameRect, ToolTipCornerRadius, ToolTipCornerRadius, FillPaint);
            canvas.DrawRoundRect(frameRect, ToolTipCornerRadius, ToolTipCornerRadius, FramePaint);

            using (var path = new SKPath())
            {
                SKPoint GetPoint(int xFactor, int yFactor) => new SKPoint(tipX + (xFactor * TriangleWidth ), tipY + (yFactor * TriangleWidth));
                path.MoveTo(GetPoint(1, 1));
                path.LineTo(GetPoint(0, 0));
                path.LineTo(GetPoint(1, -1));
                canvas.DrawPath(path, FillPaint);
                canvas.DrawPath(path, FramePaint);
            }

            var xText = xFrame + ValueLabelPadding;
            var yText = yFrame + ValueLabelPadding - textHeight;

            canvas.DrawText(Text, xText, yText, TextPaint);
        }
    }
}
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Flow.Editor
{
    public class FlowGrid : VisualElement
    {
        private const float SmallGrid = 20f;
        private const float LargeGrid = 100f;

        private readonly FlowViewTransform viewTransform;

        public FlowGrid(FlowViewTransform viewTransform)
        {
            this.viewTransform = viewTransform;

            style.position = Position.Absolute;
            style.left = 0;
            style.right = 0;
            style.top = 0;
            style.bottom = 0;

            pickingMode = PickingMode.Ignore;

            generateVisualContent += OnGenerateVisualContent;
        }

        // Meminta grid digambar ulang.
        // Dipanggil setiap Pan atau Zoom berubah.
        public void Refresh()
        {
            MarkDirtyRepaint();
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            Painter2D painter = ctx.painter2D;

            float scale = viewTransform.Scale;

            DrawGrid(
                painter,
                contentRect,
                SmallGrid * scale,
                new Color(0.22f, 0.22f, 0.22f));

            DrawGrid(
                painter,
                contentRect,
                LargeGrid * scale,
                new Color(0.30f, 0.30f, 0.30f));
        }

        private void DrawGrid(
            Painter2D painter,
            Rect rect,
            float spacing,
            Color color)
        {
            // Jangan menggambar jika spacing terlalu kecil.
            if (spacing < 6f)
                return;

            painter.strokeColor = color;
            painter.lineWidth = 1f;

            float startX = Mathf.Repeat(viewTransform.Position.x, spacing);

            for (float x = startX; x < rect.width; x += spacing)
            {
                painter.BeginPath();
                painter.MoveTo(new Vector2(x, 0));
                painter.LineTo(new Vector2(x, rect.height));
                painter.Stroke();
            }

            float startY = Mathf.Repeat(viewTransform.Position.y, spacing);

            for (float y = startY; y < rect.height; y += spacing)
            {
                painter.BeginPath();
                painter.MoveTo(new Vector2(0, y));
                painter.LineTo(new Vector2(rect.width, y));
                painter.Stroke();
            }
        }
    }
}
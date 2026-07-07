using UnityEngine;

namespace Game.Flow.Editor
{
    // Menyimpan transformasi tampilan Flow Editor.
    // Seluruh layer (Grid, Node, Connection)
    // menggunakan transform ini.
    public class FlowViewTransform
    {
        // Posisi pan editor.
        public Vector2 Position = Vector2.zero;

        // Zoom editor.
        public float Scale = 1f;
    }
}
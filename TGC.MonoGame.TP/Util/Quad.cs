using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Util
{
    internal class Quad
    {
        private static VertexPositionTexture[] vertices =
        [
    // Posiciones Normalizadas 
    new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
    new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),
    new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
    new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1))
        ];

        private static short[] indices = { 0, 1, 2, 2, 1, 3 };

        public static VertexPositionTexture[] GetVertices()
        {
            return vertices;
        }

        public static short[] GetIndices()
        {
            return indices;
        }
    }
}
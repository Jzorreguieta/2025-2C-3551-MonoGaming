using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGaming.TP.Models.Obstacles
{
    internal class Ship
    {
        private Matrix _worldMatrix;
        private Model _model;
        private BoundingBox _boundingBox; 
        private BoundingBox _worldBoundingBox;

        private const float SCALE = 0.02f;

        public BoundingBox BoundingBox => _worldBoundingBox;

        public Ship(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
        {
            var rotation = Matrix.CreateRotationY(MathHelper.ToRadians(90));
            _worldMatrix = rotation * worldMatrix;
            _model = content.Load<Model>(contentFolder3D + "Nave_1/Nave_1");
            var effect = content.Load<Effect>(contentFolderEffects + "BasicShader");

            foreach (var mesh in _model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    var meshEffect = effect.Clone();
                    meshPart.Effect = meshEffect;
                    meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.LightBlue.ToVector3());
                }
            }


            _boundingBox = CalculateBoundingBox(_model);
            UpdateWorldBoundingBox(); 
        }

        private BoundingBox CalculateBoundingBox(Model model)
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            foreach (var mesh in model.Meshes)
            {
                var transforms = mesh.ParentBone.Transform;
                foreach (var part in mesh.MeshParts)
                {
                    var vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(vertexData);

                    foreach (var vertex in vertexData)
                    {
                        var transformed = Vector3.Transform(vertex.Position, transforms);
                        min = Vector3.Min(min, transformed);
                        max = Vector3.Max(max, transformed);
                    }
                }
            }

            return new BoundingBox(min, max);
        }

        private void UpdateWorldBoundingBox()
        {
            // Escala el bounding box
            var scaleMatrix = Matrix.CreateScale(SCALE);
            var worldTransform = scaleMatrix * _worldMatrix;

            // Transformar los 8 puntos del bounding box local al mundo
            var corners = _boundingBox.GetCorners();
            var transformedCorners = new Vector3[corners.Length];
            for (int i = 0; i < corners.Length; i++)
                transformedCorners[i] = Vector3.Transform(corners[i], worldTransform);

            _worldBoundingBox = BoundingBox.CreateFromPoints(transformedCorners);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (var mesh in _model.Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform;
                var scaleMatrix = Matrix.CreateScale(SCALE);
                var world = meshWorld * scaleMatrix * _worldMatrix;

                foreach (var meshPart in mesh.MeshParts)
                {
                    var effect = meshPart.Effect;
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["World"].SetValue(world);
                }
                mesh.Draw();
            }
        }

        public void SetWorldMatrix(Matrix newWorld)
        {
            _worldMatrix = newWorld;
            UpdateWorldBoundingBox();
        }
    }
}

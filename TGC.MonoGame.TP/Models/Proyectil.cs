using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGaming.TP.Models.Modules;
using TGC.MonoGaming.TP.Util;

namespace TGC.MonoGaming.TP.Models
{
    internal class Proyectil
    {
        private Matrix _worldMatrix;
        private Model _model;
        private const float SCALE = 0.05f;
        private const float VELOCIDAD = 58.5f;

        public bool estaDestruido = false;
        private BoundingBox _boundingBoxLocal;
        private BoundingBox _boundingBoxWorld;
        public BoundingBox BoundingBox => _boundingBoxWorld;



        public Proyectil(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
        {
            _worldMatrix = worldMatrix;

            _model = content.Load<Model>(contentFolder3D + "Caja_1/Caja_1");

            var effect = content.Load<Effect>(contentFolderEffects + "BasicShader");

            foreach (var mesh in _model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    var meshEffect = effect.Clone();
                    meshPart.Effect = meshEffect;
                    meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                }
            }

            _boundingBoxLocal = CalculateBoundingBox(_model);
            UpdateBoundingBoxWorld();
        }

        private BoundingBox CalculateBoundingBox(Model model)
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            foreach (var mesh in model.Meshes)
            {
                var meshTransform = mesh.ParentBone.Transform;
                foreach (var meshPart in mesh.MeshParts)
                {
                    var vertexData = new VertexPositionNormalTexture[meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(vertexData);

                    foreach (var vertex in vertexData)
                    {
                        var transformed = Vector3.Transform(vertex.Position, meshTransform);
                        min = Vector3.Min(min, transformed);
                        max = Vector3.Max(max, transformed);
                    }
                }
            }

            return new BoundingBox(min, max);
        }

        private void UpdateBoundingBoxWorld(float reductionFactor = 1f)
        {
            // Aplica escala y mundo
            var scaleMatrix = Matrix.CreateScale(SCALE);
            var worldTransform = scaleMatrix * _worldMatrix;

            // Obtiene los 8 vértices del bounding box original
            var corners = _boundingBoxLocal.GetCorners();
            var transformedCorners = new Vector3[corners.Length];
            for (int i = 0; i < corners.Length; i++)
                transformedCorners[i] = Vector3.Transform(corners[i], worldTransform);

            // Calcula el centro real del bounding box transformado
            Vector3 center = Vector3.Zero;
            foreach (var v in transformedCorners)
                center += v;
            center /= transformedCorners.Length;

            // Reduce los vértices respecto al centro
            for (int i = 0; i < transformedCorners.Length; i++)
                transformedCorners[i] = center + (transformedCorners[i] - center) * reductionFactor;

            // Crea el bounding box final
            _boundingBoxWorld = BoundingBox.CreateFromPoints(transformedCorners);
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
            UpdateBoundingBoxWorld();
        }


        public void Update(GameTime gameTime)
        {
            var nuevoMovimiento = Vector3.Left * 4 * VELOCIDAD * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _worldMatrix = _worldMatrix * Matrix.CreateTranslation(nuevoMovimiento);
            UpdateBoundingBoxWorld();

        }

        public void Destroy()
        {
            estaDestruido = true;
        }

    }
}

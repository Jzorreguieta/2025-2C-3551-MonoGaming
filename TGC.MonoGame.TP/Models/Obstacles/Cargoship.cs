using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGaming.TP.Models.Obstacles
{
    internal class CargoShip
    {
        private Matrix _worldMatrix;
        private Model _model;

        private float scale = 0.00045f;

        private const float ROTACION_MIN = 0f;
        private const float ROTACION_MAX = 45f;

        private const float VELOCIDAD_ROTACION_MIN = 60f;
        private const float VELOCIDAD_ROTACION_MAX = 90f;

        private float velocidadDeRotacion;

        private static Random random = new Random();
        private float rotacionX;
        private float rotacionY;

        // ✅ BoundingBox
        private BoundingBox _boundingBoxLocal;
        private BoundingBox _boundingBoxWorld;
        public BoundingBox BoundingBox => _boundingBoxWorld;

        public CargoShip(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
        {
            rotacionY = random.NextSingle() * (ROTACION_MAX - ROTACION_MIN) + ROTACION_MIN;
            rotacionX = random.NextSingle() * (ROTACION_MAX - ROTACION_MIN) + ROTACION_MIN;
            velocidadDeRotacion = random.NextSingle() * (VELOCIDAD_ROTACION_MAX - VELOCIDAD_ROTACION_MIN) + VELOCIDAD_ROTACION_MIN;

            _worldMatrix = worldMatrix;
            _model = content.Load<Model>(contentFolder3D + "Nave_2/Nave_2");
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

            // ✅ Calcular BoundingBox local
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

        private void UpdateBoundingBoxWorld()
        {
            var scaleMatrix = Matrix.CreateScale(scale);
            var rotationMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(rotacionX)) *
                                 Matrix.CreateRotationY(MathHelper.ToRadians(rotacionY));
            var worldTransform = scaleMatrix * rotationMatrix * _worldMatrix;

            var corners = _boundingBoxLocal.GetCorners();
            var transformedCorners = new Vector3[corners.Length];

            for (int i = 0; i < corners.Length; i++)
                transformedCorners[i] = Vector3.Transform(corners[i], worldTransform);

            _boundingBoxWorld = BoundingBox.CreateFromPoints(transformedCorners);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var rotationXMat = Matrix.CreateRotationX(MathHelper.ToRadians(rotacionX));
            var rotationYMat = Matrix.CreateRotationY(MathHelper.ToRadians(rotacionY));

            foreach (var mesh in _model.Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform;
                var scaleMatrix = Matrix.CreateScale(scale);
                var world = meshWorld * rotationYMat * rotationXMat * scaleMatrix * _worldMatrix;

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

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotacionX += velocidadDeRotacion * deltaTime;

            // ✅ Actualizar bounding box en cada frame
            UpdateBoundingBoxWorld();
        }

        // Si en algún momento movés el objeto, también actualizarías _worldMatrix y llamás UpdateBoundingBoxWorld()
        public void SetWorldMatrix(Matrix newWorld)
        {
            _worldMatrix = newWorld;
            UpdateBoundingBoxWorld();
        }
    }
}

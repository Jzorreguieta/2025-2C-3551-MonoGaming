using System.Linq;
using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  TGC.MonoGaming.TP.Models.Obstacles;

internal class Box 
{
    private Matrix _worldMatrix;
    private Model _model;

    private Matrix _rotation;

    public Box(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix, float angle)
    {

        _worldMatrix = worldMatrix;
        _rotation = Matrix.CreateRotationX(MathHelper.ToRadians(angle));
        _model = content.Load<Model>(contentFolder3D + "Caja_1/Caja_1");
        var effect = content.Load<Effect>(contentFolderEffects + "BasicShader");

        foreach (var mesh in _model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                var meshEffect = effect.Clone();
                meshPart.Effect = meshEffect;
                meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Violet.ToVector3());
            }
        }
    }


    public void Draw(Matrix view, Matrix projection)
    {
        float scale = 0.05f;
        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        
        foreach (var mesh in _model.Meshes)
        {
            var meshWorld = mesh.ParentBone.Transform;
            var scaleMatrix = Matrix.CreateScale(scale);
            var world = meshWorld  * _rotation * scaleMatrix * _worldMatrix ;

            foreach (var meshPart in mesh.MeshParts)
            {
                var effect = meshPart.Effect;
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["World"].SetValue(world);
            }
            // Draw the mesh.
            mesh.Draw();
        }
    }
}
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  TGC.MonoGame.TP;

internal class Box 
{
    private Matrix _worldMatrix;
    private Model _model;

    public Box(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix, float angle)
    {

        //var rotation = Matrix.CreateRotationY(MathHelper.ToRadians(90));

        _worldMatrix = worldMatrix * Matrix.CreateRotationZ(MathHelper.ToRadians(angle));
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
        float scale = 0.03f;
        
        foreach (var mesh in _model.Meshes)
        {
            var meshWorld = mesh.ParentBone.Transform;
            var scaleMatrix = Matrix.CreateScale(scale);
            var world = meshWorld * _worldMatrix * scaleMatrix;
            foreach (var meshPart in mesh.MeshParts )
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
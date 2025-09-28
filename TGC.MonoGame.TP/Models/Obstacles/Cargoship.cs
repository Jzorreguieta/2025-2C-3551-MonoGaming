using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  TGC.MonoGaming.TP.Models.Obstacles;

internal class CargoShip 
{
    private Matrix _worldMatrix;
    private Model _model;
    public CargoShip(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
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
    }


    public void Draw(Matrix view, Matrix projection)
    {
        float scale = 0.010f;
        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        
        foreach (var mesh in _model.Meshes)
        {
            var meshWorld = mesh.ParentBone.Transform;
            var scaleMatrix = Matrix.CreateScale(scale);
            var world = meshWorld * scaleMatrix * _worldMatrix ;

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

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  TGC.MonoGaming.TP.Models.Modules;

internal class BasicModule : IModule
{
    private Matrix _worldMatrix;
    private Model _model;


    public BasicModule(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
    {

        _worldMatrix = worldMatrix;
        _model = content.Load<Model>(contentFolder3D + "Pasillo/Pasillo");
        var effect = content.Load<Effect>(contentFolderEffects + "BasicShader");


        foreach (var mesh in _model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                var meshEffect = effect.Clone();
                meshPart.Effect = meshEffect;
            }
        }
    }


    public void Draw(Matrix view, Matrix projection)
    {
        float scale = 0.1f;
        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.

        foreach (var mesh in _model.Meshes)
        {
            var meshWorld = mesh.ParentBone.Transform;
            var scaleMatrix = Matrix.CreateScale(scale);
            var world = meshWorld * scaleMatrix * _worldMatrix;

            foreach (var meshPart in mesh.MeshParts)
            {
                var effect = meshPart.Effect;
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["World"].SetValue(world);
                effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            }
            // Draw the mesh.
            mesh.Draw();
        }
    }

    private void GenerateObstacles(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix) { }
    private void GenerateDecoration() { }


    public void Update(GameTime gameTime)
    {

    }

    public string Modulo()
    {
        return "Basic";
    }

}
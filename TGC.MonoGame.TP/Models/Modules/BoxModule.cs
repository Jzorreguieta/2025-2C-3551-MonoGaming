using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGaming.TP.Models.Obstacles;


namespace TGC.MonoGaming.TP.Models.Modules;

internal class BoxModule : IModule
{
    private Matrix _worldMatrix;
    private Model _model;
    private List<Box> obstacles;

    public BoxModule(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
    {

        _worldMatrix = worldMatrix;
        _model = content.Load<Model>(contentFolder3D + "Pasillo/Pasillo");
        var effect = content.Load<Effect>(contentFolderEffects + "BasicShader");

        this.GenerateObstacles(content, contentFolder3D, contentFolderEffects, worldMatrix);

        foreach (var mesh in _model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                var meshEffect = effect.Clone();
                meshPart.Effect = meshEffect;
            }
        }
    }

    public void GenerateDecoration()
    {
        //Deberia generar las decoraciones del modulo.
    }


    //Deberia generar  una pocision aleatoria con respecto del centro del modulo.
    //De momento se deja con una posicion fija.
    public void GenerateObstacles(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
    {
        obstacles = new List<Box>{
            new Box( content, contentFolder3D, contentFolderEffects,  worldMatrix * Matrix.CreateTranslation(Vector3.Left*3f + Vector3.Down*9f),60f),
            new Box( content, contentFolder3D, contentFolderEffects,  worldMatrix * Matrix.CreateTranslation(Vector3.Right*10f + Vector3.Up *3f), 45f)
        };
    }

    public void Draw(Matrix view, Matrix projection)
    {
        float scale = 0.1f;
        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.

        foreach (var mesh in _model.Meshes)
        {
            var meshWorld = mesh.ParentBone.Transform;
            var scaleMatrix = Matrix.CreateScale(scale);
            var world = meshWorld * _worldMatrix * scaleMatrix;
            foreach (var meshPart in mesh.MeshParts)
            {
                var effect = meshPart.Effect;
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["World"].SetValue(world);
                effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
            }
            // Draw the mesh.
            mesh.Draw();
        }

        foreach (Box box in obstacles)
        {
            box.Draw(view, projection);
        }
    }



    public void Update(GameTime gameTime)
    {

    }
}
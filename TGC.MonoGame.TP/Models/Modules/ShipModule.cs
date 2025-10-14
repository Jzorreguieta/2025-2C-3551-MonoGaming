using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGaming.TP.Models.Obstacles;
using TGC.MonoGaming.TP.Util;

namespace TGC.MonoGaming.TP.Models.Modules;

internal class ShipModule : IModule
{
    private Matrix _worldMatrix;
    private Model _model;
    private List<Ship> obstacles = new List<Ship>();

    //Medidas del Modulo
    private float scale = 0.1f;
    private readonly int Up = 6;
    private readonly int Rigth = 20;
    private readonly int Foward = 13;

    public ShipModule(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
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
        int cantidadMaximaDeObstaculos = 2;
        for(int index = 0; index < cantidadMaximaDeObstaculos; index++)
        {
            Matrix traslacionDeNave = Matrix.CreateTranslation(Vector3.Forward * GenerateNumber(this.Foward) + Vector3.Up * GenerateNumber(this.Up) + Vector3.Right * GenerateNumber(this.Rigth));
            obstacles.Add(new Ship( content, contentFolder3D, contentFolderEffects,  worldMatrix * traslacionDeNave ));
        }
        
    }

    public void Draw(Matrix view, Matrix projection)
    {

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
                effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
            }
            // Draw the mesh.
            mesh.Draw();
        }

        foreach (Ship ship in obstacles)
        {
            ship.Draw(view, projection);
        }
    }



    public void Update(GameTime gameTime, PlayerShip player, EscenarioGenerator generator, ref List<IModule> escenario)
    {
        foreach (var obstacle in obstacles)
            obstacle.Update(gameTime, player, generator, ref escenario);

        obstacles.RemoveAll(o => o.estaDestruido);
    }

    private float GenerateNumber(float x)
    {
        Random random = new Random();
        // random.NextDouble() devuelve un valor entre 0.0 y 1.0
        // Lo transformamos para que quede entre -x y +x
        return (float)((random.NextDouble() * 2 - 1) * x);
    }

    public  string Modulo()
    {
        return "Corridor";
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  TGC.MonoGame.TP;

internal class ShipModule : Module
{
    private Matrix _worldMatrix;
    private Model _model;
    private Effect _effect;
    private List<CargoShip> obstacles;
    private float scale = 0.1f;

    public ShipModule(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
    {

        var rotation = Matrix.CreateRotationY(MathHelper.ToRadians(90));

        _worldMatrix = worldMatrix * rotation;
        _model = content.Load<Model>(contentFolder3D + "Pasillo/Pasillo");
        _effect = content.Load<Effect>(contentFolderEffects + "BasicShader").Clone();

        this.GenerateObstacles(content, contentFolder3D, contentFolderEffects,  worldMatrix);

        foreach (var mesh in _model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = _effect;
                _effect.Parameters["DiffuseColor"].SetValue(Color.LightBlue.ToVector3());
            }
        }
    }

    public void GenerateDecoration(){
        //Deberia generar las decoraciones del modulo.
    }


    //Deberia generar  una pocision aleatoria con respecto del centro del modulo.
    //De momento se deja con una posicion fija.
    public void GenerateObstacles(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix){
        obstacles = new List<CargoShip>{
            new CargoShip( content, contentFolder3D, contentFolderEffects,  worldMatrix * Matrix.CreateTranslation(Vector3.Forward * 2)),
            new CargoShip( content, contentFolder3D, contentFolderEffects,  worldMatrix * Matrix.CreateTranslation(Vector3.Backward * 2))
        };
    }

    public void Draw(Matrix view, Matrix projection)
    {
        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        foreach (var mesh in _model.Meshes)
        {
        
            var meshWorld = mesh.ParentBone.Transform;
            var scaleMatrix = Matrix.CreateScale(scale);
            // We set the main matrices for mesh to draw.
            _effect.Parameters["World"].SetValue(meshWorld * _worldMatrix * scaleMatrix);

            // Draw the mesh.
            mesh.Draw();
        }
        foreach (CargoShip ship in obstacles){
            ship.Draw(view,projection);
        }
    }

    public void Update(GameTime gameTime){

    }
}
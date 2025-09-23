using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  TGC.MonoGame.TP;

internal class CargoShip 
{
    private Matrix _worldMatrix;
    private Model _model;
    private Effect _effect;

    public CargoShip(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix)
    {

        //var rotation = Matrix.CreateRotationY(MathHelper.ToRadians(90));

        _worldMatrix = worldMatrix;
        _model = content.Load<Model>(contentFolder3D + "Nave_1/Nave_1");
        _effect = content.Load<Effect>(contentFolderEffects + "BasicShader").Clone();
        


        foreach (var mesh in _model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = _effect;
                _effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            }
        }
    }


    public void Draw(Matrix view, Matrix projection)
    {
        float scale = 0.1f;
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
    }
}
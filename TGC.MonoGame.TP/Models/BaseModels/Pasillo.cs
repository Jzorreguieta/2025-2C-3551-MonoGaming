using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Models.BaseModels
{
    internal class Pasillo
    {
        private static Model _model = null;
        public static Model GetModel(ContentManager content)
        {
            if (_model == null)
            {
                _model = content.Load<Model>(MonoGaming.ContentFolder3D + "Pasillo/Pasillo");
                var texture = content.Load<Texture2D>(MonoGaming.ContentFolderTextures + "Pasillo/Pasillo_Tex");
                var effect = content.Load<Effect>(MonoGaming.ContentFolderEffects + "BasicShaderTexture");

                foreach (var mesh in _model.Meshes)
                {
                    foreach (var meshPart in mesh.MeshParts)
                    {
                        var meshEffect = effect.Clone();
                        meshPart.Effect = meshEffect;
                        meshPart.Effect.Parameters["Texture"].SetValue(texture);

                    }
                }

            }

            return _model;
        }
    }
}
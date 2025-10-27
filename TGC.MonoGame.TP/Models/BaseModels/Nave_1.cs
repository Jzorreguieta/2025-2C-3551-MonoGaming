using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Models.BaseModels
{
    internal class Nave_1
    {
        private static Model _model = null;
        public static Model GetModel(ContentManager content)
        {
            if (_model == null)
            {
                _model = content.Load<Model>(MonoGaming.ContentFolder3D + "Nave_1/Nave_1");
                var texture = content.Load<Texture2D>(MonoGaming.ContentFolderTextures + "Nave_1/Nave1_Diff");
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
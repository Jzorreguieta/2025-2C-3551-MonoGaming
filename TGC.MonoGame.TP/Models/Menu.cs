using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Models.BaseModels;
using TGC.MonoGame.TP.Util;

namespace TGC.MonoGame.TP.Models
{

    internal class Menu
    {
        private const float SCALE = 0.1f;

        Model _model;
        List<RectangleButton> _buttons;
        MouseState _previousMouse;
        MouseState _currentMouse;
        SpriteBatch spriteBatch;
        public Menu(ContentManager content, List<RectangleButton> buttons, SpriteBatch spriteBatch)
        {
            _model = Nave_1.GetModel(content);

            // Inicializa la lista de botones
            _buttons = buttons;
            this.spriteBatch = spriteBatch;
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            foreach (var button in _buttons)
            {
                button.Update(_previousMouse, _currentMouse);
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (var mesh in _model.Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform;
                var scaleMatrix = Matrix.CreateScale(SCALE);
                var world = meshWorld * scaleMatrix * Matrix.CreateFromYawPitchRoll(MathHelper.PiOver2,0,MathHelper.PiOver4);

                foreach (var meshPart in mesh.MeshParts)
                {
                    var effect = meshPart.Effect;
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["World"].SetValue(world);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                    }
                }
                mesh.Draw();
            }

            spriteBatch.Begin(blendState: BlendState.AlphaBlend); // ¡Importante activar AlphaBlend!
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
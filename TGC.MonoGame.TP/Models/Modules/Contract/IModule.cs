using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace  TGC.MonoGaming.TP.Models.Modules;

public interface IModule
{
    public void Draw(Matrix view, Matrix projection);
    public void Update(GameTime gameTime);
    private void GenerateObstacles(ContentManager content, string contentFolder3D, string contentFolderEffects, Matrix worldMatrix) { }//Esto deberia llamar a los generadores de los obstaculos del modulo.
    private void GenerateDecoration() { }//Esto deberia llamar a los generadores de las decoraciones y pocisionarlos.

    public string Modulo();
    //private void Destroy(){}
} 


using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using TGC.MonoGaming.TP.Models.Modules;

namespace TGC.MonoGaming.TP.Util;

internal class EscenarioGenerator
{
    private readonly Random rng = new Random();
    private const int MAX_MODULES = 20;
    private readonly Matrix inicio = Matrix.Identity;
    private int lastPosition;
    private string lastModule;
    ContentManager contentManager;
    string contentFolder3D;
    string contentFolderEffects;

    private const float DISTANCE_BETWEEN_MODULES = 56.5f;

    public EscenarioGenerator(ContentManager contentManager, string contentFolder3D, string contentFolderEffects)
    {
        this.contentManager = contentManager;
        this.contentFolder3D = contentFolder3D;
        this.contentFolderEffects = contentFolderEffects;
    }

    public void GenerarEscenario( ref List<IModule> escenario)
    {
        escenario = null;
            
        Matrix modulo2 = inicio * Matrix.CreateTranslation(Vector3.Left * DISTANCE_BETWEEN_MODULES);
        Matrix modulo3 = inicio * Matrix.CreateTranslation(Vector3.Left * DISTANCE_BETWEEN_MODULES * 2);
        escenario = new List<IModule>
        {
            new BasicModule(contentManager,contentFolder3D,contentFolderEffects, inicio),
            new BasicModule(contentManager,contentFolder3D,contentFolderEffects, modulo2),
            new BasicModule(contentManager,contentFolder3D,contentFolderEffects, modulo3),
        };
        lastModule = "Basic";
        for(int index = 3; index <= MAX_MODULES; index++)
        {
            Matrix worldMatrix = inicio * Matrix.CreateTranslation(Vector3.Left * DISTANCE_BETWEEN_MODULES * index);
            lastPosition = index;
            escenario.Add(SeleccionarModulo(worldMatrix));
        }
    }

    public void AvanzarEscenario( ref List<IModule> escenario, ContentManager contentManager, string contentFolder3D, string contentFolderEffects)
    {
        escenario.RemoveAt(0);
        Matrix worldMatrix = inicio * Matrix.CreateTranslation(Vector3.Left * DISTANCE_BETWEEN_MODULES * ++lastPosition);
        escenario.Add(SeleccionarModulo(worldMatrix));
    }
    
    public IModule SeleccionarModulo(Matrix worldMatrix)
    {
        //Seguramente haya una mejor manera de hacer las cosas , deberia refactorizar  esta clase.

        int randomNumber = rng.Next(0, 101);

        switch (lastModule)
        {
            case "Basic":
                if (randomNumber < 25)
                {
                    lastModule = "Corridor";
                    return new BoxModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                }
                else if (randomNumber < 50)
                {
                    lastModule = "Corridor";
                    return new ShipModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                }
                //else if (randomNumber < 75) return new BreakableModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                
                lastModule = "Asteroid";
                return new CargoModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
            
            case "Asteroid":
                if (randomNumber < 15)
                {
                    lastModule = "Corridor";
                    return new BoxModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                }
                else if (randomNumber < 30)
                {
                    lastModule = "Corridor";
                    return new ShipModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                }
                //else if (randomNumber < 75) return new BreakableModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                lastModule = "Asteroid";
                return new CargoModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);

            
            case "Corridor": 
                if (randomNumber < 35)
                {
                    lastModule = "Corridor";
                    return new BoxModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                }
                else if (randomNumber < 70)
                {
                    lastModule = "Corridor";
                    return new ShipModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                }
                //else if (randomNumber < 85) return new BreakableModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
                lastModule = "Asteroid";
                return new CargoModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);

            default:
                lastModule = "Basic";
                return new BasicModule(contentManager, contentFolder3D, contentFolderEffects, worldMatrix);
            
        }
        
    }
}
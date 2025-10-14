using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGaming.Samples.Cameras;
using TGC.MonoGaming.TP.Models.Modules;
using TGC.MonoGaming.TP.Util;



namespace TGC.MonoGame.TP;

public class MonoGaming : Game
{
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";

    private Camera DebugCamera { get; set; }

    private readonly GraphicsDeviceManager _graphics;

    private Microsoft.Xna.Framework.Matrix _projection;
    private Microsoft.Xna.Framework.Matrix _view;

    private List<IModule> escenario;
    private EscenarioGenerator escenarioGenerator;

    private const float VELOCIDAD = 20f;
    private float posicion = 0;
    
    float tiempoAcumulado = 0f;


    public MonoGaming()
    {
        // Maneja la configuracion y la administracion del dispositivo grafico.
        _graphics = new GraphicsDeviceManager(this);

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

        // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
        // Carpeta raiz donde va a estar toda la Media.
        Content.RootDirectory = "Content";
        // Hace que el mouse sea visible.
        IsMouseVisible = true;


        //Inicializo el escenario y su generador infinito
        escenarioGenerator = new EscenarioGenerator();
        escenario = null;
    }


    protected override void Initialize()
    {
        // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

        DebugCamera = new SimpleCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.UnitZ * 150, 400, 2.0f, 1, 3000);

        // Configuramos nuestras matrices de la escena.
        _view = Matrix.CreateLookAt(new Vector3(0, 0, 300), Vector3.Zero, Vector3.Up);
        _projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 2500);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        var worldMatrix_1 = Matrix.Identity * Matrix.CreateTranslation(Vector3.Left * 60.5f);
        var worldMatrix_2 = Matrix.Identity * Matrix.CreateTranslation(Vector3.Left * 60.5f * 2);

        //Se genera el escenario.
        escenarioGenerator.GenerarEscenario(ref escenario, Content, ContentFolder3D, ContentFolderEffects);
        
        base.LoadContent();
    }


    protected override void Update(GameTime gameTime)
    {
        posicion += VELOCIDAD * (float) gameTime.ElapsedGameTime.TotalSeconds;
        //Codigo para camara simple
        DebugCamera.Update(gameTime);
        _view = DebugCamera.View;
        _projection = DebugCamera.Projection;

        tiempoAcumulado += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (tiempoAcumulado >= 1f) // cada 1 segundo
        {
            tiempoAcumulado = 0f;
            escenarioGenerator.AvanzarEscenario(ref escenario, Content, ContentFolder3D, ContentFolderEffects);
        }


        // Capturar Input teclado
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        foreach (var modulo in escenario)
        {
            modulo.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //El fondo es negro
        GraphicsDevice.Clear(Color.Black);

        foreach (IModule module in escenario)
        {
            module.Draw(_view, _projection);
        }
        //Cada modelo deberia tener su propio draw.
        //A menos que sea para prueba no deberian haber dibujos en este metodo
    }


    protected override void UnloadContent()
    {
        Content.Unload();

        base.UnloadContent();
    }
}
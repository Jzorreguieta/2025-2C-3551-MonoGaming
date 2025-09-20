using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace  TGC.MonoGame.TP;

public class TGCGame : Game
{
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";
    
    private readonly GraphicsDeviceManager _graphics;

    private Effect _effect;
    private Matrix _projection;
    private SpriteBatch _spriteBatch;
    private Matrix _view;
    private Matrix _world;

    //Modulo de prueba. Mas adelante deberia ser reemplazado por una lista de modulos
    //Deberia usara polimorfismo.
    private List<BasicModule> basicModule;

    public TGCGame()
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
    }


    protected override void Initialize()
    {
        _world = Matrix.Identity;
        _view = Matrix.CreateLookAt(new Vector3(0, 0, 300), Vector3.Zero, Vector3.Up);
        _projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 2500);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        //No se que es esto si alguien lo averigua cuenteme. Att:Guido
        _spriteBatch = new SpriteBatch(GraphicsDevice);

    

        // Cargo un efecto basico propio declarado en el Content pipeline.
        // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
        _effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

        // Cargo los modelos aca abajo utilizando el effect de arriba.

        basicModule = new List<BasicModule>
        {
            new BasicModule(Content, ContentFolder3D, ContentFolderEffects, Matrix.Identity * Matrix.CreateTranslation(Vector3.Left*100f)),

        };//ContentManager content, string contentFolder3D, string contentFolderEffects

        base.LoadContent();
    }


    protected override void Update(GameTime gameTime)
    {

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //El fonde del mundo es balnco.
        GraphicsDevice.Clear(Color.White);
        foreach (BasicModule module in basicModule)
        {
            module.Draw(_view,_projection);
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
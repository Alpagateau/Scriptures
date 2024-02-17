using System.Threading;
using WireEngine;


namespace WireEngine;
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.main();
    }
}

class Game
{

    GameWindow gameWindow;
    int i = 0;
    Vector2Int apple;
    List<Vector2Int> pos = new List<Vector2Int>();

    public void main()
    {
        gameWindow = new GameWindow().SetWindowSize(160, 40).SetCharSize(8);

        Tablet renderer = new Tablet(0, 0, 160, 1);
        Tablet renderer2 = new Tablet(0, 1, 160, 38);
        if (!gameWindow.addTablet(renderer))
            return;
        if (!gameWindow.addTablet(renderer2))
            return;

        pos.Add(new Vector2Int(80, 19));

        apple = new Vector2Int();

        gameWindow.Updating += GameLoop;
        gameWindow.inputSystem.KeyboardInput += HandleInput;
        gameWindow.Start();
    }

    void HandleInput(InputKeys key)
    {
        switch(key.key)
        {
            case ConsoleKey.LeftArrow:
                dir = 0;
                break;
            case ConsoleKey.RightArrow:
                dir = 2;
                break;
            case ConsoleKey.UpArrow:
                dir = 1;
                break;
            case ConsoleKey.DownArrow:
                dir = 3;
                break;
            default:
                break;
        }
    }

    int dir = 0;

    void GameLoop(object? o, EventArgs e)
    {
        //move the player
        if(i > 20)
        {
            gameWindow.Stop();
        }

    }
}
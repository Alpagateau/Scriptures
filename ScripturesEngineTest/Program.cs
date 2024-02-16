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

    int x = 80, y = 19;
    int lstX = 80, lstY = 19;

    public void main()
    {
        gameWindow = new GameWindow().SetWindowSize(160, 40).SetCharSize(5);

        Tablet renderer = new Tablet(0, 0, 160, 1);
        Tablet renderer2 = new Tablet(0, 1, 160, 38);
        if (!gameWindow.addTablet(renderer))
            return;
        if (!gameWindow.addTablet(renderer2))
            return;

        gameWindow.Updating += GameLoop;
        gameWindow.inputSystem.KeyboardInput += HandleInput;
        gameWindow.Start();
    }

    void HandleInput(InputKeys key)
    {
        switch(key.key)
        {
            case ConsoleKey.LeftArrow:
                x--;
                break;
            case ConsoleKey.RightArrow:
                x++;
                break;
            case ConsoleKey.UpArrow:
                y--;
                break;
            case ConsoleKey.DownArrow:
                y++;
                break;
            default:
                break;
        }
    }

    void GameLoop(object? o, EventArgs e)
    {
        gameWindow.tablets.Last().setCursorPosition(x, y);
        Console.Write("C");

        if(lstX != x || lstY != y)
        {
            gameWindow.tablets.Last().setCursorPosition(lstX, lstY);
            Console.Write(" ");
            lstX = x;
            lstY = y;
        }
        if(x > 160 || y > 38)
        {
            gameWindow.Stop();
        }
        //gameWindow.tablets.Last().setCursorPosition(x, y);
        //Console.Write(" ");
    }
}
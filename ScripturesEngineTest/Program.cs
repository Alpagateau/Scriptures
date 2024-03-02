using System.Threading;
using WireEngine;


namespace WireEngine;
class Program
{
    static void Main(string[] args)
    {   
        
        Snake game = new Snake();
        game.main();
        
    }
}

class Snake
{

    GameWindow gameWindow;
    int i = 0;

    Vector2Int apple;
    List<Vector2Int> pos = new List<Vector2Int>();

    Tablet topSection;
    Tablet renderer2;

    int dir = 0;
    bool isRunning, isAlive;

    public void main()
    {
        gameWindow = new GameWindow()
            .SetWindowSize(100, 40)
            .SetCharSize(8)
            .SetColorScheme(new ColorScheme(ConsoleColor.White, ConsoleColor.Green));

        gameWindow.ApplicationName = "SNAKE [powered by ScripturesEgine]";

        topSection = new Tablet(0, 0, 100, 2);
        renderer2 = new Tablet(0, 2, 100, 38);
        if (!gameWindow.addTablet(topSection))
            return;
        if (!gameWindow.addTablet(renderer2))
            return;

        apple = new Vector2Int(80, 20);

        gameWindow.Starting += Start;
        gameWindow.Updating += GameLoop;
        //gameWindow.inputSystem.KeyboardInput += HandleInput;
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
            case ConsoleKey.Enter:
                if (!isRunning && !isAlive)
                {
                    topSection.Clear();
                    moveApple();
                    isRunning = true;
                    isAlive = true;
                }
                else if (!isAlive)
                {
                    isRunning = false;
                    Start(this, EventArgs.Empty);
                }
                break;
            default:
                break;
        }
    }

    void moveApple()
    {
        renderer2.Write(" ", apple);
        apple.x += 2;
        apple.x *= 5;
        apple.x = apple.x % (renderer2.transform.scale.x-2);
        apple.x++;

        apple.y += 10;
        apple.y = apple.y % (renderer2.transform.scale.y-2);
        apple.y++;
        renderer2.Write(GameWindow.fullBlock + "", apple, ConsoleColor.Red);
    }

    void test(object? o, EventArgs e)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(apple.ToString());
    }

    void Start(object? o, EventArgs e)
    {
        topSection.Clear();
        i = 0;
        pos.Clear();
        pos.Add(new Vector2Int(80, 19));
        renderer2.Clear();
        topSection.Write(
            "SNAKE [powered by ScripturesEgine]               --press enter to start the game--",
            new Vector2Int(0,0) ,
            ConsoleColor.Black);
        renderer2.borderWith('#', ConsoleColor.DarkGreen);
    }

    int dd = 0;

    void GameLoop(object? o, EventArgs e)
    {
        if(gameWindow.inputSystem.GetKeyDown(KeyCode.ENTER))
        {
            if (!isRunning && !isAlive)
            {
                topSection.Clear();
                moveApple();
                isRunning = true;
                isAlive = true;
            }
            else if (!isAlive)
            {
                isRunning = false;
                Start(this, EventArgs.Empty);
            }
        }
        if (isAlive)
        {
            if(dd == 0){ 
            //move the player
            switch (dir)
            {
                case 0:
                    pos.Add(new Vector2Int(
                        pos.Last().x - 1,
                        pos.Last().y
                        ));
                    break;
                case 1:
                    pos.Add(new Vector2Int(
                        pos.Last().x,
                        pos.Last().y - 1
                        ));
                    break;
                case 2:
                    pos.Add(new Vector2Int(
                        pos.Last().x + 1,
                        pos.Last().y
                        ));
                    break;
                case 3:
                    pos.Add(new Vector2Int(
                        pos.Last().x,
                        pos.Last().y + 1
                        ));
                    break;
            }

            //check if last pos is apple
            if (pos.Last() == apple)
            {
                moveApple();
                i++;
            }
            else
            {
                renderer2.setCursorPosition(pos.First());
                Console.Write(" ");
                pos.RemoveAt(0);
            }

            //draw the player
            foreach (Vector2Int v in pos)
            {
                renderer2.setCursorPosition(v);
                Console.Write(GameWindow.fullBlock);
            }

            //check if player is dead
            //--Check if player's head is in a wall
            Vector2Int head = pos.Last();
            if (head.x == 0 || head.y == 0 || head.x == renderer2.transform.scale.x - 1 || head.y == renderer2.transform.scale.y - 1)
            {
                renderer2.Write("X", head);
                renderer2.Write("Sorry, you are dead", new Vector2Int(50, 15), ConsoleColor.DarkRed);
                isAlive = false;
            }
            int idx = pos.FindIndex(x => x == head);
            if (idx != pos.Count - 1)
            {
                renderer2.Write("X", head);
                renderer2.Write("Sorry, you are dead", new Vector2Int(50, 15), ConsoleColor.DarkRed);
                isAlive = false;
            }

            topSection.Write("Score = " + i, new Vector2Int(5, 1), ConsoleColor.Black);
            topSection.Write("apple = " + apple.x + "x " + apple.y + "y", new Vector2Int(30, 1));

            }
            //check for inputs
            if (gameWindow.inputSystem.GetKeyDown(KeyCode.UPARROW))
            {
                dir = 1;
            }
            else if (gameWindow.inputSystem.GetKeyDown(KeyCode.DOWNARROW))
            {
                dir = 3;
            }
            else if (gameWindow.inputSystem.GetKeyDown(KeyCode.LEFTARROW))
            {
                dir = 0;
            }
            else if (gameWindow.inputSystem.GetKeyDown(KeyCode.RIGHTARROW))
            {
                dir = 2;
            }
            dd++;
            if (dd > 500)
                dd = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm
{

    public class Game
    {
        public const int STATE_MENU = 0;
        public const int STATE_GAME = 1;
        public const int STATE_SCORE = 2;

        const int STATE_MENU_DO = 3;
        const int STATE_GAME_DO = 4;
        const int STATE_SCORE_DO = 5;

        const string MSG_SCORE0 = "GAME OVER!!!";
        const string MSG_SCORE1 = "FINAL SCORE : ";
        const string MSG_SCORE2 = "Press 'r' to return to menu or 'q' to quit";

        const string MSG_MENU0 = "ORM - 2014";
        const string MSG_MENU1 = "start";
        const string MSG_MENU2 = "min speed : ";
        const string MSG_MENU3 = "start speed : ";
        const string MSG_MENU4 = "start length : ";
        const string MSG_MENU5 = "quit";


        int wt;
        int state;
        bool init;

        int speed;  // ticks per update
        int startspeed;
        int minSpeed;
        int tick;

        int cursor;
        bool quit;

        int width;
        int height;
        int length;
        int score;

        struct Apple
        {
            public int x;
            public int y;
            public Apple(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        Apple apple;
        Snake snake;
        Random rand = new Random();

        public Game(int state, int speed, int minSpeed, int length, int width, int height)
        {
            changeState(state);
            this.startspeed = speed;
            this.speed = speed;
            this.minSpeed = minSpeed;
            this.width = width;
            this.height = height;
            this.length = length;
        }

        public void doTick()
        {
            tick++;
            tick %= speed;
        }

        public void changeState(int state)
        {
            this.state = state;
            init = true;
        }

        public void update()
        {
            switch (state)
            {
                case STATE_GAME:
                    if (init)
                    {
                        init = false;
                        speed = startspeed;
                        snake = new Snake(width - 2 >> 1, height - 2 >> 1, length);
                        wt = 0;
                        apple = new Apple(rand.Next(1, width - 1), rand.Next(1, height - 1));
                        score = 0;
                    }
                    else
                    {
                        state = STATE_GAME_DO;
                        goto case STATE_GAME_DO;
                    }
                    break;

                case STATE_GAME_DO:
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);

                        switch (key.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                if (snake.getDirection() != Snake.RIGHT && snake.getDirection() != Snake.LEFT)
                                {
                                    snake.setDirection(Snake.LEFT);
                                    tick = 0;
                                }
                                break;

                            case ConsoleKey.UpArrow:
                                if (snake.getDirection() != Snake.DOWN && snake.getDirection() != Snake.UP)
                                {
                                    snake.setDirection(Snake.UP);
                                    tick = 0;
                                }
                                break;

                            case ConsoleKey.RightArrow:
                                if (snake.getDirection() != Snake.LEFT && snake.getDirection() != Snake.RIGHT)
                                {
                                    snake.setDirection(Snake.RIGHT);
                                    tick = 0;
                                }
                                break;

                            case ConsoleKey.DownArrow:
                                if (snake.getDirection() != Snake.UP && snake.getDirection() != Snake.DOWN)
                                {
                                    snake.setDirection(Snake.DOWN);
                                    tick = 0;
                                }
                                break;
                        }
                    }

                    if (tick == 0)
                    {
                        snake.update();
                        // gameover
                        if ((snake.Head.x * snake.Head.y) == 0 || snake.Head.x > width - 2 || snake.Head.y > height - 2 || snake.isColliding())
                        {
                            changeState(STATE_SCORE);
                        }
                        // eat
                        if (snake.Head.x == apple.x && snake.Head.y == apple.y)
                        {
                            Console.Beep(600, 80);
                            Console.Beep(800, 80);
                            Console.Beep(1000, 80);

                            apple.x = rand.Next(1, width - 1);
                            apple.y = rand.Next(1, height - 1);
                            score++;

                            snake.addLength();

                            if (speed > minSpeed)
                                speed--;
                        }
                        // fult hack
                        if (wt < length)
                            wt++;
                    }
                    break;


                case STATE_MENU:
                    if (init)
                        init = false;
                    else
                    {
                        state = STATE_MENU_DO;
                        cursor = 0;
                        goto case STATE_MENU_DO;
                    }
                    break;

                case STATE_MENU_DO:
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                cursor--;
                                if (cursor < 0)
                                    cursor = 4;
                                break;

                            case ConsoleKey.DownArrow:
                                cursor++;
                                cursor %= 5;
                                break;

                            case ConsoleKey.LeftArrow:
                                switch (cursor)
                                {
                                    case 1:
                                        if (minSpeed > 1)
                                            minSpeed--;
                                        break;

                                    case 2:
                                        if (startspeed > minSpeed)
                                            startspeed--;
                                        break;
                                    case 3:

                                        if (length > 1)
                                            length--;
                                        break;
                                }
                                break;

                            case ConsoleKey.RightArrow:
                                switch (cursor)
                                {
                                    case 1:
                                        minSpeed++;
                                        if (startspeed < minSpeed)
                                            startspeed = minSpeed;
                                        break;
                                    case 2:
                                        startspeed++;
                                        break;
                                    case 3:
                                        length++;
                                        break;
                                }
                                break;
                            case ConsoleKey.Spacebar:
                                if (cursor == 0)
                                    changeState(STATE_GAME);
                                else if (cursor == 4)
                                    quit = true;
                                break;
                        }
                    }
                    cursor %= 5; // 5 menu items
                    break;


                case STATE_SCORE:
                    if (init)
                    {
                        init = false;
                        Console.Beep(400, 100);
                        Console.Beep(200, 150);
                        Console.Beep(100, 200);
                    }
                    else
                    {
                        state = STATE_SCORE_DO;
                        goto case STATE_SCORE_DO;
                    }
                    break;
                case STATE_SCORE_DO:
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.R)
                            changeState(STATE_MENU);
                        else if (key.Key == ConsoleKey.Q)
                            quit = true;

                    }
                    break;
            }
        }

        public void render()
        {
            switch (state)
            {
                case STATE_GAME:
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int i = 0; i < width; i++)
                    {
                        Console.SetCursorPosition(i, 0);
                        Console.Write(" ");
                        Console.SetCursorPosition(i, height - 1);
                        Console.Write(" ");
                    }
                    for (int i = 0; i < height; i++)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.Write(" ");
                        Console.SetCursorPosition(width - 1, i);
                        Console.Write(" ");
                    }

                    Console.SetCursorPosition(0, 0);
                    Console.ResetColor();
                    break;

                case STATE_GAME_DO:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.SetCursorPosition(snake.Head.x, snake.Head.y);
                    Console.Write(" ");
                    // fult hack
                    if (wt == length)
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(snake.Tail.x, snake.Tail.y);
                        Console.Write(" ");
                    }
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(apple.x, apple.y);
                    Console.Write(" ");
                    Console.ResetColor();
                    Console.SetCursorPosition(0, 0);
                    Console.Write("SCORE : " + score + " LENGTH : " + snake.getLength() + " SPEED : " + Program.FRAME_TIME * speed + "ms");
                    break;

                case STATE_SCORE:
                    int msg_x = (width - 2 >> 1) - (MSG_SCORE0.Length >> 1);
                    int msg_y = (height - 2 >> 1);
                    int msg_x2 = (width - 2 >> 1) - (MSG_SCORE2.Length >> 1);

                    Console.ResetColor();
                    Console.SetCursorPosition(msg_x, msg_y - 1);
                    Console.Write(MSG_SCORE0);
                    Console.SetCursorPosition(msg_x, msg_y);
                    Console.Write(MSG_SCORE1 + score);
                    Console.SetCursorPosition(msg_x2, msg_y + 1);
                    Console.Write(MSG_SCORE2);
                    break;
                case STATE_MENU:
                    Console.Clear();
                    Console.ResetColor();
                    goto case STATE_MENU_DO;
                case STATE_MENU_DO:
                    int msg_x4 = (width - 2 >> 1) - (MSG_MENU0.Length >> 1);
                    int msg_y2 = (height - 2 >> 1) - 2;
                    int msg_x3 = (width - 2 >> 1) - (MSG_MENU0.Length >> 1);
                    int msg_y1 = height - 2 >> 1;
                    Console.ResetColor();
                    Console.SetCursorPosition(msg_x3, msg_y1 - 3);
                    Console.Write(MSG_MENU0);
                    Console.SetCursorPosition(msg_x3, msg_y1 - 2);
                    Console.Write(MSG_MENU1);
                    Console.SetCursorPosition(msg_x3, msg_y1 - 1);
                    Console.Write(MSG_MENU2 + Program.FRAME_TIME * minSpeed + "ms");
                    Console.SetCursorPosition(msg_x3, msg_y1);
                    Console.Write(MSG_MENU3 + Program.FRAME_TIME * startspeed + "ms");
                    Console.SetCursorPosition(msg_x3, msg_y1 + 1);
                    Console.Write(MSG_MENU4 + length);
                    Console.SetCursorPosition(msg_x3, msg_y1 + 2);
                    Console.Write(MSG_MENU5);
                    Console.SetCursorPosition(msg_x4, msg_y2 + cursor);
                    for (int i = msg_x4 + MSG_MENU3.Length; i < width - 1; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(msg_x4, msg_y2 + cursor);
                    Console.ForegroundColor = ConsoleColor.White;
                    switch (cursor)
                    {
                        case 0:
                            Console.Write(MSG_MENU1);
                            break;
                        case 1:
                            Console.Write(MSG_MENU2 + Program.FRAME_TIME * minSpeed + "ms");
                            break;
                        case 2:
                            Console.Write(MSG_MENU3 + Program.FRAME_TIME * startspeed + "ms");
                            break;
                        case 3:
                            Console.Write(MSG_MENU4 + length);
                            break;
                        case 4:
                            Console.Write(MSG_MENU5);
                            break;

                    }
                    break;
            }
        }

        public bool hasQuit()
        {
            return quit;
        }
    }
}

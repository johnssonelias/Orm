using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm
{

    public class Program
    {
        // 60 fps
        public const int FRAME_TIME = 17;

        public static void Main(string[] args)
        {
            const int WINDOW_WIDTH = 60;
            const int WINDOW_HEIGHT = 30;

            int state = Game.STATE_MENU;
            int speed = 6;
            int minSpeed = 3;
            int length = 8;

            Game game = new Game(state, speed, minSpeed, length, WINDOW_WIDTH, WINDOW_HEIGHT);

            int xt = 0;

            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Console.CursorVisible = false;
            /*
            Console.Beep(600, 80);
            Console.Beep(800, 80);
            Console.Beep(1000, 80);
            */
            /*
            Console.Beep(400, 100);
            Console.Beep(200, 150);
            Console.Beep(100, 200);
            */
            while (true)
            {
                int t0 = System.Environment.TickCount;

                game.update();
                game.render();
                game.doTick();

                if (game.hasQuit())
                    break;
                // Timing  
                int dt;
                do
                {
                    dt = System.Environment.TickCount - t0;
                } while (dt <= FRAME_TIME - xt);
                if (dt > FRAME_TIME)
                    xt = dt - FRAME_TIME;
                else
                    xt = 0;
            }
        }
    }
}

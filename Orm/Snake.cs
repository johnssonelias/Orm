using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm
{
    public class Snake
    {
        public const int LEFT = 0;
        public const int UP = 1;
        public const int RIGHT = 2;
        public const int DOWN = 3;

        int startLength = 0;

        public struct Part
        {
            public int x;
            public int y;
        }


        private Part head;
        public Part Head
        {
            get
            {
                return head;
            }
        }

        private int length;

        private List<Part> parts = new List<Part>();
        public Part Tail
        {
            get
            {
                return parts.ElementAt(0);
            }
        }

        private int direction = UP;

        public Snake(int x, int y, int length)
        {
            head.x = x;
            head.y = y;
            this.length = length;
            addLength();
        }

        public void setDirection(int direction)
        {
            this.direction = direction;
        }

        public void update()
        {
            switch (direction)
            {
                case LEFT:
                    head.x--;
                    break;

                case UP:
                    head.y--;
                    break;

                case RIGHT:
                    head.x++;
                    break;

                case DOWN:
                    head.y++;
                    break;
            }
            addLength();
            if (startLength < length)
                startLength++;
            else
                parts.RemoveAt(0);
        }

        public void addLength()
        {
            Part temp;
            temp.x = head.x;
            temp.y = head.y;
            parts.Add(temp);
        }

        public bool isColliding()
        {
            for (int i = 0; i < parts.Count - 1; i++)
            {
                Part temp = parts.ElementAt(i);
                if (temp.x == head.x && temp.y == head.y)
                    return true;
            }
            return false;
        }

        public int getDirection()
        {
            return direction;
        }

        public int getLength()
        {
            return parts.Count - 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;

using System.Drawing;

namespace Wizard101_Bot
{
    class Program
    {
        public static List<Bitmap> positions = new List<Bitmap>();
        public static Bitmap dance_UP;
        public static Bitmap dance_DOWN;
        public static Bitmap dance_LEFT;
        public static Bitmap dance_RIGHT;
        public static Bitmap dance_BLANK;
        public static Bitmap screen_LOADING;
        public static Bitmap screen_LEVELUP;
        public static Bitmap screen_NOENERGY;
        public static bool opt_afk = true;
        public static bool opt_refills = false;
        public static List<VirtualKeyCode> keys;
        public static InputSimulator inSim;
        public static KeyboardSimulator keyboard;
        public static MouseSimulator mouse;
        public static double screenX = System.Windows.SystemParameters.PrimaryScreenWidth * 1;
        public static double screenY = System.Windows.SystemParameters.PrimaryScreenHeight * 1;

        static void Main2(string[] args)
        {

            Console.WriteLine("LevelUp_" + DateTime.Today.Date.ToString().Substring(0, 10) + "_" + DateTime.Now.TimeOfDay.ToString().Replace(":", "-") + ".bmp");
            Console.Read();
        }

        static void Main(string[] args)
        {
            dance_UP = new Bitmap(Image.FromFile("dance_up.bmp"));
            dance_DOWN = new Bitmap(Image.FromFile("dance_down.bmp"));
            dance_LEFT = new Bitmap(Image.FromFile("dance_left.bmp"));
            dance_RIGHT = new Bitmap(Image.FromFile("dance_right.bmp"));
            dance_BLANK = new Bitmap(Image.FromFile("dance_blank.bmp"));
            screen_LOADING = new Bitmap(Image.FromFile("screen_loading.bmp"));
            screen_LEVELUP = new Bitmap(Image.FromFile("screen_levelup.bmp"));
            screen_NOENERGY = new Bitmap(Image.FromFile("no_energy.bmp"));
            inSim = new InputSimulator();
            keyboard = new KeyboardSimulator(inSim);
            mouse = new MouseSimulator(inSim);

            //Console.WriteLine("First (IMPORTANT), is this your primary screen resolution? " + screenX + "x" + screenY);
            //string ans = Console.ReadLine();


            while (true)
            {
                keyboard.KeyUp(VirtualKeyCode.VK_D);
                Console.Clear();

                Console.Write("How many games should I play? (Leave empty or type 0 to exit): ");

                string games_str = Console.ReadLine();
                               
                if (games_str.Equals("0") || games_str.Equals(""))
                    break;

                int games = Convert.ToInt32(games_str);
                
                Console.WriteLine("Starting game in 2s");
                sleep(500);
                mouseTo(300, 300);
                mouse.LeftButtonClick();

                sleep(1500);

                int i = 0;
                while (i < games)
                {
                    if (!startDanceGame())
                        break;

                    playDanceGame();
                    finishDanceGame();

                    i++;

                    Console.Clear();

                    Console.WriteLine("Games completed: " + i);
                    if (i != games - 1)
                        Console.WriteLine("You're welcome! :)");
                    else
                        Console.WriteLine("Restarting in 2s");

                    sleep(2000);
                }

                sleep(1000);
                goAfk();

                //Console.ReadLine();
            }
            //Console.WriteLine("UP KEY being pressed in 5000 ms from now...");
            //sleep(5000);
            //InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
        }

        public static void goAfk()
        {
            if (!opt_afk)
                return;

            Console.WriteLine("Spinning...");
            
            keyboard.KeyDown(VirtualKeyCode.VK_D);

            Console.ReadLine();
        }

        public static void mouseTo(int x, int y)
        {
            //mouse.MoveMouseTo((65535/2160) * x, (65535/1440) * y);
            mouse.MoveMouseTo((65535 / screenX) * x, (65535 / screenY) * y);
        }

        public static bool getRefill()
        {

            return true;
        }

        public static bool startDanceGame()
        {
            Console.WriteLine("Starting dance game...");
            keyboard.KeyPress(VirtualKeyCode.VK_X);
            sleep(1000);
            mouseTo(284, 500); 
            sleep(100);
            mouse.LeftButtonClick(); // CHOOSE KROK
            sleep(200);

            Bitmap playbutton = ScreenCapture.captureArea(593, 581, 25, 10);
            if (cmpBitmap(playbutton, screen_NOENERGY) >= 0.8)
            {
                if (opt_refills)
                {
                    getRefill();
                }

                else
                {
                    Console.WriteLine("You've run out of energy" + ((opt_refills) ? "and have used all your refills!" : "!"));
                    sleep(1000);
                    mouseTo(173, 596);
                    sleep(100);
                    mouse.LeftButtonClick();
                    return false;
                }
            }

            mouseTo(601, 594);
            sleep(100);
            mouse.LeftButtonClick(); // PLAY
            sleep(1000);
            ScreenCapture.recordArea(388, 552, 25, 10, screen_LOADING);
            Console.WriteLine("Dance game loaded...");
            sleep(600);

            return true;
        }

        public static void finishDanceGame()
        {
            sleep(2000);
            Console.Clear();
            Console.WriteLine("Feeding your pet...");
            mouseTo(575, 597);
            sleep(100);
            mouse.LeftButtonClick(); //Next
            sleep(200);
            mouseTo(158, 485);
            sleep(100);
            mouse.LeftButtonClick(); //Snack
            sleep(200);
            mouseTo(575, 597);
            sleep(100);
            mouse.LeftButtonClick(); //Feed
            sleep(1000);

            Bitmap screen = ScreenCapture.captureArea(417, 124, 25, 10);
            double levelup = cmpBitmap(screen, screen_LEVELUP);

            //screen.Save("screen_lvlup_" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".bmp");

            //Console.WriteLine("Level Up comparison: " + levelup);

            if (levelup >= 0.8)
            {
                Console.WriteLine("Oh hey, your pet just leveled up! *Cheer*");
                Bitmap levelup_photo = ScreenCapture.captureArea(57, 57, 701, 565);
                levelup_photo.Save("LevelUp_" + DateTime.Today.Date.ToString().Substring(0, 10) + "_" + DateTime.Now.TimeOfDay.ToString().Replace(":", "-") + ".bmp");
                mouseTo(660, 560);
                sleep(1000);
                mouse.LeftButtonClick();
            }

            else
            {
                Console.WriteLine("Look at that EXP!! I'll bet you're satisfied?");
                sleep(2000);
            }

            mouseTo(132, 596);
            sleep(100);
            mouse.LeftButtonClick(); //Close
            sleep(3000);
            keys.Clear();
            ScreenCapture.recordArea(388, 552, 25, 10, screen_LOADING);
            Console.WriteLine("Dance game completed...");
            sleep(1000);
        }

        public static void playDanceGame()
        {
            Console.WriteLine("Playing the game...");

            for (int i = 3; i < 8; i++)
            {
                if (i != 3)
                    Console.Clear();
                Console.WriteLine("\nWaiting for game...\n");
                ScreenCapture.recordArea(388, 552, 25, 10, dance_BLANK);
                //Console.WriteLine("Pattern started...");

                //Console.ReadLine();

                keys = round1(i);
                sleep(400);
                playKeys(keys);
                sleep(1000);
            }


            Bitmap screen = ScreenCapture.captureArea(417, 124, 25, 10);
            double levelup = cmpBitmap(screen, screen_LEVELUP);

            //screen.Save("screen_lvlup_" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".bmp");

            //Console.WriteLine("Level Up comparison: " + levelup);

            if (levelup >= 0.8)
            {
                Console.WriteLine("Oh hey, your pet just leveled up! *Cheer*");
                Bitmap levelup_photo = ScreenCapture.captureArea(57, 57, 701, 565);
                levelup_photo.Save("LevelUp_" + DateTime.Today.Date.ToString().Substring(0, 10) + "_" + DateTime.Now.TimeOfDay.ToString().Replace(":", "-") + ".bmp");
                mouseTo(660, 560);
                sleep(2000);
                mouse.LeftButtonClick();
            }
        }

        public static void playKeys(List<VirtualKeyCode> keys)
        {
            foreach(VirtualKeyCode key in keys)
            {
                Console.WriteLine("Press key: " + key.ToString());
                keyboard.KeyPress(key);
                sleep(200);
            }
        }


        public static List<VirtualKeyCode> round1(int expect)
        {
            //SoundCapture.listen(3000, 1);
            List<Bitmap> moves = new List<Bitmap>();
            List<VirtualKeyCode> keys = new List<VirtualKeyCode>();
            //int blanks = 0;

            int itr = 0;
            for(int i = 0; i < expect; i++)
            {
                Bitmap move = ScreenCapture.recordArea2(388, 552, 25, 10);
                //itr++;

                //move.Save("dance_r1m" + (itr) + ".bmp");

                if (cmpBitmap(move, dance_BLANK) > 0.8)
                {
                    i--;
                    continue;
                }

                Console.WriteLine("Caught move! (" + (i+1) + ")");
                
                moves.Add(move);

                //if (expect == 3)
                    sleep(200);

                //if (expect == 4)
                   // sleep(250);

                //if (expect == 5 || expect == 6)
                    //sleep(225);

                //if (expect == 7)
                    //sleep(225);
            }

            sleep(300);

            foreach(Bitmap move in moves)
            {
                List<double> scores = new List<double>();
                scores.Add(cmpBitmap(move, dance_UP));
                scores.Add(cmpBitmap(move, dance_DOWN));
                scores.Add(cmpBitmap(move, dance_LEFT));
                scores.Add(cmpBitmap(move, dance_RIGHT));
                
                int index = 0;

                for (int i = 1; i < scores.Count(); i++)
                    if (scores.ElementAt(i) > scores.ElementAt(index))
                        index = i;

                if(index == 0)
                {
                    //Console.WriteLine("^ " + scores.ElementAt(index));
                    keys.Add(VirtualKeyCode.UP);
                }

                else if(index == 1)
                {
                    //Console.WriteLine("v " + scores.ElementAt(index));
                    keys.Add(VirtualKeyCode.DOWN);
                }

                else if(index == 2)
                {
                    //Console.WriteLine("< " + scores.ElementAt(index));
                    keys.Add(VirtualKeyCode.LEFT);
                }

                else if(index == 3)
                {
                    //Console.WriteLine("> " + scores.ElementAt(index));
                    keys.Add(VirtualKeyCode.RIGHT);
                }
            }

            Console.Write("\n");

            return keys;
        }

        public static void sleep(long milliseconds)
        {
            double time = DateTime.Now.TimeOfDay.TotalMilliseconds;

            while (DateTime.Now.TimeOfDay.TotalMilliseconds < time + milliseconds) { }
        }

        public static double cmpBitmap(Bitmap bmp1, Bitmap bmp2)
        {
            return cmpBitmap(bmp1, bmp2, 1.0);
        }

        public static double cmpBitmap(Bitmap bmp1, Bitmap bmp2, double threshold)
        {
            double percent = 0.0;

            if (!bmp1.Size.Equals(bmp2.Size))
            {
                return percent;
            }

            double incScore = (1D/(bmp1.Width * bmp1.Height));
            //Console.WriteLine("w = " + bmp1.Width + ", h = " + bmp1.Height + ", inc = " + incScore);

            //sleep(3000);

            for (int x = 0; x < bmp1.Width; ++x)
            {
                for (int y = 0; y < bmp1.Height; ++y)
                {
                    //Console.Write("(" + x + "," + y + ")=");
                    double cvar = colorVariation(bmp1.GetPixel(x, y), bmp2.GetPixel(x, y));
                    //Console.WriteLine("cvar: " + cvar);
                    if (cvar < 40D)
                    {
                        percent += incScore;
                        if (percent >= threshold)
                            return percent;
                        //Console.WriteLine("true, " + percent);
                        continue;
                    }
                    //Console.WriteLine("false, " + percent);

                }
            }

            //sleep(1000);
            //Console.WriteLine("\nScore: " + percent);
            return percent;
        }

        public static double colorVariation(Color color1, Color color2)
        {
            double difference_R = color1.R - color2.R;
            double difference_G = color1.G - color2.G;
            double difference_B = color1.B - color2.B;



            return Math.Abs((difference_B + difference_G + difference_R) / 3);
        }
    }
}

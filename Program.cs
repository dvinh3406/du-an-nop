﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace JumpKingCloneConsole
{
    class Program
    {
        static int playerPositionY = 0; 
        static int playerPositionX = 10; 
        static int groundLevel = 0; 
        static int screenWidth = 20; 
        static Random random = new Random();
        static int[,] platforms = new int[5, 2];
        static List<(int x, int y)> bullets = new List<(int, int)>(); 
        static List<(int x, int y)> playerBullets = new List<(int, int)>(); 
        static int score = 0; 
        static bool gameOver = false;
        static int bulletCooldown = 0; 

        public static void Main(string[] args)
        {
            GeneratePlatforms(); 

            Console.WriteLine("Nhấn phím Space để bắn, mũi tên trái để di chuyển sang trái, mũi tên phải để di chuyển sang phải. Nhấn ESC để thoát.");

            while (!gameOver)
            {
                
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Spacebar)
                    {
                        ShootPlayerBullet(); 
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        if (playerPositionX > 0)
                        {
                            playerPositionX--; 
                        }
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        if (playerPositionX < screenWidth)
                        {
                            playerPositionX++; 
                        }
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        break; 
                    }
                }

                
                Console.Clear();
                for (int i = 10; i >= 0; i--)
                {
                    bool isPlatform = false;
                    for (int j = 0; j < platforms.GetLength(0); j++)
                    {
                        if (i == platforms[j, 1])
                        {
                            Console.SetCursorPosition(platforms[j, 0], 10 - i);
                            Console.Write("--"); 
                            isPlatform = true;
                        }
                    }

                    if (i == playerPositionY)
                    {
                        Console.SetCursorPosition(Math.Max(0, Math.Min(playerPositionX, screenWidth)), 10 - i);
                        Console.Write("O"); 
                    }
                    else if (!isPlatform)
                    {
                        Console.SetCursorPosition(Math.Max(0, Math.Min(playerPositionX, screenWidth)), 10 - i);
                        Console.Write(" "); 
                    }
                }
                Console.SetCursorPosition(0, 11);
                Console.WriteLine("======= (Mặt đất)");
                Console.SetCursorPosition(0, 12);
                Console.WriteLine($"Điểm số: {score}");

                
                if (bulletCooldown == 0)
                {
                    ShootPlatformBullets();
                    bulletCooldown = 10; 
                }
                else
                {
                    bulletCooldown--;
                }

                UpdateBullets(); 
                UpdatePlayerBullets(); 

                
                Thread.Sleep(200);
            }

            Console.Clear();
            Console.WriteLine("Vui vậy thôi chứ bố dặn con này!con thấy con chơi tệ không? điểm: " + score);
        }

        static void GeneratePlatforms()
        {
            for (int i = 0; i < platforms.GetLength(0); i++)
            {
                platforms[i, 0] = random.Next(1, screenWidth - 1); 
                platforms[i, 1] = groundLevel + (i + 1) * 2 + random.Next(0, 2); 
            }
        }

        static void ShootPlayerBullet()
        {
            playerBullets.Add((playerPositionX, playerPositionY + 1)); 
        }

        static void UpdatePlayerBullets()
        {
            
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                var bullet = playerBullets[i];
                int bulletPositionY = bullet.y + 1;

                if (bulletPositionY > 10)
                {
                    playerBullets.RemoveAt(i); 
                }
                else
                {
                    playerBullets[i] = (bullet.x, bulletPositionY);

                    
                    for (int j = 0; j < platforms.GetLength(0); j++)
                    {
                        if (platforms[j, 0] == bullet.x && platforms[j, 1] == bulletPositionY)
                        {
                            platforms[j, 1] = -1; 
                            playerBullets.RemoveAt(i); 
                            score += 10;
                            break;
                        }
                    }

                    
                    if (i < playerBullets.Count) 
                    {
                        Console.SetCursorPosition(bullet.x, 10 - bulletPositionY);
                        Console.Write("|");
                    }
                }
            }
        }

        static void ShootPlatformBullets()
        {
            
            for (int i = 0; i < platforms.GetLength(0); i++)
            {
                if (platforms[i, 1] > 0)
                {
                    bullets.Add((platforms[i, 0], platforms[i, 1])); 
                }
            }
        }

        static void UpdateBullets()
        {
            
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var bullet = bullets[i];
                int bulletPositionY = bullet.y - 1; 

                if (bulletPositionY < 0)
                {
                    bullets.RemoveAt(i); 
                }
                else
                {
                    bullets[i] = (bullet.x, bulletPositionY);

                    
                    if (bulletPositionY == playerPositionY && bullet.x == playerPositionX)
                    {
                        gameOver = true; 
                        return;
                    }

                    
                    Console.SetCursorPosition(bullet.x, 10 - bulletPositionY);
                    Console.Write("*");
                }
            }
        }
    }
}
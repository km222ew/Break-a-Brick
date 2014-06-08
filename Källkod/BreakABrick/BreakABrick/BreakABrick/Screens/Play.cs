using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BreakABrick.GameComponents;
using BreakABrick.GameComponents.PowerUps;
using BreakABrick.ApplicationComponents;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace BreakABrick.Screens
{
    class Play : Screen
    {
        #region Variabler och samlingar

        Texture2D gameBackground;
        Texture2D gameHud;
        Texture2D menuBox;

        Paddle paddle;
        Texture2D paddleTexture;

        List<Ball> balls = new List<Ball>();
        Texture2D ballTexture;
        Vector2 ballMotion;

        List<Brick> bricks = new List<Brick>();
        Texture2D brickTexture;
        int indestructibleBricks;

        int currentLevel = 1;
        int totalLevels = 5;
        List<Keys> levelSelect = new List<Keys>();

        int difficulty;
        string difficultyString;
        int difficultyPaddleMod;
            
        Rectangle gameField;
        Game1 game;

        MouseState prevMouseState;
        MouseState currMouseState;
        
        Texture2D pausedBackground;
        KeyboardState prevKeyboardState;
        bool gameIspaused = false;

        bool gameIsActive;
        bool gameIsOver;
        bool levelIsComplete;
        string text;

        Button nextLevelButton;
        Texture2D nextLevelTexture;
        Rectangle nextLevelRectangle;       

        const int nPausButtons = 4,
            continueGameIndex = 0,
            resetGameIndex = 1,
            mainMenuIndex = 2,
            quitGameIndex = 3;
        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Texture2D[] pausButtonTexture = new Texture2D[nPausButtons];
        Rectangle[] pausButtonRectangle = new Rectangle[nPausButtons];
        List<Button> pausButtons = new List<Button>();

        //för att flytta "main menu"-knappen vid olika lägen
        Rectangle defaultMainMenuLocation;
        Rectangle newMainMenuLocation;

        //för att flytta "restart"-knappen vid olika lägen
        Rectangle defaultResetGameLocation;
        Rectangle newResetGameLocation;
 
        SpriteFont font;
        SpriteFont font2;
        SpriteFont font4;
        Color fontColor = Color.Lime;

        int score;
        int bonusPoints;
        int totalScore;

        //powerups
        int paddleSize;
        Texture2D laserTexture;
        Texture2D paddleShootTexture;
        Texture2D extraLifeTexture;
        Texture2D paddleUpTexture;
        Texture2D paddleDownTexture;
        Texture2D extraPointsTexture;
        Texture2D multiBallTexture;
        List<PowerUp> powerups = new List<PowerUp>();
        List<Laser> laserShots = new List<Laser>();
        Random probability = new Random();
        bool canShoot = false;
        double shootTimer;

        #endregion

        #region Konstruktor

        public Play(ContentManager content, EventHandler screenEvent, Game1 game1, Rectangle gameField, int difficulty)
        : base(screenEvent)
        {
            game = game1;

            this.difficulty = difficulty;

            if (difficulty == 0)
            {
                difficultyString = "Casual";
                difficultyPaddleMod = 10;
            }
            else if (difficulty == 1)
            {
                difficultyString = "Normal";
                difficultyPaddleMod = 0;
            }
            else
            {
                difficultyString = "Hard";
                difficultyPaddleMod = -10;
            }

            this.gameField = gameField;

            //Grafik
            gameBackground      = content.Load<Texture2D>("Images/Menu/Background/playbackground5");
            pausedBackground    = content.Load<Texture2D>("Images/Menu/Background/pausbackground");
            gameHud             = content.Load<Texture2D>("Images/Menu/Background/hudbackground2");
            menuBox             = content.Load<Texture2D>("Images/Menu/Background/box4");
            paddleTexture       = content.Load<Texture2D>("Images/Game/paddle5");
            ballTexture         = content.Load<Texture2D>("Images/Game/ball5");
            brickTexture        = content.Load<Texture2D>("Images/Game/brick3");
            extraLifeTexture    = content.Load<Texture2D>("Images/Game/Powerups/life2");
            paddleUpTexture     = content.Load<Texture2D>("Images/Game/Powerups/paddleup3");
            paddleDownTexture   = content.Load<Texture2D>("Images/Game/Powerups/paddledown2");
            extraPointsTexture  = content.Load<Texture2D>("Images/Game/Powerups/points2");
            multiBallTexture    = content.Load<Texture2D>("Images/Game/Powerups/multiball2");
            paddleShootTexture  = content.Load<Texture2D>("Images/Game/Powerups/paddleshoot2");
            laserTexture        = content.Load<Texture2D>("Images/Game/Powerups/laser3");

            pausButtonTexture[continueGameIndex]    = content.Load<Texture2D>(@"Images/Menu/continuebutton");
            pausButtonTexture[resetGameIndex]       = content.Load<Texture2D>(@"Images/Menu/restartbutton");
            pausButtonTexture[mainMenuIndex]        = content.Load<Texture2D>(@"Images/Menu/mainmenubutton");
            pausButtonTexture[quitGameIndex]        = content.Load<Texture2D>(@"Images/Menu/quitbutton");
            nextLevelTexture                        = content.Load<Texture2D>(@"Images/Menu/nextlevelbutton");

            font    = content.Load<SpriteFont>("Font/SpriteFont1");
            font2   = content.Load<SpriteFont>("Font/SpriteFont2");
            font4   = content.Load<SpriteFont>("Font/SpriteFont4");

            //Ny boll och platta
            balls.Add(new Ball(ballTexture, gameField));
            paddle = new Paddle(paddleTexture,
                                gameField,
                                Life(difficulty),
                                new Rectangle((gameField.Width - paddleTexture.Width) / 2,
                                               gameField.Height - paddleTexture.Height - 30,
                                               paddleTexture.Width + difficultyPaddleMod,
                                               paddleTexture.Height));            
            //Knappar och pausmenyn
            MenuButtonsPrep();

            for (int i = 0; i < nPausButtons; i++)
            {
                pausButtons.Add(new Button(pausButtonTexture[i], pausButtonRectangle[i]));
            }

            nextLevelButton = new Button(nextLevelTexture, nextLevelRectangle = new Rectangle(440, 410, menuButtonWidth, menuButtonHeight));

            defaultMainMenuLocation = pausButtons[mainMenuIndex].Rectangle;
            defaultResetGameLocation = pausButtons[resetGameIndex].Rectangle;

            newMainMenuLocation = new Rectangle(660, 410, menuButtonWidth, menuButtonHeight);
            newResetGameLocation = new Rectangle(440, 410, menuButtonWidth, menuButtonHeight);

            for (int i = 0; i < totalLevels; i++)
            {
                levelSelect.Add(Keys.D1 + (i));
            }            

            NewGame();
        }

        #endregion

        #region Metoder

        //Sätter knappar i mitten av skärmen med jämna mellanrum
        public void MenuButtonsPrep()
        {
            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = game.Window.ClientBounds.Height / 2 - nPausButtons / 2 *
                menuButtonHeight - (nPausButtons % 2) * menuButtonHeight / 2
                + 50;
            for (int i = 0; i < nPausButtons; i++)
            {
                pausButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }
        }

        public int Life(int difficulty)
        {
            if (difficulty == 0)
            {
                return 0;
            }
            else if (difficulty == 1)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }

        //Skapa nytt spel, återställ nivå, starta nästa nivå. Förbereder nivåerna och rensar det som inte ska finnas på spelplanen. 
        public void NewGame()
        {
            bricks.Clear();
            powerups.Clear();
            balls.Clear();
            paddleSize = 0;
            balls.Add(new Ball(ballTexture, gameField));

            //Ej avancerad "felhantering" av nivå-filer, stänger helt enkelt av spelet om filen inte finns
            if (File.Exists(@"Content/Levels/Level" + currentLevel.ToString() + ".txt"))
            {
                LoadLevel(currentLevel);
            }
            else
            {
                game.Exit();
            }

            paddle.Life = Life(difficulty);
            paddle.Position = new Rectangle((gameField.Width - paddleTexture.Width) / 2,
                                             gameField.Height - paddleTexture.Height - 30,
                                             paddleTexture.Width + difficultyPaddleMod,
                                             paddleTexture.Height);

            paddle.StartPosition();
            balls[0].Idle(paddle.Position);
        }

        //Läser in textfiler för att skapa nivåerna
        public void LoadLevel(int currentLevel)
        {
            StreamReader levelReader;

            levelReader = new StreamReader(@"Content/Levels/Level" + currentLevel.ToString() + ".txt");

            string level = levelReader.ReadToEnd();

            levelReader.Close();

            //Så att varje bricka har rätt mellanrum, alltså inte ritas på varandra
            int nextBrickX = 0;
            int nextBrickY = 0;
            indestructibleBricks = 0;

            for (int i = 0; i < level.Length; i++)
            {
                switch (level[i])
                {
                    case '0':
                        nextBrickX++;
                        break;
                    case '1':
                        if (!(nextBrickX >= 12))
                        {
                            bricks.Add(new Brick(brickTexture, new Rectangle((nextBrickX * 70) + 220, (nextBrickY * 25) + 50, 70, brickTexture.Height), 1));
                            nextBrickX++;
                        }
                        break;
                    case '2':
                        if (!(nextBrickX >= 12))
                        {
                            bricks.Add(new Brick(brickTexture, new Rectangle((nextBrickX * 70) + 220, (nextBrickY * 25) + 50, 70, brickTexture.Height), 2));
                            nextBrickX++;
                        }
                        break;
                    case '3':
                        if (!(nextBrickX >= 12))
                        {
                            bricks.Add(new Brick(brickTexture, new Rectangle((nextBrickX * 70) + 220, (nextBrickY * 25) + 50, 70, brickTexture.Height), 3));
                            nextBrickX++;
                        }
                        break;
                    case '4':
                        if (!(nextBrickX >= 12))
                        {
                            bricks.Add(new Brick(brickTexture, new Rectangle((nextBrickX * 70) + 220, (nextBrickY * 25) + 50, 70, brickTexture.Height), 4));
                            nextBrickX++;
                            indestructibleBricks++;
                        }
                        break;
                    case '\n':
                        if (!(nextBrickY >= 12))
                        {
                            nextBrickY++;
                            nextBrickX = 0;
                        }
                        break;
                }
            }                       
        }
        
        public void GameTransition(int state)
        {
            switch (state)
            {
                case 1:
                    text = "GAME OVER\n\n\n";
                    gameIsOver = true;
                    break;
                case 2:
                    text = "LEVEL COMPLETE\n\n\n"; 
                    levelIsComplete = true;
                    break;
            }
        }

        public void GameComplete()
        {
            Audio.SoundBank.PlayCue("cold");

            screenEvent.Invoke(this, new ScreenChoice(1));
        }

        //För att bestämma om ljudet för en vanlig bricka eller oförstörbar bricka ska spelas
        public void BrickSound(bool wall)
        {
            if (wall)
            {
                Audio.SoundBank.PlayCue("ballhit");
            }
            else
            {
                Random rnd = new Random();
                Audio.SoundBank.PlayCue(rnd.Next(1, 37).ToString());
            }           
        }

        //Kollas med bollen och skott från laser powerup
        public bool BrickCollision(Brick brick)
        {             
            if (brick.Life >= 4)
            {
                BrickSound(true);
            }
            else
            {
                BrickSound(false);
            }

            if (brick.Life < 4)
            {
                brick.RemoveLife();
            }

            if (brick.Life == 0)
            {
                Rectangle brickLocation = new Rectangle(brick.Position.X + 17, brick.Position.Y, 40, 40);
                Vector2 speed = new Vector2(0, probability.Next(4, 7));

                int powerUpChance = probability.Next(100);
                if (powerUpChance <= 15)
                {
                    int powerUpProbability = probability.Next(100);

                    if (powerUpProbability <= 25 && difficulty > 0)
                    {
                        powerups.Add(new ExtraPoints(extraPointsTexture, brickLocation, speed));
                    }
                    else if (powerUpProbability >= 26 && powerUpProbability <= 30 && difficulty > 0)
                    {
                        powerups.Add(new ExtraLife(extraLifeTexture, brickLocation, speed));
                    }
                    else if (powerUpProbability >= 35 && powerUpProbability <= 45)
                    {
                        powerups.Add(new PaddleUp(paddleUpTexture, brickLocation, speed));
                    }
                    else if (powerUpProbability >= 50 && powerUpProbability <= 60)
                    {
                        powerups.Add(new PaddleDown(paddleDownTexture, brickLocation, speed));
                    }
                    else if (powerUpProbability >= 70 && powerUpProbability <= 85)
                    {
                        powerups.Add(new MultiBall(multiBallTexture, brickLocation, speed));
                    }
                    else if (powerUpProbability >= 90 && powerUpProbability <= 99)
                    {
                        powerups.Add(new PaddleShoot(paddleShootTexture, brickLocation, speed));
                    }
                }

                if (difficulty >= 1)
                {
                    score += 100;
                }

                return true;
            }

            return false;             
        }
        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState();

            if (!gameIspaused && !gameIsOver && !levelIsComplete)
            {
                game.IsMouseVisible = false;

                if (prevKeyboardState.IsKeyUp(Keys.Escape) && currKeyboardState.IsKeyDown(Keys.Escape))
                {
                    Audio.SoundBank.PlayCue("blip");
                    gameIspaused = true;
                }

                //Mest för testsyfte men lämnar kvar, man kan byta nivåer med siffer-tangenterna 1-5 (inte numpad)
                foreach (var key in levelSelect)
                {
                    if (prevKeyboardState.IsKeyUp(key) && currKeyboardState.IsKeyDown(key))
                    {
                        currentLevel = levelSelect.IndexOf(key) + 1;
                        score = 0;
                        gameIsActive = false;
                        NewGame();
                    }
                }

                paddle.Update(currMouseState);

                //Kollar igenom listan med powerups om någon plockas upp med plattan
                for (int i = 0; i < powerups.Count; i++)
                {
                    powerups[i].Update();

                    //Är lite osäker på hur bra lösning detta är, men jag ville ha alla powerups på ett ställe och detta är vad jag kom fram till.
                    //Kolla vilken typ av powerup det är, spela ett ljud och utför powerup effekt.
                    if (powerups[i].Position.Intersects(paddle.Position) || powerups[i].Position.Y > gameField.Height)
                    {
                        if (powerups[i].Position.Intersects(paddle.Position))
                        {
                            if (powerups[i] is ExtraPoints)
                            {
                                Audio.SoundBank.PlayCue("powerup");

                                score = (powerups[i] as ExtraPoints).PowerUpAction(score);
                            }
                            else if (powerups[i] is ExtraLife)
                            {
                                Audio.SoundBank.PlayCue("powerup");

                                paddle.Life = (powerups[i] as ExtraLife).PowerUpAction(paddle.Life);
                            }
                            else if (powerups[i] is PaddleUp)
                            {
                                Audio.SoundBank.PlayCue("paus"); 

                                if (paddleSize >= 2)
                                {
                                    score -= 100;
                                    paddleSize = 2;
                                }
                                else
                                {
                                    paddle.Position = (powerups[i] as PaddleUp).PowerUpAction(paddle.Position);
                                    paddleSize += 1;
                                }                                
                            }
                            else if (powerups[i] is PaddleDown)
                            {
                                Audio.SoundBank.PlayCue("unpaus");  

                                if (paddleSize <= -2)
                                {
                                    score += 500;
                                    paddleSize = -2;
                                }
                                else
                                {
                                    paddle.Position = (powerups[i] as PaddleDown).PowerUpAction(paddle.Position);
                                    paddleSize -= 1;
                                }                                
                            }
                            else if (powerups[i] is MultiBall)
                            {
                                Audio.SoundBank.PlayCue("pop");  

                                int newBalls = 3 - difficulty;

                                //För att försöka förhindra att bollar får exakt samma riktning så ökar jag eller minskar riktningen per boll. 
                                //Minskar chansen mycket men kan fortfarande hända. 
                                for (int j = 0; j < newBalls; j++)
                                {
                                    if (j == 0)
                                    {
                                        balls.Add(new Ball(ballTexture, gameField, balls[0].Position, ballMotion + new Vector2(3, 0)));
                                    }
                                    else if (j == 1)
                                    {
                                        balls.Add(new Ball(ballTexture, gameField, balls[0].Position, ballMotion + new Vector2(-3, 0)));
                                    }
                                    else if (j == 2)
                                    {
                                        balls.Add(new Ball(ballTexture, gameField, balls[0].Position, ballMotion + new Vector2(8, 0)));
                                    }
                                    
                                }
                            }
                            else if (powerups[i] is PaddleShoot)
                            {
                                Audio.SoundBank.PlayCue("charge");  
                                canShoot = true;
                                shootTimer = 2.5;
                            }
                        }
                        
                        powerups.RemoveAt(i);
                    }
                }

                if (balls.Count == 0 || !gameIsActive)
                {
                    gameIsActive = false;

                    if (balls.Count == 0)
                    {
                        if (difficulty >= 1)
                        {
                            paddle.RemoveLife();

                            if (paddle.Life == 0)
                            {
                                Audio.SoundBank.PlayCue("gameover");
                                GameTransition(1);
                            }
                        }
                    }
                    
                    powerups.Clear();
                    canShoot = false;
                    shootTimer = 0;
                    laserShots.Clear();

                    if (balls.Count == 0)
                    {
                        balls.Add(new Ball(ballTexture, gameField));
                    }
                    
                    balls[0].Idle(paddle.Position);

                    if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        Audio.SoundBank.PlayCue("tubeshot");
                        Random rnd = new Random();

                        ballMotion = new Vector2(rnd.Next(-9, 10), -8);

                        balls[0].Motion = ballMotion;
                        gameIsActive = true;
                    }
                }
                else
	            {
                    for (int i = 0; i < balls.Count; i++)
                    {
                        if (balls[i].Position.Y > gameField.Height)
                        {
                            balls.RemoveAt(i);
                        }
                    }
	            }

                if (gameIsActive)
                {
                    if (bricks.Count - indestructibleBricks == 0)
                    {
                        Audio.SoundBank.PlayCue("levelcomplete");
                        bonusPoints = paddle.Life * 1000;
                        totalScore = score + bonusPoints;
                        gameIsActive = false;
                        GameTransition(2);
                    }

                    foreach (var laser in laserShots)
                    {
                        laser.Update();

                        for (int i = 0; i < bricks.Count; i++)
                        {
                            if (laser.BrickCollision(bricks[i].Position))
                            {
                                laser.Hit = true;
                                Audio.SoundBank.PlayCue("buttonhit");  

                                if (BrickCollision(bricks[i]))
                                {
                                    bricks.RemoveAt(i);                                    
                                } 
                            }
                        }
                    }

                    for (int i = 0; i < laserShots.Count; i++)
                    {
                        if (laserShots[i].Hit == true)
                        {
                            laserShots.RemoveAt(i);
                        }
                    }

                    if (canShoot)
                    {
                        shootTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                        if (shootTimer <= 0.0f)
                        {
                            canShoot = false;
                        }

                        if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Audio.SoundBank.PlayCue("pulse");  
                            laserShots.Add(new Laser(laserTexture,
                                                     new Rectangle(paddle.Position.X, 
                                                                   paddle.Position.Y, 
                                                                   laserTexture.Width, 
                                                                   laserTexture.Height), 
                                                                   new Vector2(0, -10)));
                            laserShots.Add(new Laser(laserTexture, 
                                                     new Rectangle(paddle.Position.X + paddle.Position.Width - laserTexture.Width, 
                                                                   paddle.Position.Y, 
                                                                   laserTexture.Width, 
                                                                   laserTexture.Height), 
                                                                   new Vector2(0, -10)));
                        }

                        prevMouseState = currMouseState;
                    }

                    foreach (var ball in balls)
                    {
                        ball.Update(gameTime);
                        ball.PaddleCollision(paddle.Position, currMouseState);

                        for (int i = 0; i < bricks.Count; i++)
                        {                        
                            if (ball.BrickCollision(bricks[i].Position))
                            {
                                if (BrickCollision(bricks[i]))
                                {
                                    bricks.RemoveAt(i);
                                }                                 
                            }
                        }
                    }                    
                }
            }
            else if (gameIspaused || gameIsOver || levelIsComplete)
            {
                game.IsMouseVisible = true;

                if (pausButtons[mainMenuIndex].ButtonUpdate())
                {
                    if (currentLevel == totalLevels)
                    {
                        GameComplete();
                    }
                    else
                    {
                        screenEvent.Invoke(this, new ScreenChoice(0));
                    }
                }

                if (levelIsComplete)
                {
                    if (nextLevelButton.ButtonUpdate())
                    {
                        if (currentLevel == totalLevels)
                        {
                            GameComplete();
                        }
                        else
                        {
                            score = totalScore;
                            currentLevel += 1;
                            NewGame();
                            levelIsComplete = false;
                        }
                    }
                }

                if (gameIsOver || gameIspaused)
                {
                    if (pausButtons[resetGameIndex].ButtonUpdate())
                    {
                        gameIsActive = false;
                        paddle.Life = 3;
                        score = 0;
                        NewGame();
                        gameIspaused = false;
                        gameIsOver = false;
                    }
                }

                if (gameIspaused)
                {
                    if (pausButtons[continueGameIndex].ButtonUpdate())
                    {
                        gameIspaused = false;
                    }
                    if (pausButtons[quitGameIndex].ButtonUpdate())
                    {
                        game.Exit();
                    }
                    if (prevKeyboardState.IsKeyUp(Keys.Escape) && currKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        gameIspaused = false;
                    }
                }                                   
            }

            prevMouseState = currMouseState;
            prevKeyboardState = currKeyboardState;

            base.Update(gameTime);
        }
        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameBackground, new Rectangle(0, 0, 1280, 720), Color.White);

            spriteBatch.Draw(gameHud, Vector2.Zero, Color.White);

            if (difficulty >= 1)
            {
                spriteBatch.DrawString(font, "Score \n", new Vector2(1140, 15), fontColor);
                spriteBatch.DrawString(font, score.ToString(), new Vector2(1140, 45), Color.White);
                spriteBatch.DrawString(font, "Lives \n", new Vector2(1140, 80), fontColor);
                spriteBatch.Draw(ballTexture, new Vector2(1142, 118), Color.White);
                spriteBatch.DrawString(font, "x " +paddle.Life.ToString(), new Vector2(1172, 110), Color.White);
                
            }

            spriteBatch.DrawString(font, "Level \n", new Vector2(10, 15), fontColor);
            spriteBatch.DrawString(font, currentLevel.ToString(), new Vector2(10, 45), Color.White);
            spriteBatch.DrawString(font, difficultyString, new Vector2(10, 80), fontColor);
            spriteBatch.DrawString(font2, "ESC = Pause", new Vector2(10, 680), fontColor);

            if (canShoot)
            {
                spriteBatch.Draw(paddleShootTexture, new Vector2(1140, 600), Color.White);
                spriteBatch.DrawString(font, Math.Round(shootTimer, 2).ToString(), new Vector2(1190, 605), fontColor);
            }

            foreach (var laser in laserShots)
            {
                laser.Draw(spriteBatch);
            }

            paddle.Draw(spriteBatch);

            for (int i = 0; i < bricks.Count; i++)
            {
                switch (bricks[i].Life)
                {
                    case 1: bricks[i].Color = Color.LimeGreen;
                            bricks[i].Draw(spriteBatch);
                        break;
                    case 2: bricks[i].Color = Color.Green;
                            bricks[i].Draw(spriteBatch);
                        break;
                    case 3: bricks[i].Color = Color.DarkGreen;
                            bricks[i].Draw(spriteBatch);
                        break;
                    case 4: bricks[i].Color = Color.Gray;
                            bricks[i].Draw(spriteBatch);
                        break;
                }
            }

            foreach (var ball in balls)
            {
                ball.Draw(spriteBatch);
            }

            foreach (var powerup in powerups)
            {
                powerup.Draw(spriteBatch);
            }

            if (gameIspaused || gameIsOver || levelIsComplete)
            {
                spriteBatch.Draw(pausedBackground, Vector2.Zero, Color.White);
                
                if (gameIsOver || levelIsComplete)
                {
                    pausButtons[mainMenuIndex].Rectangle = newMainMenuLocation;
                    pausButtons[resetGameIndex].Rectangle = newResetGameLocation;

                    spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width - menuBox.Width) / 2, 200), Color.SteelBlue);

                    if (difficulty >= 1)
                    {
                        spriteBatch.DrawString(font, score.ToString(), new Vector2(720, 255), Color.White);
                        spriteBatch.DrawString(font2, "Your Score", new Vector2(450, 260), fontColor);

                        if (levelIsComplete)
                        {
                            spriteBatch.DrawString(font2, "Life bonus points", new Vector2(450, 310), fontColor);
                            spriteBatch.DrawString(font, bonusPoints.ToString(), new Vector2(720, 305), Color.White);
                            spriteBatch.DrawString(font2, "Total Score", new Vector2(450, 360), fontColor);
                            spriteBatch.DrawString(font, totalScore.ToString(), new Vector2(720, 355), Color.White);
                        }
                    }
                    
                    pausButtons[mainMenuIndex].Draw(spriteBatch);

                    if (gameIsOver)
                    {
                        spriteBatch.DrawString(font, text, new Vector2(550, 210), fontColor);
                        pausButtons[resetGameIndex].Draw(spriteBatch);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, text, new Vector2(505, 210), fontColor);
                        nextLevelButton.Draw(spriteBatch);
                    }
                }
                else
                {
                    spriteBatch.DrawString(font4, "Paused", new Vector2((game.Window.ClientBounds.Width - font4.MeasureString("Paused").X) / 2, 100), fontColor);

                    pausButtons[mainMenuIndex].Rectangle = defaultMainMenuLocation;
                    pausButtons[resetGameIndex].Rectangle = defaultResetGameLocation;

                    foreach (Button button in pausButtons)
                    {
                        button.Draw(spriteBatch);
                    }
                }                
            }

            base.Draw(spriteBatch);
        }

        #endregion
    }
}

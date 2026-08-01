// Logan Phillips
// Final Game 


class Program
{
    // Constants
    private const int ROWS = 6;
    private const int COLS = 7;
    private const string SCORE_FILE = "connect4scores.txt";

    //lists for player names and symbols
    static List<string> playerNames = new List<string> { "Player Red", "Player Yellow" };
    static List<char> playerSymbols = new List<char> { 'R', 'Y' };

    static void Main()
    {
        // 2D board array
        char[,] board = new char[ROWS, COLS];

        // array for help messages
        string[][] helpMessages = new string[2][];
        helpMessages[0] = new string[] { "Enter a column number (1-7) to drop your piece.", "Example: 4" };
        helpMessages[1] = new string[] { "Commands: help, save, history, quit", "Type 'save' to save scores to file." };

        // Move history using parallel lists
        List<int> moveCols = new List<int>();
        List<int> moveRows = new List<int>();
        List<int> movePlayerIndex = new List<int>();

        // Load scores from file
        var scores = LoadScores(SCORE_FILE);

        Console.WriteLine("=== Connect Four ===");
        Console.WriteLine($"Board: {ROWS} rows x {COLS} columns");
        Console.WriteLine("Type 'help' for instructions.");
        Console.WriteLine();

        bool playAgain = true;
        while (playAgain)
        {
            InitializeBoard(ref board);
            moveCols.Clear();
            moveRows.Clear();
            movePlayerIndex.Clear();

            int currentPlayer = 0; // index into parallel lists
            bool gameOver = false;
            char winningSymbol = ' ';

            PrintBoard(in board);

            // Game loop using do-while
            do
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("{0}'s turn ({1})", playerNames[currentPlayer], playerSymbols[currentPlayer]));
                Console.Write("Enter column (1-7): ");
                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Empty input, try again.");
                    continue;
                }

                // Commands using switch
                switch (input.ToLower())
                {
                    case "help":
                        foreach (var section in helpMessages)
                        {
                            foreach (var line in section)
                            {
                                Console.WriteLine("- " + line);
                            }
                        }
                        continue;
                    case "quit":
                        Console.WriteLine("Quitting current game.");
                        gameOver = true;
                        break;
                    case "save":
                        SaveScores(SCORE_FILE, scores);
                        Console.WriteLine("Scores saved.");
                        continue;
                    case "history":
                        PrintMoveHistory(moveCols, moveRows, movePlayerIndex);
                        continue;
                    default:
                        break;
                }

                if (gameOver) break;

                if (!TryParseMove(input, out int col))
                {
                    Console.WriteLine("Invalid input. Enter a column number 1-7 or a command.");
                    continue;
                }

                col--;

            
                if (!MakeMove(ref board, col, playerSymbols[currentPlayer], out int placedRow))
                {
                    Console.WriteLine("Column full or out of range. Try another column.");
                    continue;
                }

                // Record move with lists
                moveCols.Add(col);
                moveRows.Add(placedRow);
                movePlayerIndex.Add(currentPlayer);

                PrintBoard(in board);

                // Check win using tuple
                var (isWin, winnerSymbol) = CheckWin(board, placedRow, col);
                if (isWin)
                {
                    winningSymbol = winnerSymbol;
                    Console.WriteLine();
                    Console.WriteLine(string.Format("Player {0} ({1}) wins!", GetPlayerNameBySymbol(winnerSymbol), winnerSymbol));
                    if (winnerSymbol == 'R') scores.RWins++;
                    else if (winnerSymbol == 'Y') scores.YWins++;
                    gameOver = true;
                    break;
                     // end on win
                }

                // Check for draw
                if (IsBoardFull(board))
                {
                    Console.WriteLine();
                    Console.WriteLine("It's a draw!");
                    scores.Draws++;
                    gameOver = true;
                    break;
                }

                // Switch player using ref
                SwitchPlayer(ref currentPlayer);

            } while (!gameOver);

            // post game
            Console.WriteLine();
            Console.WriteLine("Final board:");
            PrintBoard(in board);

            Console.WriteLine();
            Console.WriteLine(string.Format("Scores: R: {0} | Y: {1} | Draws: {2}", scores.RWins, scores.YWins, scores.Draws));

            // scores to a file after each match
            SaveScores(SCORE_FILE, scores);

            Console.Write("Play again? (y/n) ");
            string again = (Console.ReadLine()?.Trim() ?? string.Empty).ToLower();
            playAgain = (again == "y" || again == "yes");
        }

        Console.WriteLine("Thanks for playing. Final scores:");
        Console.WriteLine($"R: {scores.RWins} | Y: {scores.YWins} | Draws: {scores.Draws}");
    }
    static void InitializeBoard(ref char[,] board)
    {
        for (int r = 0; r < ROWS; r++)
            for (int c = 0; c < COLS; c++)
                board[r, c] = '.';
    }

    static void PrintBoard(in char[,] board)
    {
        Console.WriteLine();
        // Column headers
        Console.Write("   ");
        for (int c = 0; c < COLS; c++)
            Console.Write($" {c + 1} ");
        Console.WriteLine();
        Console.WriteLine("  +" + new string('-', COLS * 3) + "+");

        for (int r = 0; r < ROWS; r++)
        {
            Console.Write($"{r + 1,2}|");
            for (int c = 0; c < COLS; c++)
            {
                Console.Write($" {board[r, c]} ");
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("  +" + new string('-', COLS * 3) + "+");
    }
    static bool TryParseMove(string input, out int col)
    {
        col = -1;
        if (int.TryParse(input, out int parsed))
        {
            if (parsed >= 1 && parsed <= COLS)
            {
                col = parsed;
                return true;
            }
        }
        return false;
    }
    static bool MakeMove(ref char[,] board, int col, char symbol, out int placedRow)
    {
        placedRow = -1;
        if (col < 0 || col >= COLS) return false;

        // Drop from bottom (ROWS-1) upward
        for (int r = ROWS - 1; r >= 0; r--)
        {
            if (board[r, col] == '.')
            {
                board[r, col] = symbol;
                placedRow = r;
                return true;
            }
        }
        return false; // full column
    }
    static (bool, char) CheckWin(char[,] board, int lastRow, int lastCol)
    {
        char symbol = board[lastRow, lastCol];
        if (symbol == '.') return (false, ' ');

        (int dr, int dc)[] directions = new (int, int)[]
        {
            (0, 1), 
            (1, 0),
            (1, 1), 
            (1, -1)
        };

        foreach (var (dr, dc) in directions)
        {
            int count = 1;

            for (int step = 1; step < 4; step++)
            {
                int r = lastRow + dr * step;
                int c = lastCol + dc * step;
                if (r < 0 || r >= ROWS || c < 0 || c >= COLS) break;
                if (board[r, c] == symbol) count++;
                else break;
            }
            
            for (int step = 1; step < 4; step++)
            {
                int r = lastRow - dr * step;
                int c = lastCol - dc * step;
                if (r < 0 || r >= ROWS || c < 0 || c >= COLS) break;
                if (board[r, c] == symbol) count++;
                else break;
            }
            if (count >= 4) return (true, symbol);
        }

        return (false, ' ');
    }

    // Check if board is full
    static bool IsBoardFull(char[,] board)
    {
        for (int c = 0; c < COLS; c++)
            if (board[0, c] == '.') return false;
        return true;
    }

    // Switch player using ref parameter
    static void SwitchPlayer(ref int currentPlayer)
    {
        currentPlayer = (currentPlayer + 1) % playerSymbols.Count;
    }

    static string GetPlayerNameBySymbol(char symbol)
    {
        for (int i = 0; i < playerSymbols.Count; i++)
        {
            if (playerSymbols[i] == symbol) return playerNames[i];
        }
        return "Unknown";
    }

    static void PrintMoveHistory(List<int> cols, List<int> rows, List<int> players)
    {
        if (cols.Count == 0)
        {
            Console.WriteLine("No moves yet.");
            return;
        }

        Console.WriteLine("Move history:");
        for (int i = 0; i < cols.Count; i++)
        {
            Console.WriteLine(string.Format("{0}. {1} -> Col {2}, Row {3}", i + 1, playerNames[players[i]], cols[i] + 1, rows[i] + 1));
        }
    }

    struct Scores
    {
        public int RWins;
        public int YWins;
        public int Draws;
    }

    // Load scores from file
    static Scores LoadScores(string path)
    {
        Scores s = new Scores { RWins = 0, YWins = 0, Draws = 0 };
        try
        {
            if (!File.Exists(path))
            {
                SaveScores(path, s);
                return s;
            }

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                var parts = trimmed.Split(':');
                if (parts.Length != 2) continue;
                var key = parts[0].Trim();
                if (!int.TryParse(parts[1].Trim(), out int val)) continue;

                switch (key)
                {
                    case "R":
                        s.RWins = val;
                        break;
                    case "Y":
                        s.YWins = val;
                        break;
                    case "Draws":
                        s.Draws = val;
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading scores file: " + ex.Message);
        }
        return s;
    }

    // Save scores to file
    static void SaveScores(string path, Scores s)
    {
        try
        {
            var lines = new List<string>
            {
                string.Format("R:{0}", s.RWins),
                string.Format("Y:{0}", s.YWins),
                string.Format("Draws:{0}", s.Draws)
            };
            File.WriteAllLines(path, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving scores: " + ex.Message);
        }
    }
}

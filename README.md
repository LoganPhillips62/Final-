# Final-
includes final and read me file and all final questions answered


Overview and reflection.
in my original aspariation for this code was to play tick tac toe and then i realised that it was actually reall difficult and decided to do connect 4 instead.
i also wanted to have a colorfull congrats on winning graphic after but it didnt turn out that way sadly.
this is interesting to me just overall because i think coding is cool even though i do not understand it that well.
of the project all that was completed was the actual game of connect 4 with that has players take turnswith noe extra colorfull graphics.
the biggest thing i learned is my own limitations and absolutly horrable codeing speed like i am awfully slow. and that google is an amazing tool to help you frankenstine code together.

diagram. (flowchart original one is on paper and im not sure how to insert images into vs code.) 
            ┌──────────────────────┐
            │      Start Game      │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Initialize Board    │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │     Print Board       │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Player chooses a    │
            │       column          │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Drop piece in       │
            │     that column       │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Check for win       │
            └───────────┬──────────┘
                        │
        ┌───────────────┴───────────────┐
        │                                 │
        ▼                                 ▼
┌──────────────────────┐       ┌──────────────────────┐
│   If win → End game   │       │ If board full → Draw │
└──────────────────────┘       └──────────────────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Switch player       │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Repeat turn loop    │
            └───────────┬──────────┘
                        │
                        ▼
            ┌──────────────────────┐
            │   Ask play again?     │
            └───────────┬──────────┘
                        │
        ┌───────────────┴───────────────┐
        │                                 │
        ▼                                 ▼
┌──────────────────────┐       ┌──────────────────────┐
│   Yes → Restart game  │       │      No → Exit        │
└──────────────────────┘       └──────────────────────┘

following concepts and what they are for. below

#1 Const is used at the beginning for fixed values like the board size and score file name.
#2 arrays is used in the connect 4 grid 
#3 lists storing player names and symbols 
#4 if/ else is used for the game ending logic
#5 switch is used for command processing like the help command if rules need to be read
#6 do-while and while are used for the loop for if you want to play again after completing a game
#7 for is used for board intialisation and game win checking.
#8for each is used for printing the help message after you type it in.
#9 reading from a file is used for loading scores
#10 writing from a file is used to save scores for each game.
#11 input from user is used for reading moves and help commands
#12 tuples are used for the game win checking 

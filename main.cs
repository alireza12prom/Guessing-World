using System;
using System.Collections.Generic;


namespace World{

    class WorldGuess{
        private string _alphabet = "abcdefghijklmnopqrstuvwxz";
        private string[] _worlds = {"apple", "hello", "youtube", "red", "weekend"};
        private List<int> _bin  = new List<int>();
        private string _current_world;
        private char[] _current_answer;
        private List<char> _current_guessed = new List<char>();
        private int _level = 1;

        // --------> instance methods
        public void start(){
            Random rand = new Random();
            int random_index; 
            
            while (true){
                random_index = rand.Next(this._worlds.Length);
                if (! this._bin.Contains(random_index)) 
                    break;
            }
            this._bin.Add(random_index);
            this._current_world = this._worlds[random_index];
            this._current_answer = new char[this._current_world.Length];
        }

        public bool input_validation(string input){
            if (input.Length != 1 || ! _alphabet.Contains(input))
                return false;
            else{
                return true;
            }
        }

        public int move(string input){
            char Cinput = Convert.ToChar(input);

            // -1: is used, 1: wrong guess, 0: true
            if (this._current_guessed.Contains(Cinput))
                return -1;
            this._current_guessed.Add(Cinput);
            
            if (! this._current_world.Contains(input))
                return 1;

            for(int i = 0; i < this._current_world.Length; i++){
                if (this._current_world[i] == Cinput)
                    this._current_answer[i] = Cinput;
            }
            return 0;
        }

        // check whether the guess is complete of not
        public bool continue_game(){
            if (this._current_world == string.Join("", this._current_answer))
                return false;
            return true;
        }

        public void reset_game(){
            this._current_guessed.Clear();
        }

        public bool next_level(){
            if (this._level >= this._worlds.Length)
                return false;
            
            this._level += 1;
            return true;
        }
        
        
        // --------> Properties
        public string State{
            get {return string.Join(" , ", this._current_answer);}
        }

        // return the guessed characters
        public string Guessed{
            get {return string.Format("[{0}]", string.Join(" , ", this._current_guessed));}
        }

        // return the current level
        public int CLevel{
            get {return this._level;}
        }

        // return the numbre of levels
        public int Level{
            get {return this._worlds.Length;}
        }

        // return the number of guesses thet are acceptable
        public int Rounds{
            get {return this._current_world.Length + 2;}
        }
    }

    class App{
        static void Main(){
            WorldGuess game = new WorldGuess();
            int i;
            int rounds;

            // setup
            game.start();
            rounds = game.Rounds;

            string user_choice;
            for(i = 0; i <= rounds; i ++){
                Console.WriteLine("\nRound: {0}/{1} | Level: {2}/{3}", i, rounds, game.CLevel, game.Level);
                Console.WriteLine("Bin: {0}", game.Guessed);
                Console.WriteLine("State: {0}", game.State);

                // get input
                user_choice = Console.ReadLine();

                // clear console
                Console.Clear();

                // check input is valid
                if (! game.input_validation(user_choice)){
                    Console.WriteLine("Error: Please enter a alphabetical charactor");
                    continue;}
                
                // check user guess is true or not
                int res = game.move(user_choice);
                if (res == -1){
                    Console.WriteLine("Error: You have already use this world!");
                    continue;
                }else if (res == 1){
                    Console.WriteLine("Your guess is wrong! Try again :>");
                    continue;
                } else{
                    Console.WriteLine("Currect :>");
                }

                // check user guessed all the world or not
                if (game.continue_game())
                    continue;

                // check if there is no next level, end game
                if (! game.next_level()){
                    Console.Clear();
                    Console.WriteLine("Congratulation - You pass all the levels :)", game.Level);
                    return;
                }   

                Console.Write("Wins!! continue or exit [c / e]? ");
                user_choice = Console.ReadLine();
                
                if (user_choice.ToLower() == "e")
                    return;
                
                // reseting game for next level
                game.reset_game();
                game.start();
                rounds = game.Rounds;
                i = 0;
            }
            
            Console.WriteLine("Sorry, you lost :(");
        } 
    }
}
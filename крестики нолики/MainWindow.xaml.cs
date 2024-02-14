using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using Xamarin.Forms;

namespace kn
{
    public partial class MainPage : ContentPage
    {
        private string currentPlayer;
        private List<List<Button>> buttonGrid;
        private Random random;
        public ICommand ButtonCommand { get; private set; }
        public ICommand RestartCommand { get; private set; }
        public bool IsRestartButtonEnabled { get; set; }
        public MainPage()
        {
            InitializeComponent();
            currentPlayer = "X";
            buttonGrid = new List<List<Button>>();
            random = new Random();
            ButtonCommand = new Command<string>(ButtonClicked);
            RestartCommand = new Command(RestartClicked);
            Createbutton();
            EnableButtons(false);
        }

        private void Createbutton()
        {
            buttonGrid.Add(new List<Button>() { Button1, Button2, Button3 });
            buttonGrid.Add(new List<Button>() { Button4, Button5, Button6 });
            buttonGrid.Add(new List<Button>() { Button7, Button8, Button9 });
        }

        private void EnableButtons(bool enable)
        {
            foreach (var buttonRow in buttonGrid)
            {
                foreach (var button in buttonRow)
                {
                    button.IsEnabled = enable;
                }
            }
        }

        private void ButtonClicked(string buttonNumber)
        {
            int number = int.Parse(buttonNumber);
            int row = (number - 1) / 3;
            int col = (number - 1) % 3;

            buttonGrid[row][col].Text = currentPlayer;
            buttonGrid[row][col].IsEnabled = false;

            if (win(currentPlayer))
            {
                DisplayAlert("Конец игры", $"{currentPlayer} победил!", "OK");
                EnableButtons(false);
            }
            else if (fullboard())
            {
                DisplayAlert("Конец игры", "Ничья!", "OK");
                EnableButtons(false);
            }
            else
            {
                currentPlayer = (currentPlayer == "X") ? "O" : "X";
                if (currentPlayer == "O")
                {
                    robotmove();
                }
            }
        }

        private bool win(string player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (buttonGrid[i][0].Text == player && buttonGrid[i][1].Text == player && buttonGrid[i][2].Text == player)
                {
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (buttonGrid[0][i].Text == player && buttonGrid[1][i].Text == player && buttonGrid[2][i].Text == player)
                {
                    return true;
                }
            }
            if (buttonGrid[2][0].Text == player && buttonGrid[1][1].Text == player && buttonGrid[0][2].Text == player)
            {
                return true;
            }
            return false;
        }

        private bool fullboard()
        {
            foreach (var buttonRow in buttonGrid)
            {
                foreach (var button in buttonRow)
                {
                    if (button.Text == "")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void RestartClicked()
        {
            currentPlayer = "X";
            foreach (var buttonRow in buttonGrid)
            {
                foreach (var button in buttonRow)
                {
                    button.Text = "";
                    button.IsEnabled = true;
                }
            }
            EnableButtons(true);
        }

        private void robotmove()
        {
            List<Button> emptyButtons = new List<Button>();
            foreach (var buttonRow in buttonGrid)
            {
                foreach (var button in buttonRow)
                {
                    if (button.Text == "")
                    {
                        emptyButtons.Add(button);
                    }
                }
            }

            if (emptyButtons.Count > 0)
            {
                int index = random.Next(emptyButtons.Count);
                emptyButtons[index].Text = currentPlayer;
                emptyButtons[index].IsEnabled = false;
                if (win(currentPlayer))
                {
                    DisplayAlert("Конец игры", $"{currentPlayer} победил!", "OK");
                    EnableButtons(false);
                }
                else if (fullboard())
                {
                    DisplayAlert("Конец игры", "Ничья!", "OK");
                    EnableButtons(false);
                }
                else
                {
                    currentPlayer = "X";
                }
            }
        }
    }
}
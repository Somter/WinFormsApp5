using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
         Button[,] buttons = new Button[3, 3];
         bool isXTurn = true; 
         bool gameActive = false;
         string imagePathX = @"Images2\photo_5345997896035724545_m.jpg";
         string imagePathO = @"Images2\photo_5345997896035724544_m.jpg";

         Random random = new Random(); 
         Button lastComputerMove = null;

        public Form1()
        {
            InitializeComponent();
            InitializeGameField();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void InitializeGameField()
        {
            buttons[0, 0] = button1;
            buttons[0, 1] = button2;
            buttons[0, 2] = button3;
            buttons[1, 0] = button4;
            buttons[1, 1] = button5;
            buttons[1, 2] = button6;
            buttons[2, 0] = button7;
            buttons[2, 1] = button8;
            buttons[2, 2] = button9;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j].Tag = null; 
                    buttons[i, j].Click += GameButton_Click; 
                }
            }
        }

        private void GameButton_Click(object sender, EventArgs e)
        {
            if (!gameActive) return;

            Button clickedButton = sender as Button;
            if (clickedButton == null || clickedButton.Tag != null) return;

            MakeMove(clickedButton, "X");

            if (!gameActive) return;

            ComputerMove();
        }

        private void ComputerMove()
        {
            Button targetButton = null;

            if (radioButton2.Checked) 
            {
                if (lastComputerMove != null)
                {
                    var neighbors = GetNeighbors(lastComputerMove);
                    targetButton = neighbors.FirstOrDefault(b => b.Tag == null);
                }
            }

            if (targetButton == null)
            {
                var emptyButtons = buttons.Cast<Button>().Where(b => b.Tag == null).ToList();
                if (emptyButtons.Count > 0)
                {
                    targetButton = emptyButtons[random.Next(emptyButtons.Count)];
                }
            }

            if (targetButton != null)
            {
                MakeMove(targetButton, "O");
                lastComputerMove = targetButton;
            }
        }

        private Button GetStrategicMove()
        {
            var playerMoves = buttons.Cast<Button>().Where(b => b.Tag as string == "X").ToList();

            foreach (var move in playerMoves)
            {
                var neighbors = GetNeighbors(move);

                var strategicMove = neighbors.FirstOrDefault(b => b.Tag == null);
                if (strategicMove != null)
                {
                    return strategicMove; 
                }
            }

            return null;
        }

        private List<Button> GetNeighbors(Button button)
        {
            var neighbors = new List<Button>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (buttons[i, j] == button)
                    {
                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                int ni = i + di, nj = j + dj;

                                if ((di != 0 || dj != 0) && ni >= 0 && nj >= 0 && ni < 3 && nj < 3)
                                {
                                    neighbors.Add(buttons[ni, nj]);
                                }
                            }
                        }
                        return neighbors; 
                    }
                }
            }

            return neighbors; 
        }

        private void MakeMove(Button button, string player)
        {
            string imagePath = player == "X" ? imagePathX : imagePathO;

            try
            {
                button.BackgroundImage = Image.FromFile(imagePath);
                button.BackgroundImageLayout = ImageLayout.Stretch;
                button.Tag = player;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

          
            if (CheckWin())
            {
                MessageBox.Show(player == "X" ? "������ ��������!" : "�������� ��������!");
                gameActive = false;
                return;
            }

            if (buttons.Cast<Button>().All(b => b.Tag != null))
            {
                MessageBox.Show("�����!");
                gameActive = false;
                return;
            }

            isXTurn = !isXTurn;
        }

        private bool CheckWin()
        {
         
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Tag != null && buttons[i, 0].Tag == buttons[i, 1].Tag && buttons[i, 0].Tag == buttons[i, 2].Tag)
                {
                    return true;
                }

                if (buttons[0, i].Tag != null && buttons[0, i].Tag == buttons[1, i].Tag && buttons[0, i].Tag == buttons[2, i].Tag)
                {
                    return true;
                }
            }

            if (buttons[0, 0].Tag != null && buttons[0, 0].Tag == buttons[1, 1].Tag && buttons[0, 0].Tag == buttons[2, 2].Tag)
                return true;

            if (buttons[0, 2].Tag != null && buttons[0, 2].Tag == buttons[1, 1].Tag && buttons[0, 2].Tag == buttons[2, 0].Tag)
                return true;

            return false; 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("�������� ������� ���������!");
                return;
            }

            isXTurn = true;
            gameActive = true;
            lastComputerMove = null;

            foreach (Button btn in buttons)
            {
                btn.Tag = null;
                btn.BackgroundImage = null;
            }

            if (checkBox1.Checked) 
            {
                ComputerMove();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

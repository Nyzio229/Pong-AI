using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PongAI
{
    public partial class MainWindow : Window
    {
        private double ballXSpeed = 5;
        private double ballYSpeed = 5;

        private double playerSpeed = 10;
        private double aiSpeed = 5;

        private bool moveUp;
        private bool moveDown;

        private DispatcherTimer gameTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            // Ustawienie początkowych pozycji
            Canvas.SetLeft(Ball, 390);
            Canvas.SetTop(Ball, 240);

            Canvas.SetLeft(PlayerPaddle, 20);
            Canvas.SetTop(PlayerPaddle, 200);

            Canvas.SetLeft(AIPaddle, 770);
            Canvas.SetTop(AIPaddle, 200);

            // Inicjalizacja timera gry
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Pobranie aktualnej pozycji piłki
            double ballX = Canvas.GetLeft(Ball);
            double ballY = Canvas.GetTop(Ball);

            // Ruch piłki
            Canvas.SetLeft(Ball, ballX + ballXSpeed);
            Canvas.SetTop(Ball, ballY + ballYSpeed);

            // Odbicie piłki od górnej ścianki
            if (ballY <= 0)
            {
                ballYSpeed = Math.Abs(ballYSpeed); // Odbicie w dół
            }

            // Odbicie piłki od dolnej ścianki
            if (ballY >= 460)
            {
                ballYSpeed = -Math.Abs(ballYSpeed); // Odbicie w górę
            }

            // Pobranie pozycji paletek
            double playerY = Canvas.GetTop(PlayerPaddle);
            double playerX = Canvas.GetLeft(PlayerPaddle);

            double aiY = Canvas.GetTop(AIPaddle);
            double aiX = Canvas.GetLeft(AIPaddle);

            // Sprawdzenie kolizji piłki z paletką gracza
            if (ballX <= playerX + 10 && ballY + 20 >= playerY && ballY <= playerY + 80)
            {
                ballXSpeed = Math.Abs(ballXSpeed); // Odbicie w prawo
            }

            // Sprawdzenie kolizji piłki z paletką AI
            if (ballX + 20 >= aiX && ballY + 20 >= aiY && ballY <= aiY + 80)
            {
                ballXSpeed = -Math.Abs(ballXSpeed); // Odbicie w lewo
            }

            // AI zaczyna się ruszać, gdy piłka jest na jego połowie
            if (ballX > 400)
            {
                if (aiY + 40 < ballY) Canvas.SetTop(AIPaddle, aiY + aiSpeed);
                else if (aiY + 40 > ballY) Canvas.SetTop(AIPaddle, aiY - aiSpeed);
            }

            // Ruch gracza (tylko gdy trzymamy klawisz)
            if (moveUp && playerY > 0)
                Canvas.SetTop(PlayerPaddle, playerY - playerSpeed);
            else if (moveDown && playerY < 420)
                Canvas.SetTop(PlayerPaddle, playerY + playerSpeed);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                moveUp = true;
            else if (e.Key == Key.Down)
                moveDown = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                moveUp = false;
            else if (e.Key == Key.Down)
                moveDown = false;
        }
    }
}
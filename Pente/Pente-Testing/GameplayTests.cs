using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

using System.Threading;
using System.Threading.Tasks;

using Pente;
using Pente.UserControls;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente.Logic;
using Pente.Models;
using System.Reflection;
using System.Windows.Interop;

namespace Pente_Testing {
    [TestClass]
    public class GameplayTests {
        [TestMethod]
        public void TestBlackMove1CantBe3FromCenter() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackMove1CantBe3FromCenter_TestLogic);

            int count = (int)dmn.GetData("Count");
            for (int i = 0; i < count; i++) {
                Assert.IsTrue((bool)dmn.GetData($"AssertTrue[{i}]"));
            }
        }
        private static void TestBlackMove1CantBe3FromCenter_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;
                    int maxDist = 3, blackTryCount = 0;
                    foreach (UIElement uie in ug.Children) {
                        Stone s = uie as Stone;

                        int index = ug.Children.IndexOf(s);

                        int row = index / columns;  // divide
                        int column = index % columns;  // modulus

                        int rowDist = Math.Abs(row - rows / 2);
                        int colDist = Math.Abs(column - columns / 2);
                        
                        if (row == 0 && column == 0) {
                            StoneState s1 = s.CurrentState;
                            UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", s);
                            StoneState s2 = s.CurrentState;

                            AppDomain.CurrentDomain.SetData($"AssertTrue[{blackTryCount}]", s1 != s2 && s2 == StoneState.White);
                        }

                        if (rowDist < maxDist && colDist < maxDist) {
                            StoneState s1 = s.CurrentState;
                            UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", s);
                            blackTryCount++;
                            StoneState s2 = s.CurrentState;

                            AppDomain.CurrentDomain.SetData($"AssertTrue[{blackTryCount}]", s1 == s2);
                        }
                    }
                    AppDomain.CurrentDomain.SetData("Count", blackTryCount);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }

        [TestMethod]
        public void TestWhiteCapture_Horizontal() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestWhiteCapture_Horizontal_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestWhiteCapture_Horizontal_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[5 * rows + 0]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white-capture

                    bool shouldBeTrue = (ug.Children[0 * rows + 1] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[0 * rows + 2] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestWhiteCapture_Vertical() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestWhiteCapture_Vertical_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestWhiteCapture_Vertical_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 0]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 5]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 0]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 0]); //white-capture

                    bool shouldBeTrue = (ug.Children[1 * rows + 0] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[2 * rows + 0] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestWhiteCapture_Diagonal_NegativeSlope() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestWhiteCapture_Diagonal_NegativeSlope_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestWhiteCapture_Diagonal_NegativeSlope_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 5]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 3]); //white-capture

                    bool shouldBeTrue = (ug.Children[1 * rows + 1] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[2 * rows + 2] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestWhiteCapture_Diagonal_PosativeSlope() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestWhiteCapture_Diagonal_PosativeSlope_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestWhiteCapture_Diagonal_PosativeSlope_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 5]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white-capture

                    bool shouldBeTrue = (ug.Children[2 * rows + 1] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[1 * rows + 2] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }


        [TestMethod]
        public void TestBlackCapture_Horizontal() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackCapture_Horizontal_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestBlackCapture_Horizontal_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[10 * rows + 10]); //Swap order from that specified
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[5 * rows + 0]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white-capture

                    bool shouldBeTrue = (ug.Children[0 * rows + 1] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[0 * rows + 2] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestBlackCapture_Vertical() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackCapture_Vertical_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestBlackCapture_Vertical_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[10 * rows + 10]); //Swap order from that specified
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 0]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 5]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 0]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 0]); //white-capture

                    bool shouldBeTrue = (ug.Children[1 * rows + 0] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[2 * rows + 0] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestBlackCapture_Diagonal_NegativeSlope() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackCapture_Diagonal_NegativeSlope_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestBlackCapture_Diagonal_NegativeSlope_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            
            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[10 * rows + 10]); //Swap order from that specified
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 5]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 3]); //white-capture

                    bool shouldBeTrue = (ug.Children[1 * rows + 1] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[2 * rows + 2] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestBlackCapture_Diagonal_PosativeSlope() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackCapture_Diagonal_PosativeSlope_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestBlackCapture_Diagonal_PosativeSlope_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[10 * rows + 10]); //Swap order from that specified
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 5]); //white-waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white-capture

                    bool shouldBeTrue = (ug.Children[2 * rows + 1] as Stone).CurrentState == StoneState.Open &&
                        (ug.Children[1 * rows + 2] as Stone).CurrentState == StoneState.Open;
                    AppDomain.CurrentDomain.SetData("AssertTrue", shouldBeTrue);
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }

        [TestMethod]
        public void TestWhiteWinOn5Capture() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestWhiteWinOn5Capture_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestWhiteWinOn5Capture_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 1]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 0]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 3]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 4]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 1]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 3]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 0]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 3]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 4]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[5 * rows + 1]); //white --waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 3]); //white-capture
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameOverScreen mm = UIAutomationHelper.FindVisualChild<GameOverScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameOverScreen);
                        }
                    ));

                    Label b = UIAutomationHelper.FindVisualChild<Label>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "StyleWonLabel";
                        }
                    ));

                    AppDomain.CurrentDomain.SetData($"AssertTrue", (b.Content as string)?.Contains("5+ captures"));
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestBlackWinOn5Capture() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackWinOn5Capture_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestBlackWinOn5Capture_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[10 * rows + 10]); //Invert order specified
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 1]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 0]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 3]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 4]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 1]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[2 * rows + 3]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 0]); //white --usedForNextCapture
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 3]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[3 * rows + 4]); //white-capture

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 1]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[5 * rows + 1]); //white --waste
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 2]); //black
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[4 * rows + 3]); //white-capture

                    AppDomain.CurrentDomain.SetData("AssertTrue", false);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameOverScreen mm = UIAutomationHelper.FindVisualChild<GameOverScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameOverScreen);
                        }
                    ));

                    Label b = UIAutomationHelper.FindVisualChild<Label>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "StyleWonLabel";
                        }
                    ));

                    AppDomain.CurrentDomain.SetData($"AssertTrue", (b.Content as string)?.Contains("5+ captures"));
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }

        [TestMethod]
        public void TestWhiteWinOn5InARow() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestWhiteWinOn5InARow_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestWhiteWinOn5InARow_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 0]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 1]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 1]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 2]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 2]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 3]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 4]); //white-win
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameOverScreen mm = UIAutomationHelper.FindVisualChild<GameOverScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameOverScreen);
                        }
                    ));

                    Label b = UIAutomationHelper.FindVisualChild<Label>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "StyleWonLabel";
                        }
                    ));

                    AppDomain.CurrentDomain.SetData($"AssertTrue", (b.Content as string)?.Contains("5 Stones in a row"));
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        [TestMethod]
        public void TestBlackWinOn5InARow() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestBlackWinOn5InARow_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestBlackWinOn5InARow_TestLogic() {
            Dispatcher MainDispatcher = Dispatcher.CurrentDispatcher;

            App _App = new App();
            _App.InitializeComponent();
            

            Task.Run(() => {
                Thread.Sleep(500);

                MainDispatcher.Invoke(() => {
                    TextBox t1 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player1Name";
                        }
                    ));
                    t1.Text = "P1";
                    TextBox t2 = UIAutomationHelper.FindVisualChild<TextBox>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "Player2Name";
                        }
                    ));
                    t2.Text = "P2";
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameScreen gs = UIAutomationHelper.FindVisualChild<GameScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameScreen);
                        }
                    ));
                    UniformGrid ug = UIAutomationHelper.FindVisualChild<UniformGrid>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "GameGrid";
                        }
                    ));
                    int rows = ug.Rows;
                    int columns = ug.Columns;

                    //+ columns * rows
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[10 * rows + 10]); //Invert Order Specified

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 0]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 0]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 1]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 1]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 2]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 2]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 3]); //white
                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[1 * rows + 3]); //black

                    UIAutomationHelper.Click(gs, "StoneOnMouseLeftButtonDown", ug.Children[0 * rows + 4]); //white-win
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    GameOverScreen mm = UIAutomationHelper.FindVisualChild<GameOverScreen>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(GameOverScreen);
                        }
                    ));

                    Label b = UIAutomationHelper.FindVisualChild<Label>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "StyleWonLabel";
                        }
                    ));

                    AppDomain.CurrentDomain.SetData($"AssertTrue", (b.Content as string)?.Contains("5 Stones in a row"));
                });
                MainDispatcher.Invoke(() => {
                    _App.MainWindow.Focus();
                });

                Thread.Sleep(1000);

                MainDispatcher.Invoke(() => {
                    _App.Shutdown();
                });
            });
            _App.Run(new MainWindow());
        }
        
    }
}

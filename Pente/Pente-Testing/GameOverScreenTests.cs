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

namespace Pente_Testing {
    [TestClass]
    public class GameOverScreenTests {
        [TestMethod]
        public void TestGameOver_NewGame() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameOver_NewGame_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestGameOver_NewGame_TestLogic() {
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

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "NewGameButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "NewGameButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    MainMenu mm = UIAutomationHelper.FindVisualChild<MainMenu>(_App.MainWindow, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.GetType() == typeof(MainMenu);
                        }
                    ));

                    AppDomain.CurrentDomain.SetData("AssertTrue", mm != null);
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
        public void TestGameOver_Exit() {
            AppDomain dmn = AppDomain.CreateDomain("TestGameStart_WithPlayerNames_WithDefaultSize",
               AppDomain.CurrentDomain.Evidence,
               AppDomain.CurrentDomain.SetupInformation);

            dmn.DoCallBack(TestGameOver_Exit_TestLogic);

            Assert.IsTrue((bool)dmn.GetData($"AssertTrue"));
        }
        private static void TestGameOver_Exit_TestLogic() {
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

                    Button b = UIAutomationHelper.FindVisualChild<Button>(mm, (Func<FrameworkElement, bool>)(
                        (x) => {
                            return x.Name == "ExitButton";
                        }
                    ));

                    UIAutomationHelper.Click(mm, "ExitButton_Click", b);
                });

                Thread.Sleep(100);

                MainDispatcher.Invoke(() => {
                    bool works = false;
                    try {
                        _App.MainWindow.Focus();
                    } catch {
                        works = true;
                    }
                    AppDomain.CurrentDomain.SetData("AssertTrue", works);
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
